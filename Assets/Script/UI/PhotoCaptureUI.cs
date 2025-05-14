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

        cameraFrame.SetActive(true);
        showPhoto.SetActive(false);
        cameraFlash.SetActive(false);
    }

    public void SetCameraFrameActive(bool active)
    {
        CameraObject.SetActive(active);
        cameraFrame.SetActive(active);
        Debug.Log("Active : " + active);
    }

    public void DisplayPhoto(Texture2D texture)
    {
        Time.timeScale = 0;
        Sprite photoSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        photoDisplayArea.sprite = photoSprite;
        cameraFrame.SetActive(false);
        showPhoto.SetActive(true);
    }

    public void HidePhoto()
    {
        Time.timeScale = 1;
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
        if (selectedIndex == 1 && ItemManager.Instance.HasItem(ItemType.Vacuum))
        {
            VacuumObject.SetActive(true);
            CameraObject.SetActive(false);
            currentSelectedIndex = 1;
            cameraActive = false;
        }
        else if (selectedIndex == 2 && ItemManager.Instance.HasItem(ItemType.Camera))
        {
            CameraObject.SetActive(true);
            VacuumObject.SetActive(false);
            currentSelectedIndex = 2;
            cameraActive = true;
        }
    }
}
