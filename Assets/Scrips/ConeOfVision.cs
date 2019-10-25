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
    public float movementSpeed = 5.0f;
   
    private Material material;
    Color yellow = new Vector4(1.0f, 1.0f, 0.0f);
    Color orange = new Vector4(1.0f, 0.6f, 0.0f);
    Color red = new Vector4(1.0f, 0.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection = target.position - transform.position;
        float angle = Vector3.Angle(targetDirection, transform.forward);
        float seeDistance = Vector3.Distance(targetDirection, transform.forward);

        if (angle < 45.0 && seeDistance < 10.0)
        {
            Debug.Log("In Sight");
            material.color = orange;
            transform.LookAt(Player.transform);
            //transform.Rotate(transform.position);
            transform.position += transform.forward * movementSpeed * Time.deltaTime;
        }

        if(angle > 45.0 && seeDistance > 10.0)
        {
            Debug.Log("Not in Sight");
            material.color = yellow;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, maxRadius);

        Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * maxRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward * maxRadius;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);
    }


}
