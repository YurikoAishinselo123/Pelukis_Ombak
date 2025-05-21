using NUnit.Framework.Constraints;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public static DoorManager Instance;
    private bool inOffice = true;
    private CharacterController characterController;
    private string currentDoorTag = null;


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
        characterController = GetComponent<CharacterController>();
    }

    public void OpenDoor(string tag)
    {
        inOffice = true;
        switch (tag)
        {
            case "Office3ToOffice1":
                Debug.Log("move to: " + tag);
                SceneLoader.Instance.LoadOffice1();
                SpawnCharacterManager.Instance.SpawnPositionOnStart(new Vector3(5.3f, 1.1f, 19f));
                break;
            case "Office2ToOffice1":
                Debug.Log("move to: " + tag);
                SceneLoader.Instance.LoadOffice1();
                SpawnCharacterManager.Instance.SpawnPositionOnStart(new Vector3(6.5f, 1.1f, 8.8f));
                break;
            case "Office2":
                Debug.Log("move to: " + tag);
                SceneLoader.Instance.LoadOffice2();
                SpawnCharacterManager.Instance.SpawnPositionOnStart(new Vector3(-2f, -0.6f, 0.7f));
                break;
            case "Office3":
                Debug.Log("move to: " + tag);
                SceneLoader.Instance.LoadOffice3();
                SpawnCharacterManager.Instance.SpawnPositionOnStart(new Vector3(11f, 1f, 18f));
                break;
            case "Ocean":
                inOffice = false;
                Debug.Log("move to: " + tag);
                SceneLoader.Instance.LoadOcean();
                SpawnCharacterManager.Instance.SpawnPositionOnStart(new Vector3(2.9f, 1f, -0.07f));
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

        DetectDoorUI.Instance.HideDetectDoor();
    }
}
