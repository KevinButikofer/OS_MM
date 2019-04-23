using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    private List<GameObject> progs = new List<GameObject>();
    public GameObject prefabProg;
    public float minTimeBetweenSpawn;
    private float spawnTime;
    private int compteur;
    private QueueManager queueManager;
    // Start is called before the first frame update
    void Start()
    {
        queueManager = new QueueManager();
    }

    // Update is called once per frame
    void Update()
    {
        spawnTime -= Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.H) && spawnTime <= 0)
        {
            spawnTime = minTimeBetweenSpawn;
            Color color = Random.ColorHSV();
            GameObject prog = Instantiate(prefabProg);
            prog.transform.position = new Vector3(8, 1.5f, -19);
            prog.gameObject.transform.Find("Character").Find("CharacterModel").GetComponent<Renderer>().material.color = color;
            queueManager.addCharacter(prog.GetComponent<pathFolow>());

            progs.Add(prog);
        }
    }
}
