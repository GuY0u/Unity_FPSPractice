using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    //중력적용
    public float gravity = -5;
    float velocityY;    //낙하속도(벨로시티는 방향과 힘을 들고 있다.)
    CharacterController cCon;

    //몬스터 상태 이넘문
    enum EnemyState
    {
        Idle, MoveWP, Tracking, Attack, Return, Damaged, Die
    }

    EnemyState state;   //몬스터 상태변수

    public int hp = 50;     //적 체력

    float speed = 2.0f;      //적 이동속도
    float timer = 0.0f;     //일정행동에 사용할 타이머

    public GameObject target;   //목표 타겟

    /// <summary>
    /// 유용한 기능
    /// </summary>

    #region "Idle 상태에 필요한 변수들"
    float idleDurTime = 3.0f;
    #endregion

    #region "IdleMove 상태에 필요한 변수들"

    #endregion

    #region "Tracking 상태에 필요한 변수들"
    float trackingTime = 0.0f;
    #endregion

    #region "Attack 상태에 필요한 변수들"
    #endregion

    #region "Return 상태에 필요한 변수들"
    public Vector3 startPos;    //다시 돌아갈 위치
    #endregion

    #region "Damaged 상태에 필요한 변수들"
    float dmgTime = 0.0f;   //맞은 후 무적시간 타이머
    #endregion

    #region "Die 상태에 필요한 변수들"
    #endregion

    void Start()
    {
        //캐릭터컨트롤러 컴포넌트 가져오기
        cCon = GetComponent<CharacterController>();

        startPos = this.transform.position;

        //몬스터 상태 초기화
        state = EnemyState.Idle;
    }

    void Update()
    {
        velocityY += gravity * Time.deltaTime;

        if (cCon.collisionFlags == CollisionFlags.Above) //땅에 닿았는가?
        {
            Debug.Log("닿앗다");
            velocityY = 0;
        }
        else
        {
            Debug.Log("안닿앗다");
            velocityY += gravity * Time.deltaTime;
        }

        //상태에 따른 행동변화
        switch (state)
        {
            case EnemyState.Idle:
                Idle();
                break;

            case EnemyState.Tracking:
                Tracking();
                break;

            case EnemyState.Attack:
                Attack();
                break;

            case EnemyState.Return:
                Return();
                break;

            //case EnemyState.Damaged:
            //    Damaged();
            //    break;

            //case EnemyState.Die:
            //    Die();
            //    break;
        }
    }//end of void Update()

    private void Idle()
    {
        //1.플레이어와 일정범위가 되면 이동상태로 변경(탐지)
        //- 플레이어 찾기(GameObject.Find("Player"))
        //- 일정거리 20미터(거리비교 :Distance, Magnitude 등등)
        //- 상태변경 -> state = EnemyState.Move
        //- 상태전환 출력

        Debug.Log("대기중");

        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.transform.gameObject.tag == "Player")
            {
                state = EnemyState.Tracking;
            }
        }
    }

    private void MoveWP()
    {

    }

    private void Tracking()
    {
        //1. 플레이어를 향해 이동 후 공격범위 안에 들어오면 공격상태로 변경
        //2. 플레이러를 추격하더라도 처음위치에서 일정범위를 넘어가면 리턴상태로 변경
        //- 플레이어처럼 캐릭터컨트롤러를 이용하기
        //- 공격범위 1미터
        //- 상태변경
        //- 상태전환 출력
        //주기적으로 적과 플레이어의 위치 갱신
        Debug.Log("적 추적중");

        Vector3 dir = new Vector3
            ((transform.position.x - target.transform.position.x),
            0,
            (transform.position.z - target.transform.position.z));
        dir.Normalize();

        if (Vector3.Distance(transform.position, target.transform.position) < 4.0f)
        {
            state = EnemyState.Attack;
        }
        else transform.Translate(dir * speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.transform.position) > 15.0f)
        {
            trackingTime += Time.deltaTime;
            if(trackingTime >5.0f)
            {
                state = EnemyState.Return;
                trackingTime = 0.0f;
            }
        }
        else
        {
            trackingTime = 0.0f;
        }
    }



    private void Attack()
    {
        //1. 플레이어가 공격범위 안에 있다면 일정한 시간 간격으로 플레이어 공격
        //2. 플레이어가 공격범위를 벗어나면 이동상태(재추격)
        //- 공격범위 1미터
        //- 상태변경
        //- 상태전환 출력

        Debug.Log("적 공격중");

        if (Vector3.Distance(transform.position, target.transform.position) < 4.0f)
        {
            state = EnemyState.Attack;
        }
        else state = EnemyState.Tracking;

    }

    //복귀상태
    private void Return()
    {
        //몬스터가 플레이어를 추격하더라도 처음위치에서 일정범위를 벗어나면 다시 돌아옴
        //- 처음위치에서 일정범위 30미터
        //- 상태변경
        //- 상태전환 출력

        Debug.Log("제자리로 돌아가는중");
        Vector3 dir = new Vector3
            ((transform.position.x - startPos.x),
            0,
            (transform.position.z - startPos.z));
        dir.Normalize();

        //다시 거리가 가까워지면 추적
        if (Vector3.Distance(transform.position, target.transform.position) < 10.0f)
        {
            state = EnemyState.Tracking;
        }
        else transform.Translate(dir * speed * Time.deltaTime);

        //제자리도착시 idle
        if(transform.position == startPos)
        {
            state = EnemyState.Idle;
        }



    }

    //피격상태 (Any State)
    public void Damaged()
    {
        //코루틴을 사용하자
        //1. 몬스터 체력이 1이상
        //2. 다시 이전 상태를 이동을 변경
        //- 상태변경
        //- 상태전환 출력

        Debug.Log("공격받는 중");
        StartCoroutine("LoseHp");

    }

    public IEnumerator LoseHp()
    {
        hp -= 5;
        yield return new WaitForSeconds(5.0f);

    }

    //죽음상태 (Any State)
    private void Die()
    {
        //코루틴을 사용하자
        //1. 체력이 0이하
        //2. 몬스터 오브젝트 삭제
        //- 상태변경
        //- 상태전환 출력 (죽었다)

        Debug.Log("사 망");
        StartCoroutine("Dead");
    }

    public IEnumerator Dead()
    {
        Debug.Log("적사망하는 모션");
        yield return new WaitForSeconds(2.0f);
        Debug.Log("적 사라짐");
        Destroy(this.transform.gameObject);
    }
}
