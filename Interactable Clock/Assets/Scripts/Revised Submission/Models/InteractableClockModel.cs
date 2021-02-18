using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableClockModel : InteractableClockElement
{
    // Data
    [SerializeField] public ClockManagerModel clockManager;
    [SerializeField] public ClockModel clockTemplate;       // Template model used when creating new clocks
    [SerializeField] public PoolerModel pooler;
}
