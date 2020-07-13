using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//몬스터 유한 상태 머신
public class EnemyFSM1 : MonoBehaviour
{
    //몬스터 상태 이넘문
    enum EnemyState
    {
        Idle, Move, Attack, Return, Damage, Die
    }

    EnemyState state;   //몬스터 상태 변수
    //private GameObject pg;
    //private Transform target;
    //private Transform startPoint;
    //private CharacterController cc;


    /// 유용한 기능

    //#region "Idel 상태에 필요한 변수들"
    //#endregion
    //
    //#region "Move 상태에 필요한 변수들"
    //public float speed = 3.0f;
    //#endregion
    //
    //#region "Attack 상태에 필요한 변수들"
    //public float attackTime = 2f;
    //private float curTime = 0f;
    //#endregion
    //
    //#region "Return 상태에 필요한 변수들"
    //private bool isReturn = false;
    //#endregion
    //
    //#region "Damage 상태에 필요한 변수들"
    //public int hp = 10;
    //public int HP
    //{
    //    get { return hp; }
    //    set
    //    {
    //        hp = value;
    //        posState = state;
    //        state = EnemyState.Damage;
    //
    //        if (hp < 0)
    //        {
    //            hp = 0;
    //            state = EnemyState.Die;
    //        }
    //    }
    //}
    //
    //EnemyState posState;
    //#endregion
    //
    //#region "Die 상태에 필요한 변수들"
    //#endregion

    ///필요한 변수들
    public float findRange = 15f; //플레이어를 찾는 범위
    public float moveRange = 30f; //시작 지점에서 최대 이동 가능한 범위
    public float attackRange = 2f; //공격 가능 범위
    Vector3 startPoint; //몬스터 시작위치
    //Quaternion startRotation; //몬스터 시작 회전값
    Transform player; //플레이어를 찾기위해(안 그럼 모든 몬스터에 다 드래그앤드랍 해줘야 한다. 걍 코드로 찾아서 처리)
    CharacterController cc; //몬스터 이동을 위해 캐릭터 컨트롤러 필요

    //애니메이션을 제어하기 위한 애니메이터 컴포넌트
    Animator anim;

    //네비게이션
    NavMeshAgent nav;

    //몬스터 일반 변수
    int hp = 100; //체력
    int att = 5; //공격력
    float speed = 5.0f; //이동속도

    //공격 딜레이
    float attTime = 2f; //2초에 한번 공격
    float timer = 0f; //타이머

