using System;
using System.Collections;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] CameraControler cameraControler;
    [SerializeField] UI ui;
    [SerializeField] Transform car;
    [SerializeField] float sensitivity = 1.0f;
    [SerializeField] float zoom = 3.0f;
    [SerializeField] float autoCameraSpeed = 3.0f;
    [SerializeField] float doorPosition = 115.0f;
    [SerializeField] float inertiaDamping = 0.95f;
    [SerializeField] float minVelocity = 0.01f;
    [SerializeField] int afkTime = 20;

    float rotationY;
    float rotationX;
    float rotationVelocityY;
    float rotationVelocityX;
    bool canRotate = true;

    Action platformDependentUpdate;

    int afkCounter;
    bool isAfk;
    Coroutine afkCoroutine;

    void Awake(){
        rotationY = transform.localEulerAngles.y;
        rotationX = transform.localEulerAngles.x;
        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
        transform.position = car.position - transform.forward * zoom;
        canRotate = true;

        

        #if UNITY_STANDALONE
            platformDependentUpdate = UpdatePC;
        #elif UNITY_IOS
            platformDependentUpdate = UpdateIOS;
        #endif


    }
    void OnEnable() {
        afkCoroutine = StartCoroutine(AfkCounter());
    }

    void OnDisable(){
        isAfk = false;
        if(afkCoroutine != null){
            StopCoroutine(afkCoroutine);
        }
    }

    void Update(){
        if (isAfk){
            rotationVelocityY = -0.02f;
        }
        platformDependentUpdate?.Invoke();

         UpdateCameraPosition();
    }

    void UpdatePC(){
        if (canRotate){
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (Input.GetMouseButton(0)){
                ResetCounter();
                MouseRotation();
            }
            else{
                ApplyInertia();
            }

            if (scrollInput != 0f){
                MouseZoom(scrollInput);
            }

           
        }
    }

    void UpdateIOS(){
        if (canRotate){
            if (Input.touchCount > 0){
                ResetCounter();
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved){
                    TouchRotation(touch);
                }
            }
            else{
                ApplyInertia();
            }
            


        }
    }

    void TouchRotation(Touch touch){
        Vector2 delta = touch.deltaPosition;
        rotationVelocityY = delta.x * sensitivity/20;
        rotationVelocityX = delta.y * sensitivity/20;

        rotationY += rotationVelocityY;
        rotationX += rotationVelocityX;
        rotationX = Mathf.Clamp(rotationX, -0, 75);
    }
    void MouseRotation(){
        float mouseXInput = Input.GetAxis("Mouse X") * sensitivity;
        float mouseYInput = Input.GetAxis("Mouse Y") * sensitivity;

        rotationVelocityY = mouseXInput;
        rotationVelocityX = mouseYInput;

        rotationY += rotationVelocityY;
        rotationX += rotationVelocityX;
        rotationX = Mathf.Clamp(rotationX, -0, 75);
    }

    void ApplyInertia(){
        if (Mathf.Abs(rotationVelocityY) > minVelocity || Mathf.Abs(rotationVelocityX) > minVelocity){
            rotationY += rotationVelocityY;
            rotationX += rotationVelocityX;
            rotationX = Mathf.Clamp(rotationX, -0, 75);

            rotationVelocityY *= inertiaDamping;
            rotationVelocityX *= inertiaDamping;
        }
        else{
            rotationVelocityY = 0;
            rotationVelocityX = 0;
        }
    }

    void MouseZoom(float input){
        zoom -= input;
        zoom = Mathf.Clamp(zoom, 3, 6);
    }

    void UpdateCameraPosition(){
        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
        transform.position = car.position - transform.forward * zoom;
    }

    public void CarButton(){
        if (canRotate){
            if (transform.localEulerAngles.y > 180){
                StartCoroutine(FindDoor(360 - doorPosition));
            }
            else{
                StartCoroutine(FindDoor(doorPosition));
            }
        }
    }

    IEnumerator FindDoor(float targetAngle){

        canRotate = false;
        float startAngle = transform.localEulerAngles.y;
        float elapsedTime = 0;
        float duration = Mathf.Abs(Mathf.DeltaAngle(startAngle, targetAngle)) / autoCameraSpeed;

        while (elapsedTime < duration){
            rotationY = Mathf.LerpAngle(startAngle, targetAngle, elapsedTime / duration);
            UpdateCameraPosition();
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        rotationY = targetAngle;
        UpdateCameraPosition();
        StartCoroutine(ui.FadePanelAsync(0.75f, () =>{
            rotationY = startAngle;
            ui.IsInside(true);
            canRotate = true;
            cameraControler.HardSwapCamera(CameraState.InsideCamera);
        }));
    }

    public void AllowMovement(bool canMove){
        canRotate = canMove;
    }
    public void OffsetCamera(){
        if(!cameraControler.GetCamera(CameraState.SideCamera).isActiveAndEnabled){
            cameraControler.HardSwapCamera(CameraState.SideCamera);
        }
    }

    void ResetCounter(){
        afkCounter = 0;
        isAfk = false;
    }

    IEnumerator AfkCounter(){
        while(true){
            afkCounter++;
            if (afkCounter > afkTime){
                isAfk = true;
            }
            yield return new WaitForSeconds(1f);
        }
    }
}