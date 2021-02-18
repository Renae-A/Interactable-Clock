using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockController : InteractableClockElement
{
    // Update is called once per frame, calls RunClock(), RunTimer() and RunStopwatch() if these functionalities are set to run
    public void UpdateClock(ClockModel clockModel, ClockView clockView)
    {
        if (clockModel.timeRunning)
            RunClock(clockModel, clockView);

        if (clockModel.timerRunning)
            RunTimer(clockModel, clockView);

        if (clockModel.stopwatchRunning)
            RunStopwatch(clockModel, clockView);
    }

    // Removes clock from spawnedClocks the lists: GameObject, ClockView and Models in the Clock Manager
    public void OnClockDisable(GameObject clock)
    {
        // Remove the clock model at same index as ClockManagerView's spawnedClocks
        int clockIndex = app.controller.clockManager.GetClockIndex(clock);

        // Remove from model, view and gameobject lists
        app.model.clockManager.spawnedClockModels.RemoveAt(clockIndex);
        app.view.clockManager.spawnedClockViews.RemoveAt(clockIndex);

        app.view.clockManager.spawnedClocks.Remove(clock);

        // Cycles through each clock in the ClockView list spawnedClockView and sets up their index value based on their position in the list
        ResetClockIndexes();
    }

    // Cycles through each clock in the ClockView list spawnedClockView and sets up their index value based on their position in the list
    public void ResetClockIndexes()
    {
        foreach (ClockView view in app.view.clockManager.spawnedClockViews)
            view.index = app.controller.clockManager.GetClockIndex(view.gameObject);
    }

    // Adds clock to spawnedClocks list and creates/adds ClockView and ClockModel to respective lists, generates a position for the clock, resets clock index and initialises Clock values
    public void OnClockEnable(GameObject clock)
    {
        app.view.clockManager.spawnedClocks.Add(clock);

        ClockView clockView = clock.GetComponent<ClockView>();
        app.view.clockManager.spawnedClockViews.Add(clockView);

        app.Notify(InteractableClockNotification.GeneratePosition, clock);

        ClockModel clockModel = clock.GetComponent<ClockModel>();
        clockModel.timerSound = app.model.clockTemplate.timerSound;

        app.model.clockManager.spawnedClockModels.Add(clockModel);

        ResetClockIndexes();

        // Set default values
        clockModel.timeRunning = true;
        clockModel.timerRunning = false;
        clockModel.stopwatchRunning = false;

        clockModel.time = 0;
        clockModel.timer = 0;
        clockModel.stopwatch = 0;

        clockModel.period = TimePeriod.AM;

        // Set up the clock
        app.SetUpClock(clockModel, clockView);

        // Finding/using width of rect transform reference: https://answers.unity.com/questions/970545/how-do-i-set-the-widthheight-of-a-ui-element.html
        clockView.numberTransform.sizeDelta = new Vector2(ClockModel.MediumWidth, clockView.numberTransform.sizeDelta.y);

        // Setup for a default format Analog and mode Time Display
        clockView.analog.SetActive(true);
        clockView.analog_Time.SetActive(true);

        clockView.analog_TimerStopwatch.SetActive(false);
        clockView.digital.SetActive(false);
        clockView.hideableSettings.SetActive(false);
        clockView.timerStopwatchFormat.SetActive(false);
        clockView.stopwatchSettings.SetActive(false);
        clockView.timerStopwatchHourValue.SetActive(false);
        clockView.periodValue.SetActive(false);
        clockView.timeHour24.SetActive(false);
    }

    // Either hides or shows the additional settings options on button click.
    public void ToggleSettings(ClockView clockView)
    {
        clockView.hideableSettings.SetActive(!clockView.hideableSettings.activeSelf);
    }

    //	Changes whether the clock is a time display, countdown timer or stopwatch (hides/reveals appropriate objects).
    public void SetMode(ClockModel clockModel, ClockView clockView)
    {
        switch (clockView.modeDropdown.captionText.text)
        {
            case ClockModel.TimeDisplayMode:
                // Turn off Timer and Stopwatch objects
                clockView.analog_TimerStopwatch.SetActive(false);
                clockView.timerStopwatchFormat.SetActive(false);
                clockView.stopwatchSettings.SetActive(false);
                clockView.timerStopwatchHourValue.SetActive(false);

                // Turn on Time Display objects
                clockView.analog_Time.SetActive(true);
                clockView.timeFormat.SetActive(true);
                clockView.setValues.SetActive(true);

                SetTimeFormat(clockModel, clockView);
                break;
            case ClockModel.TimerMode:
                // Turn off Time Display and Stopwatch objects
                clockView.analog_Time.SetActive(false);
                clockView.timeFormat.SetActive(false);
                clockView.stopwatchSettings.SetActive(false);
                clockView.periodValue.SetActive(false);
                clockView.timeHour12.SetActive(false);
                clockView.timeHour24.SetActive(false);

                // Turn on Timer Display objects
                clockView.analog_TimerStopwatch.SetActive(true);
                clockView.timerStopwatchFormat.SetActive(true);
                clockView.timerStopwatchHourValue.SetActive(true);
                clockView.setValues.SetActive(true);

                SetTimerStopwatchFormat(clockModel, clockView);
                break;
            case ClockModel.StopwatchMode:
                // Turn off Time Display and Timer objects
                clockView.analog_Time.SetActive(false);
                clockView.timeFormat.SetActive(false);
                clockView.periodValue.SetActive(false);
                clockView.timeHour12.SetActive(false);
                clockView.timeHour24.SetActive(false);
                clockView.setValues.SetActive(false);

                // Turn on Stopwatch objects
                clockView.analog_TimerStopwatch.SetActive(true);
                clockView.timerStopwatchFormat.SetActive(true);
                clockView.stopwatchSettings.SetActive(true);
                clockView.timerStopwatchHourValue.SetActive(true);

                SetTimerStopwatchFormat(clockModel, clockView);
                break;
        }
        SetNumberDisplay(clockModel, clockView);
        SetClockHands(clockModel, clockView);
    }

    //	Changes whether the clock is analog or digital in a variety of time formats (hides/reveals appropriate objects).
    public void SetTimeFormat(ClockModel clockModel, ClockView clockView)
    {
        switch (clockView.timeDropdown.captionText.text)
        {
            case ClockModel.AnalogFormat:
                // Turn off Digital objects
                clockView.digital.SetActive(false);
                clockView.timeHour24.SetActive(false);
                clockView.periodValue.SetActive(false);
                // Turn on Analog objects
                clockView.analog.SetActive(true);
                clockView.timeHour12.SetActive(true);
                clockView.secondValue.SetActive(true);
                break;
            case ClockModel.TwentyFour_HourMinuteFormat:
                // Turn off Analog objects
                clockView.analog.SetActive(false);
                // Turn off unnecessary digital objects
                clockView.digital_Period.SetActive(false);
                clockView.periodValue.SetActive(false);
                clockView.secondValue.SetActive(false);
                clockView.timeHour12.SetActive(false);
                // Turn on format appropriate objects for digital clock
                clockView.digital.SetActive(true);
                clockView.timeHour24.SetActive(true);
                clockView.numberTransform.sizeDelta = new Vector2(ClockModel.ShortWidth, clockView.numberTransform.sizeDelta.y);  // Resize digital number text width
                break;
            case ClockModel.Twelve_HourMinuteFormat:
                // Turn off Analog objects
                clockView.analog.SetActive(false);
                // Turn off unnecessary digital objects
                clockView.secondValue.SetActive(false);
                clockView.timeHour24.SetActive(false);
                // Turn on format appropriate objects for digital clock
                clockView.digital_Period.SetActive(true);
                clockView.periodValue.SetActive(true);
                clockView.digital.SetActive(true);
                clockView.timeHour12.SetActive(true);
                clockView.numberTransform.sizeDelta = new Vector2(ClockModel.ShortWidth, clockView.numberTransform.sizeDelta.y);  // Resize digital number text width
                break;
            case ClockModel.TwentyFour_HourMinuteSecondsFormat:
                // Turn off Analog objects
                clockView.analog.SetActive(false);
                // Turn off unnecessary digital objects
                clockView.digital_Period.SetActive(false);
                clockView.periodValue.SetActive(false);
                clockView.timeHour12.SetActive(false);
                // Turn on format appropriate objects for digital clock
                clockView.secondValue.SetActive(true);
                clockView.digital.SetActive(true);
                clockView.timeHour24.SetActive(true);
                clockView.numberTransform.sizeDelta = new Vector2(ClockModel.MediumWidth, clockView.numberTransform.sizeDelta.y);  // Resize digital number text width
                break;
            case ClockModel.Twelve_HourMinuteSecondsFormat:
                // Turn off Analog objects
                clockView.analog.SetActive(false);
                // Turn off unnecessary digital objects
                clockView.timeHour24.SetActive(false);
                // Turn on format appropriate objects for digital clock
                clockView.digital_Period.SetActive(true);
                clockView.periodValue.SetActive(true);
                clockView.secondValue.SetActive(true);
                clockView.digital.SetActive(true);
                clockView.timeHour12.SetActive(true);
                clockView.numberTransform.sizeDelta = new Vector2(ClockModel.MediumWidth, clockView.numberTransform.sizeDelta.y);  // Resize digital number text width
                break;
        }
        SetNumberDisplay(clockModel, clockView);
        SetClockHands(clockModel, clockView);
    }

    //	Changes whether the clock is analog or digital (hides/reveals appropriate objects).
    public void SetTimerStopwatchFormat(ClockModel clockModel, ClockView clockView)
    {
        switch (clockView.timerStopwatchDropdown.captionText.text)
        {
            case ClockModel.AnalogFormat:
                // Turn off digital objects
                clockView.digital.SetActive(false);
                clockView.digital_Period.SetActive(false);
                // Turn on analog objects
                clockView.analog.SetActive(true);
                clockView.secondValue.SetActive(true);
                break;
            case ClockModel.DigitalFormat:
                // Turn off analog and uneeded digital objects
                clockView.analog.SetActive(false);
                clockView.digital_Period.SetActive(false);
                // Turn on digital objects
                clockView.digital.SetActive(true);
                clockView.secondValue.SetActive(true);

                // Adjust number text and its width depending on the relevent mode's format
                if (clockView.modeDropdown.captionText.text == ClockModel.TimerMode)
                {
                    clockView.numberText.text = ClockModel.ZeroTimer;
                    clockView.numberTransform.sizeDelta = new Vector2(ClockModel.MediumWidth, clockView.numberTransform.sizeDelta.y);
                }
                else if (clockView.modeDropdown.captionText.text == ClockModel.StopwatchMode)
                {
                    clockView.numberText.text = ClockModel.ZeroStopwatch;
                    clockView.numberTransform.sizeDelta = new Vector2(ClockModel.LongWidth, clockView.numberTransform.sizeDelta.y);
                }
                break;
        }
        SetNumberDisplay(clockModel, clockView);
        SetClockHands(clockModel, clockView);
    }

    // Calculates the new hour and current minutes and seconds values to seconds and sets either the time or timer value to this
    public void SetHour(ClockModel clockModel, ClockView clockView)
    {
        int newHour = 0;

        if (clockView.modeDropdown.captionText.text == ClockModel.TimeDisplayMode)
        {
            clockModel.time = RemoveCurrentHour(clockModel.time);

            // If the user is using the 12hr digital format for the Time Display
            if (clockView.timeHour12.activeSelf)
            {
                // If the time period is AM, then set new hour normally using dropdown value
                if (clockModel.period == TimePeriod.AM)
                    newHour = (clockView.timeHour12Dropdown.value * ClockModel.MinutesInHour * ClockModel.SecondsInMinute);
                // If the time period is PM, then set new hour accounting for the additional 12 hours (being after midday) using dropdown value
                else
                    newHour = ((clockView.timeHour12Dropdown.value + ClockModel.HoursOnClock) * ClockModel.MinutesInHour * ClockModel.SecondsInMinute);
            }
            // Setup normally for 24hrs
            else
                newHour = clockView.timeHour24Dropdown.value * ClockModel.MinutesInHour * ClockModel.SecondsInMinute;
        }

        // Setup normally for timer, using dropdown value
        else if (clockView.modeDropdown.captionText.text == ClockModel.TimerMode)
        {
            clockModel.timer = RemoveCurrentHour(clockModel.timer);
            newHour = clockView.timerStopwatchHourDropdown.value * ClockModel.MinutesInHour * ClockModel.SecondsInMinute;
        }

        SetValues(clockModel, clockView, newHour, clockView.modeDropdown.captionText.text);
    }

    // Calculates the new hour and current minutes and seconds values to seconds and sets either the time or timer value to this
    private float RemoveCurrentHour(float modeValue)
    {
        int currentHour = 0;

        currentHour = (int)(modeValue / ClockModel.MinutesInHour) / ClockModel.SecondsInMinute;    // In seconds
        modeValue -= currentHour * ClockModel.MinutesInHour * ClockModel.SecondsInMinute;   // Remove the current hour from value

        return modeValue;
    }

    // Calculates the new minute and current hours and seconds values to seconds and sets either the time or timer value to this
    public void SetMinute(ClockModel clockModel, ClockView clockView)
    {
        int newMinute = 0;

        if (clockView.modeDropdown.captionText.text == ClockModel.TimeDisplayMode)
            clockModel.time = RemoveCurrentMinute(clockModel.time);

        else if (clockView.modeDropdown.captionText.text == ClockModel.TimerMode)
            clockModel.timer = RemoveCurrentMinute(clockModel.timer);

        newMinute = clockView.minuteDropdown.value * ClockModel.SecondsInMinute;

        SetValues(clockModel, clockView, newMinute, clockView.modeDropdown.captionText.text);
    }

    // Calculates the new minute and current hours and seconds values to seconds and sets either the time or timer value to this
    private float RemoveCurrentMinute(float modeValue)
    {
        int currentMinute = 0;

        currentMinute = (int)(modeValue / ClockModel.MinutesInHour) % ClockModel.SecondsInMinute;
        modeValue -= currentMinute * ClockModel.SecondsInMinute;

        return modeValue;
    }

    // Calculates the new seconds and current hours and minutes values to seconds and sets either the time or timer value to this
    public void SetSeconds(ClockModel clockModel, ClockView clockView)
    {
        int newSeconds = 0;

        if (clockView.modeDropdown.captionText.text == ClockModel.TimeDisplayMode)
            clockModel.time = RemoveCurrentSecond(clockModel.time);

        else if (clockView.modeDropdown.captionText.text == ClockModel.TimerMode)
            clockModel.timer = RemoveCurrentSecond(clockModel.timer);

        newSeconds = clockView.secondDropdown.value;

        SetValues(clockModel, clockView, newSeconds, clockView.modeDropdown.captionText.text);
    }

    // Calculates the new seconds and current hours and minutes values to seconds and sets either the time or timer value to this
    private float RemoveCurrentSecond(float modeValue)
    {
        int currentSeconds = 0;

        currentSeconds = (int)modeValue % ClockModel.SecondsInMinute;
        modeValue -= currentSeconds;

        return modeValue;
    }

    // Add the selected value to either time or timer
    public void SetValues(ClockModel clockModel, ClockView clockView, int newValueToAdd, string modeSpecificValue)
    {
        if (modeSpecificValue == ClockModel.TimeDisplayMode)
            clockModel.time += newValueToAdd;
        else
            clockModel.timer += newValueToAdd;

        SetNumberDisplay(clockModel, clockView);
        SetClockHands(clockModel, clockView);
    }

    // Allows for the user to set whether the time period is AM or PM and displays the chosen option.
    public void SetPeriod(ClockModel clockModel, ClockView clockView)
    {
        clockView.periodText.text = clockView.periodDropdown.captionText.text;

        if (clockView.periodText.text == "AM")
        {
            // Don't make changes if time is already an AM value
            if (clockModel.period == TimePeriod.PM)
            {
                clockModel.period = TimePeriod.AM;
                clockModel.time -= ClockModel.TwelveHoursInSeconds;
            }
        }
        else
        {
            // Don't make changes if time is already a PM value
            if (clockModel.period == TimePeriod.AM)
            {
                clockModel.period = TimePeriod.PM;
                clockModel.time += ClockModel.TwelveHoursInSeconds;
            }
        }
    }

    // Sets the the bool controlling the current mode type's activity to true.
    public void Run(ClockModel clockModel, ClockView clockView)
    {
        switch (clockView.modeDropdown.captionText.text)
        {
            case ClockModel.TimeDisplayMode:
                clockModel.timeRunning = true;
                break;
            case ClockModel.TimerMode:
                clockModel.timerRunning = true;
                break;
            case ClockModel.StopwatchMode:
                clockModel.stopwatchRunning = true;
                break;
        }
    }

    // Sets the the bool controlling the current mode type's activity to false.
    public void Stop(ClockModel clockModel, ClockView clockView)
    {
        switch (clockView.modeDropdown.captionText.text)
        {
            case ClockModel.TimeDisplayMode:
                clockModel.timeRunning = false;
                break;
            case ClockModel.TimerMode:
                // If user clicks the stop button twice in a row, it will reset the timer
                if (!clockModel.timerRunning)
                {
                    clockModel.timer = 0;
                    clockView.numberText.text = ClockModel.ZeroTimer;
                }
                clockView.source.Stop();
                clockModel.timerRunning = false;
                break;
            case ClockModel.StopwatchMode:
                clockModel.stopwatchRunning = false;
                break;
        }
    }

    // Add time passed to time value and display it on UI
    private void RunClock(ClockModel clockModel, ClockView clockView)
    {
        clockModel.time += Time.deltaTime;
        SetClockHands(clockModel, clockView);
        SetNumberDisplay(clockModel, clockView);
    }

    // Reduce time passed to timer value and display it on UI
    private void RunTimer(ClockModel clockModel, ClockView clockView)
    {
        clockModel.timer -= Time.deltaTime;

        // If the time has reached 0 and the alert has not already started playing, call TimerFinish to play alert
        if (clockModel.timer <= 0)
            if (!clockView.source.isPlaying)
                TimerFinish(clockModel, clockView);

        SetClockHands(clockModel, clockView);
        SetNumberDisplay(clockModel, clockView);
    }

    // Plays the AudioClip passed into this script (used for timer reaching 0)
    private void TimerFinish(ClockModel clockModel, ClockView clockView)
    {
        clockModel.timer = 0;
        clockModel.timerRunning = false;
        clockView.source.Play();
    }

    // Add time passed to stopwatch value and display it on UI
    private void RunStopwatch(ClockModel clockModel, ClockView clockView)
    {
        clockModel.stopwatch += Time.deltaTime;
        SetClockHands(clockModel, clockView);
        SetNumberDisplay(clockModel, clockView);
    }

    // Set the stopwatch value to 0 and display it on UI
    public void ResetStopwatch(ClockModel clockModel, ClockView clockView)
    {
        clockModel.stopwatch = 0;
        SetNumberDisplay(clockModel, clockView);
    }

    // Displays the time, timer or stopwatch value as speficied format (analog)
    private void SetClockHands(ClockModel clockModel, ClockView clockView)
    {
        Quaternion hourRotation = Quaternion.identity;
        Quaternion minuteRotation = Quaternion.identity;
        Quaternion secondRotation = Quaternion.identity;

        float modeValue = 0;
        float segmentValue = 0;

        switch (clockView.modeDropdown.captionText.text)
        {
            case ClockModel.TimeDisplayMode:
                modeValue = clockModel.time;
                segmentValue = ClockModel.HoursOnClock;
                break;
            case ClockModel.TimerMode:
                modeValue = clockModel.timer;
                segmentValue = ClockModel.HoursOnTimerStopwatch;
                break;
            case ClockModel.StopwatchMode:
                modeValue = clockModel.stopwatch;
                segmentValue = ClockModel.HoursOnTimerStopwatch;
                break;
        }

        // Use of euler angles for clock hands reference: https://www.youtube.com/watch?v=pbTysQw-WNs
        // Hour hand
        clockView.analog_HourHand.transform.eulerAngles = new Vector3(0, 0, -(((modeValue / (ClockModel.ClockMultiplier * ClockModel.MinutesInHour)) / segmentValue) 
            / ClockModel.FullRotationDegrees) * ClockModel.TwelveHoursInSeconds);
        // Minute hand
        clockView.analog_MinuteHand.transform.eulerAngles = new Vector3(0, 0, -((modeValue / (ClockModel.ClockMultiplier * ClockModel.SecondsInMinute))
            / ClockModel.FullRotationDegrees) * ClockModel.TwelveHoursInSeconds);
        // Second hand
        clockView.analog_SecondHand.transform.eulerAngles = new Vector3(0, 0, -((modeValue / ClockModel.ClockMultiplier) / ClockModel.FullRotationDegrees)
            * ClockModel.TwelveHoursInSeconds);
    }

    // Displays the time, timer or stopwatch value as speficied format (digital)
    private void SetNumberDisplay(ClockModel clockModel, ClockView clockView)
    {
        switch (clockView.modeDropdown.captionText.text)
        {
            case ClockModel.TimeDisplayMode:
                int timeInt = (int)clockModel.time;
                int timeIntPM = (int)(clockModel.time - ClockModel.TwelveHoursInSeconds);

                // If time is less than or equal to 12 hours in seconds, change the time period
                if (clockModel.time >= 0 && clockModel.time < ClockModel.TwelveHoursInSeconds && clockModel.period == TimePeriod.PM)
                {
                    clockModel.period = TimePeriod.AM;
                    clockView.periodText.text = ClockModel.AM;
                }

                // If time is greater than than or equal to 24 hours in seconds, change the time period
                else if (clockModel.time >= ClockModel.TwelveHoursInSeconds && clockModel.time < (ClockModel.TwelveHoursInSeconds * 2) && clockModel.period == TimePeriod.AM)
                {
                    clockModel.period = TimePeriod.PM;
                    clockView.periodText.text = ClockModel.PM;
                }

                // If time pasts 24hr, refresh to 0
                if (clockModel.time >= ClockModel.TwentyFourHoursInSeconds)
                    clockModel.time -= ClockModel.TwentyFourHoursInSeconds;

                switch (clockView.timeDropdown.captionText.text)
                {
                    case ClockModel.TwentyFour_HourMinuteFormat:
                        DisplayDigital(clockView, ClockModel.HourMinuteDisplay, timeInt);
                        break;
                    case ClockModel.Twelve_HourMinuteFormat:
                        // Check time period to see if hour values need to be reduced for 12hr time
                        if (clockModel.period == TimePeriod.PM)
                            Adjust12HourTime(clockView, timeIntPM, ClockModel.HourMinuteDisplay);
                        else
                            Adjust12HourTime(clockView, timeInt, ClockModel.HourMinuteDisplay);
                        break;
                    case ClockModel.TwentyFour_HourMinuteSecondsFormat:
                        DisplayDigital(clockView, ClockModel.HourMinuteSecondDisplay, timeInt);
                        break;
                    case ClockModel.Twelve_HourMinuteSecondsFormat:
                        // Check time period to see if hour values need to be reduced for 12hr time
                        if (clockModel.period == TimePeriod.PM)
                            Adjust12HourTime(clockView, timeIntPM, ClockModel.HourMinuteSecondDisplay);
                        else
                            Adjust12HourTime(clockView, timeInt, ClockModel.HourMinuteSecondDisplay);
                        break;
                }

                break;
            case ClockModel.TimerMode:
                int timerInt = (int)clockModel.timer;

                // Displays the timer value in 00:00:00 format if timer value is 0 or greater
                if (timerInt >= 0)
                    DisplayDigital(clockView, ClockModel.HourMinuteSecondDisplay, timerInt);

                break;
            case ClockModel.StopwatchMode:
                int stopwatchInt = (int)clockModel.stopwatch;

                // Creating a variable where the decimal values of stopwatch can be used as an integer (needed for display code)
                float stopwatchDecimals = (clockModel.stopwatch - stopwatchInt) * 100;
                int stopwatchIntDecimals = (int)stopwatchDecimals;

                // Displays the stopwatch value in 00:00:00.00 format
                DisplayDigital(clockView, ClockModel.HourMinuteSecondMillisecondDisplay, stopwatchInt, false, stopwatchIntDecimals);

                break;
        }
    }

    // Check if 0 hrs and convert display to 12 instead 
    private void Adjust12HourTime(ClockView clockView, int timeInt, string format)
    {
        // Uses tempTime (additional 12hrs added to time) while the hour is 0 to display 12:00
        if (((timeInt / ClockModel.SecondsInMinute) / ClockModel.MinutesInHour) == 0)
            DisplayDigital(clockView, format, timeInt, true);

        // Displays the actual time while the hour is not 0
        else
            DisplayDigital(clockView, format, timeInt);
    }

    // Displays the digital display, using parameters for format (e.g. 00:00, 00:00:00, etc), modeValue (time, timer or stopwatch as an integer), tempValue (whether the hour is at 0 or not)
    // and stopwatchIntDecimals (the decimal values as an int for the stopwatch)
    private void DisplayDigital(ClockView clockView, string format, int modeValue, bool tempVal = false, int stopwatchIntDecimals = 0)
    {
        int usedHourModeValue;

        if (tempVal)
            usedHourModeValue = modeValue + ClockModel.TwelveHoursInSeconds;
        else
            usedHourModeValue = modeValue;

        // Check the type of format used for this display and set appropriately
        switch (format)
        {
            case ClockModel.HourMinuteDisplay:
                clockView.numberText.text = string.Format(format, ((usedHourModeValue / ClockModel.SecondsInMinute) / ClockModel.MinutesInHour),
                    ((modeValue / ClockModel.SecondsInMinute) % ClockModel.MinutesInHour));
                break;
            case ClockModel.HourMinuteSecondDisplay:
                clockView.numberText.text = string.Format(format, ((usedHourModeValue / ClockModel.SecondsInMinute) / ClockModel.MinutesInHour), 
                    ((modeValue / ClockModel.SecondsInMinute) % ClockModel.MinutesInHour), (modeValue % ClockModel.SecondsInMinute));
                break;
            case ClockModel.HourMinuteSecondMillisecondDisplay:
                clockView.numberText.text = string.Format(ClockModel.HourMinuteSecondMillisecondDisplay, ((modeValue / ClockModel.SecondsInMinute) / ClockModel.MinutesInHour), 
                    ((modeValue / ClockModel.SecondsInMinute) % ClockModel.MinutesInHour), (modeValue % ClockModel.SecondsInMinute), (stopwatchIntDecimals % ClockModel.MillisecondsInSecond));
                break;
        }
    }
}
