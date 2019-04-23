using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePortal : MonoBehaviour
{
    public MemoryManagement mM;
    Rigidbody blocRigidbody;
    Bloc bloc;
    public GameObject orangePortal;
    public float portalTime = 2;
    public GameObject keyPrefab;
    
    private void OnCollisionStay(Collision collision)
    {
        bloc = collision.collider.gameObject.GetComponent<Bloc>();
        if (bloc != null)
        {
            blocRigidbody = collision.collider.gameObject.GetComponent<Rigidbody>();
        }
        else
            blocRigidbody = null;
    }
    private void OnCollisionExit(Collision collision)
    {
        bloc = null;
        blocRigidbody = null;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
        if(portalTime <= 0)
        {
            orangePortal.SetActive(false);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = true;
        }       
        else
        {
            portalTime -= Time.deltaTime;
        }
        if (blocRigidbody != null && bloc != null && blocRigidbody.velocity.magnitude <= 0.01f)
        {
            

            GetComponent<SpriteRenderer>().enabled = true;
            orangePortal.SetActive(true);
            List<Bloc> blocs = new List<Bloc>();
            blocs.Add(bloc);

            for (int i = 1; i < bloc.size; i++)
            {
                Bloc newBloc = Instantiate(bloc);
                newBloc.InitText(blocRigidbody.gameObject.GetInstanceID().ToString());
                newBloc.gameObject.SetActive(false);
                blocs.Add(newBloc);
            }

            int startIdx = mM.AllocateMemory(blocs);
           
            if (startIdx != -1)
            {
                GameObject key = Instantiate(keyPrefab, new Vector3(0, 0, 0), new Quaternion());
                key.transform.Find("KeyModel").GetComponent<Renderer>().material.color = bloc.GetComponent<Renderer>().material.color;
                key.transform.Find("KeyText").GetComponent<TextMesh>().text = bloc.GetComponentInChildren<TextMesh>().text;
                foreach (Bloc b in blocs)
                {
                    b.gameObject.SetActive(true);
                }
            }
            else
            {
                foreach(Bloc b in blocs)
                {
                    Destroy(b);
                }
            }
           
           

            bloc = null;
            blocRigidbody = null;
        }
    }
}
