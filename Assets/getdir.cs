using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getdir : MonoBehaviour
{

    public Vector3 dir;
    public Vector3 oldPos;
    
    private Rigidbody rb;
    public GameObject plane;
    // Start is called before the first frame update
    void Start()
    {
        oldPos = gameObject.transform.position;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = (transform.position - oldPos) / Time.deltaTime;
        oldPos = transform.position;

        if (dir.magnitude > 0.1f)
        {
            // Calculate the rotation based on movement direction
            Quaternion velocityRotation = Quaternion.LookRotation(dir);

            // Combine the GameObject's current rotation with the velocity-based rotation
            // Apply the velocity rotation first, then the GameObject's rotation
            plane.transform.rotation = Quaternion.LookRotation(dir) * Quaternion.Inverse(gameObject.transform.rotation);
        }
        


        plane.transform.position = oldPos;
    }
}
