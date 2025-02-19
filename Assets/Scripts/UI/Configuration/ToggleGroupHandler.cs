using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ToggleGroupHandler : MonoBehaviour{
    void Start(){
        UpdateChildImageColors();
    }

    public void UpdateChildImageColors(){
        foreach (Transform child in transform){
            Image childImage = child.GetComponent<Image>();

            Toggle childToggle = child.GetComponent<Toggle>();

            if (childImage != null && childToggle != null){
                if (childToggle.isOn){
                    childImage.color = Color.grey;
                }
                else{
                    childImage.color = Color.white;
                }
            }
            else if (childImage != null){
                childImage.color = Color.white;
            }
        }
    }
}
