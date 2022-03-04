using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LoadPrefs : MonoBehaviour
{
    [Header("General Options")]
    [SerializeField]
    bool _canUse = false;
    [SerializeField]
    MenuController _menuController;

    [Header("Graphics Options")]
    [SerializeField]
    Slider _brightnessSlider;
    [SerializeField]
    TMP_Text _brightnessValueText;
    float _defaultBrightness = 0.0f;

    [SerializeField]
    TMP_Dropdown _qualityDropdown;
    int _defaultQuality = 3;

    [SerializeField]
    Toggle _fullscreenToggle;

    [Header("Audio Options")]
    [SerializeField]
    TMP_Text _volumeTextValue;
    [SerializeField]
    Slider _volumeSlider;
    float _defaultVolume = 100;

    [Header("Gameplay Options")]
    [SerializeField]
    TMP_Text _sensitivityValueText;
    [SerializeField]
    Slider _sensitivitySlider;
    float _defaultSensitivity = 2;
    [SerializeField]
    Toggle _invertYToggle;

    [Header("Misc")]
    [SerializeField]
    Volume _post;
    ColorAdjustments _ca;

    private void Awake()
    {
        if (_canUse)
        {
            _post.profile.TryGet(out _ca);

            if (PlayerPrefs.HasKey("masterBrightness"))
            {
                float localBrightness = PlayerPrefs.GetFloat("masterBrightness");

                _brightnessValueText.text = localBrightness.ToString("0.0");
                _brightnessSlider.value = localBrightness;
                _ca.postExposure.value = localBrightness;
            } 
            else
            {
                _brightnessValueText.text = _defaultBrightness.ToString("0.0");
                _brightnessSlider.value = _defaultBrightness;
                _ca.postExposure.value = _defaultBrightness;
            }

            if (PlayerPrefs.HasKey("masterFullscreen"))
            {
                int localFullscreen = PlayerPrefs.GetInt("masterFullscreen");

                if (localFullscreen == 1)
                {
                    Screen.fullScreen = true;
                    _fullscreenToggle.isOn = true;
                }
                else
                {
                    Screen.fullScreen = false;
                    _fullscreenToggle.isOn = false;
                }
            } 
            else
            {
                Screen.fullScreen = true;
                _fullscreenToggle.isOn = true;
            }

            if (PlayerPrefs.HasKey("masterQuality"))
            {
                int localQuality = PlayerPrefs.GetInt("masterQuality");

                _qualityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }
            else
            {
                _qualityDropdown.value = _defaultQuality;
                QualitySettings.SetQualityLevel(_defaultQuality);
            }

            if (PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");

                _volumeTextValue.text = localVolume.ToString();
                _volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }
            else
            {
                _volumeTextValue.text = _defaultVolume.ToString();
                _volumeSlider.value = _defaultVolume;
                AudioListener.volume = _defaultVolume;
            }

            if (PlayerPrefs.HasKey("masterSensitivity"))
            {
                float localSensitivity = PlayerPrefs.GetFloat("masterSensitivity");

                _sensitivityValueText.text = localSensitivity.ToString();
                _sensitivitySlider.value = localSensitivity;
                _menuController.mainSensitivity = Mathf.RoundToInt(localSensitivity);
            }
            else
            {
                _sensitivityValueText.text = _defaultSensitivity.ToString();
                _sensitivitySlider.value = _defaultSensitivity;
                _menuController.mainSensitivity = Mathf.RoundToInt(_defaultSensitivity);
            }

            if (PlayerPrefs.HasKey("masterInvertY"))
            {
                int localInvertY = PlayerPrefs.GetInt("masterInvertY");

                if (localInvertY == 1)
                {
                    _invertYToggle.isOn = true;
                }
                else
                {
                    _invertYToggle.isOn = false;
                }
            }
            else
            {
                _invertYToggle.isOn = false;
            }
        }
    }
}
