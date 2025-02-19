using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurationController : MonoBehaviour{
    
    [SerializeField] string file;
    [SerializeField] TextMeshProUGUI descText;
    string path;
    List<CarConfig> carConfigs = new List<CarConfig>();

    void Start(){
        path = Path.Combine(Application.dataPath, "Data/", file);
        LoadData(path);
    }
    public void LoadData(string path){
        string fileContent;

        if(File.Exists(path)){
            fileContent = File.ReadAllText(path);
        }
        else{
            Debug.Log("file not found");
            return; // dodac error popup
        }
        carConfigs = ParseInput(fileContent);
        descText.text = carConfigs[0].desc;
    }

    List<CarConfig> ParseInput(string input){
        List<CarConfig> carConfigs = new List<CarConfig>();
        var sections = input.Split(new[] { "Version=" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var section in sections){
            var lines = section.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var version = lines[0].Trim();
            var desc = lines[1].Split(new[] { "Desc=" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            var drive = lines[2].Split(new[] { "Drive=" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim().Split(',').Select(d => d.Trim()).ToList();
            var color = lines[3].Split(new[] { "Color=" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim().Split(',').Select(int.Parse).ToList();
            var rims = lines[4].Split(new[] { "Rims=" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim().Split(',').Select(int.Parse).ToList();
            var packs = lines[5].Split(new[] { "Packs=" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim().Split(',').Select(p => p.Trim()).ToList();

            carConfigs.Add(new CarConfig(version, desc, drive, color, rims, packs));
        }

        return carConfigs;
    }

    public void LoadVersionPanel(string chosenVersion){
        CarConfig chosenConfig = carConfigs.FirstOrDefault(c => c.version == chosenVersion);
        descText.text = chosenConfig.desc;
    }
}
