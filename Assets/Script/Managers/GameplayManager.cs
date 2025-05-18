using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public bool onGameplay = false;
    private bool onInteraction = false;
    public static GameplayManager Instance;

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

    void Update()
    {
        Debug.Log("interaction : " + onInteraction);
    }

    public void TalkingWithNPC()
    {
        onInteraction = true;
    }

    public void FinishTalkingWithNPC()
    {
        onInteraction = false;
    }

    public bool OnInteraction()
    {
        return onInteraction;
    }

}
