using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateTrigger : MonoBehaviour
{
    Transform _plate;
    [SerializeField]
    GameObject _object;
    Vector3 startPos;
    Vector3 _offset = new Vector3(0, 0.1f, 0);

    int _triggerCount = 0;

    private void Start()
    {
        _plate = transform.GetChild(0);
        startPos = _plate.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Box"))
        {
            _triggerCount++;
            if (_triggerCount == 1)
            {
                if(_object != null) _object.GetComponent<ObjectMovement>().OperateObject();
                if (_plate.transform.position == startPos) _plate.transform.position = _plate.transform.position - _offset;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Box"))
        {
            _triggerCount--;
            if (_triggerCount == 0)
            {
                if (_object != null) _object.GetComponent<ObjectMovement>().OperateObject();
                if (_plate.transform.position == startPos - _offset) _plate.transform.position = _plate.transform.position + _offset;
                _triggerCount = 0;
            }
        }
    }
}
