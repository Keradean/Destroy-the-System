using UnityEngine;
using UnityEngine.SceneManagement;
using Project.Scripts.Dennis.Game;

public class DefeatScreenUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject defeatScreen;

    [Header("Game Manager")]
    [SerializeField] private GameManager gameManager;

    [Header("Scenes")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void Awake()
    {
        if (!gameManager)
        {
            gameManager = FindAnyObjectByType<GameManager>();
        }

        defeatScreen.SetActive(false);
    }

    private void OnEnable()
    {
        if (gameManager)
        {
            gameManager.OnLose += ShowDefeatScreen;
        }
    }

    private void OnDisable()
    {
        if (gameManager)
        {
            gameManager.OnLose -= ShowDefeatScreen;
        }
    }

    private void ShowDefeatScreen()
    {
        defeatScreen.SetActive(true);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}