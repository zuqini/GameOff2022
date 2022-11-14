using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public GameObject objectToPool;
    public int amountToPool;
    public bool shouldExpand = false;
}

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;
    public List<ObjectPoolItem> itemsToPool;
    private List<GameObject> pooledObjects;

    void Awake()
    {
        if (SharedInstance != null && SharedInstance != this) 
        { 
            Destroy(this.gameObject);
            return;
        }

        SharedInstance = this;
        //DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        foreach (var item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }
    public GameObject GetPooledObject(string tag)
    {
        //@TODO: Refactor this
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
            {
                return pooledObjects[i];
            }
        }

        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag && item.shouldExpand)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);
                return obj;
            }
        }
        return null;
    }
}
