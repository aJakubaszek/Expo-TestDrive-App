using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class TestDriveSchedule : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dateDropdown;
    [SerializeField] TextMeshProUGUI chosenDate;
    [SerializeField] TMP_Dropdown timeDropdown;
    [SerializeField] TextMeshProUGUI chosenTime;
    [SerializeField] TMP_InputField chosenName;
    [SerializeField] TestDriveManager testDriveManager;
    [SerializeField] RectTransform drivesListTransform;
    [SerializeField] GameObject upcomingDrivesPrefab;

    string path;
    List<GameObject> loadedDrives;
    List<TestDrive> testDrivesList;
    DateTime startDate;
    DateTime endDate;
    DateTime startTime;
    DateTime endTime;
    void Start(){
        loadedDrives = new List<GameObject>();
        path = Path.Combine(Application.dataPath, "Data/EventData.txt");
        LoadData(); 
        Debug.Log(startDate);
        Debug.Log(endTime);
        Debug.Log(endDate);
        LoadDates();
    }
    public void RefreshDrives(){
        foreach (GameObject drive in loadedDrives){
            Destroy(drive);
        }
        StartCoroutine(testDriveManager.GetAppointments(chosenDate.text, appointments =>{
            if (appointments != null){
                foreach (var drive in appointments){
                    GameObject newDrive = GameObject.Instantiate(upcomingDrivesPrefab, drivesListTransform);
                    string croppedDate = drive.date.Substring(0, drive.date.Length - 9);
                    newDrive.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{croppedDate} {drive.hour} | {drive.name}";
                    loadedDrives.Add(newDrive);
                }
            }
            else{
                Debug.LogError("Drives list is empty.");
            }
        }));
    }
    bool LoadData(){
         if (File.Exists(path)){
            string[] lines = File.ReadAllLines(path);

            foreach (string line in lines){
                string[] keyValue = line.Split('=');
                switch (keyValue[0]){
                    case "start_date":
                        startDate = DateTime.Parse(keyValue[1]);
                        break;
                    case "end_date":
                        endDate = DateTime.Parse(keyValue[1]);
                        break;
                    case "start_time":
                        startTime = DateTime.Parse(keyValue[1]);
                        break;
                    case "end_time":
                        endTime = DateTime.Parse(keyValue[1]);
                        break;
                }
            }
            return true;
        }
        else{
            Debug.LogError("EventData.txt file not found");
            return false;
        }
    }

    void LoadDates(){
        dateDropdown.ClearOptions();
        DateTime now = DateTime.Now;
        if(now.Date <= endDate){
            DateTime nextDate = now;
            while(nextDate < startDate){
                nextDate = nextDate.AddDays(1);
            }
            List<string> options = new List<string>();
            while(nextDate.Date <= endDate.Date){

                options.Add(nextDate.ToString("yyyy-MM-dd"));
                nextDate = nextDate.AddDays(1);
            }
            dateDropdown.AddOptions(options);
        }
    }
    public void ReloadTime(){
        timeDropdown.ClearOptions();
        DateTime now = DateTime.Now;
        DateTime pickedDate = DateTime.Parse(chosenDate.text);
        
        List<String> availableHours = new List<string>();
        DateTime loopTime = startTime;

        if(pickedDate.Date == now.Date){
            loopTime = now;
        }
        if(loopTime.Minute > 0 && loopTime.Minute <= 30){
            int missingTime = 30 - loopTime.Minute;
            loopTime = loopTime.AddMinutes(missingTime);
            Debug.Log("Poprawka minut");
        }
        else if(loopTime.Minute > 30){
           int missingTime = 60 - loopTime.Minute;
           loopTime = loopTime.AddMinutes(missingTime);
           Debug.Log("Poprawka minut2");
        }
        while(loopTime.TimeOfDay <= endTime.TimeOfDay){
            availableHours.Add(loopTime.ToString("H:mm"));
            loopTime = loopTime.AddMinutes(30);
        }
        timeDropdown.AddOptions(availableHours);
    }

    public async void SendNewDrive(){

            TestDrive appointment = new TestDrive{
                name = chosenName.text,
                date = chosenDate.text,
                hour = chosenTime.text
            };
            StartCoroutine(testDriveManager.AddAppointment(appointment));

            chosenName.text = "";
    }

    
}
