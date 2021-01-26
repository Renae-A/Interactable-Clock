using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum TimePeriod
{
    AM,
    PM
}

public class Clock : MonoBehaviour
{
    private ClockManager clockManager;

    // GameObjects that will be have their active state changed based on mode and format values

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

    private GameObject timeFormat;
    private GameObject timerStopwatchFormat;

    private GameObject setValues;
    private GameObject timeHour12;
    private GameObject timeHour24;
    private GameObject timerStopwatchHourValue;
    private GameObject minuteValue;
    private GameObject secondValue;
    private GameObject periodValue;

    private GameObject startAndStop;
    private GameObject stopwatchSettings;

    // Important types with values that incur events and changes within the application
    private TMP_Dropdown modeDropdown;
    private TMP_Dropdown timeDropdown;
    private TMP_Dropdown timerStopwatchDropdown;

    private TMP_Dropdown timeHour12Dropdown;
    private TMP_Dropdown timeHour24Dropdown;
    private TMP_Dropdown timerStopwatchHourDropdown;
    private TMP_Dropdown minuteDropdown;
    private TMP_Dropdown secondDropdown;
    private TMP_Dropdown periodDropdown;

    private TextMeshProUGUI numberText;
    private RectTransform numberTransform;
    private TextMeshProUGUI periodText;

    private float time;
    private float timer;
    private float stopwatch;

    private bool timeRunning;
    private bool timerRunning;
    private bool stopwatchRunning;

    private int shortWidth;
    private int mediumWidth;
    private int longWidth;

    private AudioSource source;
    public AudioClip timerSound;

    private TimePeriod period;

    private const int twelveHoursInSeconds = 43200;

