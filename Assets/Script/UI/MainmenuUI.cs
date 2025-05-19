using UnityEngine;
using UnityEngine.UI;


public class MainmenuUI : MonoBehaviour
{

    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    void Awake()
    {
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    void Start()
    {
        AudioManager.Instance.PlayMainThemeBacksound();
    }

    public void StartGame()
    {
        GameplayManager.Instance.onGameplay = true;
        SceneLoader.Instance.LoadOffice1();
        CursorManager.Instance.HideCursor();
        SpawnCharacterManager.Instance.SpawnPositionOnStart(new Vector3(2.53f, 1.075f, 1.74f));
    }

    public void QuitGame()
    {
        SceneLoader.Instance.QuitGame();
    }
}
