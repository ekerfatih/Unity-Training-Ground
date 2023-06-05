using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class World : MonoBehaviour {

    private static World _instance;
    public static World Instance { get { return _instance; } }

    public int dayStartTime = 240; // 4am
    public int dayEndTime = 1360; // 10 pm  22 * 60 = 1360
    private int dayLength { get { return dayEndTime - dayStartTime; } }
    private float sunDayRotatationPerMinunte { get { return 180f / dayLength; } }
    private float sunNightRotatationPerMinunte { get { return 180f / (1440 - dayLength); } }

    public Transform dirlight;
    public TMP_Text clockText;
    public int Day = 1;
    [Range(4,0)]
    public float clockSpeed;
    public int HordeNightFrequency = 7;
    public bool IsHordeNight {
        get {
            if(Day % HordeNightFrequency == 0) {
                return true;
            }
            return false;
        }
    }
    [SerializeField] private int _timeOfDay;
    public int TimeOfDay {
        get { return _timeOfDay; }
        set {
            _timeOfDay = value;
            // 1440 minutes in a day
            if (_timeOfDay > 1439) {
                Day++;
                _timeOfDay = 0;
            }
            UpdateClock();

            float rotAmount;
            if (_timeOfDay > dayStartTime && _timeOfDay < dayEndTime) {
                rotAmount = (_timeOfDay - dayStartTime) * sunDayRotatationPerMinunte;
                Debug.Log("1");
            }
            else if (_timeOfDay >= dayEndTime) {
                rotAmount = dayLength * sunDayRotatationPerMinunte;
                rotAmount += ((_timeOfDay - dayStartTime - dayLength) * sunNightRotatationPerMinunte);
                Debug.Log("2");
            }
            else {
                rotAmount = dayLength * sunDayRotatationPerMinunte;
                rotAmount += (1440 - dayEndTime) * sunNightRotatationPerMinunte;
                rotAmount += _timeOfDay * sunNightRotatationPerMinunte;
                Debug.Log("3");
            }
            
            

            dirlight.eulerAngles = new Vector3( rotAmount, 0f, 0f);
        }
    }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Debug.LogError("Removing additional");
        }
        else {
            _instance = this;
        }
    }

    private void UpdateClock() {
        int hours = TimeOfDay / 60;
        int minutes = TimeOfDay - (hours * 60);
        string dayText;
        if (IsHordeNight)
            dayText = string.Format("<color=red>{0}</color>", Day.ToString());
        else
            dayText = Day.ToString();
        clockText.text = string.Format("DAY : {0} TIME : {1}:{2}",dayText, hours.ToString("D2"),minutes.ToString("D2"));
    }

    private float secondCounter = 0f;

    private void Update() {
        secondCounter += Time.deltaTime;
        if(secondCounter > clockSpeed) {
            TimeOfDay++;
            secondCounter -= clockSpeed;
        }
    }

}