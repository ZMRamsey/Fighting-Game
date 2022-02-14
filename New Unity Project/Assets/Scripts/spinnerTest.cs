using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinnerTest : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
     	transform.Rotate(speed * Time.deltaTime, speed * Time.deltaTime, speed * Time.deltaTime);   
    }

	public void AdjustSeed (float newSpeed){
		speed = newSpeed;
	}
}
