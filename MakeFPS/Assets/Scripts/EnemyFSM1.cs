using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM1 : MonoBehaviour
{
    //중력적용
    public float gravity = -5;
    float velocityY;    //낙하속도(벨로시티는 방향과 힘을 들고 있다.)
    CharacterController cCon;

    //몬스터 상태 이넘문
    enum EnemyState
    {
        Idle, MoveWP, Move, Attack, Return, Damaged, Die
    }

    EnemyState state;   //몬스터 상태변수



    /// <summary>
    /// 유용한 기능
    /// </summary>

    #region "Idle 상태에 필요한 변수들"

    #endregion

    #region "IdleMove 상태에 필요한 변수들"

    #endregion

    #region "Move 상태에 필요한 변수들"

    #endregion

    #region "Attack 상태에 필요한 변수들"

    #endregion

    #region "Return 상태에 필요한 변수들"

    #endregion

    #region "Damaged 상태에 필요한 변수들"

    #endregion

    #region "Die 상태에 필요한 변수들"
    #endregion

    ///필요한 변수들
    public float findRange = 15f;   //플레이어를 찾는 범위
    public float moveRange = 30f;  //시작지점에서 최대 이동가능한 범위
    public float attackRange = 2f; //공격가능범위
    Vector3 startpoint;           //몬스터 시작위치
    Transform player;             //플레이어를 찾기위해
    CharacterController cc;       //몬스터 이동을 위해 캐릭터 컨트롤러

    ///몬스터 일반 변수
    int hp = 100;      //몬스터 체력
    int att = 5;       //몬스터 공격력
    float speed = 5.0f; //몬스터 이동속도

    float attTime = 2f; //2초에 한번 공격
    float timer = 0f;   //타이머

    void Start()
    {


        //몬스터 상태 초기화
        state = EnemyState.Idle;
        //시작지점 저장
        startpoint = transform.position;
        //플레이어 트랜스폼 컴포넌트
        player = GameObject.Find("Player").transform;

        //캐릭터컨트롤러 컴포넌트 가져오기
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {

        //상태에 따른 행동변화
        switch (state)
        {
            case EnemyState.Idle:
                Idle();
                break;

            case EnemyState.Move:
                Move();
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

        //Vector3 dir = transform.position - player.position;
        //float distance = dir.magnitude;

        if (Vector3.Distance(transform.position, player.position) < findRange)
        {
            state = EnemyState.Move;
            print("상태전환 : Idle -> Move");
        }


    }

    private void MoveWP()
    {

    }

    private void Move()
    {
        //1. 플레이어를 향해 이동 후 공격범위 안에 들어오면 공격상태로 변경
        //2. 플레이러를 추격하더라도 처음위치에서 일정범위를 넘어가면 리턴상태로 변경
        //- 플레이어처럼 캐릭터컨트롤러를 이용하기
        //- 공격범위 1미터
        //- 상태변경
        //- 상태전환 출력
        //주기적으로 적과 플레이어의 위치 갱신
        Debug.Log("적 추적중");

        //이동중 이동할 수 있는 최대범위에 들어왔을떼
        if (Vector3.Distance(transform.position, startpoint) > moveRange)
        {
            state = EnemyState.Return;
            print("상태전환 : Move->Return");
        }
        //리턴상태가 아니면 플레이어를 추격해야 한다
        else if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            //플레이어를 추격
            Vector3 dir = (player.position - transform.position).normalized;
            //dir.Normalize();

            //몬스터가 백스텝으로 쫓아온다
            //몬스터가 타겟을 바라보도록 하자
            ////방법2 : LookAt함수 사용
            //transform.LookAt(player);
            ////방법1 : 앞의 기준을 dir로 해준다
            //transform.forward = dir;

            ////좀더 자연스럽게 회전처리를 하고 싶다
            //transform.forward = Vector3.Lerp(transform.forward, dir, 10 * Time.deltaTime);
            //여기도 문제가 있다 지금 회전 처리를 하면서 벡터의 러프를 사용하
            //타겟과 본인이 일직선상일경우 백덤블링으로 회전한다.

            //최종적으로 자연스러운 회전처리를 하려면 결국 쿼터니언을 사용해야한다.
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 5 * Time.deltaTime);

            ////캐릭터 컨트롤러를 이용해서이동하기
            //cc.Move(dir * speed * Time.deltaTime);
            //중력이 적용안되는 문제가 있다

            //중력문제를 해결하기 위해 심플무브를 사용한다.
            //심플무브는 최소한의 물리가 적용되어 중력문제를 해결할 수 있다.
            //단 내부적으로 시간처리를 하기때문에 Time.deltaTime을 사용하지 않는다.
            cc.SimpleMove(dir * speed);
        }
        else  //공격범위 안에 들어옴
        {
            state = EnemyState.Attack;
            print("상태전환 : Move->Attack");
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

        //공격범위안에 들어옴
        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            //일정 시간마다 플레이어를 공격하기
            timer += Time.deltaTime;
            if (timer > attTime)
            {
                print("공격");
                //플레이어의 필요한 스크립트 컴포넌트를 가져와서 데미지를 주면
                //player.GetComponent<PlayerMove>().hitDamage(att);

                //타이머 초기화
                timer = 0f;
            }
            else
            {
                state = EnemyState.Move;
                print("상태전환 : Attack -> Move");
                timer = 0f;
            }
        }


    }

    //복귀상태
    private void Return()
    {
        //몬스터가 플레이어를 추격하더라도 처음위치에서 일정범위를 벗어나면 다시 돌아옴
        //- 처음위치에서 일정범위 30미터
        //- 상태변경
        //- 상태전환 출력

        Debug.Log("제자리로 돌아가는중");

        //시작위치까지 도달하지 않을때는 이동
        //도착하면 대기상태로 변경
        if (Vector3.Distance(transform.position, startpoint) > 0.1f)
        {
            Vector3 dir = (startpoint - transform.position).normalized;
            cc.SimpleMove(dir * speed);
        }
        else
        {
            //위치값을 초기값으로
            transform.position = startpoint;
            state = EnemyState.Idle;
            print("상태 변환 : Return -> Idle");
        }

    }

    //플레이어쪽에서 충돌감지를 할 수 있으니 이ㅣ함수는 퍼블릭으로 만든다
    public void hitDamage(int value)
    {
        //예외처리
        //피격상태이거나, 죽은 상태일때는 데미지 충접으로 주지 않는다
        if (state == EnemyState.Damaged || state == EnemyState.Die)

            //체력 깎기
            hp -= value;
        //몬스터의 체력이 1이상이면 피격상태
        if(hp>0)
        {
            state = EnemyState.Damaged;
            print("상태전환 : AnyState -> Damaged");
            print("Hp : " + hp);
            Damaged();
        }
        else
        {
            state = EnemyState.Die;
            print("상대 전환 : AnyState -> Die");

            Die();
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

        StartCoroutine(DamageProc());


    }

    //피격상태 처리용 코루틴
    IEnumerator DamageProc()
    {
        //피격모션 시간만큼 기다리기
        yield return new WaitForSeconds(1.0f);
        //현재상태를 이동으로 전환
        state = EnemyState.Move;
        print("상태전환 : Damaged -> Move");
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

        //진행중인 모든 코루틴은 정지한다.
        StopAllCoroutines();

        //죽음상태를 처리하기 위해 코루틴 실행
        StartCoroutine(DieProc());
    }

    IEnumerator DieProc()
    {
        //캐릭터컨트롤러 비활성화
        cc.enabled = false;

        //2초후에 자기자신을 제거한다
        yield return new WaitForSeconds(2.0f);
        print("게임오브젝트 삭제");
        Destroy(gameObject);
    }


    private void OnDrawGizmos()
    {
        //공격가능범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        //플레이어 탐지 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, findRange);
        //이동 가능한 최대 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startpoint, moveRange);

    }
}