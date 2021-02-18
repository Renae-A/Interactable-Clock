using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockManagerView : MonoBehaviour
{
    public List<ClockView> spawnedClockViews = new List<ClockView>();

    // Visible clocks in scene
    [HideInInspector] public List<GameObject> spawnedClocks;

    // Button to trigger the spawn of a new clock
    public Button spawnClockButton;

    // Components
    [HideInInspector] public GridLayoutGroup grid;
}
