using System.Collections.Generic;
using UnityEngine;

public class SimplePool : MonoBehaviour
{
    public List<GameObject> PooledObjects;
    public GameObject ObjectToPool;
    public int AmountToPool;
    public bool Extendable = true;

    private void Start()
    {
        this.PooledObjects = new List<GameObject>(this.AmountToPool);
        this.GeneratePool(this.AmountToPool);
    }

    public void ResetPool()
    {
        foreach(GameObject gameObject in this.PooledObjects)
        {
            gameObject.SetActive(false);
        }
    }

    public GameObject GetPooledObject()
    {
        GameObject toReturn = null;
        for (int idx = 0;toReturn == null && idx < this.PooledObjects.Count; idx++)
        {
            if (!this.PooledObjects[idx].activeInHierarchy)
            {
                toReturn = this.PooledObjects[idx];
            }
        }
        if (this.Extendable && toReturn == null)
        {
            this.ExtendPool();
            toReturn = this.GetPooledObject();
        }
        return toReturn;
    }

    private void ExtendPool()
    {
        int oldAmount = this.PooledObjects.Count;
        this.GeneratePool(this.PooledObjects.Count + this.AmountToPool, this.PooledObjects.Count);
    }

    private void GeneratePool(int idxEnd = 0, int idxStart = 0)
    {
        GameObject tmp;
        for (int idx = idxStart; idx < idxEnd; idx++)
        {
            tmp = GameObject.Instantiate(this.ObjectToPool,this.transform);
            tmp.SetActive(false);
            this.PooledObjects.Add(tmp);
        }
    }
}
