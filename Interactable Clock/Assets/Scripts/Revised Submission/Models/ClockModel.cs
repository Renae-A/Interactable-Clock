using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockModel : InteractableClockElement
{
    // Values for time, timer and stopwatch (in seconds)
    [HideInInspector] public float time;
    [HideInInspector] public float timer;
    [HideInInspector] public float stopwatch;

    // The current period of time (AM/PM)
    [HideInInspector] public TimePeriod period;

    // The bools that reflect if the clock's Time Display, Timer and Stopwatch is running or not
    [HideInInspector] public bool timeRunning;
    [HideInInspector] public bool timerRunning;
    [HideInInspector] public bool stopwatchRunning;

    // Audio clip for timer alert
    public AudioClip timerSound;

    // Constant values
    // Analog constants
    [HideInInspector] public const int HoursOnClock = 12;
    [HideInInspector] public const int HoursOnTimerStopwatch = 60;

    [HideInInspector] public const int FullRotationDegrees = 360;

    // Time constants
    [HideInInspector] public const int MinutesInHour = 60;
    [HideInInspector] public const int SecondsInMinute = 60;
    [HideInInspector] public const int MillisecondsInSecond = 1000;

    [HideInInspector] public const int TwelveHoursInSeconds = 43200;
    [HideInInspector] public const int TwentyFourHoursInSeconds = 86400;

    [HideInInspector] public const int ClockMultiplier = 20; // The value of 20 was found through trial and error.

    // Digital format constants
    [HideInInspector] public const string HourMinuteDisplay = "{0:D2}:{1:D2}";
    [HideInInspector] public const string HourMinuteSecondDisplay = "{0:D2}:{1:D2}:{2:D2}";
    [HideInInspector] public const string HourMinuteSecondMillisecondDisplay = "{0:D2}:{1:D2}:{2:D2}.{3:D2}";

    // Zero display constants
    [HideInInspector] public const string ZeroTimer = "00:00:00";
    [HideInInspector] public const string ZeroStopwatch = "00:00:00.00";

    // Mode name constants
    [HideInInspector] public const string TimeDisplayMode = "Time Display";
    [HideInInspector] public const string TimerMode = "Timer";
    [HideInInspector] public const string StopwatchMode = "Stopwatch";

    // Format name constants
    [HideInInspector] public const string AnalogFormat = "Analog";
    [HideInInspector] public const string DigitalFormat = "Digital";
    [HideInInspector] public const string TwentyFour_HourMinuteFormat = "hh:mm (24hr)";
    [HideInInspector] public const string Twelve_HourMinuteFormat = "hh:mm (12hr)";
    [HideInInspector] public const string TwentyFour_HourMinuteSecondsFormat = "hh:mm:ss (24hr)";
    [HideInInspector] public const string Twelve_HourMinuteSecondsFormat = "hh:mm:ss (12hr)";
    [HideInInspector] public const string AM = "AM";
    [HideInInspector] public const string PM = "PM";

    // Format display contants (each width represents the needed rect transform width for 00:00, 00:00:00 and 00:00:00.00 formats)
    [HideInInspector] public const int ShortWidth = 37;
    [HideInInspector] public const int MediumWidth = 57;
    [HideInInspector] public const int LongWidth = 77;

    // Clock Width and Height
    [HideInInspector] public const int HalfClockWidthAndHeight = 50;
}
