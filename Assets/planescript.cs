using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planescript : MonoBehaviour
{

    public GameObject scalp;
    public getdir getdir;
    // Start is called before the first frame update
    void Start()
    {
        getdir = scalp.GetComponent<getdir>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = getdir.oldPos;
        gameObject.transform.rotation = Quaternion.Euler(getdir.dir);
    }
}
