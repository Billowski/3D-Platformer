using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    bool _pause;
    [SerializeField]
    Image _menu;

    public void GamePause()
    {
        if (!_pause)
        {
            Time.timeScale = 0;
            //_input.SwitchCurrentActionMap("UI");
            _menu.gameObject.SetActive(true);
            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
            _pause = true;
        }
        else
        {
            Time.timeScale = 1;
            //_input.SwitchCurrentActionMap("Player");
            _menu.gameObject.SetActive(false);
            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
            _pause = false;
        }
    }
}
