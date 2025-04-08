using UnityEngine;
using UnityEngine.UI;


public class MainmenuUI : MonoBehaviour
{

    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    void Awake()
    {
        AudioManager.Instance.PlayMainThemeBacksound();
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
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
