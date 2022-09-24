using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _rightAngleLimit;
    [SerializeField] private float _leftAngleLimit;
    [SerializeField] private float _minVelocity;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.angularVelocity = _minVelocity;
    }

    void FixedUpdate()
    {
        PendulumMovement();
    }

    void Update()
    {

    }

    public void PendulumMovement()
    {
        if(transform.rotation.z > 0
            && transform.rotation.z < _rightAngleLimit
            && (_rb.angularVelocity > 0)
            && _rb.angularVelocity < _minVelocity) {
            _rb.angularVelocity = _minVelocity;
        } else if(transform.rotation.z < 0
            && transform.rotation.z > _leftAngleLimit
            && (_rb.angularVelocity < 0)
            && _rb.angularVelocity > _minVelocity * -1) {
            _rb.angularVelocity = _minVelocity * -1;
        }
    }
}