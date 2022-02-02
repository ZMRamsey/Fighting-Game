using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrestling : MonoBehaviour
{
    public GameObject hand1;
    public GameObject hand2;
    public GameObject pointer;
    public float pointerSpeed;
    public float jiggleAmount;
    float rotatorTime;
    bool jiggleFlipper;
    Vector3 startPos1;
    Vector3 startPos2;

    // Start is called before the first frame update
    void Start()
    {
        rotatorTime = 0;
        jiggleFlipper = false;
        startPos1 = hand1.transform.position;
        startPos2 = hand2.transform.position;
    }

    //Player has stopped meter, check how well they've done
    void MeterStopped()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        rotatorTime += pointerSpeed * 0.001f;
        float finRot = Mathf.Sin(rotatorTime * 10) * 100;
        Quaternion rotating = new Quaternion();
        rotating = Quaternion.Euler(0, 0, finRot);
        pointer.transform.rotation = rotating;

        if (jiggleFlipper)
        {
            Vector3 jiggle = new Vector3(hand1.transform.position.x + Random.Range(-jiggleAmount, jiggleAmount), hand1.transform.position.y + Random.Range(-jiggleAmount, jiggleAmount));
            hand1.transform.position = jiggle;
            hand2.transform.position = startPos2;
        }
        else
        {
            Vector3 jiggle = new Vector3(hand2.transform.position.x + Random.Range(-jiggleAmount, jiggleAmount), hand2.transform.position.y + Random.Range(-jiggleAmount, jiggleAmount));
            hand2.transform.position = jiggle;
            hand1.transform.position = startPos1;
        }

        jiggleFlipper = !jiggleFlipper;

    }
}
