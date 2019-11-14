using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform PlayerTransform;
    public bool LookAtPlayer = false;
    public bool RotateAroundPlayer = true;
    public float RotationSpeed = 2.0f;
    private Vector3 offset;
    public float SmoothFactor = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - PlayerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (RotateAroundPlayer)
        {
            Quaternion cameraTurn = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationSpeed, Vector3.up);
            offset = cameraTurn * offset;
        }

        Vector3 newPosition = PlayerTransform.position + offset;
        transform.position = Vector3.Slerp(transform.position, newPosition, SmoothFactor);
        
        if(LookAtPlayer || RotateAroundPlayer)
        {
            transform.LookAt(PlayerTransform);
        }

    }
}