    void Start()
    {
        //내가 한 것
        //
        //startPoint = GameObject.Find("enemySponPoint").transform;
        //pg = GameObject.Find("Player");
        //
        //cc = GetComponent<CharacterController>();

        //몬스터 상태 초기화
        state = EnemyState.Idle;
        startPoint = transform.position;
        //startRotation = transform.rotation;
        //플레이어 트렌스폼 컴포넌트
        player = GameObject.Find("Player").transform;
        //캐릭터 컨트롤러 컴포넌트
        cc = GetComponent<CharacterController>();
        //애니메이터 컴포너너트
        anim = GetComponentInChildren<Animator>();

        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //상태에 따른 행동처리
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
            case EnemyState.Damage:
                Damage();
                break;
            case EnemyState.Die:
                Die();
                break;
        }

    } // end of void Update()

    private void Idle()
    {
        //1. 플레이어와 일정 범위가 되면 이동 상태로 변경 (탐지 범위)
        //- 플레이어 찾기 (GameObject.Find("Player"))
        //- 일정거리 20미터 (거리비교 : Distance, magnitue 사용)
        //- 상태 변경   //state = EnemyState.Move;
        //- 상태 전환 출력

        ///<summary>
        ///내가 한 것
        ///Debug.Log("대기대기");
        ///if(Vector3.Distance(transform.position, pg.transform.position) <= 20)
        ///{
        ///    target = pg.transform;
        ///    Debug.Log("이동으로 전환!");
        ///    state = EnemyState.Move;
        ///}
        ///</summary>

        //Vector3 dir = transform.position - player.position;
        //float distance = dir.magnitude;
        //if(dir.magnitude < findRange)
        //if(distance < findRange)
        //{
        //
        //}

        if (Vector3.Distance(transform.position, player.position) < findRange)
        {
            state = EnemyState.Move;
            print("상태전환 : Idle -> Move");

            //애니메이션
            anim.SetTrigger("Move");
        }
    }

    private void Move()
    {
        //1. 플레이어를 향해 이동 후 공격범위 안에 들어오면 공격상태로 변경.
        //2. 플레이어를 추격하더라도 처음 위치에서 일정 범위를 넘어가면 리턴상태로 변경.
        //- 플레이어처럼 캐릭터컨트롤러를 이용하기.
        //- 공격범위 1미터
        //- 상태 변경
        //- 상태 전환 출력

        ///<summary>
        ///내가 한 것
        ///리턴중이 아닐 때
        ///if (!isReturn)
        ///{
        ///    //공격범위 밖이라면
        ///    if (Vector3.Distance(transform.position, target.position) > 5.0f)
        ///    {
        ///        //바라보는 방향
        ///        //transform.eulerAngles = pg.transform.position - transform.position;
        ///        transform.LookAt(target);
        ///        Debug.Log("플레이어를 향해!");
        ///        //transform.position += transform.forward * speed * Time.deltaTime;
        ///
        ///        Debug.Log("startPoint : " + startPoint.position);
        ///        //만일 따라가다가 탐색 범위를 벗어난다면
        ///        if (Vector3.Distance(transform.position, startPoint.position) >= 30.0f)
        ///        {
        ///            //리턴으로 바꿔준다.
        ///            Debug.Log("리턴으로 전환!");
        ///            state = EnemyState.Return;
        ///        }
        ///    }
        ///    //안으로 들어 왔다면
        ///    else
        ///    {
        ///        Debug.Log("공격으로 전환!");
        ///        state = EnemyState.Attack;
        ///    }
        ///}
        ///리턴 중일 때
        ///else
        ///{
        ///    //바라보는 방향
        ///    transform.LookAt(target);
        ///    Debug.Log("원위치를 향해!");
        ///    //transform.position += transform.forward * speed * Time.deltaTime;
        ///    Debug.Log("startPoint : " + startPoint.position);
        ///    //만일 스타트포인트에 거의 도착 했다면
        ///    if (Vector3.Distance(transform.position, target.position) <= 2.0f)
        ///    {
        ///        //리턴으로 바꿔준다.
        ///        Debug.Log("대기로 전환!");
        ///        isReturn = false;
        ///        state = EnemyState.Idle;
        ///    }
        ///}
        ///</summary>

        //이동 증 이동할 수 있는 최대 점위에 들아왔을 때
        if (Vector3.Distance(transform.position, startPoint) > moveRange)
        {
            state = EnemyState.Return;
            print("상태전환 : Move -> Return");

            //애니메이션
            anim.SetTrigger("Return");
        }
        //리턴상태가 아니면 플레이어를 추격해야 한다.
        else if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            //플레이어를 추격
            //이동방향 (벡터의 뺄셈)
            Vector3 dir = (player.position - transform.position).normalized;
            //dir.Normalize();

            //몬스터가 벡스텝으로 쫓아온다.
            //몬스터가 타겟을 바라보도록 하자
            //방법1
            //transform.forward = dir;
            //방법2
            ///transform.LookAt(player);
            ///
            nav.SetDestination(player.position);

            //좀더 자연스럽게 회전처리를 하고싶다.
            //여기서도 문제가 

            //transform.forward = Vector3.Lerp(transform.forward, dir, 10 * Time.deltaTime);

            //최종적으로 자연스럽게 회전처리를 하려면  결국 쿼터니온을 사용해야 한다.
            ///transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);

            //캐릭터 컨트롤러를 이용해서 이동하기
            //cc.Move(dir * speed * Time.deltaTime);
            //중력이 적용안되는 문제가 있다.

            //중력문제를 해결하기 위해서 심플무브를 사용한다.
            //심플무브는 최소한의 물리가 적용되어 중력문제를 해결할 수 있다.
            //단 내부적으로 시간처리를 하기 때문에 Time.deltaTime을 사용하지 않는다.
            ///cc.SimpleMove(dir * speed);
        }
        else //공격범위 안에 들어옴
        {
            state = EnemyState.Attack;
            print("상태전환 : Move -> Attack");
            anim.SetTrigger("Attack");
        }
    }

    private void Attack()
    {
        //1. 플레이어가 공격범위 안에 있다면 일정한 시간 간격으로 플레이어 공격
        //2. 플레이어가 공격범위를 벗어나면 이동상태(재추격)로 변경
        //- 공격범위 1미터
        //- 상태 변경
        //- 상태 전환 출력

        ///<sumery>
        ///내가 한 것
        ///먼저 플레이어와의 거리 확인
        ///if (Vector3.Distance(transform.position, target.position) > 5.0f)
        ///{
        ///    state = EnemyState.Move;
        ///}
        ///
        ///curTime += Time.deltaTime;
        ///
        ///if(curTime > attackTime)
        ///{
        ///    Debug.Log("공겨어어어어억!!!");
        ///    curTime = 0f;
        ///}
        ///</sumery>

        //공격범위 안에 들어옴
        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            //일정 시간마다 플레이어를 공격하기
            timer += Time.deltaTime;
            if (timer > attTime)
            {
                print("공격");
                //플레이어의 필요한 스크립트를 가져와서 데미지를 주면 된다.
                //player.GetComponent<PlayerMove>().hitDamage(att);
                anim.SetTrigger("Attack");
                //타이머 초기화
                timer = 0f;
            }
        }
        else //현재 상태를 무브로 전환하기
        {
            state = EnemyState.Move;
            print("상태전화 : Attack -> Move");
            anim.SetTrigger("Move");
            //타이머 초기화
            timer = 0f;
        }

    }

    private void Return()
    {
        //1. 몬스터가 플레이어를 추격하더라도 처음 위치에서 일정 범위를 벗어나면 다시 돌아옴.
        //- 처음위치에서 일정범위 30미터
        //- 상태 변경
        //- 상태 전환 출력

        ///<summary>
        ///내가 한 것
        ///target = startPoint;
        ///isReturn = true;
        ///Debug.Log("돌아가아아아아악!");
        ///state = EnemyState.Move;
        ///</summary>


        //시작위치까지 도달하지 않을 때는 이동
        //도착하면 대기 상태로 벼녕
        if (Vector3.Distance(transform.position, startPoint) > 0.1f)
        {
            Vector3 dir = (startPoint - transform.position).normalized;
            ///transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);

            nav.SetDestination(startPoint);
            //cc.SimpleMove(dir * speed);
        }
        else
        {
            //위치값을 초기값으로
            ///transform.position = startPoint;
            ///transform.rotation = Quaternion.identity;
            
            //Quaternion.identity; => 쿼터니온 값을 0으로 바꿔준다.

            state = EnemyState.Idle;
            print("상태전화 : Return -> Idle");

            //애니메이션
            anim.SetTrigger("Idle");
        }
    }

    //플레이어 쪽에서 충돌감지를 할 수 있으니 이함수는 퍼블릭으로 만들자
    public void hitDamage(int value)
    {
        //예외처리
        //피격상태이거나, 죽은 상태일때는 데미지를 충첩으로 주지 않는다.
        if (state == EnemyState.Damage || state == EnemyState.Die) return;

        //체력 깎기
        hp -= value;

        //몬스터의 체력이 1이상이면 피격상태
        if (hp > 0)
        {
            state = EnemyState.Damage;
            print("상태전화 : AnyState -> Damage");
            anim.SetTrigger("Damaged");
        }
        else
        {
            state = EnemyState.Die;
            print("상태전화 : AnyState -> Die");
            anim.SetTrigger("Die");
        }
    }

    //피격상태(AnyState)
    private void Damage()
    {
        //코루틴을 사용하자
        //1. 몬스터 체력이 1이상
        //2. 다시 상태를 이전 상태로 변셩
        //- 상태 변경
        //- 상태 전환 출력

        ///<summary>
        ///내가 한 것
        ///StartCoroutine(DamageCoroutine());
        ///</summary>

        //피격상태를 처리하기 휘란 코루틴을 실행한다.
        StartCoroutine(DamageProc());

    }

    //피격상태 처리용 코루틴
    IEnumerator DamageProc()
    {
        //피격모션 시간만큼 기다리기
        yield return new WaitForSeconds(1.0f);

        print("맞았다!!");

        state = EnemyState.Move;
        print("상태전화 : Damage -> Move");
        anim.SetTrigger("Move");

    }

    //피격상태(AnyState)
    private void Die()
    {
        //코루틴을 사용하자
        //1. 체력이 0이하
        //2. 몬스터 오브젝트 삭제
        //- 상태 변경
        //- 상태 전환 출력 (죽었다)

        ///<summary>
        ///내가 한 것
        ///StartCoroutine(DieCoroutine());
        ///</summary>

        //진행중인 모든 코루틴은 정지한다.
        StopAllCoroutines();

        //죽음상태를 처리하기 위한 코루틴 실행
        StartCoroutine(DieProc());
    }

    IEnumerator DieProc()
    {
        //캐릭터컨트롤러 비활성화
        cc.enabled = false;

        //2초후에 자기자신을 제거한다.
        yield return new WaitForSeconds(2.0f);
        print("죽었다!!!");
        Destroy(gameObject);
    }

    ///<summary>
    ///IEnumerator DamageCoroutine()
    ///{
    ///    Debug.Log("아픔");
    ///    state = posState;
    ///    yield return 0;
    ///}
    ///
    ///IEnumerator DieCoroutine()
    ///{
    ///    Debug.Log("주금");
    ///    yield return 0;
    ///}
    ///</summary>

    private void OnDrawGizmos()
    {
        //공격가능 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        //플레이어 찾을 수 있는 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, findRange);
        //이동 가능한 최대 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPoint, moveRange);
    }
}
