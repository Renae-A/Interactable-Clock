using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

// Base class for all elements in this application.
public class InteractableClockElement : MonoBehaviour
{
    private InteractableClockApplication application;

    // Gives access to the application and all instances.
    public InteractableClockApplication app { get { return application; } }

    private void Awake()
    {
        application = GameObject.FindObjectOfType<InteractableClockApplication>();
    }
}

// The application class for this program, Interactable Clock
public class InteractableClockApplication : MonoBehaviour
{
    // References to instances of MVC for this application
    [SerializeField] public InteractableClockModel model;
    [SerializeField] public InteractableClockController controller;
    [SerializeField] public InteractableClockView view;

    // Init processes here
    void Start()
    {
        model = GetComponentInChildren<InteractableClockModel>();
        controller = GetComponentInChildren<InteractableClockController>();
        view = GetComponentInChildren<InteractableClockView>();

        // Set up references to the clock manager
        model.clockManager = GetComponentInChildren<ClockManagerModel>();
        view.clockManager = GetComponentInChildren<ClockManagerView>();
        controller.clockManager = GetComponentInChildren<ClockManagerController>();

        // Set up references to the pooler
        model.pooler = GetComponentInChildren<PoolerModel>();
        controller.pooler = GetComponentInChildren<PoolerController>();

        // Set up references to the clock
        model.clockTemplate = GetComponentInChildren<ClockModel>();
        controller.clock = GetComponentInChildren<ClockController>();

        // Intialise values ClockManager and Pooler
        SetUpPooler();
        SetUpClockManager();

        // Set up draggable controller reference
        controller.draggable = GetComponentInChildren<DraggableController>();
    }

    // Notify function takes a string notification message and sends it to the Interactable Clock Controller
    public void Notify(string eventPath)
    {
        controller.OnNotification(eventPath);
    }

    // Overloaded Notify function takes a string notification message and target GameObject to apply changes to and sends it to the Interactable Clock Controller
    public void Notify(string eventPath, GameObject target)
    {
        controller.OnNotification(eventPath, target);
    }

    // Overloaded Notify function takes a string notification message and target index value of clock from all lists to apply changes to and sends it to the Interactable Clock Controller
    public void Notify(string eventPath, int targetIndex)
    {
        controller.OnNotification(eventPath, targetIndex);
    }

    // Overloaded Notify function takes a string notification message, object that triggered the notification, target GameObject to apply changes to and the PointerEventData that 
    // triggered this notification and sends it to the Interactable Clock Controller
    public void Notify(string eventPath, GameObject triggeredObject, GameObject target, PointerEventData eventData)
    {
        controller.OnNotification(eventPath, triggeredObject, target, eventData);
    }

    // Overloaded Notify function takes a string notification message and the PointerEventData that triggered this notification and sends it to the Interactable Clock Controller
    public void Notify(string eventPath, PointerEventData eventData)
    {
        controller.OnNotification(eventPath, eventData);
    }

    // SET UP OF ALL CLASSES
    // Setup and intialise ClockManager
    private void SetUpClockManager()
    {
        view.clockManager.spawnedClocks = new List<GameObject>();

        view.clockManager.grid = view.clockManager.GetComponent<GridLayoutGroup>();

        // Spawn first clock
        Notify(InteractableClockNotification.SpawnClock);

        // Stop the user from interacting with the remove button (until new clock is spawned)
        view.clockManager.spawnedClocks[0].GetComponentInChildren<Button>().interactable = false;
    }

