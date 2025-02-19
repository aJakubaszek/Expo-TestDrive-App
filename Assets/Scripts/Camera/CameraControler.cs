using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CameraState{
    MainCamera,
    SideCamera,
    InsideCamera,
    FollowCamera
}

public class CameraControler : MonoBehaviour{
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera sideCamera;
    [SerializeField] Camera insideCamera;
    [SerializeField] Camera followCamera;

    Camera currentCamera;

    void Start(){
        currentCamera = mainCamera;
    }

    public void HardSwapCamera(CameraState newCamera){
        currentCamera.GameObject().SetActive(false);
        currentCamera = GetCamera(newCamera);
        currentCamera.GameObject().SetActive(true);
    }

    public void SoftSwapCamera(CameraState newCamera){
        currentCamera.enabled = false;
        currentCamera = GetCamera(newCamera);
        currentCamera.enabled = true;
    }

    public Camera GetCamera(CameraState chosenCamera){
        switch(chosenCamera){
            case CameraState.MainCamera:
                return mainCamera;
            case CameraState.SideCamera:
                return sideCamera;
            case CameraState.InsideCamera:
                return insideCamera;
            case CameraState.FollowCamera:
                return followCamera;
            default:
            return mainCamera;
        }
    }
}
