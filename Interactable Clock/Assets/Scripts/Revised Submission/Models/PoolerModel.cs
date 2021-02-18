using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolerModel : InteractableClockElement
{
    // Pool class to store the prefab to instantiate and the max size of the pool
    [System.Serializable]
    public class Pool
    {
        public GameObject clockPrefab;
        public int size;
    }

    public Pool pool;
    [HideInInspector] public Queue<GameObject> clockPool;

    [HideInInspector] public Collider2D clockPrefabCollider;
    [HideInInspector] public Rigidbody2D clockPrefabRigidbody;

    [HideInInspector] public List<Collider2D> allColliders;
    [HideInInspector] public List<Rigidbody2D> allRigidbodies;
}
