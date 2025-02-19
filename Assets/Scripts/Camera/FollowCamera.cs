using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform mainCameraTransform;
    Vector3 newPosition;
    void Update(){
        newPosition = mainCameraTransform.right + mainCameraTransform.position;
        gameObject.transform.rotation = mainCameraTransform.rotation;
        gameObject.transform.position = newPosition;
    }
}
