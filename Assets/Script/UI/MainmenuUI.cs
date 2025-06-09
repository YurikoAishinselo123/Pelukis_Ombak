using UnityEngine;
using UnityEngine.UI;


public class MainmenuUI : MonoBehaviour
{

    [SerializeField] private Button startButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button cutsceneButton;
    [SerializeField] private Button quitButton;

    void Awake()
    {
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
        newGameButton.onClick.AddListener(NewGame);
        cutsceneButton.onClick.AddListener(CutScene);
    }

    void Start()
    {
        AudioManager.Instance.PlayMainThemeBacksound();
    }

    private void StartGame()
    {
        GameplayManager.Instance.onGameplay = true;
        SceneLoader.Instance.LoadOffice1();
        GameplayManager.Instance.ContinueGame();
        CursorManager.Instance.HideCursor();
        SpawnCharacterManager.Instance.SpawnPositionOnStart(new Vector3(2.53f, 1.075f, 1.74f));
    }

    private void NewGame()
    {
        SaveSystemManager.Instance.ResetMissionProgress();
        MissionManager.Instance?.ReinitializeProgress();
        SceneLoader.Instance.LoadCutscene();
        // GameplayManager.Instance.onGameplay = true;
        // SceneLoader.Instance.LoadOffice1();
        // GameplayManager.Instance.NewGame();
        // CursorManager.Instance.HideCursor();
        // SpawnCharacterManager.Instance.SpawnPositionOnStart(new Vector3(2.53f, 1.075f, 1.74f));
    }

    private void CutScene()
    {
        SceneLoader.Instance.LoadCutscene();
    }

    private void QuitGame()
    {
        SceneLoader.Instance.QuitGame();
    }
}
