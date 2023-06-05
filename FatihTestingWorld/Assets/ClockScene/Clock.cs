using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour {

    [SerializeField] private Transform hoursPivot, minutesPivot, secondsPivot;
    private const float HoursToDegrees = -30, MinutesToDegrees = -6f, SecondsToDegrees = -6f;

    // private void Update() {
    //     DateTime time = DateTime.Now;
    //     hoursPivot.localRotation = 
    //         Quaternion.Euler(0, 0, HoursToDegrees * time.Hour);
    //     minutesPivot.localRotation =
    //         Quaternion.Euler(0f, 0f, MinutesToDegrees * time.Minute);
    //     secondsPivot.localRotation =
    //         Quaternion.Euler(0f, 0f, SecondsToDegrees * time.Second);
    // }
    
    void Update () {
        TimeSpan time = DateTime.Now.TimeOfDay;
        hoursPivot.localRotation =
            Quaternion.Euler(0f, 0f, HoursToDegrees * (float)time.TotalHours);
        minutesPivot.localRotation =
            Quaternion.Euler(0f, 0f, MinutesToDegrees * (float)time.TotalMinutes);
        secondsPivot.localRotation =
            Quaternion.Euler(0f, 0f, SecondsToDegrees * (float)time.TotalSeconds);
    }


}