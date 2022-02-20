using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField]
    GameObject _loaderCanvas;
    [SerializeField]
    Slider _progressBar;

    float _progress;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //public async void LoadScene(string sceneName)
    //{
    //    _progress = 0;
    //    _progressBar.value = 0;

    //    AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
    //    scene.allowSceneActivation = false;

    //    _loaderCanvas.SetActive(true);

    //    do
    //    {
    //        _progress = Mathf.Clamp01(scene.progress / 0.9f);
    //    } while (scene.progress < 0.9f);

    //    scene.allowSceneActivation = true;
    //    _loaderCanvas.SetActive(false);
    //}

    public void LoadScene(string sceneName)
    {
        StartCoroutine(Loading(sceneName));
    }

    IEnumerator Loading(string sceneName)
    {
        _progress = 0;
        _progressBar.value = 0;

        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        _loaderCanvas.SetActive(true);

        while (!scene.isDone)
        {
            _progress = Mathf.Clamp01(scene.progress / 0.9f);
            _progressBar.value = Mathf.MoveTowards(_progressBar.value, _progress, 3 * Time.deltaTime);
            if (_progressBar.value == 1.0f)
            {
                scene.allowSceneActivation = true;
            }

            yield return null;
        }

        _loaderCanvas.SetActive(false);
    }
}
