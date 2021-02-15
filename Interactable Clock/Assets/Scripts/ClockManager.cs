using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockManager : MonoBehaviour
{
    public GameObject clockPrefab;
    private Collider2D clockPrefabCollider;

    private List<GameObject> spawnedClocks;

    private GridLayoutGroup grid;
    private Toggle gridViewToggle;

    private const int HalfClockWidthAndHeight = 50;

    // Start is called before the first frame update
    void Start()
    {
        spawnedClocks = new List<GameObject>();

        grid = GetComponent<GridLayoutGroup>();

        clockPrefabCollider = clockPrefab.GetComponent<Collider2D>();
        clockPrefabCollider.enabled = true;

        // Find the clock already in the scene and add it to the list of spawned clocks
        GameObject initialClock = GetComponentInChildren<Clock>().gameObject;
        spawnedClocks.Add(initialClock);

        // Stop the user from interacting with the remove button (until new clock is spawned)
        spawnedClocks[0].GetComponentInChildren<Button>().interactable = false;
    }

    //	Instantiates a new clock, using a clock prefab passed into this script and adds the clock to the list of spawned clocks.
    public void SpawnClock()
    {
        // Cap number of clocks allowed to be spawned at 32 if on grid view
        if (spawnedClocks.Count >= 32 && grid.enabled) { }

        else
        {
            GameObject newClock = Instantiate(clockPrefab, transform);
            spawnedClocks.Add(newClock);
        }

        // Allow for any clock to be removed if there is more than one clock in the scene
        if (spawnedClocks.Count > 1)
            spawnedClocks[0].GetComponentInChildren<Button>().interactable = true;
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
        Destroy(clock);

        // Stop the user from removing the last clock in the scene 
        if (spawnedClocks.Count == 1)
            spawnedClocks[0].GetComponentInChildren<Button>().interactable = false;
    }

    // Turn on/off every clock's collider based on Autofit Toggle setting
    public void ToggleAutofit()
    {
        clockPrefabCollider.enabled = !clockPrefabCollider.enabled;

        foreach (GameObject clock in spawnedClocks)
        {
            Collider2D clockCollider = clock.GetComponent<Collider2D>();
            clockCollider.enabled = !clockCollider.enabled;
        }
    }
}
