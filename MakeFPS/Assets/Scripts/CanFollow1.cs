using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanFollow1 : MonoBehaviour
{
    //카메라가 플레이어를 따라다니기
    //플레이어한테 바로 카메라를 붙여서 이동해도 된다
    //하지만 게임에 따라서 드라마틱한 연출이 필요한 경우에
    //타겟을 따라다니도록 하는게 1인칭에서 3인칭으로 또는 그 반대로 변경이 쉬워진다
    //또한 순간이동이 아닌 슈팅게임에서 꼬랑지가 따라다니는것같은 효과도 연출이 가능하다
    //지금은 우리 눈 역할만 하기 때문에 그냥 순간이동 시킨다

    //카메라가 따라다닐 타겟(플레이어)
    public Transform target1st;
    public Transform target3rd;
    public float followSpeed = 10.0f;

    bool isFPS = false;

    Vector3 temp; 

    // Start is called before the first frame update
    void Start()
    {
       temp = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeView();
    }

    private void ChangeView()
    {
        if(Input.GetKeyDown("1"))
        {
            isFPS = true;
        }
        if(Input.GetKeyDown("2"))
        {
            isFPS = false;
        }

        if(isFPS)
        {
            transform.position = target1st.position;
        }
        else
        {
            transform.position = target3rd.position;
        }
    }
}
