using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject _door;
    Animator _animator;

    int _IDDoor;

    bool _doorState = false;

    private void Awake()
    {
        _animator = _door.GetComponent<Animator>();
        _IDDoor = Animator.StringToHash("DoorState");
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _doorState = !_doorState;
            _animator.SetBool(_IDDoor, _doorState);
        }
    }
}
