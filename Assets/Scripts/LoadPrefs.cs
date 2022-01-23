using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [SerializeField]
    TMP_Dropdown _qualityDropdown;

    [SerializeField]
    Toggle _fullscreenToggle;

    [Header("Audio Options")]
    [SerializeField]
    TMP_Text _volumeTextValue;
    [SerializeField]
    Slider _volumeSlider;

    [Header("Gameplay Options")]
    [SerializeField]
    TMP_Text _sensitivityValueText;
    [SerializeField]
    Slider _sensitivitySlider;
    [SerializeField]
    Toggle _invertYToggle;

    private void Awake()
    {
        if (_canUse)
        {
            if (PlayerPrefs.HasKey("masterBrightness"))
            {
                float localBrightness = PlayerPrefs.GetFloat("masterBrightness");

                _brightnessValueText.text = localBrightness.ToString("0.0");
                _brightnessSlider.value = localBrightness;
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

            if (PlayerPrefs.HasKey("masterQuality"))
            {
                int localQuality = PlayerPrefs.GetInt("masterQuality");

                _qualityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }

            if (PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");

                _volumeTextValue.text = localVolume.ToString();
                _volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }

            if (PlayerPrefs.HasKey("masterSensitivity"))
            {
                float localSensitivity = PlayerPrefs.GetFloat("masterSensitivity");

                _sensitivityValueText.text = localSensitivity.ToString("0.0");
                _sensitivitySlider.value = localSensitivity;
                _menuController.mainSensitivity = Mathf.RoundToInt(localSensitivity);
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
        }
    }
}
