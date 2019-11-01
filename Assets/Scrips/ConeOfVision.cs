using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConeOfVision : MonoBehaviour
{
    public Transform Player;
    public float maxAngle = 45.0f;
    public float maxRadius = 10.0f;
    //public float movementSpeed = 3.0f;

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

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, (Player.position - transform.position).normalized * maxRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward * maxRadius);
    }

    public static bool inFieldOfView (Transform checkObject, Transform target, float maxAngle, float maxRadius)
    {
        Collider[] overlaps = new Collider[10];
        int count = Physics.OverlapSphereNonAlloc(checkObject.position, maxRadius, overlaps);


        return false;
    }
}
