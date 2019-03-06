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

    private bool isMoveCouroutineRunning = false;
    private bool startStartMove = true;
    private bool endStartMove = false;
    private bool endEndingMove = false;
    private bool startEndMove = false;

    WaitForFixedUpdate wait;
    // Start is called before the first frame update
    void Start()
    {
        wait = new WaitForFixedUpdate();
        foreach(Transform child in GameObject.Find("StartPath").transform)
        {
            startPath.Add(child);
        }
        foreach (Transform child in GameObject.Find("EndPath").transform)
        {
            endPath.Add(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentIdx < startPath.Count && !isMoveCouroutineRunning && startStartMove)
        {
            isMoveCouroutineRunning = true;
            animator._animRunHolding = true;
            StartCoroutine(MoveNext(this.transform.position, startPath));
            currentIdx++;
        }
        else if(endStartMove)
        {
            animator._animRun = false;
            currentIdx = 0;
            isMoveCouroutineRunning = false;

            GetComponent<Bloc>().ReleaseBloc();

            startStartMove = false;
            startEndMove = true;
            endStartMove = false;
        }
        if(currentIdx < endPath.Count && !isMoveCouroutineRunning && startEndMove && !endEndingMove)
        {
            animator._animRun = true;
            isMoveCouroutineRunning = true;
            StartCoroutine(MoveNext(transform.position, endPath));
            currentIdx++;
        }
        else if(endEndingMove)
        {
            animator._animRun = false;
            currentIdx = 0;
            isMoveCouroutineRunning = false;
            startEndMove = false;
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
