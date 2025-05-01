using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public static DoorManager Instance;
    private bool inOffice = true;

    private string currentDoorTag = null;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OpenDoor(string tag)
    {
        inOffice = true;
        switch (tag)
        {
            case "Office1":
                Debug.Log("move to: " + tag);
                AudioManager.Instance.PlayMainThemeBacksound();
                SceneLoader.Instance.LoadOffice1();
                break;
            case "Office2":
                Debug.Log("move to: " + tag);
                SceneLoader.Instance.LoadOffice2();
                break;
            case "Office3":
                Debug.Log("move to: " + tag);
                SceneLoader.Instance.LoadOffice3();
                break;
            case "Ocean":
                inOffice = false;
                Debug.Log("move to: " + tag);
                SceneLoader.Instance.LoadOcean();
                break;
        }
        if (inOffice)
            PlayerController.Instance.OfficeEnvirontment();
        else
            PlayerController.Instance.OceanEnvirontment();


        PhotoCaptureUI.Instance.HideDetectDoor();
    }
}
