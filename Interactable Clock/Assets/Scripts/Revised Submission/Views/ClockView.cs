using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClockView : InteractableClockElement
{
    public int index = 0;

    // GameObjects that will be have their active state changed based on mode and format values
    [HideInInspector] public GameObject analog;                  // The Analog child object of Clock

    [HideInInspector] public GameObject analog_Time;             // The Analog numbers object for Time Display of Clock
    [HideInInspector] public GameObject analog_TimerStopwatch;   // The Analog numbers object for Timer and Stopwatch of Clock

    [HideInInspector] public GameObject analog_HourHand;         // The Analog hour hand object
    [HideInInspector] public GameObject analog_MinuteHand;       // The Analog minute hand object
    [HideInInspector] public GameObject analog_SecondHand;       // The Analog second hand object

    [HideInInspector] public GameObject digital;                 // The Digital child object of Clock

    [HideInInspector] public GameObject digital_Numbers;         // The Digital numbers object for all modes' digital display
    [HideInInspector] public GameObject digital_Period;          // The Digital period object for displaying the time period

    [HideInInspector] public GameObject hideableSettings;        // The Hideable Settings child object of Clock

    [HideInInspector] public GameObject mode;                    // The mode dropdown object

    [HideInInspector] public GameObject timeFormat;              // The format dropdown object used for Time Display mode
    [HideInInspector] public GameObject timerStopwatchFormat;    // The format dropdown object used for Timer and Stopwatch modes

    [HideInInspector] public GameObject setValues;               // The parent object containing the dropdown children for hour, minute and seconds
    [HideInInspector] public GameObject timeHour12;              // The dropdown object for hours (up to 12)
    [HideInInspector] public GameObject timeHour24;              // The dropdown object for hours (up to 24)
    [HideInInspector] public GameObject timerStopwatchHourValue; // The dropdown object for hours (up to 60)
    [HideInInspector] public GameObject minuteValue;             // The dropdown object for minutes
    [HideInInspector] public GameObject secondValue;             // The dropdown object for seconds
    [HideInInspector] public GameObject periodValue;             // The dropdown object for time period

    [HideInInspector] public GameObject startAndStop;            // The parent object containing the button children for start and stop
    [HideInInspector] public GameObject stopwatchSettings;       // The reset button object

    // Important types with values that incur events and changes within the application
    // Dropdowns for mode and format options
    [HideInInspector] public TMP_Dropdown modeDropdown;
    [HideInInspector] public TMP_Dropdown timeDropdown;
    [HideInInspector] public TMP_Dropdown timerStopwatchDropdown;

    // Dropdowns for hour, minute, second and period values
    [HideInInspector] public TMP_Dropdown timeHour12Dropdown;
    [HideInInspector] public TMP_Dropdown timeHour24Dropdown;
    [HideInInspector] public TMP_Dropdown timerStopwatchHourDropdown;
    [HideInInspector] public TMP_Dropdown minuteDropdown;
    [HideInInspector] public TMP_Dropdown secondDropdown;
    [HideInInspector] public TMP_Dropdown periodDropdown;

    // Relevant for Text objects
    [HideInInspector] public TextMeshProUGUI numberText;
    [HideInInspector] public RectTransform numberTransform;
    [HideInInspector] public TextMeshProUGUI periodText;

    // Audio source for timer alert
    [HideInInspector] public AudioSource source;

    // Notify application of clock enabled event
    private void OnEnable()
    {
        app.Notify(InteractableClockNotification.OnClockEnable, gameObject);
    }

    // Notify application of clock disabled event
    private void OnDisable()
    {
        if (app)
            app.Notify(InteractableClockNotification.OnClockDisable, gameObject);
    }

    // Notify application of clock removed event
    public void RemoveClock()
    {
        app.Notify(InteractableClockNotification.RemoveClock, gameObject);
    }

    // Notify application of clock settings toggled event
    public void ToggleSettings()
    {
        app.Notify(InteractableClockNotification.ToggleSettings, index);
    }

    // Update is called once per frame
    void Update()
    {
        app.Notify(InteractableClockNotification.UpdateClock, index);
    }

    // Notify application of clock mode changed event
    public void SetMode()
    {
        app.Notify(InteractableClockNotification.SetClockMode, index);
    }

    // Notify application of clock time format changed event
    public void SetTimeFormat()
    {
        app.Notify(InteractableClockNotification.SetTimeFormat, index);
    }

    // Notify application of clock timer/stopwatch changed event
    public void SetTimerStopwatchFormat()
    {
        app.Notify(InteractableClockNotification.SetTimerStopwatchFormat, index);
    }

    // Notify application of clock hour changed event
    public void SetHour()
    {
        app.Notify(InteractableClockNotification.SetHour, index);
    }

    // Notify application of clock minute changed event
    public void SetMinute()
    {
        app.Notify(InteractableClockNotification.SetMinute, index);
    }

    // Notify application of clock second changed event
    public void SetSecond()
    {
        app.Notify(InteractableClockNotification.SetSecond, index);
    }

    // Notify application of clock period changed event
    public void SetPeriod()
    {
        app.Notify(InteractableClockNotification.SetPeriod, index);
    }

    // Notify application of clock run event
    public void Run()
    {
        app.Notify(InteractableClockNotification.Run, index);
    }

    // Notify application of clock stop event
    public void Stop()
    {
        app.Notify(InteractableClockNotification.Stop, index);
    }

    // Notify application of stopwatch reset event
    public void ResetStopwatch()
    {
        app.Notify(InteractableClockNotification.ResetStopwatch, index);
    }
}
