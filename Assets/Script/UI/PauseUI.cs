using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    public static PauseUI Instance;
    public GameObject PauseCanvas;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;
    private bool isPaused = false;

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
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        if (InputManager.Instance.Back)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        PauseCanvas.SetActive(true);
        CursorManager.Instance.ShowCursor();
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        CursorManager.Instance.HideCursor();
        PauseCanvas.SetActive(false);
    }

    public void QuitGame()
    {
        PauseCanvas.SetActive(false);
        SceneLoader.Instance.LoadMainMenu();
    }
}
