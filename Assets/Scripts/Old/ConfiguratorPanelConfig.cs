using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Build.Pipeline;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.UI;

public class ConfiguratorPanelConfig : MonoBehaviour
{

    [SerializeField] ConfigurationControler controler;

    [SerializeField] RectTransform scrollView;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject headerPrefab;
    [SerializeField] GameObject colorButtonPrefab;
    [SerializeField] List<string> drives;
    [SerializeField] List<Material> mainMaterials;
    [SerializeField] List<Material> rimsMaterials;
    [SerializeField] List<string> packages;
    [SerializeField] TMP_FontAsset buttonFont;
    [SerializeField] float baseButtonDistance;
    [SerializeField] int headerFontSize;

    float topDistance = 0;
    float leftDistance = 30;
    float scrollSize;

    List<Button> materialButtons;

    TextMeshPro displayText;
    GameObject configPanel;

    [ContextMenu("Create UI Components")]
    public void InspectorButton()
    {
        CreateUIComponents();
    }

    void CreateUIComponents(){
        Button thisButton = gameObject.GetComponent<Button>();
        configPanel = SetupMainObject();

        GenerateUI();

        scrollView.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, -topDistance); //powtarzac po zmianie widoku w kontrolerze
        scrollView.anchoredPosition = new Vector2(0, 0);
        thisButton.onClick.AddListener(() => scrollView.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, -topDistance));
        //thisButton.onClick.AddListener(() => controler.SwapPanels(configPanel));
    }

    void SetupTextButtonHorizontaly(string text){
        if (text == null) return;
        leftDistance = 30;

        GameObject newButton = Instantiate(buttonPrefab, configPanel.transform);
        RectTransform buttonRectTransform = newButton.GetComponent<RectTransform>();
        buttonRectTransform.anchoredPosition = new Vector3(0, topDistance);
        
        topDistance -= buttonRectTransform.rect.height + baseButtonDistance;
        newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text; 

    }
    void SetupColorsButtonVerticaly(List<Material> materials, Action<Material> designatedFunction){
        leftDistance = 30;
        foreach (Material carMaterial in materials){
            GameObject newButton = Instantiate(colorButtonPrefab, configPanel.transform);
            RectTransform buttonRectTransform = newButton.GetComponent<RectTransform>();
            buttonRectTransform.anchoredPosition = new Vector3(leftDistance, topDistance);
            leftDistance += buttonRectTransform.rect.width + baseButtonDistance;

            Image buttonImage = newButton.GetComponent<Image>();
            buttonImage.color = carMaterial.GetColor("_Color");
            newButton.GetComponent<Button>().onClick.AddListener(() => designatedFunction(carMaterial));
        }
        topDistance -= colorButtonPrefab.GetComponent<RectTransform>().rect.height + baseButtonDistance;
    }


    void SetupHeaderText(string text){
        GameObject newHeader = Instantiate(headerPrefab, configPanel.transform);
        RectTransform headerTransform = newHeader.GetComponent<RectTransform>();
        TextMeshProUGUI headerText = newHeader.GetComponent<TextMeshProUGUI>();
        
        headerText.text = text;
        headerTransform.anchoredPosition = new Vector3(0, topDistance);

        topDistance -= headerTransform.rect.height + baseButtonDistance;;

        }
    GameObject SetupMainObject(){
        GameObject newPanel = new GameObject();
        RectTransform newPanelTransform = newPanel.AddComponent<RectTransform>();
        newPanelTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scrollView.rect.width);
        newPanelTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scrollView.rect.height);
        newPanelTransform.SetParent(scrollView,false);
        newPanelTransform.anchorMin = new Vector2(0, 1);
        newPanelTransform.anchorMax = new Vector2(0, 1);
        newPanelTransform.pivot = new Vector2(0, 1);
        return newPanel;
    }

    void GenerateUI(){
        SetupHeaderText("Drive");
        foreach (string drive in drives){
            SetupTextButtonHorizontaly(drive);
        }
        SetupHeaderText("Color");
        SetupColorsButtonVerticaly(mainMaterials, controler.ChangeCarColor);
        SetupHeaderText("Rims");
        SetupColorsButtonVerticaly(rimsMaterials, controler.ChangeRimsColor);
        SetupHeaderText("Packages");
        foreach (string package in packages){
            SetupTextButtonHorizontaly(package);
        }

        configPanel.SetActive(false);
    }
}
