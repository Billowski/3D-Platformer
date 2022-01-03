using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Rigidbody _bulletRigidbody;
    [SerializeField]
    private float _speed = 20.0f;

    private void Awake()
    {
        _bulletRigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        _bulletRigidbody.velocity = transform.forward * _speed;
        Destroy(gameObject, 5);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
