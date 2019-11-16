using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteeringBehavior))]
public class Sheep : MonoBehaviour
{
    SteeringBehavior sb;

    // Start is called before the first frame update
    void Start()
    {
        sb = GetComponent<SteeringBehavior>();
        sb.SeekOn(new Vector3(10, 1, 10));
        sb.WanderOn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
