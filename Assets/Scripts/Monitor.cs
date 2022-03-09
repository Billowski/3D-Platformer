using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Monitor : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _textDisplay;
    [SerializeField]
    Image _screen;

    Material _screenMat;

    [SerializeField]
    string[] _sentences;

    [SerializeField]
    float _typingSpeed;
    [SerializeField]
    float _delay = 5.0f;

    [SerializeField]
    bool _onStart;
    bool _isRunning;

    public bool isRunning { get { return _isRunning; } }

    private void Start()
    {
        _screenMat = Instantiate(_screen.GetComponent<Image>().material);
        if (_onStart)
        {
            ScreenOn();
        }
    }

    public void ScreenOn()
    {
        StartCoroutine(Screen());
    }

    IEnumerator Screen()
    {
        _isRunning = true;

        float overscanY = _screenMat.GetFloat("_OverscanY");

        while(overscanY != 0.05f)
        {
            overscanY = Mathf.MoveTowards(overscanY, 0.05f, 1.0f * Time.deltaTime);
            _screenMat.SetFloat("_OverscanY", overscanY);
            _screen.GetComponent<Image>().material = _screenMat;
            yield return null;
        }

        //for (int i = 0; i < _sentences.Length; i++)
        //{
        //    foreach (char letter in _sentences[i].ToCharArray())
        //    {
        //        //if(_textDisplay.text.Length > 0)
        //        //{
        //        //    _textDisplay.text = _textDisplay.text.Substring(0, _textDisplay.text.Length - 1);
        //        //}
        //        //_textDisplay.text += letter + "_";
        //        _textDisplay.text += letter;
        //        yield return new WaitForSeconds(_typingSpeed);
        //    }
        //    yield return new WaitForSeconds(5.0f);
        //    _textDisplay.text = "";
        //}

        for (int i = 0; i < _sentences.Length; i++)
        {
            _textDisplay.text = _sentences[i];
            _textDisplay.ForceMeshUpdate();
            for(int j = 0; j < _textDisplay.text.Length; j++)
            {
                char character = _textDisplay.textInfo.characterInfo[j].character;
                if (character.ToString() != " ")
                {
                    Color32[] vertexColors = _textDisplay.textInfo.meshInfo[0].colors32;
                    int vertexIndex = _textDisplay.textInfo.characterInfo[j].vertexIndex;
                    Color32 color = new Color32(30, 55, 153, 255);
                    vertexColors[vertexIndex + 0] = color;
                    vertexColors[vertexIndex + 1] = color;
                    vertexColors[vertexIndex + 2] = color;
                    vertexColors[vertexIndex + 3] = color;
                    _textDisplay.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                    yield return new WaitForSeconds(_typingSpeed);
                }
            }
            yield return new WaitForSeconds(_delay);
            _textDisplay.text = "";
        }

        while (overscanY != 0.5f)
        {
            overscanY = Mathf.MoveTowards(overscanY, 0.5f, 1.0f * Time.deltaTime);
            _screenMat.SetFloat("_OverscanY", overscanY);
            _screen.GetComponent<Image>().material = _screenMat;
            yield return null;
        }

        _isRunning = false;
    }
}
