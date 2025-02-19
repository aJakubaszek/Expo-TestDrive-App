using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurationControler : MonoBehaviour
{
    GameObject activePanel;
    [SerializeField] GameObject car;
    [SerializeField] List<GameObject> rims;
    [SerializeField] GameObject seams;

    GameObject panel;
    string drive;
    string package;
    bool isColorMatch = false;



    public void ChangeCarColor(Material material){ 
        MeshRenderer carRenderer = car.GetComponent<MeshRenderer>();
        carRenderer.material = material;
        if(isColorMatch){
            ColorMatch();
        }
    }

    public void ChangeCarSeams(Material material){
        MeshRenderer seamsRenderer = seams.GetComponent<MeshRenderer>();
        seamsRenderer.material = material;
    }

    public void ChangeRimsColor(Material material){
        foreach(GameObject rim in rims){
            MeshRenderer rimRenderer = rim.GetComponent<MeshRenderer>();
            rimRenderer.material = material;
        }
        isColorMatch = false;
    }

    public void ChangeDrive(string newDrive){
        drive = newDrive;
    }

    public void ChangePackage(string newPackage){
        package = newPackage;
    }

    public void ColorMatch(){
        MeshRenderer carRenderer = car.GetComponent<MeshRenderer>();
        foreach(GameObject rim in rims){
            MeshRenderer rimRenderer = rim.GetComponent<MeshRenderer>();
            rimRenderer.material = carRenderer.material;
        }
        isColorMatch = true;
    }

    public void SwapPanel(GameObject newPanel){
        if(panel != null){
        panel.SetActive(false);
        }
        panel = newPanel;
        panel.SetActive(true);
    }
}
