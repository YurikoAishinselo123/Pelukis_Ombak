using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public bool onGameplay = false;
    public static GameplayManager Instance;
    private bool isTalkingWithNPC = false;
    private bool newGame = false;
    public bool pauseGame = false;

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
        onGameplay = false;
    }

    public void TalkingWithNPC()
    {
        isTalkingWithNPC = true;
    }

    public void FinishTalkingWithNPC()
    {
        isTalkingWithNPC = false;
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("HasNewGame", 1);
        newGame = true;
    }

    public void ContinueGame()
    {
        newGame = false;
    }

    public bool NewGameStatus()
    {
        return newGame;
    }

    public bool OnInteractionWithNPC()
    {
        return isTalkingWithNPC;
    }

}
