using System.Drawing;
using UnityEngine;

public class MoveEnemies : MonoBehaviour
{
    Rigidbody EnemyRB;
    [SerializeField] private GameObject Points;
    public Transform[] WayPoints;
    [SerializeField] float speed = 2.0f;
    [SerializeField] float wait = 2.0f;
    Vector3 targetPosition;
    private int pointIndex;
    private int pointCount;
    private int direction = 1;
    void Awake()
    {
        EnemyRB = GetComponent<Rigidbody>();
        WayPoints = new Transform[Points.transform.childCount];
        for(int i = 0; i < Points.transform.childCount; i++)
        {
            WayPoints[i] = Points.transform.GetChild(i);
        }
    }
    void Start()
    {
        pointCount = Points.transform.childCount;
        pointIndex = 1;
        targetPosition = WayPoints[pointIndex].position;
    }
    void FixedUpdate()
    {
        if(WayPoints.Length < 2) return;
        Vector3 newPosition = Vector3.MoveTowards(EnemyRB.position, targetPosition, speed * Time.deltaTime);
        EnemyRB.MovePosition(newPosition);
        if(newPosition == targetPosition)
        {
            NextPoint();
        }
    }
    void NextPoint()
    {
        pointIndex = (pointIndex + 1) % WayPoints.Length;
        targetPosition = WayPoints[pointIndex].position;
    }
}
