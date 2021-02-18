using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableClockView : InteractableClockElement
{
    // Reference to the objects
    public ClockManagerView clockManager;
    public ClockView clockTemplate;         // Template view used when creating new clocks
}
