using UnityEngine;

public class CameraEnvirontmentManager : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;

    public void SetSolidColorBackground(Color color)
    {
        if (targetCamera == null) return;

        targetCamera.clearFlags = CameraClearFlags.SolidColor;
        targetCamera.backgroundColor = color;
    }

    public void SetSkyboxBackground()
    {
        if (targetCamera == null) return;

        targetCamera.clearFlags = CameraClearFlags.Skybox;
    }

    public void SetDepthOnly()
    {
        if (targetCamera == null) return;

        targetCamera.clearFlags = CameraClearFlags.Depth;
    }

    public void SetUnClear()
    {
        if (targetCamera == null) return;

        targetCamera.clearFlags = CameraClearFlags.Nothing;
    }
}
