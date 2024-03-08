using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D Rb2D;
    [HideInInspector]public Animator _animator;
    [HideInInspector]public PhysicsCheck _physicsCheck;

    [Header("基础参数")] 
    public float NormalSpeed;
    public float RushSpeed;
    public float CurrentSpeed;
    /// <summary>
    /// 面朝方向
    /// </summary>
    public Vector3 FaceDir;
    public float HurtForce;
    [Header("计时器")] 
    public float WaitTime;
    public float WaitCount;
    public float LostPlayerTime;
    public float LostPlayerCount;
    
    [Header("状态")]
    public bool ifWait;
    public bool ifHurt;
    public bool ifDead;
    
    [Header("检测")] 
    public Vector2 CenterOffset;
    public Vector2 CheckSize;
    public float CheckDistance;
    public LayerMask AttackLayer;
        
    private Transform Attacker;

    public BaseState Currentstate;
    protected BaseState Patrolstate;
    protected BaseState RushState;
    public Collider2D Target;
    
    protected virtual void Awake()
    {
        Rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _physicsCheck = GetComponent<PhysicsCheck>();
    }

    private void OnEnable()
    {
        Currentstate = Patrolstate;
        Currentstate.OnEnterState(this);
    }

    protected virtual void Start()
    {
        LostPlayerCount = LostPlayerTime;
        WaitCount = WaitTime;
    }

    protected virtual void Update()
    {
        FaceDir = new Vector3(-transform.localScale.x, 0, 0);
        Currentstate.LogicUpdate();
        TimeCounter();
        
    }

    protected virtual void FixedUpdate()
    {

        if (!ifHurt&&!ifDead&&!ifWait)
        {
            Move();
        }
        Currentstate.PhysicsUpdate();
    }

    private void OnDisable()
    {
        Currentstate.OnExitState();
    }

   

    

    #region 连续执行方法
    
    protected virtual void Move()
    {
        Rb2D.velocity = new Vector2(FaceDir.x * CurrentSpeed*Time.deltaTime, Rb2D.velocity.y);
    }

    protected virtual void TimeCounter()
    {
        if (ifWait)
        {
            WaitCount -= Time.deltaTime;
            if (WaitCount<=0)
            {
                ifWait = false;
                WaitCount = WaitTime;
                transform.localScale = new Vector3(FaceDir.x, 1, 1);
                _animator.SetBool("Walk",true);
            }
        }

        if (!LookingForPlayerBox()&& LostPlayerCount>0)
        {
            LostPlayerCount -= Time.deltaTime;
        }
        else if (LookingForPlayerBox())
        {
            LostPlayerCount = LostPlayerTime;
        }
        
       
    }

    public bool LookingForPlayerBox()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)CenterOffset,
            CheckSize, 0, FaceDir, CheckDistance,
            AttackLayer);
    }

    public bool LookingForPlayerCircle()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x,transform.position.y+2), CheckDistance,AttackLayer);
        if (colliders.Length!=0)
        {
            return true;
        }
        return false;
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
                
        Gizmos.color=Color.yellow;
        Gizmos.DrawWireCube(transform.position + (Vector3)CenterOffset,CheckSize);
        Gizmos.DrawWireSphere(transform.position, CheckDistance);
    }

    #region 事件执行方法

    public void SwitchState(EnemyState state)
    {
        var newState = state switch
        {
            EnemyState.Patrol => Patrolstate,
            EnemyState.Chase => RushState,
            _ => null
        };
        Currentstate.OnExitState();
        Currentstate = newState;
        Currentstate.OnEnterState(this);
    }

    public void TakeDamage(Transform attacker)
    {
        Attacker = attacker;
        if (attacker.position.x-transform.position.x>0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (attacker.position.x-transform.position.x<0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        ifHurt = true;
        _animator.SetTrigger("Hurt");
        Vector2 dir = new Vector2(transform.position.x-attacker.position.x , 0).normalized;
        Rb2D.velocity = new Vector2(0, Rb2D.velocity.y);
        StartCoroutine(OnHurt(dir));
    }

    private IEnumerator OnHurt(Vector2 dir)
    {
        
        Rb2D.AddForce(dir*HurtForce,ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.45f);
        ifHurt = false;
    }

    public void OnDead()
    {
        _animator.SetBool("Dead",true);
        ifDead = true;
    }

    public void DestoryAfterAnimation()
    {
        Destroy(this.gameObject);
    }

    #endregion


   
}
