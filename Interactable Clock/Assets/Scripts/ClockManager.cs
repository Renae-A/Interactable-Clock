using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockManager : MonoBehaviour
{
    public Button spawnClockButton;

    // Components
    private GridLayoutGroup grid;
    private Pooler pooler;

    // Visible clocks in scene
    private List<GameObject> spawnedClocks;

    private const int HalfClockWidthAndHeight = 50;

    // Start is called before the first frame update
    void Start()
    {
        spawnedClocks = new List<GameObject>();

        grid = GetComponent<GridLayoutGroup>();
        pooler = GetComponent<Pooler>();

        // Spawn first clock
        SpawnClock();

        // Stop the user from interacting with the remove button (until new clock is spawned)
        spawnedClocks[0].GetComponentInChildren<Button>().interactable = false;  
    }

    //	Instantiates a new clock, using a clock prefab passed into this script and adds the clock to the list of spawned clocks.
    public void SpawnClock()
    {
        GameObject newClock = pooler.SpawnClockFromPool();

        // Only run the follwing code if clock was spawned
        if (newClock)
        {
            spawnedClocks.Add(newClock);

            // Allow for any clock to be removed if there is more than one clock in the scene
            if (spawnedClocks.Count == 2)
                spawnedClocks[0].GetComponentInChildren<Button>().interactable = true;

            // Stop clock spawns at 32 clocks
            else if (spawnedClocks.Count == 32)
                spawnClockButton.interactable = false;

            newClock.transform.SetAsLastSibling();
        }
    }

    // Generate a position that does not collide with another clock
    public void GenerateNewPosition(GameObject clock)
    {
        Vector2 screenCentre = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 pos = screenCentre + new Vector2(Random.Range(-(Screen.width / 2) + HalfClockWidthAndHeight, (Screen.width / 2) - HalfClockWidthAndHeight),
            Random.Range(-(Screen.height / 2) + HalfClockWidthAndHeight, (Screen.height / 2) - HalfClockWidthAndHeight));

        clock.transform.position = pos;
    }

    // Removes the clock passed into this function from the list of spawned clocks and deletes the clock GameObject.
    public void RemoveClock(GameObject clock)
    {
        spawnedClocks.Remove(clock);
        clock.SetActive(false);

        pooler.clockPool.Enqueue(clock);

        // Stop the user from removing the last clock in the scene 
        if (spawnedClocks.Count == 1)
            spawnedClocks[0].GetComponentInChildren<Button>().interactable = false;

        // Allow clock spawns at 31 or less clocks
        if (spawnedClocks.Count == 31)
            spawnClockButton.interactable = true;
    }

    // Turn on/off every clock's collider based on Autofit Toggle setting
    public void ToggleAutofit()
    {
        pooler.ToggleClockPrefabCollider();

        foreach (Collider2D clockCollider in pooler.allColliders)  
            clockCollider.enabled = !clockCollider.enabled;
    }

    private float runningTotal = 0f;
    private int frameCount = 0;
    private float timer = 0f;
    public Text fpsText;

    private void Update()
    {
        float fps = 1.0f / Time.smoothDeltaTime;
        timer += Time.deltaTime;
        float averagefps = runningTotal / frameCount;

        if (timer <= 1.0f)
        {
            runningTotal += fps;
            frameCount++;
        }
        else
        {
            fpsText.text = averagefps.ToString();
            runningTotal = 0f;
            frameCount = 0;
            timer = 0f;
        }
    }
}
