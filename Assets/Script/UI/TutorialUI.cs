using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    [Header("Tutorial Setup")]
    public Sprite[] tutorialSprites;
    public Canvas tutorialCanvas;
    public Image tutorialImage;
    public Button nextButton;
    public TMP_Text buttonText;
    public static TutorialUI Instance;
    private bool finishTutorial = false;

    private int currentIndex = 0;
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
    }

    void Start()
    {
        if (tutorialSprites.Length == 0 || tutorialImage == null || nextButton == null || buttonText == null)
        {
            Debug.LogError("TutorialUI is not set up correctly in the Inspector!");
            return;
        }

        tutorialImage.sprite = tutorialSprites[currentIndex];
        nextButton.onClick.AddListener(NextSlide);
        UpdateButtonLabel();
    }

    void Update()
    {
        if (InputManager.Instance.SpaceKey)
        {
            NextSlide();
        }
    }

    void NextSlide()
    {
        currentIndex++;

        if (currentIndex < tutorialSprites.Length)
        {
            tutorialImage.sprite = tutorialSprites[currentIndex];
            UpdateButtonLabel();
        }
        else
        {
            finishTutorial = true;
            tutorialCanvas.enabled = false;
        }
    }

    void UpdateButtonLabel()
    {
        if (currentIndex == tutorialSprites.Length - 1)
        {
            buttonText.text = "OK [Space]";
        }
        else
        {
            buttonText.text = "Ok [Space]";
        }
    }

    public void ShowTutorialUI()
    {
        if (!finishTutorial)
            tutorialCanvas.enabled = true;
    }

    public void HideTutorialUI()
    {
        Debug.Log("Hide UI");
        tutorialCanvas.enabled = false;
    }
}
