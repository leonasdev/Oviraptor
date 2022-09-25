using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseMotor : MonoBehaviour
{
    [SerializeField] private HingeJoint2D _joint;
    [SerializeField] private float _swingSpeed;
    private bool flag;
    private int _factor = 1;
    private JointMotor2D motor;

    public GameObject player;
    [SerializeField] private float _velocityThreshold;
    [SerializeField] private float _rightPushForce;
    [SerializeField] private float _leftPushForce;
    [SerializeField] private float _initPushForce;

    void Start()
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

        _initPushForce = playerRb.velocity.x > 0 ? Mathf.Abs(_initPushForce) : Mathf.Abs(_initPushForce)*-1;
        print(_initPushForce);

        playerRb.rotation = 90;
        playerRb.velocity = new Vector2(0, 0);
        playerRb.angularVelocity = 0;
        _joint.connectedBody = player.GetComponent<Rigidbody2D>();
        _joint.connectedBody.AddForce(new Vector2(_initPushForce, 0));
        print("start");
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate()
    {
        // if(_joint.limitState == JointLimitState2D.UpperLimit && flag) {
            // _factor = -1;
            // flag = false;
            // motor.motorSpeed = 0;
        // } else if(_joint.limitState == JointLimitState2D.LowerLimit && !flag) {
            // _factor = 1;
            // flag = true;
            // motor.motorSpeed = 0;
        // }
// 
        // motor.motorSpeed = _swingSpeed * _factor;
        // motor.motorSpeed = Mathf.Lerp(motor.motorSpeed, _swingSpeed * _factor, Time.deltaTime);
        // _joint.motor = motor;

        print($"jointAngle: {_joint.jointAngle}, jointLimitMax: {_joint.limits.max}, rbV: {_joint.connectedBody.angularVelocity}");

        // if(_joint.limitState == JointLimitState2D.UpperLimit && !flag) {
        //     _joint.connectedBody.AddForce(new Vector2(-500, -800));            flag = true;
        //     flag = true;
        //     print("push left");
        // } else if(_joint.limitState == JointLimitState2D.LowerLimit && flag) {
        //     _joint.connectedBody.AddForce(new Vector2(500, -800));
        //     flag = false;
        //     print("push right");
        // }

        if(_joint.connectedBody.angularVelocity > _velocityThreshold && !flag) {
            _joint.connectedBody.AddForce(new Vector2(_rightPushForce, 0));
            flag = true;
        } else if(_joint.connectedBody.angularVelocity < _velocityThreshold * -1 && flag) {
            _joint.connectedBody.AddForce(new Vector2(_leftPushForce, 0));
            flag = false;
        }
    }
}
