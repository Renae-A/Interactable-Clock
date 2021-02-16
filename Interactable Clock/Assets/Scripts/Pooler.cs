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

    // Start is called before the first frame update
    void Start()
    {
        clockPrefabCollider = pool.clockPrefab.GetComponent<Collider2D>();
        clockPrefabCollider.enabled = true;

        clockPool = new Queue<GameObject>();

        for (int i = 0; i < pool.size; i++)
        {
            GameObject clock = Instantiate(pool.clockPrefab, transform);
            clock.SetActive(false);
            clockPool.Enqueue(clock);
        }

        allColliders = GetAllPoolClockColliders();
    }

    public GameObject SpawnClockFromPool()
    {
        GameObject clockToSpawn = clockPool.Dequeue();

        clockToSpawn.SetActive(true);

        return clockToSpawn;
    }

    public void ToggleClockPrefabCollider()
    {
        clockPrefabCollider.enabled = !clockPrefabCollider.enabled;
    }

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