using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    bool _pause;

    [SerializeField]
    GameObject _player;
    [SerializeField]
    GameObject _menu;
    [SerializeField]
    GameObject _continue;
    [SerializeField]
    GameObject _eventSystem;
    [SerializeField]
    Volume _post;
    ColorAdjustments _ca;


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

        float brightness = PlayerPrefs.GetFloat("masterBrightness");
        _post.profile.TryGet(out _ca);
        _ca.postExposure.value = brightness;
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

    public IEnumerator Death()
    {
        yield return new WaitForSeconds(0.1f);
        _player.SetActive(false);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
