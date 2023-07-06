using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Credit goes to raywenderlich.com for this object pooling script
[System.Serializable]
public class ObjectPoolItem
{
    public GameObject objectToPool;
    public int amountToPool;
}
public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler sharedInstance;

    public List<GameObject> pooledObjects;
    public List<ObjectPoolItem> itemsToPool;
    
    private void Awake()
    {
        sharedInstance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        pooledObjects = new List<GameObject>();

        foreach(ObjectPoolItem item in itemsToPool)
        {
            //Create all the objects we want to pool then deactivate them for later use
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = Instantiate(item.objectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(string _tag)
    {
        //Any script that calls this method will grab an inactive pooled object to use it
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == _tag)
            {
                //Grab the available (inactive) pooled object
                return pooledObjects[i];
            }
        }

        //If there's no available pooled objects then we return nothing
        return null;
    }
}
