using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    private List<GameObject> progs = new List<GameObject>();
    public GameObject prefabProg;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            Color color = Random.ColorHSV();
            GameObject prog = Instantiate(prefabProg);
            prog.transform.position = new Vector3(8, 1.5f, -30);
            prog.gameObject.transform.Find("Character").Find("CharacterModel").GetComponent<Renderer>().material.color = color;
            progs.Add(prog);
        }
    }
}
