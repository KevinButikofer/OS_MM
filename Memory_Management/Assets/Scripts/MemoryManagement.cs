using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryManagement : MonoBehaviour
{
    public GameObject datasSpots;
    public GameObject prefabData;
    public List<MemoryBlock> memoriesBlock = new List<MemoryBlock>();
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform dataSpot in datasSpots.transform)
        {
            foreach (Transform t in dataSpot)
            {
                GameObject m = Instantiate(prefabData, t);
                MemoryBlock mem = new MemoryBlock(t);
                mem.Data = m;
                memoriesBlock.Add(mem);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Search the first big enough free memory block
    /// </summary>
    public void allocateMemory(Bloc b)
    {
        int space = 0;
        for(int i = 0; i < memoriesBlock.Count - 1; i++)
        {
            if (!memoriesBlock[i].isOccupied)
            {
                space++;
                if(space >= b.size)
                {
                    memoriesBlock[0].Data = b.gameObject;
                }
            }  
            else
            {
                space = 0;
            }
        }
        //we need to do some space or use swap ???
    }
}
