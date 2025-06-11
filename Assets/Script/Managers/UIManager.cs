using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject gameplayUI;
    public bool hideUIWhenCameraActive;
    public bool detectManagerActive = true;
    private bool isMissionUIVisible = false;

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

    void Start()
    {
        InventoryUIManager.Instance.ShowInventoryCanvas();
    }

    public void HideAllUI()
    {
        Debug.Log("Hide");
        PhotoCaptureUI.Instance.SetCameraFrameActive(false);
        MissionUIManager.Instance.HideMissionUI();
        InventoryUIManager.Instance.HideInventoryCanvas();
        PhotoCaptureUI.Instance.HidePhoto();
        TutorialUI.Instance.HideTutorialUI();
        InteractionUIManager.Instance.HideInteractionUI();
    }


    void Update()
    {
        HandleMission();
    }


    private void HandleMission()
    {
        if (InputManager.Instance.Mission && GameplayManager.Instance.onGameplay)
        {
            // Debug.Log("Mission : " + isMissionUIVisible);
            if (isMissionUIVisible)
            {
                MissionUIManager.Instance.HideMissionUI();
                InventoryUIManager.Instance.ShowInventoryCanvas();
            }
            else
            {
                if (!GameplayManager.Instance.OnInteractionWithNPC())
                {
                    MissionUIManager.Instance.ShowMissionUI();
                    InventoryUIManager.Instance.HideInventoryCanvas();
                }
            }
            isMissionUIVisible = !isMissionUIVisible;
        }
    }

    public void HideGameplayUI()
    {
        Debug.Log("Hide UI");
        gameplayUI.SetActive(false);
    }

    public void ShowGameplayUI()
    {
        gameplayUI.SetActive(true);

        // Cnt works because the transition manager already destroy before show the ui
        // ShowGameplayUIDelayed(0.5f);
    }


    // private IEnumerator ShowGameplayUIDelayed(float delay)
    // {
    //     Debug.Log("tes");
    //     yield return new WaitForSeconds(delay);
    //     gameplayUI.SetActive(true);
    // }
}
