using UnityEngine;

public class SetTargetPosition : MonoBehaviour
{
    [SerializeField] GameObject player;
    float fixedx = 0.0f;
    Vector3 targetPosition;
    void Start()
    {
        fixedx = player.transform.position.x;
    }
    void Update()
    {
        targetPosition = player.transform.position;
        transform.position = new Vector3(fixedx, targetPosition.y, targetPosition.z);
    }
}
