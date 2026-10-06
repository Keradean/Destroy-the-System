using UnityEngine;
using UnityEngine.SceneManagement;
using Project.Scripts.Dennis.Game;

public class VictoryScreenUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject nextLevelButton;

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

        if (victoryScreen)
        {
            victoryScreen.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (gameManager)
        {
            gameManager.OnWin += ShowVictoryScreen;
        }
    }

    private void OnDisable()
    {
        if (gameManager)
        {
            gameManager.OnWin -= ShowVictoryScreen;
        }
    }

    private void ShowVictoryScreen()
    {
        if (victoryScreen)
        {
            victoryScreen.SetActive(true);
        }

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextLevelButton)
        {
            nextLevelButton.SetActive(
                nextSceneIndex < SceneManager.sceneCountInBuildSettings
            );
        }
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
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
        SceneManager.LoadScene("MainMenu");
    }

    [ContextMenu("Test: Show Victory Screen")]
    private void TestShowVictoryScreen()
    {
        ShowVictoryScreen();
    }
}