using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public static DoorManager Instance;

    private string currentDoorTag = null;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OpenDoor(string tag)
    {
        switch (tag)
        {
            case "Office1":
                Debug.Log("move to: " + tag);
                // SceneLoader.Instance.LoadOffice1();
                break;
            case "Office2":
                Debug.Log("move to: " + tag);
                // SceneLoader.Instance.LoadOffice2();
                break;
            case "Office3":
                Debug.Log("move to: " + tag);
                // SceneLoader.Instance.LoadOffice3();
                break;
            case "Ocean":
                Debug.Log("move to: " + tag);
                SceneLoader.Instance.LoadOcean();
                break;
        }

        // PhotoCaptureUI.Instance.HideDetectDoor();
    }
}
