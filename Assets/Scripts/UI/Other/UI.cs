using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using DG.Tweening;
using JetBrains.Annotations;
using System;


public class UI : MonoBehaviour
{

    [SerializeField] GameObject sidePanel;
    [SerializeField] MoveCamera cam;
    [SerializeField] CameraControler cameraControler;
    [SerializeField] Image buttonSeat;
    [SerializeField] Image buttonCar;
    [SerializeField] Image fadePanel;
    [SerializeField] float animationSpeed;

    GameObject activePanel;
    float panelWidth;
    Image activeButton;
    int nextLanguageID = 0;
    bool panelMoving = false;
    bool isInside = false;
    void Start(){
        activeButton = buttonCar;
        sidePanel.SetActive(false);
        panelWidth = sidePanel.GetComponent<RectTransform>().rect.width;
    }

    public void SwapButton(Image target){
        if(panelMoving){
            return;
        }
        activeButton.color = Color.clear;
        activeButton = target;
        activeButton.color = Color.white;
        if(activeButton != buttonCar){
            buttonSeat.color = Color.clear;
            cam.AllowMovement(false);
        }
        else{
            buttonSeat.color = Color.white;
            cam.AllowMovement(true);
        }
    }

    public IEnumerator SetLocale(){
        if(nextLanguageID != LocalizationSettings.AvailableLocales.Locales.Count-1){
            nextLanguageID++;
        }
        else{
            nextLanguageID = 0;
        }
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[nextLanguageID];
        
    }
    public void SetLocaleWrap(){
        StartCoroutine(SetLocale());
    }

    public void NewPanel(GameObject targetPanel){ 
         if (targetPanel != activePanel && !panelMoving) {
            panelMoving = true;
            RectTransform sidePanelRect = sidePanel.GetComponent<RectTransform>();

            if (activePanel != null) {
                Vector2 outsidePosition = sidePanelRect.anchoredPosition + new Vector2(panelWidth * 1.2f, 0);
                sidePanelRect.DOAnchorPos(outsidePosition, animationSpeed).SetEase(Ease.InOutQuad).OnComplete(() => {
                    activePanel.SetActive(false);
                    activePanel = targetPanel;
                    if(targetPanel != null){
                        activePanel.SetActive(true);
                    }
                    Vector2 insidePosition = sidePanelRect.anchoredPosition - new Vector2(panelWidth * 1.2f, 0);
                    sidePanelRect.DOAnchorPos(insidePosition, animationSpeed).SetEase(Ease.InOutQuad).OnComplete(() => {
                        panelMoving = false;
                    });
            });
        } else {
            sidePanelRect.anchoredPosition += new Vector2(panelWidth * 1.2f, 0);
            sidePanel.SetActive(true);

            activePanel = targetPanel;
            activePanel.SetActive(true);

            Vector2 insidePosition = sidePanelRect.anchoredPosition - new Vector2(panelWidth * 1.2f, 0);
            sidePanelRect.DOAnchorPos(insidePosition, animationSpeed).SetEase(Ease.InOutQuad).OnComplete(() => {
                panelMoving = false;
            });
        }
    }
    
    }
    public void HidePanel(){
        if(!panelMoving && activePanel != null){
        panelMoving = true;
        RectTransform sidePanelRect = sidePanel.GetComponent<RectTransform>();
        Vector2 outsidePosition = sidePanelRect.anchoredPosition + new Vector2(panelWidth * 1.2f, 0);
        sidePanelRect.DOAnchorPos(outsidePosition, animationSpeed).SetEase(Ease.InOutQuad).OnComplete(() => {
            activePanel.SetActive(false);
            activePanel = null;
            sidePanel.SetActive(false);
            panelMoving = false;
            Vector2 insidePosition = sidePanelRect.anchoredPosition - new Vector2(panelWidth * 1.2f, 0);
            sidePanelRect.anchoredPosition = insidePosition;

            });
        }
    }
    public void FadeInPanel(GameObject targetPanel){
        StartCoroutine(FadeInAsync(targetPanel));
    }
    public IEnumerator FadeInAsync(GameObject targetPanel){
        if (!panelMoving) {
            panelMoving = true;
            targetPanel.SetActive(true);
            //targetPanel.DOFade(1f, 0.5f).OnComplete( ()=>{panelMoving = false;});
            panelMoving = false;
        }
        yield return null;
    }
    public void FadeOutPanel(GameObject targetPanel){
            targetPanel.SetActive(false);
        
    }
     public IEnumerator FadePanelAsync(float fadeTime, Action onComplete){
        fadePanel.DOFade(1f, fadeTime).OnComplete( ()=>{
            onComplete.Invoke();
            fadePanel.DOFade(0f, fadeTime);
        });
        yield return null;
    }

    public void OffsetCamera(){
        if (!isInside){
            if (!cameraControler.GetCamera(CameraState.FollowCamera).isActiveAndEnabled){
                StartCoroutine(FadePanelAsync(0.6f, () => {cameraControler.HardSwapCamera(CameraState.FollowCamera);}));
            }
        }
        else{
             if (!cameraControler.GetCamera(CameraState.SideCamera).isActiveAndEnabled){
                StartCoroutine(FadePanelAsync(0.6f, () => {cameraControler.HardSwapCamera(CameraState.SideCamera);}));
             }
        }
    }
    public void MainCamera(){
        if(!isInside){
            if (!cameraControler.GetCamera(CameraState.MainCamera).isActiveAndEnabled){
                StartCoroutine(FadePanelAsync(0.6f, () => {cameraControler.HardSwapCamera(CameraState.MainCamera);}));
             }
        }
        else{
            if (!cameraControler.GetCamera(CameraState.InsideCamera).isActiveAndEnabled){
                StartCoroutine(FadePanelAsync(0.6f, () => {cameraControler.HardSwapCamera(CameraState.InsideCamera);}));
             }
        }
    }
    public void IsInside(bool change){
        isInside = change;
    }
}
