using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloc : MonoBehaviour
{
    public GameObject prefabDataCube;
    public RenderTexture texture;

    public Color color;
    public GameObject dataCube;
    public int size = 1;
    public int posInQueue;
    // Start is called before the first frame update
    void Start()
    {
        int r = Random.Range(10000, 999999999);
        string cubeText = r.ToString();
        dataCube = Instantiate(prefabDataCube, transform);
        dataCube.transform.localScale.Set(size, 1, 1);
        dataCube.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 1.2f);
        Color c = gameObject.transform.Find("Character").Find("CharacterModel").GetComponent<Renderer>().material.color;
        dataCube.GetComponent<Renderer>().material.color = c;

        foreach(Transform t in dataCube.transform)
        {
            t.gameObject.GetComponent<TextMesh>().text = cubeText;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ReleaseBloc()
    {
        dataCube.GetComponent<Rigidbody>().useGravity = true;
        dataCube.transform.parent = null;
    }
}
