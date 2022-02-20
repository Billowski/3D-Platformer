using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    bool _pause;

    [SerializeField]
    GameObject _menu;
    [SerializeField]
    GameObject _continue;
    [SerializeField]
    GameObject _eventSystem;

    private void Start()
    {
        PlayerPrefs.SetString("savedLevel", SceneManager.GetActiveScene().name);
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        InputSystem.onDeviceChange += (device, change) =>
        {
            switch (change)
            {
                case InputDeviceChange.Added:
                    _eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(_continue);
                    break;

                case InputDeviceChange.Removed:
                    _eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
                    break;
            }
        };
    }

    public void GamePause()
    {
        if (!_pause)
        {
            Time.timeScale = 0;
            _menu.SetActive(true);
            //if (Gamepad.all.Count > 0) _eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(_continue);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
            _pause = true;
        }
        else
        {
            Time.timeScale = 1;
            _menu.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
            _pause = false;
        }
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1;
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
        SceneManager.LoadScene(0);
    }
}
