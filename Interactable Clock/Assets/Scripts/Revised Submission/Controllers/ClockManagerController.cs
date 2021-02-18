using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockManagerController : InteractableClockElement
{
    // Returns a specific clock's index - used to refer to same clock in view and model spwanedClock lists
    public int GetClockIndex(GameObject clock)
    {
        int clockIndex = app.view.clockManager.spawnedClocks.IndexOf(clock);
        return clockIndex;
    }

    // Returns a specific clock view for specific clock
    public ClockView GetClockView(int clockIndex)
    {
        return app.view.clockManager.spawnedClockViews[clockIndex];
    }

    // Returns a specific clock model for specific clock
    public ClockModel GetClockModel(int clockIndex)
    {
        return app.model.clockManager.spawnedClockModels[clockIndex];
    }

    //	Instantiates a new clock, using a clock prefab passed into this script and adds the clock to the list of spawned clocks.
    public void SpawnClock()
    {
        GameObject newClock = app.controller.pooler.SpawnClockFromPool();

        // Only run the following code if clock was spawned
        if (newClock)
        {
            // Allow for any clock to be removed if there is more than one clock in the scene
            if (app.view.clockManager.spawnedClocks.Count == 2)
                app.view.clockManager.spawnedClocks[0].GetComponentInChildren<Button>().interactable = true;

            // Stop clock spawns at 32 clocks
            else if (app.view.clockManager.spawnedClocks.Count == 32)
                app.view.clockManager.spawnClockButton.interactable = false;

            newClock.transform.SetAsLastSibling();
        }
    }

    // Generate a random position on the screen and sets the clock position to this
    public void GenerateNewPosition(GameObject clock)
    {
        Vector2 screenCentre = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 pos = screenCentre + new Vector2(Random.Range(-(Screen.width / 2) + ClockModel.HalfClockWidthAndHeight, (Screen.width / 2) - ClockModel.HalfClockWidthAndHeight),
            Random.Range(-(Screen.height / 2) + ClockModel.HalfClockWidthAndHeight, (Screen.height / 2) - ClockModel.HalfClockWidthAndHeight));

        clock.transform.position = pos;
    }

    // Removes the clock passed into this function from the list of spawned clocks and deletes the clock GameObject.
    public void RemoveClock(GameObject clock)
    {
        clock.SetActive(false);

        app.model.pooler.clockPool.Enqueue(clock);

        // Stop the user from removing the last clock in the scene 
        if (app.view.clockManager.spawnedClocks.Count == 1)
            app.view.clockManager.spawnedClocks[0].GetComponentInChildren<Button>().interactable = false;

        // Allow clock spawns at 31 or less clocks
        if (app.view.clockManager.spawnedClocks.Count == 31)
            app.view.clockManager.spawnClockButton.interactable = true;
    }

    // Turn on/off every clock's collider based on Autofit Toggle setting
    public void ToggleAutofit()
    {
        app.controller.pooler.ToggleClockPrefabColliderAndRigidbody();

        foreach (Collider2D clockCollider in app.model.pooler.allColliders)
            clockCollider.enabled = !clockCollider.enabled;

        foreach (Rigidbody2D clockRigidbody in app.model.pooler.allRigidbodies)
            clockRigidbody.isKinematic = !clockRigidbody.isKinematic;
    }
}
