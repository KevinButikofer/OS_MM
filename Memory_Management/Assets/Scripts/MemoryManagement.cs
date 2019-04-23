using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MemoryManagement : MonoBehaviour
{
    public GameObject datasSpots;
    public GameObject prefabData;
    public addressDisplay addressDisplay;
    public bool isFull = false;

    public List<Transform> datasSpotsList = new List<Transform>();
    public List<MemoryBlock> memoriesBlock = new List<MemoryBlock>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform dataSpot in datasSpots.transform)
        {
            foreach (Transform t in dataSpot)
            {
                datasSpotsList.Add(t);
                //GameObject m = Instantiate(prefabData, t);
                MemoryBlock mem = new MemoryBlock(t);
                //mem.Data = m;
                memoriesBlock.Add(mem);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            int key = Random.Range(0, addressDisplay.address.Count);
            int i = addressDisplay.address.ElementAt(key).Value;
            Debug.Log(i);
            int size = memoriesBlock[i].Data.GetComponent<Bloc>().size;
            Debug.Log(size);
            FreeMemory(i, key, size);
        }
    }
    /// <summary>
    /// Search the first big enough free memory block
    /// </summary>
    public int AllocateMemory(List<Bloc> blocs)
    {
        if (!isFull)
        {
            int space = 0;
            int startIdx = 0;
            int totalSpace = 0;
            foreach (MemoryBlock block in memoriesBlock)
            {
                if (!block.isOccupied)
                    totalSpace++;
            }
            if (totalSpace < blocs.Count)
            {
                return -1;
            }
            for (int i = 0; i < memoriesBlock.Count; i++)
            {
                if (!memoriesBlock[i].isOccupied)
                {
                    space++;
                    if (space >= blocs.Count)
                    {
                        for (int j = 0; j < blocs.Count; j++)
                        {
                            memoriesBlock[startIdx + j].Data = blocs[j].gameObject;

                            blocs[j].transform.position = memoriesBlock[startIdx + j].transform.position;
                            blocs[j].transform.forward = Vector3.right;
                        }
                        addressDisplay.AddAdress(blocs[0].gameObject.GetInstanceID(), startIdx);

                        if (totalSpace - blocs.Count() == 0)
                        {
                            isFull = true;
                        }
                        return startIdx;
                    }
                }
                else
                {
                    startIdx = i + 1;
                    space = 0;
                }
            }
            Debug.Log("Defrag");
            DefragMemory2();
            return AllocateMemory(blocs);
        }
        return -1;
    }
    public void FreeMemory(int idx, int size)
    {
        for(int i = idx; i < idx + size - 1; i++)
        {
            memoriesBlock[i].Free();
        }
        
    }
    public void FreeMemory(int idx, int key, int size)
    {
        for (int i = idx; i < idx + size - 1; i++)
        {
            Debug.Log("idx " + i);
            memoriesBlock[i].Free();
        }
        addressDisplay.RemoveAdress(key);

    }
    public void DefragMemory()
    {
        List<int> freeSpace = new List<int>();
        MemoryBlock currentData = null;
        int currentDataIdx = -1;
        for (int i = 0; i < memoriesBlock.Count - 1; i++)
        {
            if(!memoriesBlock[i].isOccupied)
            {
                if(currentData != null)
                {
                    //end of a block
                    Bloc b = currentData.Data.GetComponent<Bloc>();
                    int startIdx = EnouhSpaceStartIndex(freeSpace, b.size);
                    if (startIdx != -1)
                    {
                        freeSpace.RemoveRange(startIdx, b.size);
                        //move data and update dictonnary
                        addressDisplay.address[b.gameObject.GetInstanceID()] = startIdx;
                        for (int j = startIdx; j < b.size; j++)
                        {                            
                            memoriesBlock[j].Data = memoriesBlock[currentDataIdx+j].Data;
                            memoriesBlock[currentDataIdx + j].Free();
                            memoriesBlock[j].Data.transform.position = memoriesBlock[j].transform.position;
                        }
                    }
                }
                freeSpace.Add(i);                
            }
            if(currentData == null)
            {
                currentData = memoriesBlock[i];
                currentDataIdx = i;
            }
        }
    }
    public void DefragMemory2()
    {
        int spaceCount = 0;
        MemoryBlock currentData = null;
        int currentDataIdx = -1;
        int startIdx = 0;
        for (int i = 0; i < memoriesBlock.Count - 1; i++)
        {
            if (!memoriesBlock[i].isOccupied)
            {
                if(spaceCount == 0)
                {
                    startIdx = i;
                }
                spaceCount++;
            }
            //on decale pour remplir l'espace vide
            else
            {
                currentData = memoriesBlock[i];
                currentDataIdx = i;
                Bloc b = currentData.Data.GetComponent<Bloc>();

                for (int j = startIdx; j < b.size; j++)
                {
                    memoriesBlock[j].Data = memoriesBlock[currentDataIdx + j].Data;
                    memoriesBlock[currentDataIdx + j].Free();
                    memoriesBlock[j].Data.transform.position = memoriesBlock[j].transform.position;
                }
                i = startIdx + b.size;
            }
        }
     }
    public int EnouhSpaceStartIndex(List<int> list, int size)
    {
        int prevI = list[0];
        int space = 0;
        for(int i = 1; i < list.Count; i++)
        {
            if(i - prevI == 1)
            {
                space++;
                if(space >= size)
                {
                    return list[i - size];
                }
            }
        }
        return -1;
    }
}
