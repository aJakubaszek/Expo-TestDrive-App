using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] CarLoader loader;
    [SerializeField] int id;
    [SerializeField] ImageDownloadManager imageDownloader;


    void Start(){
        StartCoroutine(LoadSceneAsync(id));
        GameObject loadingManager = GameObject.Find("SceneManager");
    }
   
    IEnumerator LoadSceneAsync(int sceneId){
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        operation.allowSceneActivation = false;

        while (!operation.isDone){
            float progressValue = Mathf.Clamp01(operation.progress);
            if(loader.UpdateLoader(progressValue) && imageDownloader.HasLoadingFinished()){
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

}

