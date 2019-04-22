using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryBlock
{
    public bool isOccupied = false;
    public Transform transform;
    public string addr;
    private GameObject data;
    public GameObject Data
    {
        get
        {
            return data;
        }
        set
        {
            data = value;
            isOccupied = true;
        }       
    }

    public MemoryBlock(Transform t)
    {
        this.transform = t;
    }
    public void Free()
    {
        GameObject.Destroy(data);
        data = null;
        isOccupied = false;
    }
}
