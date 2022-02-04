using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    [SerializeField]
    Renderer _renderer;

    [SerializeField]
    GameObject _object;

    [SerializeField]
    bool _onStart;
    bool _click = false;

    private void Start()
    {
        if (_onStart)
        {
            _click = !_click;
            _renderer.material.color = Color.green;
            if(_object != null)
            {
                _object.GetComponent<ObjectMovement>().OperateObject();
            }
        }
        else
        {
            _renderer.material.color = Color.red;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _click = !_click;
        _renderer.material.color = _click ? Color.green : Color.red;
        
        _object.GetComponent<ObjectMovement>().OperateObject();
    }
}
