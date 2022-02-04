using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject _object;

    Vector3 _offset = new Vector3(0, 0.1f, 0);
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _object.GetComponent<ObjectMovement>().OperateObject();
            transform.position = transform.position - _offset;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _object.GetComponent<ObjectMovement>().OperateObject();
            transform.position = transform.position + _offset;
        }
    }
}
