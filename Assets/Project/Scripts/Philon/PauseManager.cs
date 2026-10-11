using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] List<GameObject> pausePanel;

    [SerializeField] InputActionReference pauseAction;

    private static PauseManager instance; // Dennis

    private void Awake() // Dennis
    {
        instance = this; // Dennis
        var ui = Project.Scripts.Dennis.UI.UIReferences.Find(); // Dennis
        if (ui == null) return; // Dennis
        if (pausePanel.Count < 2) pausePanel = new List<GameObject> { null, null }; // Dennis
        if (pausePanel[0] == null) pausePanel[0] = ui.PauseMenu; // Dennis
        if (pausePanel[1] == null) pausePanel[1] = ui.LevelUpScreen; // Dennis
    }

    private void OnEnable()
    {
        pauseAction.action.Enable();
        pauseAction.action.performed += OnPause;
    }
    private void OnDisable()
    {
        pauseAction.action.performed -= OnPause;
        pauseAction.action.Disable();
    }

    public static bool IsPaused { get; private set; } = false;


    public void SetPause()
    {
        bool pause = false;
        foreach (var panel in pausePanel)
        {
            if (panel != null && panel.activeSelf) // Dennis
            {
                pause = true;
                break;
            }
        }
        IsPaused = pause;
        Time.timeScale = IsPaused ? 0f : 1f;
    }

    public void Continue()
    {
        if (instance != null && instance != this) { instance.Continue(); return; } // Dennis
        if (pausePanel.Count == 0 || pausePanel[0] == null) return; // Dennis
        pausePanel[0].SetActive(false);
        SetPause();
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (pausePanel.Count == 0 || pausePanel[0] == null) return; // Dennis
        pausePanel[0].SetActive(!pausePanel[0].activeSelf);
        SetPause();
    }
}
