using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCaptureUI : MonoBehaviour
{
    public static PhotoCaptureUI Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private Image photoDisplayArea;
    [SerializeField] private GameObject showPhoto;
    [SerializeField] private GameObject cameraFlash;
    [SerializeField] private GameObject cameraFrame;
    [SerializeField] private float flashTime = 0.3f;

    [Header("Temp Settings")]
    [SerializeField] private GameObject CameraObject;
    [SerializeField] private GameObject VacuumObject;
    [SerializeField] private GameObject detectDoorCanvas;
    private int currentSelectedIndex = -1;
    private bool cameraActive = false;

    void Awake()
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

    // Temp
    void Update()
    {
        CheckSelectedItem();
    }

    void Start()
    {
        //Temp
        CameraObject.SetActive(false);
        VacuumObject.SetActive(false);
        detectDoorCanvas.SetActive(false);


        cameraFrame.SetActive(true);
        showPhoto.SetActive(false);
        cameraFlash.SetActive(false);
    }

    public void SetCameraFrameActive(bool active)
    {
        cameraFrame.SetActive(active);
    }

    public void DisplayPhoto(Texture2D texture)
    {
        Sprite photoSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        photoDisplayArea.sprite = photoSprite;
        cameraFrame.SetActive(false);
        showPhoto.SetActive(true);
    }

    public void HidePhoto()
    {
        photoDisplayArea.sprite = null;
        cameraFrame.SetActive(true);
        showPhoto.SetActive(false);
    }

    public IEnumerator FlashEffect()
    {
        cameraFlash.SetActive(true);
        yield return new WaitForSeconds(flashTime);
        cameraFlash.SetActive(false);
    }

    public bool CameraActive()
    {
        return cameraActive;
    }

    // Temp Script
    public void CheckSelectedItem()
    {
        int selectedIndex = InputManager.Instance.GetSelectedItemByKey();

        if (selectedIndex == currentSelectedIndex)
        {
            // Pressed the same key again — toggle OFF
            if (selectedIndex == 1)
            {
                VacuumObject.SetActive(false);
            }
            else if (selectedIndex == 2)
            {
                CameraObject.SetActive(false);
                cameraActive = false;
            }

            currentSelectedIndex = -1; // Reset
            return;
        }

        // Turn ON selected item, and turn OFF others
        if (selectedIndex == 1 && ItemManager.Instance.HasVacuum())
        {
            VacuumObject.SetActive(true);
            CameraObject.SetActive(false);
            currentSelectedIndex = 1;
            cameraActive = false;
        }
        else if (selectedIndex == 2 && ItemManager.Instance.HasCamera())
        {
            CameraObject.SetActive(true);
            VacuumObject.SetActive(false);
            currentSelectedIndex = 2;
            cameraActive = true;
        }
    }

    // Temp script
    public void ShowDetectDoor()
    {
        detectDoorCanvas.SetActive(true);
    }

    public void HideDetectDoor()
    {
        detectDoorCanvas.SetActive(false);
    }
}
