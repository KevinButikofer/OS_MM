using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public int nbPeople = 0;
    public volatile bool isSomoneInside = false;
    private List<pathFolow> charactersWaiting = new List<pathFolow>();
    // Start is called before the first frame update
    void Start()
    {
    }

    public void leaveQueue(pathFolow character)
    {
        charactersWaiting.Remove(character);
        updatePos();
        nbPeople--;
    }
    public void addCharacter(pathFolow character)
    {
        
        character.queueManager = this;
        character.posInQueue = nbPeople;
        charactersWaiting.Add(character);
        nbPeople++;
    }

    public void updatePos()
    {
        int pos = 0;
        foreach (pathFolow character in charactersWaiting)
        {
            character.posInQueue = pos;
            pos++;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
