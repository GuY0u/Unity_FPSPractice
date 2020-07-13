using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    //카메라를 마우스 움직이는 방향으로 회전하기
    public float speed = 150f; //회전속도(Time.DeltaTime을 통해 1초에 150도 회전)

    //회전각도 직접 제어
    float angleX;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        float h = Input.GetAxis("Mouse X");
        angleX += h * speed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, angleX, 0);

    }
}
