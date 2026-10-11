using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject exitButton; // Dennis

    // Dennis: im Browser kann man das Spiel nicht beenden, deshalb Exit dort ausblenden
    private void Awake()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (exitButton == null) exitButton = GameObject.Find("Exit");
        if (exitButton != null) exitButton.SetActive(false);
#endif
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}