using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public Transform m_anchor; //圓點
    public float g = 9.8f; //重力加速度
    private Vector3 m_rotateAxis; //旋轉軸
    private float w = 0; //角速度

    void Start()
    {
        m_rotateAxis = Vector3.Cross(transform.position - m_anchor.position, Vector3.down);
    }

    void DoPhysics()
    {
        float r = Vector3.Distance(m_anchor.position, transform.position);
        float l = Vector3.Distance(new Vector3(m_anchor.position.x, transform.position.y, m_anchor.position.z), transform.position);
        // 當鐘擺擺動到另外一側時, l 為負, 則角速度alpha為負
        Vector3 axis = Vector3.Cross(transform.position - m_anchor.position, Vector3.down);
        if(Vector3.Dot(axis, m_rotateAxis) < 0) {
            l = -l;
        }
        float cosalpha = l / r;
        // 求角加速度
        float alpha = (cosalpha * g) / r;
        // 累計角速度
        w += alpha * Time.deltaTime;
        // 求角位移 (乘以180 / PI 是為了將弧度轉為角度)
        float thelta = w * Time.deltaTime * 180.0f / Mathf.PI/2 ;
        // 繞圓點m_anchor的旋轉軸m_rotateAxis旋轉thelta角度
        transform.RotateAround(m_anchor.position, m_rotateAxis, thelta);
    }

    void Update()
    {
        DoPhysics();
    }
}
