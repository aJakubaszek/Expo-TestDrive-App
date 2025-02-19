using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ImageDownloadManager : MonoBehaviour {
    [SerializeField] string manifestUrl;
    [SerializeField] string path;
    [SerializeField] string baseUrl;
    bool filesDownloaded = false;
    string fullPath;

    List<string> imageUrls = new List<string>();
    List<Texture> textures = new List<Texture>();

    void Awake() {
        fullPath = Path.Combine(Application.dataPath, path);
        StartCoroutine(DownloadUrls());
    }

    void OnApplicationQuit() {
        string[] files = Directory.GetFiles(fullPath);
        foreach (string file in files) {
            File.Delete(file);
        }
    }

    IEnumerator DownloadUrls() {
        using (UnityWebRequest request = UnityWebRequest.Get(manifestUrl)) {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError(request.error);
            } else {
                string[] lines = request.downloadHandler.text.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                imageUrls.AddRange(lines);
                yield return StartCoroutine(PreloadImages());
            }
        }
        filesDownloaded = true;
    }

    IEnumerator PreloadImages() {
        foreach (string url in imageUrls) {
            string imagePath = Path.Combine(fullPath, url);
            if (!File.Exists(imagePath)) {
                yield return StartCoroutine(DownloadImage(url));
            } 
        }
    }

    IEnumerator DownloadImage(string url) { 
        string imageUrl = Path.Combine(baseUrl, url);
        string imagePath = Path.Combine(fullPath, url);

        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl)) {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError(request.error);
            } 
            else {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                byte[] imageBytes = texture.EncodeToJPG();
                Directory.CreateDirectory(Path.GetDirectoryName(imagePath));
                File.WriteAllBytes(imagePath, imageBytes);
            }
        }
    }

    public bool HasLoadingFinished(){
        return filesDownloaded;
    }
}