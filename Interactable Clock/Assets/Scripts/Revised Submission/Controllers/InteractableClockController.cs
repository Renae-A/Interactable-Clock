using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableClockController : InteractableClockElement
{
    [SerializeField] public ClockManagerController clockManager;
    [SerializeField] public ClockController clock;
    [SerializeField] public PoolerController pooler;
    [SerializeField] public DraggableController draggable;

    // Calls the appropriate controller function based on eventPath message
    public void OnNotification(string eventPath)
    {
        switch (eventPath)
        {
            // Exits out of the application.
            case InteractableClockNotification.QuitApplication:
                Application.Quit();
                break;
            // Instantiates a new clock, using a clock prefab passed into this script and adds the clock to the list of spawned clocks.
            case InteractableClockNotification.SpawnClock:
                clockManager.SpawnClock();
                break;
            // Turn on/off every clock's collider based on Autofit Toggle setting.
            case InteractableClockNotification.ToggleAutofit:
                clockManager.ToggleAutofit();
                break;
        }
    }

    // Overloaded function calls the appropriate controller function based on eventPath message and passes in the GameObject that will have changes made to it.
    public void OnNotification(string eventPath, GameObject target)
    {
        switch (eventPath)
        {
            // Adds clock to spawnedClocks list and creates/adds ClockView and ClockModel to respective lists, generates a position for the clock, resets clock index and initialises Clock values.
            case InteractableClockNotification.OnClockEnable:
                clock.OnClockEnable(target);
                break;
            // Removes clock from spawnedClocks the lists: GameObject, ClockView and Models in the Clock Manager.
            case InteractableClockNotification.OnClockDisable:
                clock.OnClockDisable(target);
                break;
            // Generate a random position on the screen and sets the clock position to this.
            case InteractableClockNotification.GeneratePosition:
                clockManager.GenerateNewPosition(target);
                break;
            // Removes the clock passed into this function from the list of spawned clocks and deletes the clock GameObject.
            case InteractableClockNotification.RemoveClock:
                clockManager.RemoveClock(target);
                break;    
        }
    }

    // Overloaded function calls the appropriate controller function based on eventPath message and passes in the index value for the specific clock that will have changes made to it.
    public void OnNotification(string eventPath, int clockIndex)
    {
        switch (eventPath)
        {
            // Update is called once per frame, calls RunClock(), RunTimer() and RunStopwatch() if these functionalities are set to run.
            case InteractableClockNotification.UpdateClock:
                clock.UpdateClock(clockManager.GetClockModel(clockIndex), clockManager.GetClockView(clockIndex));
                break;
            // Either hides or shows the additional settings options on button click.
            case InteractableClockNotification.ToggleSettings:
                clock.ToggleSettings(clockManager.GetClockView(clockIndex));
                break;
            // Changes whether the clock is a time display, countdown timer or stopwatch (hides/reveals appropriate objects).
            case InteractableClockNotification.SetClockMode:
                clock.SetMode(clockManager.GetClockModel(clockIndex), clockManager.GetClockView(clockIndex));
                break;
            // Changes whether the clock is analog or digital in a variety of time formats (hides/reveals appropriate objects).
            case InteractableClockNotification.SetTimeFormat:
                clock.SetTimeFormat(clockManager.GetClockModel(clockIndex), clockManager.GetClockView(clockIndex));
                break;
            // Changes whether the clock is analog or digital (hides/reveals appropriate objects).
            case InteractableClockNotification.SetTimerStopwatchFormat:
                clock.SetTimerStopwatchFormat(clockManager.GetClockModel(clockIndex), clockManager.GetClockView(clockIndex));
                break;
            // Calculates the new hour and current minutes and seconds values to seconds and sets either the time or timer value to this.
            case InteractableClockNotification.SetHour:
                clock.SetHour(clockManager.GetClockModel(clockIndex), clockManager.GetClockView(clockIndex));
                break;
            // Calculates the new minute and current hours and seconds values to seconds and sets either the time or timer value to this.
            case InteractableClockNotification.SetMinute:
                clock.SetMinute(clockManager.GetClockModel(clockIndex), clockManager.GetClockView(clockIndex));
                break;
            // Calculates the new seconds and current hours and minutes values to seconds and sets either the time or timer value to this.
            case InteractableClockNotification.SetSecond:
                clock.SetSeconds(clockManager.GetClockModel(clockIndex), clockManager.GetClockView(clockIndex));
                break;
            // Allows for the user to set whether the time period is AM or PM and displays the chosen option.
            case InteractableClockNotification.SetPeriod:
                clock.SetPeriod(clockManager.GetClockModel(clockIndex), clockManager.GetClockView(clockIndex));
                break;
            // Sets the the bool controlling the current mode type's activity to true.
            case InteractableClockNotification.Run:
                clock.Run(clockManager.GetClockModel(clockIndex), clockManager.GetClockView(clockIndex));
                break;
            // Sets the the bool controlling the current mode type's activity to false.
            case InteractableClockNotification.Stop:
                clock.Stop(clockManager.GetClockModel(clockIndex), clockManager.GetClockView(clockIndex));
                break;
            // Set the stopwatch value to 0 and display it on UI.
            case InteractableClockNotification.ResetStopwatch:
                clock.ResetStopwatch(clockManager.GetClockModel(clockIndex), clockManager.GetClockView(clockIndex));
                break;
        }
    }

    // Overloaded function calls the appropriate controller function based on eventPath message, takes the object that triggered the notification, the GameObject that will have changes made to it and the PointerEventData that triggered
    // the notification.
    public void OnNotification(string eventPath, GameObject objectTriggered, GameObject target, PointerEventData eventData)
    {
        // Moves the UI Clock’s current position to the desired position on canvas within window of application.
        if (eventPath == InteractableClockNotification.OnDrag)
            draggable.OnClockDrag(eventData, objectTriggered, target);
    }

    // Overloaded function calls the appropriate controller function based on eventPath message and the PointerEventData that triggered the notification.
    public void OnNotification(string eventPath, PointerEventData eventData)
    {
        // Unlock mouse when the user stops dragging
        if (eventPath == InteractableClockNotification.OnRelease)
            draggable.OnClockEndDrag(eventData);
    }
}
