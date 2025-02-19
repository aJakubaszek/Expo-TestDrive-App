using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDriveTester : MonoBehaviour
{
    public TestDriveManager testDriveManager;

    private void Start()
    {
        if (testDriveManager == null)
        {
            Debug.LogError("TestDriveManager is not assigned.");
            return;
        }

        TestDrive appointment = new TestDrive
        {
            name = "Karol",
            date = DateTime.Now.ToString("yyyy-MM-dd"),
            hour = "17:00"
        };

        StartCoroutine(testDriveManager.AddAppointment(appointment));

        StartCoroutine(testDriveManager.GetAppointments(DateTime.Now.ToString("yyyy-MM-dd"), appointments =>
        {
            if (appointments != null)
            {
                foreach (var a in appointments)
                {
                    Debug.Log("Appointment: " + a.name + " at " + a.hour);
                }
            }
            else
            {
                Debug.LogError("Appointments list is null.");
            }
        }));

        StartCoroutine(testDriveManager.CheckAppointment(DateTime.Now.ToString("yyyy-MM-dd"), "16:00", exists =>
        {
            Debug.Log("Appointment exists: " + exists);
        }));
        StartCoroutine(testDriveManager.GetAppointments(DateTime.Now.ToString("yyyy-MM-dd"), appointments =>
        {
            // Iterate through appointments and print each one
            foreach (var appointment in appointments)
            {
                Debug.Log("Appointment: " + appointment.name + " on " + appointment.date + " at " + appointment.hour);
            }
        }));
    }
        
    
}

