using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    Rigidbody _bulletRigidbody;

    [SerializeField]
    float _speed = 20.0f;

    private void Awake()
    {
        _bulletRigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        _bulletRigidbody.velocity = transform.forward * _speed;
        Destroy(gameObject, 5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 1) Destroy(gameObject);
    }
}