using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DetectDoorUI : MonoBehaviour
{
    public static DetectDoorUI Instance { get; private set; }

    [SerializeField] private GameObject detectDoorCanvas;

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

    void Start()
    {
        HideDetectDoor();
    }

    public void ShowDetectDoor()
    {
        detectDoorCanvas.SetActive(true);
    }

    public void HideDetectDoor()
    {
        detectDoorCanvas.SetActive(false);
    }
}
