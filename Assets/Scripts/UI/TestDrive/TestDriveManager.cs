using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class TestDriveManager : MonoBehaviour
{
    private string apiUrl = "https://localhost:7035/api/Appointments";

    public IEnumerator AddAppointment(TestDrive appointment){
        string json = JsonUtility.ToJson(appointment);
        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST")){
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success){
                Debug.LogError("Error adding appointment: " + request.error);
            }
            else{
                Debug.Log("Appointment added successfully");
            }
        }
    }

public IEnumerator GetAppointments(string date, Action<List<TestDrive>> callback){
    using (UnityWebRequest request = UnityWebRequest.Get(apiUrl + "/" + date)){
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success){
            Debug.LogError("Error getting appointments: " + request.error);
        }
        else
        {
            string json = request.downloadHandler.text;

            try{
                List<TestDrive> appointments = JsonConvert.DeserializeObject<List<TestDrive>>(json);

                if (appointments != null){
                    callback(appointments);
                }
                else{
                    Debug.LogError("Failed to parse appointments JSON: List<TestDrive> is null.");
                }
            }
            catch (Exception e){
                Debug.LogError("Failed to parse appointments JSON: " + e.Message);
            }
        }
    }
}

    public IEnumerator CheckAppointment(string date, string hour, Action<bool> callback){
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl + "/check?date=" + date + "&hour=" + hour)){
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success){
                Debug.LogError("Error checking appointment: " + request.error);
                callback(false);
            }
            else{
                bool exists = bool.Parse(request.downloadHandler.text);
                callback(exists);
            }
        }
    }

    [Serializable]
    private class Wrapper<TestDrive>{
        public List<TestDrive> items;
    }
}