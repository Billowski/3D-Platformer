using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuController : MonoBehaviour
{
    [SerializeField]
    GameObject _eventSystem;

    [Header("Poziomy do za³adowania")]
    string _newGameLevel = "Level1";
    string _levelToLoad;

    [Header("Graphics Options")]
    float _defaultBrightness = 0.0f;
    int _defaultQuality = 3;
    [SerializeField]
    GameObject _brightnessSlider;
    [SerializeField]
    TMP_Text _brightnessValueText;

    [SerializeField]
    TMP_Dropdown _resolutionDropdown;
    [SerializeField]
    TMP_Dropdown _qualityDropdown;
    [SerializeField]
    Toggle _fullscreenToggle;
    Resolution[] _resolutions;
    Resolution _resolution;
    int _qualityLevel;
    bool _isFullscreen;
    float _brightnessLevel;
    
    [Header("Audio Options")]
    [SerializeField]
    float _defaultVolume = 100;
    [SerializeField]
    TMP_Text _volumeValueText;
    [SerializeField]
    GameObject _volumeSlider;

    [Header("Gameplay Options")]
    [SerializeField]
    float _defaultSensitivity = 2;
    public float mainSensitivity = 2;
    [SerializeField]
    TMP_Text _sensitivityValueText;
    [SerializeField]
    GameObject _sensitivitySlider;
    [SerializeField]
    Toggle _invertYToggle;

    [Header("Confirmation")]
    [SerializeField]
    GameObject _confirmationPrompt;

    [Header("Misc")]
    [SerializeField]
    Volume _post;
    ColorAdjustments _ca;
    [SerializeField]
    GameObject _panel;
    [SerializeField]
    GameObject _newGamePanel;
    [SerializeField]
    GameObject _optionsPanel;
    [SerializeField]
    GameObject _graphicsPanel;
    [SerializeField]
    GameObject _audioPanel;
    [SerializeField]
    GameObject _gameplayPanel;
    [SerializeField]
    GameObject _exitPanel;
    [SerializeField]
    GameObject _newGameButton;
    [SerializeField]
    GameObject _newGameYesButton;
    [SerializeField]
    GameObject _continueButton;
    [SerializeField]
    GameObject _graphicsButton;
    [SerializeField]
    GameObject _exitYesButton;

    private void Start()
    {
        if (PlayerPrefs.HasKey("savedLevel"))
        {
            _continueButton.SetActive(true);
        }
        else
        {
            _continueButton.SetActive(false);
        }

        if(Gamepad.all.Count > 0) _eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(_newGameButton);

        InputSystem.onDeviceChange += (device, change) =>
        {
            switch (change)
            {
                case InputDeviceChange.Added:
                    if(_panel.activeInHierarchy) _eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(_newGameButton);
                    if(_newGamePanel.activeInHierarchy) _eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(_newGameYesButton);
                    if(_optionsPanel.activeInHierarchy) _eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(_graphicsButton);
                    if(_graphicsPanel.activeInHierarchy) _eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(_brightnessSlider);
                    if(_audioPanel.activeInHierarchy) _eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(_volumeSlider);
                    if(_gameplayPanel.activeInHierarchy) _eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(_sensitivitySlider);
                    if(_exitPanel.activeInHierarchy) _eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(_exitYesButton);
                    break;

                case InputDeviceChange.Removed:
                    _eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
                    break;
            }
        };

        _post.profile.TryGet(out _ca);

        _resolutions = Screen.resolutions;
        _resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height + " @" + _resolutions[i].refreshRate + "Hz";
            options.Add(option);

            if (_resolutions[i].width == Screen.width && _resolutions[i].height == Screen.height) currentResolutionIndex = i;
        }

        if (!PlayerPrefs.HasKey("masterResolution"))
        {
            PlayerPrefs.SetInt("masterResolution", currentResolutionIndex);
        }

        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.value = currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();
    }

    public void Select(Transform panel)
    {
        panel.gameObject.SetActive(true);

        if(Gamepad.all.Count > 0)
        {
            bool enable = false;
            foreach (Transform child in panel)
            {
                if (!enable)
                {
                    if (child.GetComponent<Button>() != null || child.GetComponent<Slider>() != null)
                    {
                        _eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(child.gameObject);
                        enable = true;
                    }
                }
            }
        }
    }

    public void NewGameDialogYes()
    {
        LevelManager.Instance.LoadScene(_newGameLevel);
    }

    public void Continue()
    {
        _levelToLoad = PlayerPrefs.GetString("savedLevel");
        LevelManager.Instance.LoadScene(_levelToLoad);
    }

    public void ExitDialogYes()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        _brightnessValueText.text = brightness.ToString("0.0");
    }

    public void SetResolution(int resolutionIndex)
    {
        _resolution = _resolutions[resolutionIndex];
    }

    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        _isFullscreen = isFullscreen;
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
        _ca.postExposure.value = _brightnessLevel;

        PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

        PlayerPrefs.SetInt("masterFullscreen", (_isFullscreen ? 1 : 0));
        Screen.fullScreen = _isFullscreen;

        Screen.SetResolution(_resolution.width, _resolution.height, Screen.fullScreen, _resolution.refreshRate);

        StartCoroutine(ConfirmationBox());
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        _volumeValueText.text = volume.ToString();
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void SetSensitivity(float sensitivity)
    {
        mainSensitivity = Mathf.RoundToInt(sensitivity);
        _sensitivityValueText.text = sensitivity.ToString();
    }

    public void GameplayApply()
    {
        if (_invertYToggle.isOn)
        {
            PlayerPrefs.SetInt("masterInvertY", 1);
        } 
        else
        {
            PlayerPrefs.SetInt("masterInvertY", 0);
        }

        PlayerPrefs.SetFloat("masterSensitivity", mainSensitivity);
        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string MenuType)
    {
        switch (MenuType)
        {
            case "Graphics":
                _brightnessSlider.GetComponent<Slider>().value = _defaultBrightness;
                _brightnessValueText.text = _defaultBrightness.ToString("0.0");

                _qualityDropdown.value = _defaultQuality;
                QualitySettings.SetQualityLevel(_defaultQuality);

                _fullscreenToggle.isOn = true;
                Screen.fullScreen = true;

                int defaultResolution = PlayerPrefs.GetInt("masterResolution");
                Screen.SetResolution(_resolutions[defaultResolution].width, _resolutions[defaultResolution].height, Screen.fullScreen, _resolutions[defaultResolution].refreshRate);
                _resolutionDropdown.value = defaultResolution;

                GraphicsApply();
                break;

            case "Audio":
                AudioListener.volume = _defaultVolume;
                _volumeSlider.GetComponent<Slider>().value = _defaultVolume;
                _volumeValueText.text = _defaultVolume.ToString();

                VolumeApply();
                break;

            case "Gameplay":
                _sensitivityValueText.text = _defaultSensitivity.ToString();
                _sensitivitySlider.GetComponent<Slider>().value = _defaultSensitivity;
                mainSensitivity = _defaultSensitivity;

                _invertYToggle.isOn = false;

                GameplayApply();
                break;
        }
    }

    public IEnumerator ConfirmationBox()
    {
        _confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        _confirmationPrompt.SetActive(false);
    }
}
