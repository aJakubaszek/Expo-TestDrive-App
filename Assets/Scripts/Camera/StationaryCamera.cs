using System;
using UnityEngine;

public class StationaryCamera : MonoBehaviour
{
    [SerializeField] float sensitivity = 5.0f;
    [SerializeField] UI ui;
    [SerializeField] CameraControler cameraControler;

    float rotationY;
    float rotationX;
    Vector3 startingRotation;

    Action platformDependentUpdate;
    
    void Awake(){
        startingRotation = transform.localEulerAngles;
        rotationY = transform.localEulerAngles.y;
        rotationX = transform.localEulerAngles.x;
        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);

        #if UNITY_STANDALONE
            platformDependentUpdate = UpdatePC;
        #elif UNITY_IOS
            platformDependentUpdate = UpdateIOS;
        #endif
    }

    void OnDisable(){
        transform.localEulerAngles = startingRotation;
    }

    void Update(){
        platformDependentUpdate();
    }

    void UpdatePC(){
        if (Input.GetMouseButton(0)){
            MouseRotation();
        }
    }

    void UpdateIOS(){
        if (Input.touchCount > 0){
            TouchRotation();
        }
    }
    public void ChangeCamera(){
         StartCoroutine(ui.FadePanelAsync(0.75f, () =>{
            ui.IsInside(false);
            cameraControler.HardSwapCamera(CameraState.MainCamera);
        }));
    }
    void MouseRotation(){
        float mouseXInput = Input.GetAxis("Mouse X") * sensitivity;
        float mouseYInput = Input.GetAxis("Mouse Y") * sensitivity;

        rotationY += mouseXInput;
        rotationX -= mouseYInput;

        rotationX = Mathf.Clamp(rotationX, -90, 90);

        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
    }

    void TouchRotation(){
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Moved){
            float mouseXInput = touch.deltaPosition.x * sensitivity * 0.01f;
            float mouseYInput = touch.deltaPosition.y * sensitivity * 0.01f;

            rotationY += mouseXInput;
            rotationX -= mouseYInput;

            rotationX = Mathf.Clamp(rotationX, -90, 90);

            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
        }
    }
    
}