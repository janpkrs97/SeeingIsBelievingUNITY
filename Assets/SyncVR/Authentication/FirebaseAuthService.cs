using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SyncVR.Authentication
{
    public static class FirebaseAuthService
    {
        private const string getCustomTokenURLProd = "https://europe-west1-optimum-time-233909.cloudfunctions.net/api-get-custom-token";
        private const string getCustomTokenURLDev = "https://europe-west1-syncvr-dev.cloudfunctions.net/api-get-custom-token";
        private const string loginWithCustomTokenURL = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithCustomToken?key=";
        private const string refreshIdTokenURL = "https://securetoken.googleapis.com/v1/token?key=";

        private const string rejectedString = "REJECTED";

        private const float backoffPeriod = 60f;

        private static bool isRefreshed = false;
        private static string customToken = "";
        public static string idToken { get; private set; }
        private static string refreshToken = "";
        private static float expiresIn = 0f;

        public static bool isLoggingIn { get; private set; }
        public static bool isLoggedIn { get; private set; } = false;
        public static bool isUseDevelopment { get; private set; }

        private static string firebaseApiKey = "";
        private static string getCustomTokenURL = getCustomTokenURLProd;

        public static void SetUseDevelopmentEnvironment(bool useDev)
        {
            isUseDevelopment = useDev;
            if (isUseDevelopment)
            {
                getCustomTokenURL = getCustomTokenURLDev;
            }
            else
            {
                getCustomTokenURL = getCustomTokenURLProd;
            }
        }

        public static IEnumerator Login(string app_key, string device_id = "")
        {
            if (isLoggingIn || isLoggedIn)
            {
                Debug.Log("Login called twice! Aborting!");
                yield break;
            }
            isLoggingIn = true;
            idToken = "";

            yield return GetCustomToken(app_key, device_id);
            while (customToken == "")
            {
                yield return new WaitForSeconds(backoffPeriod);
                yield return GetCustomToken(app_key, device_id);
            }
            if (customToken == rejectedString)
            {
                isLoggingIn = false;
                isLoggedIn = false;
                yield break;
            }

            yield return LoginWithCustomToken(customToken, firebaseApiKey);
            while (idToken == "")
            {
                yield return new WaitForSeconds(backoffPeriod);
                yield return LoginWithCustomToken(customToken, firebaseApiKey);
            }

            isLoggedIn = true;

            float t = Time.realtimeSinceStartup + expiresIn;
            while (true)
            {
                // start refreshing 120 seconds before expiry
                if (Time.realtimeSinceStartup < (t - 120))
                {
                    yield return null;
                }
                else
                {
                    isRefreshed = false;
                    yield return RefreshIDToken(refreshToken, firebaseApiKey);
                    while (!isRefreshed)
                    {
                        yield return new WaitForSeconds(backoffPeriod);
                        yield return RefreshIDToken(refreshToken, firebaseApiKey);
                    }

                    t = Time.realtimeSinceStartup + expiresIn;
                    Debug.Log("Refreshed ID Token!");
                }
            }
        }

        public static bool AuthenticateRequest(UnityWebRequest www)
        {
            if (idToken == "")
            {
                return false;
            }

            www.SetRequestHeader("Authorization", "Bearer " + idToken);
            return true;
        }

        private static IEnumerator GetCustomToken(string app_key, string device_id)
        {
            Dictionary<string, string> postData = new Dictionary<string, string> { { "app_key", app_key }, { "device_id", device_id } };

            using (UnityWebRequest www = new UnityWebRequest(getCustomTokenURL))
            {
                www.method = "POST";
                www.SetRequestHeader("Content-Type", "application/json");
                www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(postData)));
                www.downloadHandler = new DownloadHandlerBuffer();

                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ProtocolError && www.responseCode < 500)
                {
                    customToken = rejectedString;
                }
                else if (www.result == UnityWebRequest.Result.ProtocolError && www.responseCode >= 500)
                {
                    customToken = "";
                }
                else if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    customToken = "";
                }
                else
                {
                    //Debug.Log(www.responseCode + " " + www.downloadHandler.text);
                    try
                    {
                        JObject response = JsonConvert.DeserializeObject<JObject>(www.downloadHandler.text);
                        customToken = response.Value<string>("custom-token");
                        firebaseApiKey = response.Value<string>("firebase-api-key");
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Error deserializing json! " + e.StackTrace);
                        customToken = "";
                    }
                }
            }
        }

        private static IEnumerator LoginWithCustomToken(string token, string api_key)
        {
            Dictionary<string, object> postData = new Dictionary<string, object> { { "token", token }, { "returnSecureToken", true } };

            using (UnityWebRequest www = new UnityWebRequest(loginWithCustomTokenURL + api_key))
            {
                www.method = "POST";
                www.SetRequestHeader("Content-Type", "application/json");
                www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(postData)));
                www.downloadHandler = new DownloadHandlerBuffer();

                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ProtocolError)
                {
                    //Debug.Log(www.responseCode + " " + www.downloadHandler.text);
                    idToken = "";
                    refreshToken = "";
                }
                else if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    idToken = "";
                    refreshToken = "";
                }
                else
                {
                    //Debug.Log(www.responseCode + " " + www.downloadHandler.text);
                    try
                    {
                        JObject response = JsonConvert.DeserializeObject<JObject>(www.downloadHandler.text);
                        idToken = response.Value<string>("idToken");
                        refreshToken = response.Value<string>("refreshToken");
                        expiresIn = response.Value<float>("expiresIn");
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Error deserializing json! " + e.StackTrace);
                        idToken = "";
                        refreshToken = "";
                    }
                }
            }
        }

        private static IEnumerator RefreshIDToken(string refresh_token, string api_key)
        {
            Dictionary<string, string> postData = new Dictionary<string, string> { { "grant_type", "refresh_token" }, { "refresh_token", refresh_token } };

            using (UnityWebRequest www = new UnityWebRequest(refreshIdTokenURL + api_key))
            {
                www.method = "POST";
                www.SetRequestHeader("Content-Type", "application/json");
                www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(postData)));
                www.downloadHandler = new DownloadHandlerBuffer();

                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log(www.responseCode + " " + www.downloadHandler.text);
                }
                else if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                }
                else
                {
                    //Debug.Log(www.responseCode + " " + www.downloadHandler.text);
                    try
                    {
                        JObject response = JsonConvert.DeserializeObject<JObject>(www.downloadHandler.text);
                        idToken = response.Value<string>("id_token");
                        refreshToken = response.Value<string>("refresh_token");
                        expiresIn = response.Value<float>("expires_in");
                        isRefreshed = true;
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Error deserializing json! " + e.StackTrace);
                    }
                }
            }
        }

    }
}