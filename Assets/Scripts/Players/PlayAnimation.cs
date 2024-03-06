using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D Rb2D;
    private PhysicsCheck _physicsCheck;
    private PlayerControler _playerControler;

    private void Awake()
    {
        animator=GetComponent<Animator>();
        Rb2D=GetComponent<Rigidbody2D>();
        _physicsCheck = GetComponent<PhysicsCheck>();
        _playerControler = GetComponent<PlayerControler>();
    }

    private void Update()
    {
        SetAnimation();
    }

    public void SetAnimation()
    {
        animator.SetFloat("VelocityX",Mathf.Abs(Rb2D.velocity.x));
        animator.SetFloat("VelocityY",Rb2D.velocity.y);
        animator.SetBool("ifOnGround",_physicsCheck.ifOnGround);
        animator.SetBool("IfDead",_playerControler.IfDead);
        animator.SetBool("IfAttack",_playerControler.IfAttack);
    }
    //播放受伤动画
    public void PlayHurt()
    {
        animator.SetTrigger("hurt");
    }

    public void PlayerAttack()
    {
        animator.SetTrigger("Attack");
        
    }
    
}
