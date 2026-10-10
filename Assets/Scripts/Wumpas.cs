using UnityEngine;

public class Wumpa : MonoBehaviour
{
    [Header("Random ranges (each fruit picks its own)")]
    [SerializeField] Vector2 popHeightRange = new Vector2(1f, 2.5f);
    [SerializeField] Vector2 popDurationRange = new Vector2(0.4f, 0.8f);
    [SerializeField] Vector2 startDelayRange = new Vector2(0f, 0.3f);

    [SerializeField] float restHeight = 0.5f;
    [SerializeField] float spinSpeed = 90f;

    float popHeight, popDuration, startDelay;
    Vector3 startPos;
    float timer = 0f;

    void Start()
    {
        startPos = transform.position;
        popHeight = Random.Range(popHeightRange.x, popHeightRange.y);
        popDuration = Random.Range(popDurationRange.x, popDurationRange.y);
        startDelay = Random.Range(startDelayRange.x, startDelayRange.y);
    }

    void Update()
    {
        timer += Time.deltaTime;

        float activeTime = timer - startDelay;
        if (activeTime > 0f && activeTime < popDuration)
        {
            float t = activeTime / popDuration;
            float arc = 4f * t * (1f - t);
            float y = arc * popHeight + t * restHeight;
            transform.position = startPos + Vector3.up * y;
        }
        else if (activeTime >= popDuration)
        {
            transform.position = startPos + Vector3.up * restHeight;
        }

        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);
    }
}