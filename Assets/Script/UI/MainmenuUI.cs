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
        SceneLoader.Instance.LoadOffice1();
    }

    public void QuitGame()
    {
        SceneLoader.Instance.QuitGame();
    }
}
