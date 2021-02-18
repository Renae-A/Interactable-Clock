using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Object Pooling reference: https://www.youtube.com/watch?v=tdSmKaJvCoA
public class Pooler : MonoBehaviour
{
    // Pool class to store the prefab to instantiate and the max size of the pool
    [System.Serializable]
    public class Pool
    {
        public GameObject clockPrefab;
        public int size;
    }

    public Pool pool;
    public Queue<GameObject> clockPool;

    private Collider2D clockPrefabCollider;

    public List<Collider2D> allColliders;

    // Setup and intialise Pooler
    void Awake()
    {
        // Setup clock Prefab collider reference and enable the collider at the start of the application
        clockPrefabCollider = pool.clockPrefab.GetComponent<Collider2D>();
        clockPrefabCollider.enabled = true;

        clockPool = new Queue<GameObject>();

        // Fill the pool with 32 clocks and set their active to false
        for (int i = 0; i < pool.size; i++)
        {
            GameObject clock = Instantiate(pool.clockPrefab, transform);
            clock.SetActive(false);
            clockPool.Enqueue(clock);
        }

        // Set up reference to all clock colliders
        allColliders = GetAllPoolClockColliders();
    }

    // Removes a clock from the pool and sets it active, returns the relevant clock
    public GameObject SpawnClockFromPool()
    {
        GameObject clockToSpawn = clockPool.Dequeue();

        clockToSpawn.SetActive(true);

        return clockToSpawn;
    }

    // Turns on/off the prefab's collider 
    public void ToggleClockPrefabCollider()
    {
        clockPrefabCollider.enabled = !clockPrefabCollider.enabled;
    }

    // Creates an empty list of 2D colliders and adds all spawned and non-spanwed clock colliders to the list, returns the list
    private List<Collider2D> GetAllPoolClockColliders()
    {
        List<Collider2D> colliders = new List<Collider2D>();

        foreach (GameObject clock in clockPool)
        {
            colliders.Add(clock.GetComponent<Collider2D>());
        }

        return colliders;
    }
}