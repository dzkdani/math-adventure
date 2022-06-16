using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public static class APIRequests
{
    public static bool sukses;

#region Coroutines
    public static IEnumerator Post<T>(this WWWForm form, string uri, Action<T, string> handler, ResponseType responseType)
    {
        using UnityWebRequest request = UnityWebRequest.Post(uri, form);
        
        request.SetRequestHeader("Authorization", "Bearer "+ PlayerPrefs.GetString("token"));
        
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            try
            {
                T response = JsonUtility.FromJson<T>(request.downloadHandler.text);
                handler(response, responseType.ToString());
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Could not parse {request.downloadHandler.text}. {ex.Message}");
            }   
        }
    }

    public static IEnumerator Get<T>(this string uri, Action<T, string> handler, ResponseType responseType)
    {
        sukses = false;
        using UnityWebRequest request = UnityWebRequest.Get(uri);
        
        request.SetRequestHeader("Authorization", "Bearer "+ PlayerPrefs.GetString("token"));
        
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            try
            {
                T response = JsonUtility.FromJson<T>(request.downloadHandler.text);
                handler(response, responseType.ToString());
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Could not parse {request.downloadHandler.text}. {ex.Message}");
            }
            sukses = true;
        }
    }
#endregion

#region  Async
    public static async Task<T> GetWebRequest<T>(this string uri)
    {
        T result = await AsyncGet<T>(uri);
        return result;
    }

    public static async Task<T> PostWebRequest<T>(this string uri, WWWForm form)
    {
        T result = await AsyncPost<T>(uri, form);
        return result;
    }

    public static async Task<T> AsyncGet<T>(string uri)
    {
        sukses = false;
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            request.SetRequestHeader("Authorization", "Bearer "+ PlayerPrefs.GetString("token"));
            request.SendWebRequest();
        
            while (!request.isDone) await Task.Yield();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                try
                {
                    T response = JsonUtility.FromJson<T>(request.downloadHandler.text);
                    return response;
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Could not parse {request.downloadHandler.text}. {ex.Message}");
                }
                sukses = true;
            }
            return default;   
        }
    }

    private static async Task<T> AsyncPost<T>(string uri, WWWForm form)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {
            request.SetRequestHeader("Authorization", "Bearer "+ PlayerPrefs.GetString("token"));
            request.SendWebRequest();
        
            while (!request.isDone) await Task.Yield();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                try
                {
                    T response = JsonUtility.FromJson<T>(request.downloadHandler.text);
                    return response;
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Could not parse {request.downloadHandler.text}. {ex.Message}");
                }
            }
            return default;   
        }
    }
#endregion
}
