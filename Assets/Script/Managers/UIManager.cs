using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public bool hideUIWhenCameraActive;
    public bool detectManagerActive = true;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    public void HideAllUI()
    {
        Debug.Log("Hide");
        PhotoCaptureUI.Instance.SetCameraFrameActive(false);
        MissionUIManager.Instance.HideMissionUI();
        DetectItemUI.Instance.HideDetectItemUI();
        DetectDoorUI.Instance.HideDetectDoor();
        InventoryUIManager.Instance.HideInventoryCanvas();
        PhotoCaptureUI.Instance.HidePhoto();
    }
}
