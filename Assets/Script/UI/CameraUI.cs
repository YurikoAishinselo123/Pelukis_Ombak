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

    void Start()
    {
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
}
