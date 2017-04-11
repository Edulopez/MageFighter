using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooling : MonoBehaviour {

    public static ObjectPooling Current;
    public int size = 10;

    public GameObject pooledObject;
    public List<GameObject> pooledObjects;
    public bool willGrow = true;

    void Awake()
    {
        Current = this;
    }
	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < size; i++)
        {
            var obj = (GameObject)Instantiate(pooledObject);
            obj.SetActive( false);
            pooledObjects.Add(obj);
        }
    }
	
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            Debug.Log(pooledObjects[i].activeInHierarchy);
            if (!pooledObjects[i].activeInHierarchy)
            {
                Debug.Log("Returning Pooled object ID: " + i);
                return pooledObjects[i];
            }
        }

        if(willGrow)
        {
            var obj = (GameObject)Instantiate(pooledObject);
            pooledObjects.Add(obj);
            Debug.Log("Returning Pooled object ID: " + (pooledObjects.Count -1));

            return obj;
        }

        return null;
    }
}
