using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public static DoorManager Instance;
    private bool inOffice = true;
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

    public void OpenDoor(string tag)
    {
        inOffice = true;

        switch (tag)
        {
            case "Office3ToOffice1":
                SceneLoader.Instance.LoadSceneWithVideo("Office1", new Vector3(5.3f, 1.1f, 19f));
                break;
            case "Office2ToOffice1":
                SceneLoader.Instance.LoadSceneWithVideo("Office1", new Vector3(6.5f, 1.1f, 8.8f));
                break;
            case "Office2":
                SceneLoader.Instance.LoadSceneWithVideo("Office2", new Vector3(-2f, -0.6f, 0.7f));
                break;
            case "Office3":
                SceneLoader.Instance.LoadSceneWithVideo("Office3", new Vector3(11f, 1f, 18f));
                break;
            case "Ocean":
                inOffice = false;
                TutorialUI.Instance.HideTutorialUI();
                SceneLoader.Instance.LoadSceneWithVideo("Ocean", new Vector3(2.9f, 1f, -0.07f));
                break;
        }

        if (inOffice)
        {
            PlayerController.Instance.OfficeEnvirontment();
            AudioManager.Instance.PlayMainThemeBacksound();
        }
        else
        {
            PlayerController.Instance.OceanEnvirontment();
            AudioManager.Instance.PlayExplorationBacksound();
        }
    }
}
