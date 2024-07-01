using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectPool;
    [SerializeField] private int poolSize;

    private Queue<GameObject> pool;

    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        pool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            var objectInstance = Instantiate(objectPool);
            objectInstance.SetActive(false);
            pool.Enqueue(objectInstance);
        }
    }

    public GameObject GetBulletFromPool(Vector3 position, Quaternion rotation)
    {
        var objectInstance = pool.Dequeue();
        objectInstance.transform.position = position;
        objectInstance.transform.rotation = rotation;
        objectInstance.SetActive(true);

        return objectInstance;

    }

    public void ReturnBulletToPool(GameObject objectInstance)
    {
        if (objectInstance != null)
        {
            // Deactivate the object
            objectInstance.SetActive(false);

            // Reset velocity if it has Rigidbody
            Rigidbody rb = objectInstance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // Reset position and rotation
            objectInstance.transform.position = Vector3.zero;
            objectInstance.transform.rotation = Quaternion.identity;

            // Return to pool
            pool.Enqueue(objectInstance);
        }
    }
}



