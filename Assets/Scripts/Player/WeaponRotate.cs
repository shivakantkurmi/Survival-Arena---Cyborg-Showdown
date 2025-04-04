using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotate : MonoBehaviour
{

    public Vector3 rotationAxis=new Vector3(0,1,0);
    public float rotateSpeed=5f;

    void Update()
    {
        transform.Rotate(rotationAxis * rotateSpeed * Time.deltaTime); 
    }
}
