using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Used for representing the current period (AM or PM)
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

    private GameObject analog_Time;             // The Analog numbers object for Time Display of Clock
    private GameObject analog_TimerStopwatch;   // The Analog numbers object for Timer and Stopwatch of Clock

    private GameObject analog_HourHand;         // The Analog hour hand object
    private GameObject analog_MinuteHand;       // The Analog minute hand object
    private GameObject analog_SecondHand;       // The Analog second hand object

    private GameObject digital;                 // The Digital child object of Clock

    private GameObject digital_Numbers;         // The Digital numbers object for all modes' digital display
    private GameObject digital_Period;          // The Digital period object for displaying the time period

    private GameObject hideableSettings;        // The Hideable Settings child object of Clock

    private GameObject mode;                    // The mode dropdown object

    private GameObject timeFormat;              // The format dropdown object used for Time Display mode
    private GameObject timerStopwatchFormat;    // The format dropdown object used for Timer and Stopwatch modes

    private GameObject setValues;               // The parent object containing the dropdown children for hour, minute and seconds
    private GameObject timeHour12;              // The dropdown object for hours (up to 12)
    private GameObject timeHour24;              // The dropdown object for hours (up to 24)
    private GameObject timerStopwatchHourValue; // The dropdown object for hours (up to 60)
    private GameObject minuteValue;             // The dropdown object for minutes
    private GameObject secondValue;             // The dropdown object for seconds
    private GameObject periodValue;             // The dropdown object for time period

    private GameObject startAndStop;            // The parent object containing the button children for start and stop
    private GameObject stopwatchSettings;       // The reset button object

    // Important types with values that incur events and changes within the application
    // Dropdowns for mode and format options
    private TMP_Dropdown modeDropdown;
    private TMP_Dropdown timeDropdown;
    private TMP_Dropdown timerStopwatchDropdown;

    // Dropdowns for hour, minute, second and period values
    private TMP_Dropdown timeHour12Dropdown;
    private TMP_Dropdown timeHour24Dropdown;
    private TMP_Dropdown timerStopwatchHourDropdown;
    private TMP_Dropdown minuteDropdown;
    private TMP_Dropdown secondDropdown;
    private TMP_Dropdown periodDropdown;

    // Relevant for Text objects
    private TextMeshProUGUI numberText;
    private RectTransform numberTransform;
    private TextMeshProUGUI periodText;

    // Values for time, timer and stopwatch (in seconds)
    private float time;
    private float timer;
    private float stopwatch;

    // The current period of time (AM/PM)
    private TimePeriod period;

    // The bools that reflect if the clock's Time Display, Timer and Stopwatch is running or not
    private bool timeRunning;
    private bool timerRunning;
    private bool stopwatchRunning;

    // Audio source and clip for timer alert
    private AudioSource source;
    public AudioClip timerSound;

    // Constant values
    // Analog constants
    private const int HoursOnClock = 12;
    private const int HoursOnTimerStopwatch = 60;

    private const int FullRotationDegrees = 360;

    // Time constants
    private const int MinutesInHour = 60;
    private const int SecondsInMinute = 60;
    private const int MillisecondsInSecond = 1000;

    private const int TwelveHoursInSeconds = 43200;
    private const int TwentyFourHoursInSeconds = 86400;

    private const int ClockMultiplier = 20; // The value of 20 was found through trial and error.

    // Digital format constants
    private const string HourMinuteDisplay = "{0:D2}:{1:D2}";
    private const string HourMinuteSecondDisplay = "{0:D2}:{1:D2}:{2:D2}";
    private const string HourMinuteSecondMillisecondDisplay = "{0:D2}:{1:D2}:{2:D2}.{3:D2}";

    // Zero display constants
    private const string ZeroTimer = "00:00:00";
    private const string ZeroStopwatch = "00:00:00.00";

    // Mode name constants
    private const string TimeDisplayMode = "Time Display";
    private const string TimerMode = "Timer";
    private const string StopwatchMode = "Stopwatch";

    // Format name constants
    private const string AnalogFormat = "Analog";
    private const string DigitalFormat = "Digital";
    private const string TwentyFour_HourMinuteFormat = "hh:mm (24hr)";
    private const string Twelve_HourMinuteFormat = "hh:mm (12hr)";
    private const string TwentyFour_HourMinuteSecondsFormat = "hh:mm:ss (24hr)";
    private const string Twelve_HourMinuteSecondsFormat = "hh:mm:ss (12hr)";
 
    // Format display contants (each width represents the needed rect transform width for 00:00, 00:00:00 and 00:00:00.00 formats)
    private const int ShortWidth = 37;
    private const int MediumWidth = 57;
    private const int LongWidth = 77;
    
    // Setup variables and find child objects by tag and setup all the GameObject member variables
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

        // Set default values
        timeRunning = true;
        timerRunning = false;
        stopwatchRunning = false;

        time = 0;
        timer = 0;
        stopwatch = 0;

        period = TimePeriod.AM;

        // Finding/using width of rect transform reference: https://answers.unity.com/questions/970545/how-do-i-set-the-widthheight-of-a-ui-element.html
        numberTransform.sizeDelta = new Vector2(MediumWidth, numberTransform.sizeDelta.y);
    }


    // Start is called before the first frame update
    void Start()
    {
        // Setup for a default format Analog and mode Time Display
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
            case TimeDisplayMode:
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
            case TimerMode:
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
            case StopwatchMode:
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
            case AnalogFormat:
                // Turn off Digital objects
                digital.SetActive(false);
                timeHour24.SetActive(false);
                periodValue.SetActive(false);
                // Turn on Analog objects
                analog.SetActive(true);
                timeHour12.SetActive(true);
                secondValue.SetActive(true);
                break;
            case TwentyFour_HourMinuteFormat:
                // Turn off Analog objects
                analog.SetActive(false);
                // Turn off unnecessary digital objects
                digital_Period.SetActive(false);
                periodValue.SetActive(false);
                secondValue.SetActive(false);
                timeHour12.SetActive(false);
                // Turn on format appropriate objects for digital clock
                digital.SetActive(true);
                timeHour24.SetActive(true);
                numberTransform.sizeDelta = new Vector2(ShortWidth, numberTransform.sizeDelta.y);  // Resize digital number text width
                break;
            case Twelve_HourMinuteFormat:
                // Turn off Analog objects
                analog.SetActive(false);
                // Turn off unnecessary digital objects
                secondValue.SetActive(false);
                timeHour24.SetActive(false);
                // Turn on format appropriate objects for digital clock
                digital_Period.SetActive(true);
                periodValue.SetActive(true);
                digital.SetActive(true);
                timeHour12.SetActive(true);
                numberTransform.sizeDelta = new Vector2(ShortWidth, numberTransform.sizeDelta.y);  // Resize digital number text width
                break;
            case TwentyFour_HourMinuteSecondsFormat:
                // Turn off Analog objects
                analog.SetActive(false);
                // Turn off unnecessary digital objects
                digital_Period.SetActive(false);
                periodValue.SetActive(false);
                timeHour12.SetActive(false);
                // Turn on format appropriate objects for digital clock
                secondValue.SetActive(true);
                digital.SetActive(true);
                timeHour24.SetActive(true);
                numberTransform.sizeDelta = new Vector2(MediumWidth, numberTransform.sizeDelta.y);  // Resize digital number text width
                break;
            case Twelve_HourMinuteSecondsFormat:
                // Turn off Analog objects
                analog.SetActive(false);
                // Turn off unnecessary digital objects
                timeHour24.SetActive(false);
                // Turn on format appropriate objects for digital clock
                digital_Period.SetActive(true);
                periodValue.SetActive(true);
                secondValue.SetActive(true);
                digital.SetActive(true);
                timeHour12.SetActive(true);
                numberTransform.sizeDelta = new Vector2(MediumWidth, numberTransform.sizeDelta.y);  // Resize digital number text width
                break;
        }
    }

    //	Changes whether the clock is analog or digital (hides/reveals appropriate objects).
    public void SetTimerStopwatchFormat()
    {
        switch (timerStopwatchDropdown.captionText.text)
        {
            case AnalogFormat:
                // Turn off digital objects
                digital.SetActive(false);
                digital_Period.SetActive(false);
                // Turn on analog objects
                analog.SetActive(true);
                secondValue.SetActive(true);
                break;
            case DigitalFormat:
                // Turn off analog and uneeded digital objects
                analog.SetActive(false);
                digital_Period.SetActive(false);
                // Turn on digital objects
                digital.SetActive(true);
                secondValue.SetActive(true);

                // Adjust number text and its width depending on the relevent mode's format
                if (modeDropdown.captionText.text == TimerMode)
                {
                    numberText.text = ZeroTimer;
                    numberTransform.sizeDelta = new Vector2(MediumWidth, numberTransform.sizeDelta.y);
                }
                else if (modeDropdown.captionText.text == StopwatchMode)
                {
                    numberText.text = ZeroStopwatch;
                    numberTransform.sizeDelta = new Vector2(LongWidth, numberTransform.sizeDelta.y);
                }
                break;
        }
    }

    // Calculates the new hour and current minutes and seconds values to seconds and sets either the time or timer value to this
    public void SetHour()
    {
        int newHour = 0;

        if (modeDropdown.captionText.text == TimeDisplayMode)
        {
            time = RemoveCurrentHour(time);

            // If the user is using the 12hr digital format for the Time Display
            if (timeHour12.activeSelf)
            {
                // If the time period is AM, then set new hour normally using dropdown value
                if (period == TimePeriod.AM)
                    newHour = (timeHour12Dropdown.value * MinutesInHour * SecondsInMinute);
                // If the time period is PM, then set new hour accounting for the additional 12 hours (being after midday) using dropdown value
                else
                    newHour = ((timeHour12Dropdown.value + HoursOnClock) * MinutesInHour * SecondsInMinute);
            }
            // Setup normally for 24hrs
            else
                newHour = timeHour24Dropdown.value * MinutesInHour * SecondsInMinute;
        }

        // Setup normally for timer, using dropdown value
        else if (modeDropdown.captionText.text == TimerMode)
        {
            timer = RemoveCurrentHour(timer);
            newHour = timerStopwatchHourDropdown.value * MinutesInHour * SecondsInMinute;
        }

        SetValues(newHour, modeDropdown.captionText.text);
    }

    // Calculates the new hour and current minutes and seconds values to seconds and sets either the time or timer value to this
    private float RemoveCurrentHour(float modeValue)
    {
        int currentHour = 0;

        currentHour = (int)(modeValue / MinutesInHour) / SecondsInMinute;    // In seconds
        modeValue -= currentHour * MinutesInHour * SecondsInMinute;   // Remove the current hour from value

        return modeValue;
    }

    // Calculates the new minute and current hours and seconds values to seconds and sets either the time or timer value to this
    public void SetMinute()
    {
        int newMinute = 0;
       
        if (modeDropdown.captionText.text == TimeDisplayMode)
            time = RemoveCurrentMinute(time);
        
        else if (modeDropdown.captionText.text == TimerMode)
            timer = RemoveCurrentMinute(timer);

        newMinute = minuteDropdown.value * SecondsInMinute;

        SetValues(newMinute, modeDropdown.captionText.text);
    }

    // Calculates the new minute and current hours and seconds values to seconds and sets either the time or timer value to this
    private float RemoveCurrentMinute(float modeValue)
    {
        int currentMinute = 0;

        currentMinute = (int)(modeValue / MinutesInHour) % SecondsInMinute;
        modeValue -= currentMinute * SecondsInMinute;

        return modeValue;
    }


    // Calculates the new seconds and current hours and minutes values to seconds and sets either the time or timer value to this
    public void SetSeconds()
    {
        int newSeconds = 0;

        if (modeDropdown.captionText.text == TimeDisplayMode)
            time = RemoveCurrentSecond(time);

        else if (modeDropdown.captionText.text == TimerMode)
            timer = RemoveCurrentSecond(timer);

        newSeconds = secondDropdown.value;

        SetValues(newSeconds, modeDropdown.captionText.text);
    }

    // Calculates the new seconds and current hours and minutes values to seconds and sets either the time or timer value to this
    private float RemoveCurrentSecond(float modeValue)
    {
        int currentSeconds = 0;

        currentSeconds = (int)modeValue % SecondsInMinute;
        modeValue -= currentSeconds;

        return modeValue;
    }

    // Add the selected value to either time or timer
    public void SetValues(int newValueToAdd, string modeSpecificValue)
    {
        if (modeSpecificValue == TimeDisplayMode)
            time += newValueToAdd;
        else
            timer += newValueToAdd;

        SetNumberDisplay();
        SetClockHands();
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
                time -= TwelveHoursInSeconds;
            }
        }
        else
        {
            // Don't make changes if time is already a PM value
            if (period == TimePeriod.AM)
            {
                period = TimePeriod.PM;
                time += TwelveHoursInSeconds;
            }
        }
    }

    // Sets the the bool controlling the current mode type's activity to true.
    public void Run()
    {
        switch (modeDropdown.captionText.text)
        {
            case TimeDisplayMode:
                timeRunning = true;
                break;
            case TimerMode:
                timerRunning = true;
                break;
            case StopwatchMode:
                stopwatchRunning = true;
                break;
        }
    }

    // Sets the the bool controlling the current mode type's activity to false.
    public void Stop()
    {
        switch (modeDropdown.captionText.text)
        {
            case TimeDisplayMode:
                timeRunning = false;
                break;
            case TimerMode:
                // If user clicks the stop button twice in a row, it will reset the timer
                if (!timerRunning)
                {
                    timer = 0;
                    numberText.text = ZeroTimer;
                }
                source.Stop();
                timerRunning = false;
                break;
            case StopwatchMode:
                stopwatchRunning = false;
                break;
        }
    }

    // Add time passed to time value and display it on UI
    private void RunClock()
    {
        time += Time.deltaTime;
        SetClockHands();
        SetNumberDisplay();
    }

    // Reduce time passed to timer value and display it on UI
    private void RunTimer()
    {
        timer -= Time.deltaTime;

        // If the time has reached 0 and the alert has not already started playing, call TimerFinish to play alert
        if (timer <= 0)
            if (!source.isPlaying)
                TimerFinish();

        SetNumberDisplay();
    }

    // Plays the AudioClip passed into this script (used for timer reaching 0)
    private void TimerFinish()
    {
        timer = 0;
        timerRunning = false;
        source.Play();
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

    // Displays the time, timer or stopwatch value as speficied format (analog)
    private void SetClockHands()
    {
        Quaternion hourRotation = Quaternion.identity;
        Quaternion minuteRotation = Quaternion.identity;
        Quaternion secondRotation = Quaternion.identity;

        float modeValue = 0;
        float segmentValue = 0;

        switch (modeDropdown.captionText.text)
        {
            case TimeDisplayMode:
                modeValue = time;
                segmentValue = HoursOnClock;
                break;
            case TimerMode:
                modeValue = timer;
                segmentValue = HoursOnTimerStopwatch;
                break;
            case StopwatchMode:
                modeValue = stopwatch;
                segmentValue = HoursOnTimerStopwatch;
                break;
        }

        // Use of euler angles for clock hands reference: https://www.youtube.com/watch?v=pbTysQw-WNs
        // Hour hand
        analog_HourHand.transform.eulerAngles = new Vector3(0, 0, -(((modeValue / (ClockMultiplier * MinutesInHour)) / segmentValue) / FullRotationDegrees) * TwelveHoursInSeconds);
        // Minute hand
        analog_MinuteHand.transform.eulerAngles = new Vector3(0, 0, -((modeValue / (ClockMultiplier * SecondsInMinute)) / FullRotationDegrees) * TwelveHoursInSeconds);
        // Second hand
        analog_SecondHand.transform.eulerAngles = new Vector3(0, 0, -((modeValue / ClockMultiplier) / FullRotationDegrees) * TwelveHoursInSeconds);

        Debug.Log(time);
    }

    // Displays the time, timer or stopwatch value as speficied format (digital)
    private void SetNumberDisplay()
    {
        switch (modeDropdown.captionText.text)
        {
            case TimeDisplayMode:
                int timeInt = (int)time;
                int timeIntPM = (int)(time - TwelveHoursInSeconds);

                // If time is less than or equal to 12 hours in seconds, change the time period
                if (time >= 0 && time < TwelveHoursInSeconds && period == TimePeriod.PM)
                    period = TimePeriod.AM;

                // If time is greater than than or equal to 24 hours in seconds, change the time period
                else if (time >= TwelveHoursInSeconds && time < (TwelveHoursInSeconds * 2) && period == TimePeriod.AM)
                    period = TimePeriod.PM;

                periodText.text = period.ToString();

                // If time pasts 24hr, refresh to 0
                if (time >= TwentyFourHoursInSeconds)
                    time -= TwentyFourHoursInSeconds;

                switch (timeDropdown.captionText.text)
                {
                    case TwentyFour_HourMinuteFormat:
                        DisplayDigital(HourMinuteDisplay, timeInt);
                        break;
                    case Twelve_HourMinuteFormat:
                        // Check time period to see if hour values need to be reduced for 12hr time
                        if (period == TimePeriod.PM)
                            Adjust12HourTime(timeIntPM, HourMinuteDisplay);
                        else
                            Adjust12HourTime(timeInt, HourMinuteDisplay);
                        break;
                    case TwentyFour_HourMinuteSecondsFormat:
                        DisplayDigital(HourMinuteSecondDisplay, timeInt);                    
                        break;
                    case Twelve_HourMinuteSecondsFormat:
                        // Check time period to see if hour values need to be reduced for 12hr time
                        if (period == TimePeriod.PM)
                            Adjust12HourTime(timeIntPM, HourMinuteSecondDisplay);
                        else
                            Adjust12HourTime(timeInt, HourMinuteSecondDisplay);
                        break;
                }

                break;
            case TimerMode:
                int timerInt = (int)timer;

                // Displays the timer value in 00:00:00 format if timer value is 0 or greater
                if (timerInt >= 0)
                    DisplayDigital(HourMinuteSecondDisplay, timerInt);

                break;
            case StopwatchMode:
                int stopwatchInt = (int)stopwatch;

                // Creating a variable where the decimal values of stopwatch can be used as an integer (needed for display code)
                float stopwatchDecimals = (stopwatch - stopwatchInt) * 100;
                int stopwatchIntDecimals = (int)stopwatchDecimals;

                // Displays the stopwatch value in 00:00:00.00 format
                DisplayDigital(HourMinuteSecondMillisecondDisplay, stopwatchInt, false, stopwatchIntDecimals);

                break;
        }
    }

    // Check if 0 hrs and convert display to 12 instead 
    private void Adjust12HourTime(int timeInt, string format)
    {
        // Uses tempTime (additional 12hrs added to time) while the hour is 0 to display 12:00
        if (((timeInt / SecondsInMinute) / MinutesInHour) == 0)
            DisplayDigital(format, timeInt, true);

        // Displays the actual time while the hour is not 0
        else
            DisplayDigital(format, timeInt);
    }

    // Displays the digital display, using parameters for format (e.g. 00:00, 00:00:00, etc), modeValue (time, timer or stopwatch as an integer), tempValue (whether the hour is at 0 or not)
    // and stopwatchIntDecimals (the decimal values as an int for the stopwatch)
    private void DisplayDigital(string format, int modeValue, bool tempVal = false, int stopwatchIntDecimals = 0)
    {
        int usedHourModeValue;

        if (tempVal)
            usedHourModeValue = modeValue + TwelveHoursInSeconds;
        else
            usedHourModeValue = modeValue;

        // Check the type of format used for this display and set appropriately
        switch (format)
        {
            case HourMinuteDisplay:
                numberText.text = string.Format(format, ((usedHourModeValue / SecondsInMinute) / MinutesInHour), ((modeValue / SecondsInMinute) % MinutesInHour));
                break;
            case HourMinuteSecondDisplay:
                numberText.text = string.Format(format, ((usedHourModeValue / SecondsInMinute) / MinutesInHour), ((modeValue / SecondsInMinute) % MinutesInHour), (modeValue % SecondsInMinute));
                break;
            case HourMinuteSecondMillisecondDisplay:
                numberText.text = string.Format(HourMinuteSecondMillisecondDisplay, ((modeValue / SecondsInMinute) / MinutesInHour), ((modeValue / SecondsInMinute) % MinutesInHour),
                    (modeValue % SecondsInMinute), (stopwatchIntDecimals % MillisecondsInSecond));
                break;
        }
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
