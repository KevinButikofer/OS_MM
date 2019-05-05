using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathFolow : MonoBehaviour
{
    public int moveSpeed;
    private List<Transform> startPath = new List<Transform>();
    private List<Transform> endPath = new List<Transform>();
    private int currentIdx = 0;
    public CharacterAnimation animator;
    public Animator doorLeftAnim;
    public Animator doorRightAnim;
    public GameObject door;
    private bool isMoveCouroutineRunning = false;
    private bool startStartMove = true;
    private bool endStartMove = false;
    private bool endEndingMove = false;
    private bool startEndMove = false;
    private bool hasStoreData = false;
    public int posInQueue = -1;
    public QueueManager queueManager;

    private static MemoryManagement memoryManagement;
    private static addressDisplay addressDisplay;

    public float timeBeforeReturn = -10;

    public int cubeSize;
    public int cubeIdx;

    public GameObject prefabDataCube;
    public GameObject dataCube;

    WaitForFixedUpdate wait;
    // Start is called before the first frame update
    void Start()
    {
        if(memoryManagement == null)
            memoryManagement = FindObjectOfType<MemoryManagement>();
        if(addressDisplay == null)
            addressDisplay = FindObjectOfType<addressDisplay>();

        dataCube = Instantiate(prefabDataCube, gameObject.transform.Find("Character").Find("Character").Find("DataSpawn"));
        dataCube.GetComponent<Renderer>().material.color = transform.transform.Find("Character").Find("CharacterModel").GetComponent<Renderer>().material.color;
        dataCube.GetComponent<Bloc>().InitText();
        dataCube.GetComponent<Bloc>().prog = this;
        cubeIdx = dataCube.GetComponent<Bloc>().GetInstanceID();
        cubeSize = (dataCube.GetComponent<Bloc>()).size;
        doorLeftAnim = GameObject.Find("door_left").GetComponent<Animator>();
        doorRightAnim = GameObject.Find("door_right").GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
        foreach(Transform child in GameObject.Find("StartPath").transform)
        {
            startPath.Add(child);
        }
        foreach (Transform child in GameObject.Find("EndPath").transform)
        {
            endPath.Add(child);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, -19 - 4 * posInQueue);
    }
    private void OnDestroy()
    {        
        if(posInQueue == -1)
        {
            queueManager.isSomoneInside = false;
            posInQueue = -2;
        }
        queueManager.leaveQueue(this);
    }
    // Update is called once per frame
    void Update()
    {
        timeBeforeReturn -= Time.deltaTime;
        if (posInQueue > 0 && transform.position.z >= -19 - 4 * posInQueue)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -19 - 4 * posInQueue);
            return;
        }

        if (posInQueue == 0 && queueManager.isSomoneInside && transform.position.z >= -19)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -19);
            return;
        }

        //if (queueManager.isSomoneInside && posInQueue == 0 && transform.position.z >= -19 - 4 * posInQueue)
        //return;
        //if (queueManager.isSomoneInside && posInQueue >= 0 && transform.position.z >= -19)
        //return;

        //if (transform.position.z < -19 - 4 * posInQueue && posInQueue > 0)
        //{
        //return;
        //}



        if (transform.position.z > -19)
        {
            if (transform.position.z < -4) // Est en train de se déplacer à l'intérieur
                queueManager.isSomoneInside = true;
            if (posInQueue == 0) // Si position est de 0, enlève de la queue
            {
                queueManager.leaveQueue(this);
                posInQueue = -1;
            }
        }
        if (transform.position.z > -18) // Arrive devant les portes
        {
            doorLeftAnim.SetBool("opendoor", true);
            doorRightAnim.SetBool("opendoor", true);
        }
        if (transform.position.z > -12) // Fermeture des portes
        {
            doorLeftAnim.SetBool("opendoor", false);
            doorRightAnim.SetBool("opendoor", false);
        }
        if (transform.position.z > -4 && posInQueue <= -1) // Fini son truc..
        {
            queueManager.isSomoneInside = false;
            posInQueue = -2;
        }




        if (currentIdx < startPath.Count && !isMoveCouroutineRunning && startStartMove)
        {
            isMoveCouroutineRunning = true;
            if (hasStoreData)
                animator._animRun = true;
            else
                animator._animRunHolding = true;
            StartCoroutine(MoveNext(this.transform.position, startPath));
            currentIdx++;
        }
        else if (endStartMove)
        {
            animator._animRunHolding = false;
            currentIdx = 0;
            isMoveCouroutineRunning = false;


            if (!hasStoreData)
                dataCube.GetComponent<Bloc>().ReleaseBloc();
            else
            {
                Bloc b = dataCube.GetComponent<Bloc>();
                if (addressDisplay.address.TryGetValue(b.GetInstanceID(), out int idx))
                {
                    memoryManagement.FreeMemory(idx, b.GetInstanceID(), b.size);
                }
                else
                {
                    print("Key Error");
                }
            }
            

            startStartMove = false;
            startEndMove = true;
            endStartMove = false;
        }
        if (currentIdx < endPath.Count && !isMoveCouroutineRunning && startEndMove && !endEndingMove)
        {
            animator._animRun = true;
            isMoveCouroutineRunning = true;
            StartCoroutine(MoveNext(transform.position, endPath));
            currentIdx++;
        }
        else if (endEndingMove && timeBeforeReturn < -10)
        {
            animator._animRun = false;
            currentIdx = 0;
            isMoveCouroutineRunning = false;
            startEndMove = false;
            if (!hasStoreData)
            {
                hasStoreData = true;
                timeBeforeReturn = Random.Range(5, 40);
            }
            else
            {
                posInQueue = -2;
                Destroy(this.gameObject);
            }
        }
        if (timeBeforeReturn <= 0 && timeBeforeReturn > -10)
        {
            timeBeforeReturn = -10.0f;
            queueManager.addCharacter(this);
            currentIdx = 0;
            isMoveCouroutineRunning = false;
            startStartMove = true;
            endStartMove = false;
            endEndingMove = false;
            startEndMove = false;
            currentIdx = 0;
            posInQueue = -1;
            transform.position = new Vector3(7.0f, 1.5f, -40.0f);
        }
    }
    IEnumerator RotateNext(Quaternion startRot, List<Transform> path, int idx)
    {
        Vector3 targetDir = path[idx].position - transform.position;
        float step = moveSpeed * Time.deltaTime * 5;        
        float t = 0;
        while (t <= 1.0f)
        {
            t += step;
            transform.rotation = Quaternion.Lerp(startRot, Quaternion.LookRotation(targetDir), t);
            yield return wait;
        }
    }
    IEnumerator MoveNext(Vector3 startPos, List<Transform>path)
    {
        Vector3 nextPos = path[currentIdx].position;
        float step = (moveSpeed / (startPos - nextPos).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        StartCoroutine(RotateNext(transform.rotation, path, currentIdx));
        
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time           
            transform.position = Vector3.Lerp(startPos, nextPos, t); // Move objectToMove closer to b
            yield return wait;         // Leave the routine and return here in the next frame            
        }
        transform.position = nextPos;
        isMoveCouroutineRunning = false;
        if (currentIdx == path.Count)
        {
            if (startEndMove) 
                endEndingMove = true;
            else
                endStartMove = true;
            animator._animRun = false;
        }

    }
}
