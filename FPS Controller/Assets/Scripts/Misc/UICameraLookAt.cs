using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraLookAt : MonoBehaviour
{
    public Camera lookAt;

    // Start is called before the first frame update
    void Start()
    {
        lookAt = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = lookAt.transform.position - transform.position;
        v.x = v.z = 0f;
        transform.LookAt(lookAt.transform.position - v);
        transform.rotation = (lookAt.transform.rotation);
    }
}
