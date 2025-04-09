using UnityEngine;

public class DoorTransition : MonoBehaviour
{
    private string currentDoorTag = null;

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
                    SceneLoader.Instance.LoadOffice1();
                    break;
                case "Office2":
                    SceneLoader.Instance.LoadOffice2();
                    break;
                case "Office3":
                    SceneLoader.Instance.LoadOffice3();
                    break;
                case "Ocean":
                    SceneLoader.Instance.LoadOcean();
                    break;
            }

            currentDoorTag = null;
            PhotoCaptureUI.Instance.HideDetectDoor();
        }
    }

    private bool IsDoorTag(string tag)
    {
        return tag == "Office1" || tag == "Office2" || tag == "Office3" || tag == "Ocean";
    }
}
