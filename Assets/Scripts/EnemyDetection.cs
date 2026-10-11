using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] float wait = 2.0f;
    CharacterMovementAndAnimationController controller;
    Rigidbody rb;
    DetectCheckpoints detectCheckpoints;
    CharacterController characterController;
    Collectibles collectibles;
    bool isDead = false;
    static bool diedLastRound = false;
    ShowBlackout showBlackout;
    AudioManager audioManager;
    bool fellIntoPitt = false;
    CinemachineCamera cam;
    Animator animator;
    
    void Awake()
    {
        detectCheckpoints = GetComponent<DetectCheckpoints>();
        rb = GetComponent<Rigidbody>();
        controller = FindFirstObjectByType<CharacterMovementAndAnimationController>();
        characterController = GetComponent<CharacterController>();
        showBlackout = FindFirstObjectByType<ShowBlackout>();
        collectibles = FindFirstObjectByType<Collectibles>();
        audioManager = FindFirstObjectByType<AudioManager>();
        cam = FindFirstObjectByType<CinemachineCamera>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        if (!diedLastRound) return;
        diedLastRound = false;
        if(collectibles.GetLivesNums() > 0)
        {
            collectibles.dcreaseLives();
            if (detectCheckpoints.GetHasBeenTocheckpoint())
            {
                characterController.enabled = false;
                transform.position = detectCheckpoints.GetCheckpointPosition() + Vector3.up * 1.0f;
                characterController.enabled = true;
            }
        }
        else
        {
            detectCheckpoints.ResetBeenToCheckpoint();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            if(isDead) return;
            if (!controller.GetIsSpining())
            {
                Debug.Log("You are dead!!");
                PlayerDeath();
            }
            else
            {
                KillTheEnemy(other.gameObject);
            }
        }
        else if(other.tag == "Pitt")
        {
            cam.Follow = null;
            if(isDead) return;
            fellIntoPitt = true;
            PlayerDeath();
        }
    }

    private void KillTheEnemy(GameObject enemy)
    {
        //enemy death animation pending
        //play sound 
        //play particles
        //how do i improve the
        Debug.Log("Enemy Dead");
        Destroy(enemy);
    }

    private void PlayerDeath()
    {
        //Play Animation
        //play sound
        //play particles
        isDead = true;
        diedLastRound = true;
        StartCoroutine(Respwan());
    }

    IEnumerator Respwan()
    {
        controller.DisableControls();
        if (fellIntoPitt)
        {
            audioManager.PlaySounds(AudioManager.Sound.FellInPit);
        }
        else
        {
            //controller.enabled = false;
            animator.SetTrigger("Death");
            audioManager.PlaySounds(AudioManager.Sound.Death);
            showBlackout.ShowTheBlackout();
        }
        yield return new WaitForSeconds(wait);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
