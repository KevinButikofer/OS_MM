using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloc : MonoBehaviour
{
    //public GameObject prefabDataCube;

    public Color color;
    public GameObject dataCube;
    public int size;
    public int posInQueue;
    // Start is called before the first frame update
    void Start()
    {
        size = Random.Range(1, 15);
        int r = GetInstanceID();//Random.Range(10000, 999999999);
        string cubeText = r.ToString();
        //dataCube = Instantiate(prefabDataCube, gameObject.transform.Find("Character").Find("Character").Find("DataSpawn"));
        transform.localScale.Set(1, 1, 1);
        

        foreach(Transform t in transform)
        {
            t.gameObject.GetComponent<TextMesh>().text = cubeText;
        }
    }
    public void setColor(Color c)
    {
        c = transform.parent.parent.parent.gameObject.transform.Find("CharacterModel").GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = c;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void ReleaseBloc()
    {
        GetComponent<Rigidbody>().useGravity = true;
        transform.parent = null;
    }
}