    // Setup and intialise Clock
    public void SetUpClock(ClockModel clockModel, ClockView clockView)
    {
        clockView.source = clockView.GetComponent<AudioSource>();
        clockView.source.clip = clockModel.timerSound;

        // Organises and sets up all variables of the clock (saving the user from having to pass them in manually)
        Transform[] childTransforms = clockView.GetComponentsInChildren<Transform>();

        foreach (Transform t in childTransforms)
        {
            switch (t.tag)
            {
                // ANALOG
                case "Analog":
                    clockView.analog = t.gameObject;
                    break;
                case "Analog Time":
                    clockView.analog_Time = t.gameObject;
                    break;
                case "Analog Timer":
                    clockView.analog_TimerStopwatch = t.gameObject;
                    break;
                case "Hour":
                    clockView.analog_HourHand = t.gameObject;
                    break;
                case "Minute":
                    clockView.analog_MinuteHand = t.gameObject;
                    break;
                case "Second":
                    clockView.analog_SecondHand = t.gameObject;
                    break;
                // DIGITAL
                case "Digital":
                    clockView.digital = t.gameObject;
                    break;
                case "Digital Numbers":
                    clockView.digital_Numbers = t.gameObject;
                    break;
                case "Digital Period":
                    clockView.digital_Period = t.gameObject;
                    break;
                // SETTINGS
                case "Hideable Settings":
                    clockView.hideableSettings = t.gameObject;
                    break;
                case "Mode":
                    clockView.mode = t.gameObject;
                    break;
                case "Time Format":
                    clockView.timeFormat = t.gameObject;
                    break;
                case "Timer Format":
                    clockView.timerStopwatchFormat = t.gameObject;
                    break;
                case "Set Values":
                    clockView.setValues = t.gameObject;
                    break;
                case "Time Hour 12":
                    clockView.timeHour12 = t.gameObject;
                    break;
                case "Time Hour 24":
                    clockView.timeHour24 = t.gameObject;
                    break;
                case "Timer Hour Value":
                    clockView.timerStopwatchHourValue = t.gameObject;
                    break;
                case "Minute Value":
                    clockView.minuteValue = t.gameObject;
                    break;
                case "Second Value":
                    clockView.secondValue = t.gameObject;
                    break;
                case "Period Value":
                    clockView.periodValue = t.gameObject;
                    break;
                case "Stopwatch Settings":
                    clockView.stopwatchSettings = t.gameObject;
                    break;
                case "Start and Stop":
                    clockView.startAndStop = t.gameObject;
                    break;
            }
        }

        clockView.numberTransform = clockView.digital_Numbers.GetComponent<RectTransform>();

        clockView.modeDropdown = clockView.mode.GetComponent<TMP_Dropdown>();
        clockView.timeDropdown = clockView.timeFormat.GetComponent<TMP_Dropdown>();
        clockView.timerStopwatchDropdown = clockView.timerStopwatchFormat.GetComponent<TMP_Dropdown>();

        clockView.timeHour24Dropdown = clockView.timeHour24.GetComponent<TMP_Dropdown>();
        clockView.timeHour12Dropdown = clockView.timeHour12.GetComponent<TMP_Dropdown>();
        clockView.timerStopwatchHourDropdown = clockView.timerStopwatchHourValue.GetComponent<TMP_Dropdown>();
        clockView.minuteDropdown = clockView.minuteValue.GetComponent<TMP_Dropdown>();
        clockView.secondDropdown = clockView.secondValue.GetComponent<TMP_Dropdown>();
        clockView.periodDropdown = clockView.periodValue.GetComponent<TMP_Dropdown>();

        clockView.numberText = clockView.digital_Numbers.GetComponent<TextMeshProUGUI>();
        clockView.numberTransform = clockView.numberText.GetComponent<RectTransform>();
        clockView.periodText = clockView.digital_Period.GetComponent<TextMeshProUGUI>();
    }

    // Setup and intialise Pooler
    private void SetUpPooler()
    {
        // Setup clock Prefab collider reference and enable the collider at the start of the application
        model.pooler.clockPrefabCollider = model.pooler.pool.clockPrefab.GetComponent<Collider2D>();
        model.pooler.clockPrefabCollider.enabled = true;

        // Setup clock Prefab rigidbody reference and enable the rigidbody at the start of the application
        model.pooler.clockPrefabRigidbody = model.pooler.pool.clockPrefab.GetComponent<Rigidbody2D>();
        model.pooler.clockPrefabRigidbody.isKinematic = false;

        model.pooler.clockPool = new Queue<GameObject>();

        // Fill the pool with 32 clocks and set their active to false
        for (int i = 0; i < model.pooler.pool.size; i++)
        {
            GameObject clock = Instantiate(model.pooler.pool.clockPrefab, view.clockManager.transform);
            clock.SetActive(false);
            model.pooler.clockPool.Enqueue(clock);
        }

        // Set up reference to all clock colliders
        Notify(InteractableClockNotification.GetAllClockCollidersAndRigidbodies);
    }
}
