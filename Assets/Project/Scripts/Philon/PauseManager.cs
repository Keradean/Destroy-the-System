using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
    [SerializeField] List<GameObject> pausePanel;

    [SerializeField] InputActionReference pauseAction;

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
            if (panel.activeSelf)
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
        pausePanel[0].SetActive(false);
        SetPause();
    }

    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        pausePanel[0].SetActive(!pausePanel[0].activeSelf);
        SetPause();
    }
}
