using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [SerializeField]
    Vector3 _rotation;
    void Update()
    {
        transform.Rotate(_rotation * Time.deltaTime);
    }
}
