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

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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

    // Temp Script
    public void CheckSelectedItem()
    {
        int selectedIndex = InputManager.Instance.GetSelectedItemByKey();
        if (selectedIndex == 1)
        {
            VacuumObject.SetActive(true);
            CameraObject.SetActive(false);
        }
        if (selectedIndex == 2)
        {
            CameraObject.SetActive(true);
            VacuumObject.SetActive(false);
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
