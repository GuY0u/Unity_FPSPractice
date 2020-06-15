using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float speed = 5.0f;
    CharacterController cCon;

    // Start is called before the first frame update
    void Start()
    {
        //캐릭터컨트롤러 컴포넌트 가져오기
        cCon = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //Vector3 Dir = (Vector3.forward * v) + (Vector3.right * h);
        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();


        //transform.Translate(dir * speed * Time.deltaTime);
        //카메라가 보는 방향으로 이동해야 한다
        dir = Camera.main.transform.TransformDirection(dir);
        //transform.Translate(dir * speed * Time.deltaTime);

        //심각한 문제 : 하늘 날라다님, 땅 뚫음, 충돌처리 안됨
        //캐럭터 컨트롤러 컴포넌트를 사용한다
        //캐릭터컨트롤러는 충돌감지만 하고 물리가 적용안된다
        //따라서 충돌감지를 하기 위해서는 반드시
        //캐릭터컨트롤러 컴포넌트가 제공해주는 함수로 이동처리해야 한다.
        cCon.Move(dir * speed * Time.deltaTime);
    }
}
