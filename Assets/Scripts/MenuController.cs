using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuController : MonoBehaviour
{
    [Header("Poziomy do za³adowania")]
    string _newGameLevel = "Level1";
    string _levelToLoad;

    [SerializeField]
    GameObject _continueButton;

    [Header("Graphics Options")]
    float _defaultBrightness = 1.0f;
    int _defaultQuality = 1;
    [SerializeField]
    Slider _brightnessSlider;
    [SerializeField]
    TMP_Text _brightnessValueText;

    [SerializeField]
    TMP_Dropdown _resolutionDropdown;
    [SerializeField]
    TMP_Dropdown _qualityDropdown;
    [SerializeField]
    Toggle _fullscreenToggle;
    Resolution[] _resolutions;
    int _qualityLevel;
    bool _isFullscreen;
    float _brightnessLevel;
    
    [Header("Audio Options")]
    [SerializeField]
    float _defaultVolume = 100;
    [SerializeField]
    TMP_Text _volumeValueText;
    [SerializeField]
    Slider _volumeSlider;

    [Header("Gameplay Options")]
    [SerializeField]
    float _defaultSensitivity = 4;
    public float mainSensitivity = 4;
    [SerializeField]
    TMP_Text _sensitivityValueText;
    [SerializeField]
    Slider _sensitivitySlider;
    [SerializeField]
    Toggle _invertYToggle;

    [Header("Confirmation")]
    [SerializeField]
    GameObject _confirmationPrompt;

    private void Start()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            _continueButton.SetActive(true);
        }
        else
        {
            _continueButton.SetActive(false);
        }

        _resolutions = Screen.resolutions;
        _resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height;
            options.Add(option);

            if (_resolutions[i].width == Screen.width && _resolutions[i].height == Screen.height) currentResolutionIndex = i;
        }

        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.value = currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();
    }

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(_newGameLevel);
    }

    public void Continue()
    {
        _levelToLoad = PlayerPrefs.GetString("SavedLevel");
        SceneManager.LoadScene(_levelToLoad);
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
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        _isFullscreen = isFullscreen;
    }

    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);

        PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

        PlayerPrefs.SetInt("masterFullscreen", (_isFullscreen ? 1 : 0));
        Screen.fullScreen = _isFullscreen;

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
                _brightnessSlider.value = _defaultBrightness;
                _brightnessValueText.text = _defaultBrightness.ToString("0.0");

                _qualityDropdown.value = _defaultQuality;
                QualitySettings.SetQualityLevel(_defaultQuality);

                _fullscreenToggle.isOn = false;
                Screen.fullScreen = false;

                Resolution currentResolution = Screen.currentResolution;
                Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
                _resolutionDropdown.value = _resolutions.Length;

                GraphicsApply();
                break;
            case "Audio":
                AudioListener.volume = _defaultVolume;
                _volumeSlider.value = _defaultVolume;
                _volumeValueText.text = _defaultVolume.ToString();

                VolumeApply();
                break;

            case "Gameplay":
                _sensitivityValueText.text = _defaultSensitivity.ToString();
                _sensitivitySlider.value = _defaultSensitivity;
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
