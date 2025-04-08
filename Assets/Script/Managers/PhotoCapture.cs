using System.Collections;
using System.IO;
using UnityEngine;

public class PhotoCapture : MonoBehaviour
{
    private Texture2D screenCapture;
    private bool viewingPhoto;

    private string photoPath;

    void Start()
    {
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        photoPath = Path.Combine(Application.persistentDataPath, "photo.png");
        PhotoCaptureUI.Instance.HidePhoto();
    }

    void Update()
    {
        if (InputManager.Instance.GetCapturePhotoInput)
        {
            if (!viewingPhoto)
                StartCoroutine(CapturePhoto());
            else
                RemovePhoto();
        }

        if (Input.GetKeyDown(KeyCode.H) && !viewingPhoto)
        {
            LoadAndShowSavedPhoto();
        }
    }

    IEnumerator CapturePhoto()
    {

        viewingPhoto = true;
        PhotoCaptureUI.Instance.SetCameraFrameActive(false);
        yield return new WaitForEndOfFrame();

        Rect region = new Rect(0, 0, Screen.width, Screen.height);
        screenCapture.ReadPixels(region, 0, 0);
        screenCapture.Apply();

        File.WriteAllBytes(photoPath, screenCapture.EncodeToPNG());
        Debug.Log($"Photo saved to: {photoPath}");

        yield return StartCoroutine(PhotoCaptureUI.Instance.FlashEffect());
        PhotoCaptureUI.Instance.DisplayPhoto(screenCapture);
    }

    void LoadAndShowSavedPhoto()
    {
        if (File.Exists(photoPath))
        {
            byte[] bytes = File.ReadAllBytes(photoPath);
            Texture2D loadedTex = new Texture2D(2, 2);
            loadedTex.LoadImage(bytes);

            PhotoCaptureUI.Instance.DisplayPhoto(loadedTex);
            viewingPhoto = true;
        }
        else
        {
            Debug.LogWarning("No saved photo found!");
        }
    }

    void RemovePhoto()
    {
        PhotoCaptureUI.Instance.HidePhoto(); ;
        viewingPhoto = false;
    }
}
