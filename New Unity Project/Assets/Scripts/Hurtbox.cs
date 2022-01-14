using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Not the same job as hitregister?

public class Hurtbox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<Ball>())
        {
            Debug.Log("ouchie");
        }
    }
}
