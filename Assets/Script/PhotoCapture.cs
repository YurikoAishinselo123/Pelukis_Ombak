using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture : MonoBehaviour
{
    [Header("Photo Taker")]
    [SerializeField] private Image photoDisplayArea;
    [SerializeField] private GameObject photoFrame;

    [Header("Flash Effect")]
    [SerializeField] private GameObject cameraFlash;
    [SerializeField] private float flashTime = 0.3f;

    private Texture2D screenCapture;
    private bool viewingPhoto;

    private string photoPath;

    void Start()
    {
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        photoPath = Path.Combine(Application.persistentDataPath, "photo.png");
    }

    void Update()
    {
        if (InputManager.Instance.GetCapturePhotoInput())
        {
            if (!viewingPhoto)
            {
                StartCoroutine(CapturePhoto());
            }
            else
            {
                RemovePhoto();
            }
        }
        // Temp Input
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (!viewingPhoto)
            {
                // Debug.Log("tse");
                LoadAndShowSavedPhoto();
            }
        }
    }

    IEnumerator CapturePhoto()
    {
        viewingPhoto = true;
        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);
        screenCapture.ReadPixels(regionToRead, 0, 0);
        screenCapture.Apply();

        // Save as PNG
        byte[] bytes = screenCapture.EncodeToPNG();
        File.WriteAllBytes(photoPath, bytes);
        Debug.Log($"Photo saved to: {photoPath}");

        // Show
        ShowPhoto(screenCapture);
        StartCoroutine(CameraFlashEffect());
    }

    void LoadAndShowSavedPhoto()
    {
        if (File.Exists(photoPath))
        {
            byte[] bytes = File.ReadAllBytes(photoPath);
            Texture2D loadedTex = new Texture2D(2, 2); // size will be overwritten by LoadImage
            loadedTex.LoadImage(bytes);

            ShowPhoto(loadedTex);
        }
        else
        {
            Debug.LogWarning("No saved photo found!");
        }
    }

    void ShowPhoto(Texture2D texture)
    {
        Sprite photoSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        photoDisplayArea.sprite = photoSprite;
        photoFrame.SetActive(true);
        viewingPhoto = true;
    }

    IEnumerator CameraFlashEffect()
    {
        cameraFlash.SetActive(true);
        yield return new WaitForSeconds(flashTime);
        cameraFlash.SetActive(false);
    }

    void RemovePhoto()
    {
        viewingPhoto = false;
        photoFrame.SetActive(false);
    }
}
