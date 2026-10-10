using UnityEngine;

public class MoveEnemies : MonoBehaviour
{
    Rigidbody EnemyRB;
    [SerializeField] private GameObject Points;
    public Transform[] WayPoints;
    [SerializeField] float speed = 2.0f;

    [Header("Facing (tick only on the turtle)")]
    [SerializeField] bool faceMoveDirection = false;
    [SerializeField] float turnSpeed = 180f;   // degrees per second

    Vector3 targetPosition;
    private int pointIndex;
    float yawOffset;
    Vector3 startEuler;

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

        // work out how the model's front differs from Unity's forward,
        // assuming it starts out facing the first target
        startEuler = EnemyRB.rotation.eulerAngles;
        Vector3 d = targetPosition - EnemyRB.position;
        float dirYaw = Mathf.Atan2(d.x, d.z) * Mathf.Rad2Deg;
        yawOffset = Mathf.DeltaAngle(dirYaw, startEuler.y);
    }

    void FixedUpdate()
    {
        if (WayPoints.Length < 2) return;

        Vector3 pos = EnemyRB.position;

        // ignore the waypoint's Y: use our own current Y
        Vector3 flatTarget = new Vector3(targetPosition.x, pos.y, targetPosition.z);
        Vector3 toTarget = flatTarget - pos;

        if (toTarget.magnitude < Mathf.Max(0.1f, speed * Time.fixedDeltaTime))
        {
            NextPoint();
            return;
        }

        Vector3 dir = toTarget.normalized;
        Vector3 vel = EnemyRB.linearVelocity;   // use .velocity on older Unity versions
        EnemyRB.linearVelocity = new Vector3(dir.x * speed, vel.y, dir.z * speed);

        if (faceMoveDirection)
        {
            float dirYaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            Quaternion targetRot = Quaternion.Euler(startEuler.x, dirYaw + yawOffset, startEuler.z);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * Time.fixedDeltaTime);
        }
    }

    void NextPoint()
    {
        pointIndex = (pointIndex + 1) % WayPoints.Length;
        targetPosition = WayPoints[pointIndex].position;
    }
}