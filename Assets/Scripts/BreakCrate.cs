using UnityEngine;

public class BreakCrate : MonoBehaviour
{
    CharacterMovementAndAnimationController controller;
    bool playerinside = false;
    private void Awake() {
        controller = FindFirstObjectByType<CharacterMovementAndAnimationController>();
    }
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
        {
            playerinside = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player")
        {
            playerinside = false;
        }
    }
    void Update()
    {
        BrakeThecrate();
    }
    void BrakeThecrate()
    {
        if(controller.GetIsSpining() && playerinside)
        {
            //play aniamtion
            //play sound
            //play particles
            Debug.Log("Break Crate");
            Destroy(this.gameObject);
        }
    }
}
