using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject _object;
    Transform _target;
    Renderer _renderer;

    [SerializeField]
    bool _onStart;
    bool _click = false;

    private void Start()
    {
        _target = transform.GetChild(0);
        _renderer = _target.GetComponent<Renderer>();

        if (_onStart)
        {
            _click = !_click;
            _renderer.material.color = Color.green;
            if(_object != null) _object.GetComponent<ObjectMovement>().OperateObject();
        }
        else
        {
            _renderer.material.color = Color.red;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            _click = !_click;
            _renderer.material.color = _click ? Color.green : Color.red;

            if (_object != null) _object.GetComponent<ObjectMovement>().OperateObject();
        }
    }
}
