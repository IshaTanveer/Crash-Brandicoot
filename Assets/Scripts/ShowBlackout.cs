using UnityEngine;

public class ShowBlackout : MonoBehaviour
{
    public void ShowTheBlackout()
    {
        Camera cam = Camera.main;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.black;
        cam.cullingMask = LayerMask.GetMask("Player");
    }
}
