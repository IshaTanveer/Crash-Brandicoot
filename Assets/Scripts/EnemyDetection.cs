using System;
using System.Collections;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    Vector2 checkpoint;
    [SerializeField] float wait = 2.0f;
    CharacterMovementAndAnimationController controller;
    Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        checkpoint = transform.position;
        controller = FindFirstObjectByType<CharacterMovementAndAnimationController>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && !controller.GetIsSpining())
        {
            Debug.Log("You are dead!!");
            PlayerDeath();
        }
        else if(other.tag == "Enemy" && controller.GetIsSpining())
        {
            KillTheEnemy(other.gameObject);
        }
    }

    private void KillTheEnemy(GameObject enemy)
    {
        Debug.Log("Enemy Dead");
        Destroy(enemy);
    }

    private void PlayerDeath()
    {
        //Play Animation
        //play sound
        //play particles
        StartCoroutine(Respwan());
    }

    IEnumerator Respwan()
    {
        transform.localScale = new Vector3(0,0,0);
        yield return new WaitForSeconds(wait);
        transform.position = checkpoint;
        transform.localScale = new Vector3(1,1,1);
    }
}
