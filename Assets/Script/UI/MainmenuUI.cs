using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;


public class MainmenuUI : MonoBehaviour
{

    [SerializeField] private Button startButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button cutsceneButton;
    [SerializeField] private Button quitButton;
    // private Dictionary<int, int> missionProgress;


    void Awake()
    {
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
        newGameButton.onClick.AddListener(NewGame);
        cutsceneButton.onClick.AddListener(CutScene);
    }

    void Update()
    {
        // missionProgress = SaveSystemManager.Instance.LoadMissionProgress();
        // Debug.Log("Tes mission progress : " + missionProgress.Count);
        // if (missionProgress == null || missionProgress.Count <= 3)
        // {
        //     startButton.gameObject.SetActive(false);
        // }

        bool hasPlayedCutscene = PlayerPrefs.GetInt("HasPlayedCutscene", 0) == 1;
        if (!hasPlayedCutscene)
        {
            startButton.gameObject.SetActive(false);
        }

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
        AudioManager.Instance.PlayOfficeBacksound();
        SpawnCharacterManager.Instance.SpawnPositionOnStart(new Vector3(2.53f, 1.075f, 1.74f));
    }

    private void NewGame()
    {
        SaveSystemManager.Instance.ResetMissionProgress();
        MissionManager.Instance?.ReinitializeProgress();
        // SaveSystemManager.Instance.ResetCollectedItems();
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
