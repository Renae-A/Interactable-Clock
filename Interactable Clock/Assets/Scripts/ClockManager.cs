using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockManager : MonoBehaviour
{
    public GameObject clockPrefab;

    private List<GameObject> spawnedClocks;

    private bool lastClock;

    // Start is called before the first frame update
    void Start()
    {
        spawnedClocks = new List<GameObject>();

        // Find the clock already in the scene and add it to the list of spawned clocks
        GameObject initialClock = GetComponentInChildren<Clock>().gameObject;
        spawnedClocks.Add(initialClock);

        // Stop the user from interacting with the remove button (until new clock is spawned)
        spawnedClocks[0].GetComponentInChildren<Button>().interactable = false;
        lastClock = true;

        lastClock = true;
    }

    //	Instantiates a new clock, using a clock prefab passed into this script and adds the clock to the list of spawned clocks.
    public void SpawnClock()
    {
        GameObject newClock = Instantiate(clockPrefab, transform);
        spawnedClocks.Add(newClock);

        // Allow for any clock to be removed if there is more than one clock in the scene
        if (spawnedClocks.Count > 1 && lastClock)
        {
            spawnedClocks[0].GetComponentInChildren<Button>().interactable = true;
            lastClock = false;
        }
    }

    // Removes the clock passed into this function from the list of spawned clocks and deletes the clock GameObject.
    public void RemoveClock(GameObject clock)
    {
        spawnedClocks.Remove(clock);
        Destroy(clock);

        // Stop the user from removing the last clock in the scene 
        if (spawnedClocks.Count == 1 && !lastClock)
        {
            spawnedClocks[0].GetComponentInChildren<Button>().interactable = false;
            lastClock = true;
        }
    }
}
