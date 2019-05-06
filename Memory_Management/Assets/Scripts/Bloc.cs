using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloc : MonoBehaviour
{
    //public GameObject prefabDataCube;

    public Color color;
    public int size;
    public GameObject prog;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        //dataCube = Instantiate(prefabDataCube, gameObject.transform.Find("Character").Find("Character").Find("DataSpawn"));
    }
    public void InitText(string s="")
    {
        string cubeText;
        if (s.Equals(""))
        {
            int r = GetInstanceID();
            cubeText = r.ToString();
        }
        else
        {
            cubeText = s;
        }
        foreach (Transform t in transform)
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
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
        transform.parent = null;
    }
}
