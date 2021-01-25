using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    private ClockManager clockManager;

    private GameObject analog;                  // The Analog child object of Clock

    private GameObject analog_Time;              // The Analog numbers object for Time Display of Clock
    private GameObject analog_TimerStopwatch;    // The Analog numbers object for Timer and Stopwatch of Clock

    private GameObject analog_HourHand;
    private GameObject analog_MinuteHand;
    private GameObject analog_SecondHand;

    private GameObject digital;                 // The Digital child object of Clock

    private GameObject digital_Numbers;
    private GameObject digital_Period;

    private GameObject hideableSettings;        // The Hideable Settings child object of Clock

    private GameObject mode;

    private GameObject timeFormatDropdown;
    private GameObject timerStopwatchDropdown;

    private GameObject setValues;

    private GameObject stopwatchSettings;


    private void Awake()
    {
        clockManager = GetComponentInParent<ClockManager>();

        // Organises and sets up all variables of the clock (saving the user from having to pass them in manually)
        Transform[] childTransforms = GetComponentsInChildren<Transform>();

        foreach (Transform t in childTransforms)
        {
            switch (t.tag)
            {
                // ANALOG
                case "Analog":
                    analog = t.gameObject;
                    break;
                case "Analog Time":
                    analog_Time = t.gameObject;
                    break;
                case "Analog Timer":
                    analog_TimerStopwatch = t.gameObject;
                    break;
                case "Hour":
                    analog_HourHand = t.gameObject;
                    break;
                case "Minute":
                    analog_MinuteHand = t.gameObject;
                    break;
                case "Second":
                    analog_SecondHand = t.gameObject;
                    break;
                // DIGITAL
                case "Digital":
                    digital = t.gameObject;
                    break;
                case "Digital Numbers":
                    digital_Numbers = t.gameObject;
                    break;
                case "Digital Period":
                    digital_Period = t.gameObject;
                    break;
                // SETTINGS
                case "Hideable Settings":
                    hideableSettings = t.gameObject;
                    break;
                case "Mode":
                    mode = t.gameObject;
                    break;
                case "Time Format":
                    timeFormatDropdown = t.gameObject;
                    break;
                case "Timer Format":
                    timerStopwatchDropdown = t.gameObject;
                    break;
                case "Set Values":
                    setValues = t.gameObject;
                    break;
                case "Stopwatch Settings":
                    stopwatchSettings = t.gameObject;
                    break;
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        analog_TimerStopwatch.SetActive(false);
        digital.SetActive(false);
        hideableSettings.SetActive(false);
        timerStopwatchDropdown.SetActive(false);
        stopwatchSettings.SetActive(false);
    }

    // Calls the ClockManager’s RemoveClock function, passing in this clock as the parameter.
    public void RemoveClock()
    {
        clockManager.RemoveClock(gameObject);
    }

    // Either hides or shows the additional settings options on button click.
    public void ToggleSettings()
    {
        hideableSettings.SetActive(!hideableSettings.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
