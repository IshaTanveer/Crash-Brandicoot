using UnityEngine;

public class DetectCheckpoints : MonoBehaviour
{
    public static bool hasBeenTocheckpoint = false;
    public static Vector3 checkpointPosition;
    void Start()
    {
        hasBeenTocheckpoint = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CheckpointCrate")
        {
            //Save the checkpoint
            Debug.Log("checkpoint detected");
            hasBeenTocheckpoint = true;
            checkpointPosition = other.gameObject.transform.position;

        }
    }
    public bool GetHasBeenTocheckpoint()
    {
        return hasBeenTocheckpoint;
    }
    public Vector3 GetCheckpointPosition()
    {
        return checkpointPosition;
    }
    public void ResetBeenToCheckpoint()
    {
        hasBeenTocheckpoint = false;
    }
}
