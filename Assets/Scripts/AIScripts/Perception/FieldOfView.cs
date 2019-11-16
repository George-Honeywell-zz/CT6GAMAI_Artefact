using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Perception))]
public class FieldOfView : MonoBehaviour
{
    public float viewRadius = 10.0f;
    [Range(0, 360)]
    public float viewAngle = 45.0f;
    public LayerMask TargetLayer;
    public LayerMask obstacleLayer;
    public List<Transform> visibleTargets = new List<Transform>();

    public float meshResolution;
    public int edgeResolveIterations;
    public float edgeDistanceThreshold;
    public MeshFilter viewMeshFilter;
    Mesh viewMesh;
    public bool drawField = true;

    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        InvokeRepeating("VisibleTargets", 0.2f, 0.2f);
    }

    void VisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targets = Physics.OverlapSphere(transform.position, viewRadius, TargetLayer);

        foreach(Collider target in targets)
        {
            Vector3 toTarget = (target.transform.position - transform.position);
            Vector3 targetNormalized = toTarget.normalized;

            if(Vector3.Angle(transform.forward, targetNormalized) < viewAngle / 2
                && !Physics.Raycast(transform.position, targetNormalized, toTarget.magnitude, obstacleLayer))
            {
                visibleTargets.Add(target.transform);
            }
        }


        Perception perception = GetComponent<Perception>();

        perception.clearView();
        foreach(Transform target in visibleTargets)
        {
            perception.addMemory(target.gameObject);
        }
    }

    public void LateUpdate()
    {
        if (drawField)
        {
            viewMeshFilter.gameObject.SetActive(true);
            DrawFieldView();

            foreach(Transform target in visibleTargets)
            {
                Debug.DrawLine(transform.position, target.position, Color.red);
            }
        }
        else
        {
            viewMeshFilter.gameObject.SetActive(false);
        }
    }

    void DrawFieldView()
    {
        Gizmos.DrawWireSphere(transform.position, viewRadius);
        Gizmos.DrawWireSphere(transform.position, 20.0f);

        Vector3 fovLine1 = Quaternion.AngleAxis(viewAngle, transform.up) * transform.forward * viewRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-viewAngle, transform.up) * transform.forward * viewRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);

        Gizmos.color = Color.red;
        //Gizmos.DrawRay(transform.position, (Player.position - transform.position).normalized * maxRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward * viewRadius);
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for(int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDistanceThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
            if(newViewCast.hit == minViewCast.hit && !edgeDistanceThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }
        return new EdgeInfo(minPoint, maxPoint);
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 direction = DirectionFromAngle(globalAngle, true);
        RaycastHit hit;

        if(Physics.Raycast(transform.position, direction, out hit, viewRadius, obstacleLayer))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + direction * viewRadius, viewRadius, globalAngle);
        }
    }

    public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _distance, float _angle)
        {
            hit = _hit;
            point = _point;
            distance = _distance;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

}
