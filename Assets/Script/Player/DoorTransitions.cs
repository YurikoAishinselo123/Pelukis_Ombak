using UnityEngine;
using UnityEngine.Rendering;

public class DoorTransition : MonoBehaviour
{
    private string currentDoorTag = null;
    // [SerializeField] private Camera cam;

    private void OnTriggerEnter(Collider other)
    {
        if (IsDoorTag(other.tag))
        {
            currentDoorTag = other.tag;
            PhotoCaptureUI.Instance.ShowDetectDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsDoorTag(other.tag))
        {
            if (other.tag == currentDoorTag)
            {
                currentDoorTag = null;
                PhotoCaptureUI.Instance.HideDetectDoor();
            }
        }
    }

    private void Update()
    {
        HandleDoorInteraction();
    }

    private void HandleDoorInteraction()
    {
        if (currentDoorTag != null && InputManager.Instance.Interact)
        {
            switch (currentDoorTag)
            {
                case "Office1":
                    Debug.Log("Office 1");
                    HideOceanEffect();
                    AudioManager.Instance.PlayMainThemeBacksound();
                    PlayerController.Instance.OfficeEnvirontment();
                    SceneLoader.Instance.LoadOffice1();
                    break;
                case "Office2":
                    Debug.Log("Office 2");
                    HideOceanEffect();
                    AudioManager.Instance.PlayMainThemeBacksound();
                    PlayerController.Instance.OfficeEnvirontment();
                    SceneLoader.Instance.LoadOffice2();
                    break;
                case "Office3":
                    Debug.Log("Office 3");
                    HideOceanEffect();
                    AudioManager.Instance.PlayMainThemeBacksound();
                    PlayerController.Instance.OfficeEnvirontment();
                    SceneLoader.Instance.LoadOffice3();
                    break;
                case "Ocean":
                    Debug.Log("Ocean");
                    AudioManager.Instance.PlayExplorationBacksound();
                    PlayerController.Instance.OceanEnvirontment();
                    ShowOceanEffect();
                    SceneLoader.Instance.LoadOcean();
                    break;
            }

            currentDoorTag = null;
            PhotoCaptureUI.Instance.HideDetectDoor();
        }
    }

    public void ShowOceanEffect()
    {
        // if (cam != null)
        // {
        //     Volume volume = cam.GetComponent<Volume>();
        //     if (volume != null)
        //     {
        //         volume.enabled = true;
        //     }
        // }
    }

    public void HideOceanEffect()
    {
        // Camera cam = Camera.main;
        // if (cam != null)
        // {
        //     Volume volume = cam.GetComponent<Volume>();
        //     if (volume != null)
        //     {
        //         volume.enabled = false;
        //     }
        // }
    }

    private bool IsDoorTag(string tag)
    {
        return tag == "Office1" || tag == "Office2" || tag == "Office3" || tag == "Ocean";
    }
}