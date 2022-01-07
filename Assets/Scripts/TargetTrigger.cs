using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    [SerializeField]
    Renderer _renderer;

    [SerializeField]
    GameObject _object;

    bool _click = true;

    private void OnTriggerEnter(Collider other)
    {
        _renderer.material.color = _click ? Color.green : Color.red;
        _click = !_click;

        _object.GetComponent<ObjectMovement>().OperateObject();
    }
}
