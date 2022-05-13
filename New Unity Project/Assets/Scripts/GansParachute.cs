using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GansParachute : MonoBehaviour
{
    
    void Update()
    {
        if(GetComponent<Rigidbody>().velocity.y < 0) {
            GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody>().velocity, 1);
        }
    }
}
