using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableClockNotification : MonoBehaviour
{
    // Application
    public const string QuitApplication = "app.quit";

    // Clock Manager
    public const string SpawnClock = "spawn.clock";
    public const string RemoveClock = "remove.clock";
    public const string GeneratePosition = "generation.position";
    public const string ToggleAutofit = "toggle.autofit";

    // Pooler
    public const string GetAllClockCollidersAndRigidbodies = "get.colliders.rigidbodies";

    // Clock
    public const string OnClockEnable = "clock.enable";
    public const string OnClockDisable = "clock.disable";
    public const string UpdateClock = "clock.update";
    public const string ToggleSettings = "toggle.settings";
    public const string SetClockMode = "set.mode";
    public const string SetTimeFormat = "set.time.format";
    public const string SetTimerStopwatchFormat = "set.timer.format";
    public const string SetHour = "set.hour";
    public const string SetMinute = "set.minute";
    public const string SetSecond = "set.seconds";
    public const string SetPeriod = "set.period";
    public const string Run = "run.clock";
    public const string Stop = "stop.clock";
    public const string ResetStopwatch = "reset.stopwatch";

    // Draggable
    public const string OnDrag = "drag";
    public const string OnRelease = "release";
}
