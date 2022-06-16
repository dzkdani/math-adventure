using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
public class QuizImageLoader : MonoBehaviour
{
    [SerializeField] Sprite CurrentLoadedSprite; 

    public async Task<Sprite> LoadSprite(string url)
    {
        return await GetSprite(url, onError, onSuccess);
    } 
    
    private void onError(string _error)
    {
        Debug.Log($"Error : {_error}");
    }

    private void onSuccess(Texture2D _success)
    {
        Debug.Log($"Sucess : {_success.ToString()}");
        CurrentLoadedSprite = Sprite.Create(_success, new Rect(0,0, _success.width, _success.height), Vector2.zero);
    }

    private async Task<Sprite> GetSprite(string url, Action<string> onError, Action<Texture2D> onSuccess)
    {
        using (UnityWebRequest request =  UnityWebRequestTexture.GetTexture(url))
        {
            var task = request.SendWebRequest();

            while (!task.isDone) await Task.Yield();

            if (request.result != UnityWebRequest.Result.Success)
            {
                onError(request.error);
            } 
            else 
            {
                DownloadHandlerTexture downloadHandlerTexture = request.downloadHandler as DownloadHandlerTexture;
                onSuccess(downloadHandlerTexture.texture);
                return Sprite.Create(downloadHandlerTexture.texture, new Rect(0,0, downloadHandlerTexture.texture.width, downloadHandlerTexture.texture.height), Vector2.zero);
            }
            return default;
        }
    }
}
