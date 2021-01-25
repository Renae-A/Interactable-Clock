using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    private TextMeshProUGUI numberText;
    private GameObject digital_Period;
    private TextMeshProUGUI periodText;

    private GameObject hideableSettings;        // The Hideable Settings child object of Clock

    private GameObject mode;
    private TMP_Dropdown modeDropdown;

    private GameObject timeFormat;
    private TMP_Dropdown timeDropdown;
    private GameObject timerStopwatchFormat;
    private TMP_Dropdown timerStopwatchDropdown;

    private GameObject setValues;
    private GameObject timeHourValue;
    private GameObject timerStopwatchHourValue;
    private GameObject minuteValue;
    private GameObject secondValue;
    private GameObject periodValue;

    private GameObject startAndStop;
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
                    timeFormat = t.gameObject;
                    break;
                case "Timer Format":
                    timerStopwatchFormat = t.gameObject;
                    break;
                case "Set Values":
                    setValues = t.gameObject;
                    break;
                case "Time Hour Value":
                    timeHourValue = t.gameObject;
                    break;
                case "Timer Hour Value":
                    timerStopwatchHourValue = t.gameObject;
                    break;
                case "Minute Value":
                    minuteValue = t.gameObject;
                    break;
                case "Second Value":
                    secondValue = t.gameObject;
                    break;
                case "Period Value":
                    periodValue = t.gameObject;
                    break;
                case "Stopwatch Settings":
                    stopwatchSettings = t.gameObject;
                    break;
                case "Start and Stop":
                    startAndStop = t.gameObject;
                    break;
            }
        }

        modeDropdown = mode.GetComponent<TMP_Dropdown>();
        timeDropdown = timeFormat.GetComponent<TMP_Dropdown>();
        timerStopwatchDropdown = timerStopwatchFormat.GetComponent<TMP_Dropdown>();
        numberText = digital_Numbers.GetComponent<TextMeshProUGUI>();
        periodText = digital_Period.GetComponent<TextMeshProUGUI>();
    }


    // Start is called before the first frame update
    void Start()
    {
        analog_TimerStopwatch.SetActive(false);
        digital.SetActive(false);
        hideableSettings.SetActive(false);
        timerStopwatchFormat.SetActive(false);
        stopwatchSettings.SetActive(false);
        timerStopwatchHourValue.SetActive(false);
        periodValue.SetActive(false);
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

    //	Changes whether the clock is a time display, countdown timer or stopwatch (hides/reveals appropriate objects).
    public void SetMode()
    {
        switch (modeDropdown.captionText.text)
        {
            case "Time Display":
                // Turn off Timer and Stopwatch objects
                analog_TimerStopwatch.SetActive(false);
                timerStopwatchFormat.SetActive(false);
                stopwatchSettings.SetActive(false);
                timerStopwatchHourValue.SetActive(false);

                // Turn on Time Display objects
                analog_Time.SetActive(true);
                timeFormat.SetActive(true);
                timeHourValue.SetActive(true);
                setValues.SetActive(true);

                SetTimeFormat();
                break;
            case "Timer":
                // Turn off Time Display and Stopwatch objects
                analog_Time.SetActive(false);
                timeFormat.SetActive(false);
                stopwatchSettings.SetActive(false);
                periodValue.SetActive(false);
                timeHourValue.SetActive(false);

                // Turn on Timer Display objects
                analog_TimerStopwatch.SetActive(true);
                timerStopwatchFormat.SetActive(true);
                timerStopwatchHourValue.SetActive(true);
                setValues.SetActive(true);

                SetTimerStopwatchFormat();
                break;
            case "Stopwatch":
                // Turn off Time Display and Timer objects
                analog_Time.SetActive(false);
                timeFormat.SetActive(false);
                periodValue.SetActive(false);
                timeHourValue.SetActive(false);
                setValues.SetActive(false);

                // Turn on Stopwatch objects
                analog_TimerStopwatch.SetActive(true);
                timerStopwatchFormat.SetActive(true);
                stopwatchSettings.SetActive(true);
                timerStopwatchHourValue.SetActive(true);

                SetTimerStopwatchFormat();
                break;
        }     
    }

    //	Changes whether the clock is analog or digital in a variety of time formats (hides/reveals appropriate objects).
    public void SetTimeFormat()
    {
        switch (timeDropdown.captionText.text)
        {
            case "Analog":
                digital.SetActive(false);
                analog.SetActive(true);
                break;
            case "hh:mm (24hr)":
                analog.SetActive(false);
                digital_Period.SetActive(false);
                digital.SetActive(true);
                numberText.text = "hh:mm";
                break;
            case "hh:mm (12hr)":
                analog.SetActive(false);
                digital_Period.SetActive(true);
                digital.SetActive(true);
                numberText.text = "hh:mm";
                break;
            case "hh:mm:ss (24hr)":
                analog.SetActive(false);
                digital_Period.SetActive(false);
                digital.SetActive(true);
                numberText.text = "hh:mm:ss";
                break;
            case "hh:mm:ss (12hr)":
                analog.SetActive(false);
                digital_Period.SetActive(true);
                digital.SetActive(true);
                numberText.text = "hh:mm:ss";
                break;
        }
    }

    //	Changes whether the clock is analog or digital (hides/reveals appropriate objects).
    public void SetTimerStopwatchFormat()
    {
        switch (timerStopwatchDropdown.captionText.text)
        {
            case "Analog":
                digital.SetActive(false);
                analog.SetActive(true);
                break;
            case "Digital":
                analog.SetActive(false);
                digital_Period.SetActive(false);
                digital.SetActive(true);

                if (modeDropdown.captionText.text == "Timer")
                    numberText.text = "hh:mm:ss";

                else if (modeDropdown.captionText.text == "Stopwatch")
                    numberText.text = "hh:mm:ss.SS";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    { 
        
    }
}
