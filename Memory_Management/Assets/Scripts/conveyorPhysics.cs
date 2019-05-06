using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conveyorPhysics : MonoBehaviour
{
    public float speed;
    public float visualSpeedScalar;

    private Vector3 direction;
    private float currentScroll;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentScroll = currentScroll + Time.deltaTime * speed * visualSpeedScalar;
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(-currentScroll, 0);
    }
    private void OnCollisionStay(Collision otherThing)
    {
        direction = -transform.right;
        direction = direction * speed;

        otherThing.rigidbody.AddForce(direction, ForceMode.Acceleration);
    }
}
