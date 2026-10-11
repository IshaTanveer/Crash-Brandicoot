using UnityEngine;

public class MoveEnemies : MonoBehaviour
{
    Rigidbody EnemyRB;
    [SerializeField] private GameObject Points;
    public Transform[] WayPoints;
    [SerializeField] float speed = 2.0f;

    Vector3 targetPosition;
    private int pointIndex;

    void Awake()
    {
        EnemyRB = GetComponent<Rigidbody>();
        WayPoints = new Transform[Points.transform.childCount];
        for (int i = 0; i < Points.transform.childCount; i++)
        {
            WayPoints[i] = Points.transform.GetChild(i);
        }
    }

    void Start()
    {
        pointIndex = 1;
        targetPosition = WayPoints[pointIndex].position;
    }

    void FixedUpdate()
    {
        if (WayPoints.Length < 2) return;

        Vector3 pos = EnemyRB.position;

        Vector3 flatTarget = new Vector3(targetPosition.x, pos.y, targetPosition.z);
        Vector3 toTarget = flatTarget - pos;

        if (toTarget.magnitude < Mathf.Max(0.1f, speed * Time.fixedDeltaTime))
        {
            NextPoint();
            return;
        }

        Vector3 dir = toTarget.normalized;
        Vector3 vel = EnemyRB.linearVelocity;  
        EnemyRB.linearVelocity = new Vector3(dir.x * speed, vel.y, dir.z * speed);
    }

    void NextPoint()
    {
        pointIndex = (pointIndex + 1) % WayPoints.Length;
        targetPosition = WayPoints[pointIndex].position;
    }
}