    private void Awake()
    {
        clockManager = GetComponentInParent<ClockManager>();

        source = GetComponent<AudioSource>();
        source.clip = timerSound;

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
                case "Time Hour 12":
                    timeHour12 = t.gameObject;
                    break;
                case "Time Hour 24":
                    timeHour24 = t.gameObject;
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

        timeHour24Dropdown = timeHour24.GetComponent<TMP_Dropdown>();
        timeHour12Dropdown = timeHour12.GetComponent<TMP_Dropdown>();
        timerStopwatchHourDropdown = timerStopwatchHourValue.GetComponent<TMP_Dropdown>();
        minuteDropdown = minuteValue.GetComponent<TMP_Dropdown>();
        secondDropdown = secondValue.GetComponent<TMP_Dropdown>();
        periodDropdown = periodValue.GetComponent<TMP_Dropdown>();

        numberText = digital_Numbers.GetComponent<TextMeshProUGUI>();
        numberTransform = numberText.GetComponent<RectTransform>();
        periodText = digital_Period.GetComponent<TextMeshProUGUI>();

        timeRunning = true;
        timerRunning = false;
        stopwatchRunning = false;

        time = 0;
        timer = 0;
        stopwatch = 0;

        shortWidth = 37;
        mediumWidth = 57;
        longWidth = 77;

        period = TimePeriod.AM;

        // Reference: https://answers.unity.com/questions/970545/how-do-i-set-the-widthheight-of-a-ui-element.html
        numberTransform.sizeDelta = new Vector2(mediumWidth, numberTransform.sizeDelta.y);
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
        timeHour24.SetActive(false);
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
                setValues.SetActive(true);

                SetTimeFormat();
                break;
            case "Timer":
                // Turn off Time Display and Stopwatch objects
                analog_Time.SetActive(false);
                timeFormat.SetActive(false);
                stopwatchSettings.SetActive(false);
                periodValue.SetActive(false);
                timeHour12.SetActive(false);
                timeHour24.SetActive(false);

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
                timeHour12.SetActive(false);
                timeHour24.SetActive(false);
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
                periodValue.SetActive(false);
                analog.SetActive(true);
                break;
            case "hh:mm (24hr)":
                analog.SetActive(false);
                digital_Period.SetActive(false);
                periodValue.SetActive(false);
                secondValue.SetActive(false);
                timeHour12.SetActive(false);
                digital.SetActive(true);
                timeHour24.SetActive(true);
                numberTransform.sizeDelta = new Vector2(shortWidth, numberTransform.sizeDelta.y);
                break;
            case "hh:mm (12hr)":
                analog.SetActive(false);
                secondValue.SetActive(false);
                timeHour24.SetActive(false);
                digital_Period.SetActive(true);
                periodValue.SetActive(true);
                digital.SetActive(true);
                timeHour12.SetActive(true);
                numberTransform.sizeDelta = new Vector2(shortWidth, numberTransform.sizeDelta.y);
                break;
            case "hh:mm:ss (24hr)":
                analog.SetActive(false);
                digital_Period.SetActive(false);
                periodValue.SetActive(false);
                timeHour12.SetActive(false);
                secondValue.SetActive(true);
                digital.SetActive(true);
                timeHour24.SetActive(true);
                numberTransform.sizeDelta = new Vector2(mediumWidth, numberTransform.sizeDelta.y);
                break;
            case "hh:mm:ss (12hr)":
                analog.SetActive(false);
                timeHour24.SetActive(false);
                digital_Period.SetActive(true);
                periodValue.SetActive(true);
                secondValue.SetActive(true);
                digital.SetActive(true);
                timeHour12.SetActive(true);
                numberTransform.sizeDelta = new Vector2(mediumWidth, numberTransform.sizeDelta.y);
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
                digital_Period.SetActive(true);
                analog.SetActive(true);
                break;
            case "Digital":
                analog.SetActive(false);
                digital_Period.SetActive(false);
                digital.SetActive(true);

                if (modeDropdown.captionText.text == "Timer")
                {
                    numberText.text = "00:00:00";
                    numberTransform.sizeDelta = new Vector2(mediumWidth, numberTransform.sizeDelta.y);
                }
                else if (modeDropdown.captionText.text == "Stopwatch")
                {
                    numberText.text = "00:00:00.00";
                    numberTransform.sizeDelta = new Vector2(longWidth, numberTransform.sizeDelta.y);
                }
                break;
        }
    }

    // Calculates the hours, minutes and seconds values to seconds and sets either the time or timer value to this
    public void SetValues()
    {
        if (modeDropdown.captionText.text == "Time Display")
        {
            if (timeHour12.activeSelf)
            {
                if (period == TimePeriod.AM)
                    time = (timeHour12Dropdown.value * 60 * 60) + (minuteDropdown.value * 60) + secondDropdown.value;
                else
                    time = ((timeHour12Dropdown.value + 12) * 60 * 60) + (minuteDropdown.value * 60) + secondDropdown.value;
            }


            else
                time = (timeHour24Dropdown.value * 60 * 60) + (minuteDropdown.value * 60) + secondDropdown.value;
        }


        if (modeDropdown.captionText.text == "Timer")
            timer = (timerStopwatchHourDropdown.value * 60 * 60) + (minuteDropdown.value * 60) + secondDropdown.value;

        SetNumberDisplay();
    }

    // Allows for the user to set whether the time period is AM or PM and displays the chosen option.
    public void SetPeriod()
    {
        periodText.text = periodDropdown.captionText.text;

        if (periodText.text == "AM")
        {
            // Don't make changes if time is already an AM value
            if (period == TimePeriod.PM)
            {
                period = TimePeriod.AM;
                time -= twelveHoursInSeconds;
            }
        }
        else
        {
            // Don't make changes if time is already a PM value
            if (period == TimePeriod.AM)
            {
                period = TimePeriod.PM;
                time += twelveHoursInSeconds;
            }
        }
    }

    // Sets the the bool controlling the current mode type's activity to true.
    public void Run()
    {
        switch (modeDropdown.captionText.text)
        {
            case "Time Display":
                timeRunning = true;
                break;
            case "Timer":
                timerRunning = true;
                break;
            case "Stopwatch":
                stopwatchRunning = true;
                break;
        }
    }

    // Sets the the bool controlling the current mode type's activity to false.
    public void Stop()
    {
        switch (modeDropdown.captionText.text)
        {
            case "Time Display":
                timeRunning = false;
                break;
            case "Timer":
                // If user clicks the stop button twice in a row, it will reset the timer
                if (!timerRunning)
                {
                    timer = 0;
                    numberText.text = "00:00:00";
                }
                timerRunning = false;
                break;
            case "Stopwatch":
                stopwatchRunning = false;
                break;
        }
    }

    // Add time passed to time value and display it on UI
    private void RunClock()
    {
        time += Time.deltaTime;
        Debug.Log(Time.deltaTime);
        SetNumberDisplay();
    }

    // Reduce time passed to timer value and display it on UI
    private void RunTimer()
    {
        timer -= Time.deltaTime;
        SetNumberDisplay();
    }

    // Add time passed to stopwatch value and display it on UI
    private void RunStopwatch()
    {
        stopwatch += Time.deltaTime;
        SetNumberDisplay();
    }

    // Set the stopwatch value to 0 and display it on UI
    public void ResetStopwatch()
    {
        stopwatch = 0;
        SetNumberDisplay();
    }

    // Displays the time, timer or stopwatch value as speficied format
    private void SetNumberDisplay()
    {
        switch (modeDropdown.captionText.text)
        {
            case "Time Display":
                int timeInt = (int)time;
                int timeIntPM = (int)(time - twelveHoursInSeconds);

                // If time is less than or equal to 12 hours in seconds, change the time period
                if (time >= 0 && time < twelveHoursInSeconds && period == TimePeriod.PM)
                {
                    period = TimePeriod.AM;
                    periodText.text = "AM";
                }
                // If time is greater than than or equal to 24 hours in seconds, change the time period
                else if (time >= twelveHoursInSeconds && time < (twelveHoursInSeconds * 2) && period == TimePeriod.AM)
                {
                    period = TimePeriod.PM;
                    periodText.text = "PM";
                }

                // If time pasts 24hr, refresh to 0
                if (time >= (twelveHoursInSeconds * 2))
                    time -= (twelveHoursInSeconds * 2);

                switch (timeDropdown.captionText.text)
                {
                    case "hh:mm (24hr)":
                        numberText.text = string.Format("{0:D2}:{1:D2}",
                                           ((timeInt / 60) / 60), ((timeInt / 60) % 60));
                        break;
                    case "hh:mm (12hr)":
                        // Check time period to see if hour values need to be reduced for 12hr time
                        if (period == TimePeriod.PM)
                            Adjust12HourTimeShortFormat(timeIntPM);
                        else
                            Adjust12HourTimeShortFormat(timeInt);
                        break;
                    case "hh:mm:ss (24hr)":
                        numberText.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                   ((timeInt / 60) / 60), ((timeInt / 60) % 60), (timeInt % 60));
                        break;
                    case "hh:mm:ss (12hr)":
                        // Check time period to see if hour values need to be reduced for 12hr time
                        if (period == TimePeriod.PM)
                            Adjust12HourTimeLongFormat(timeIntPM);
                        else
                            Adjust12HourTimeLongFormat(timeInt);
                        break;
                }

                break;
            case "Timer":
                int timerInt = (int)timer;

                numberText.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                    ((timerInt / 60) / 60), ((timerInt / 60) % 60), (timerInt % 60));
                break;
            case "Stopwatch":
                int stopwatchInt = (int)stopwatch;

                float stopwatchDecimals;
                stopwatchDecimals = (stopwatch - stopwatchInt) * 100;

                int stopwatchIntDecimals = (int)stopwatchDecimals;

                numberText.text = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D2}",
                                    ((stopwatchInt / 60) / 60), ((stopwatchInt / 60) % 60), (stopwatchInt % 60), (stopwatchIntDecimals % 60));
                break;
        }
    }

    // Check if 0 hrs and convert display to 12 instead (for 00:00 format)
    private void Adjust12HourTimeShortFormat(int timeInt)
    {
        int tempTime = timeInt + twelveHoursInSeconds;

        if (((timeInt / 60) / 60) == 0)
            numberText.text = string.Format("{0:D2}:{1:D2}",
                       ((tempTime / 60) / 60), ((timeInt / 60) % 60));
        
        else
            numberText.text = string.Format("{0:D2}:{1:D2}",
                       ((timeInt / 60) / 60), ((timeInt / 60) % 60));
    }

    // Check if 0 hrs and convert display to 12 instead (for 00:00:00 format)
    private void Adjust12HourTimeLongFormat(int timeInt)
    {
        int tempTime = timeInt + twelveHoursInSeconds;

        if (((timeInt / 60) / 60) == 0)
            numberText.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                ((tempTime / 60) / 60), ((timeInt / 60) % 60), (timeInt % 60));
        
        else
            numberText.text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                ((timeInt / 60) / 60), ((timeInt / 60) % 60), (timeInt % 60));
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRunning)
            RunClock();

        if (timerRunning)
            RunTimer();

        if (stopwatchRunning)
            RunStopwatch();
    }
}
