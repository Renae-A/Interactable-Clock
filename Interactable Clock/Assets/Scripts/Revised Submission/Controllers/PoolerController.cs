using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolerController : InteractableClockElement
{
    // Removes a clock from the pool and sets it active, returns the relevant clock
    public GameObject SpawnClockFromPool()
    {
        GameObject clockToSpawn = app.model.pooler.clockPool.Dequeue();

        clockToSpawn.SetActive(true);

        return clockToSpawn;
    }

    // Turns on/off the prefab's collider and rigidbody. Resets pooler's list of all the clocks' colliders and rigidbodies
    public void ToggleClockPrefabColliderAndRigidbody()
    {
        app.model.pooler.clockPrefabCollider.enabled = !app.model.pooler.clockPrefabCollider.enabled;
        app.model.pooler.clockPrefabRigidbody.isKinematic = !app.model.pooler.clockPrefabRigidbody.isKinematic;

        app.model.pooler.allColliders = GetAllClockColliders();
        app.model.pooler.allRigidbodies = GetAllRigidbodies();
    }

    // Creates an empty list of 2D colliders and adds all spawned and non-spanwed clock colliders to the list, returns the list
    public List<Collider2D> GetAllClockColliders()
    {
        List<Collider2D> colliders = new List<Collider2D>();

        foreach (GameObject clock in app.model.pooler.clockPool)
            colliders.Add(clock.GetComponent<Collider2D>());
        foreach (GameObject clock in app.view.clockManager.spawnedClocks)
            colliders.Add(clock.GetComponent<Collider2D>());

        return colliders;
    }

    // Creates an empty list of 2D rigidbodies and adds all spawned and non-spanwed clock rigidbodies to the list, returns the list
    public List<Rigidbody2D> GetAllRigidbodies()
    {
        List<Rigidbody2D> rigidbodies = new List<Rigidbody2D>();

        foreach (GameObject clock in app.model.pooler.clockPool)
            rigidbodies.Add(clock.GetComponent<Rigidbody2D>());
        foreach (GameObject clock in app.view.clockManager.spawnedClocks)
            rigidbodies.Add(clock.GetComponent<Rigidbody2D>());

        return rigidbodies;
    }
}
