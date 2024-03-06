using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D Rb2D;
    protected Animator _animator;
    private PhysicsCheck _physicsCheck;

    [Header("基础参数")] 
    public int MaxHealth;
    public int CurrentHealth;
    public float NormalSpeed;
    public float RushSpeed;
    public float CurrentSpeed;
    public Vector3 Direction;
    public float HurtForce;
    [Header("计时器")] 
    public float WaitTime;

    private float WaitCount;
    [Header("z状态")]
    public bool ifWait;
    public bool ifHurt; 
        
    public Transform Attacker;
    
    private void Awake()
    {
        Rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _physicsCheck = GetComponent<PhysicsCheck>();
    }

    private void Start()
    {
        CurrentSpeed = NormalSpeed;
        WaitCount = WaitTime;
    }

    private void Update()
    {
        Direction = new Vector3(-transform.localScale.x, 0, 0);
        if ((_physicsCheck.ifTorchLeftWall&&Direction.x<0) || (_physicsCheck.ifTorchRightWall && Direction.x > 0))
        {
            ifWait = true;
            _animator.SetBool("Walk",false);
        }
        TimeCounter();
    }

    private void FixedUpdate()
    {
        
            Move();
        
    }

    public virtual void Move()
    {
        Rb2D.velocity = new Vector2(Direction.x * CurrentSpeed*Time.deltaTime, Rb2D.velocity.y);
    }

    public void TimeCounter()
    {
        if (ifWait)
        {
            WaitCount -= Time.deltaTime;
            if (WaitCount<=0)
            {
                ifWait = false;
                WaitCount = WaitTime;
                transform.localScale = new Vector3(Direction.x, 1, 1);
                _animator.SetBool("Walk",true);
            }
        }
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
        StartCoroutine(OnHurt(dir));
    }

    private IEnumerator OnHurt(Vector2 dir)
    {
        
        Rb2D.AddForce(dir*HurtForce,ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.45f);
        ifHurt = false;
    }
}
