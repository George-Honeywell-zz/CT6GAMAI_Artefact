using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConeOfVision : MonoBehaviour
{

    public Transform target;
    public GameObject Player;
    public float maxAngle = 45.0f;
    public float maxRadius = 10.0f;
    public float movementSpeed = 3.0f;
   
    private Material material;


    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, maxRadius);
        Gizmos.DrawWireSphere(transform.position, 20.0f);

        Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * maxRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward * maxRadius;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);
    }
}
