using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooling : MonoBehaviour {

   // public static ObjectPooling Current;
    public int size = 10;

    public GameObject pooledObject;
    public List<GameObject> pooledObjects;
    public bool willGrow = true;

    protected void Awake()
    {
        //Current = this;
    }
	// Use this for initialization
	protected void Start ()
    {
       // Current = this;
        pooledObjects = new List<GameObject>();
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
            if (!pooledObjects[i].activeInHierarchy)
            {
               // Debug.Log("Returning Pooled object ID: " + i);
                pooledObjects[i].SetActive(true);
                return pooledObjects[i];
            }
        }

        if(willGrow)
        {
            var obj = (GameObject)Instantiate(pooledObject);
            pooledObjects.Add(obj);
           // Debug.Log("Returning Pooled object ID: " + (pooledObjects.Count -1));
            return obj;
        }

        return null;
    }

    public int GetActiveAmount()
    {
        var res = 0;

        for(int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i].activeInHierarchy)
                res++;
        }
        return res;
    }
}
