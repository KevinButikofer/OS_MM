using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    private List<GameObject> progs = new List<GameObject>();
    public GameObject prefabProg;
    public float minTimeBetweenSpawn = 2.0f;
    private float spawnTime;
    public bool isGamePaused = true;
    
    private int compteur;
    private QueueManager queueManager;
    // Start is called before the first frame update
    void Start()
    {
        queueManager = FindObjectOfType<QueueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGamePaused)
        {
            spawnTime -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.H) && spawnTime <= 0)
            {
                Color color = Random.ColorHSV();
                GameObject prog = Instantiate(prefabProg);
                prog.transform.position = new Vector3(8, 1.5f, -30);
                prog.gameObject.transform.Find("Character").Find("CharacterModel").GetComponent<Renderer>().material.color = color;
                prog.GetComponent<pathFolow>().queueManager = queueManager;
                queueManager.addCharacter(prog.GetComponent<pathFolow>());
                progs.Add(prog);
                spawnTime = minTimeBetweenSpawn;
            }
        }
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
