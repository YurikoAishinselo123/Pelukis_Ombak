using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public bool onGameplay = false;
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
}
