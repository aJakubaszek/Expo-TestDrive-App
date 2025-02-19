using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLoader : MonoBehaviour{
    public UILineRenderer lineRenderer;
    List<Vector2> linePoints;
    float loaderMoveDistance = 80;
    float moveTime = 0.1f;
    int loaderProgress;
    bool isAnimating = false;
    bool isLoadingFinished = false;
    

    Transform loaderTransform;
    
    void Start(){
        linePoints = new List<Vector2>();
        loaderTransform = gameObject.transform;
        linePoints.Add(loaderTransform.position);
        loaderProgress = 0;
    }
    
    public bool UpdateLoader(float progress) {
        if (isAnimating || ((5 * loaderProgress) / 100f >= progress)){
            return isLoadingFinished;
        }
        

        loaderProgress++;

        Vector3 moveDirection = (loaderProgress % 2 == 1) ? new Vector3(loaderMoveDistance, loaderMoveDistance, 0) : new Vector3(loaderMoveDistance, -loaderMoveDistance, 0);
        Quaternion targetRotation = (loaderProgress % 2 == 1) ? Quaternion.Euler(0, 0, 45) : Quaternion.Euler(0, 0, -45);

        isAnimating = true;

        loaderTransform.rotation = targetRotation;
        loaderTransform.DOMove(loaderTransform.position + moveDirection, moveTime).SetEase(Ease.Linear).OnComplete(() => {
            //linePoints.Add(loaderTransform.position);
           // lineRenderer.points = linePoints.ToArray();
            //lineRenderer.SetAllDirty();
            
            isAnimating = false;
        });
        if(loaderProgress >= 18){
            isLoadingFinished = true;
        }
        else{
            isLoadingFinished = false;
        }
        return isLoadingFinished;
    }

}
