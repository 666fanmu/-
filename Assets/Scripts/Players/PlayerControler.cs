using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerControler : MonoBehaviour
{
    private PlayerInputControl _playerInputControl;
    private PhysicsCheck _physicsCheck;
    private PlayAnimation _playAnimation;
    private CapsuleCollider2D coll;
    
    private Rigidbody2D Rb2D;
    
    public Vector2 inputDirection;

    [Header("物理材质")] 
    public PhysicsMaterial2D Wall;

    public PhysicsMaterial2D Normal;
    
    [Header("基本参数")]
    public float MoveSpeed;
    public float JumpForce;
    public float HurtForce;
    
    [Header("状态")]
    public bool IfHurt;
    public bool IfDead=false;
    public bool IfAttack = false;
    public bool IfCrouch = false;
    
    private void Awake()
    {
        _playerInputControl = new PlayerInputControl();
        
        Rb2D = this.GetComponent<Rigidbody2D>();
        _physicsCheck = GetComponent<PhysicsCheck>();
        _playAnimation = GetComponent<PlayAnimation>();
        coll = GetComponent<CapsuleCollider2D>();
        
        _playerInputControl.GamePlay.Jump.started += Jump;
        
        _playerInputControl.GamePlay.Attack.started += Attack;
    }

    


    private void OnEnable()
    {
        _playerInputControl.Enable();
    }

    private void FixedUpdate()
    {
        if (!IfHurt&&!IfAttack)
        {
            Move();
        }
    }

    private void Update()
    {
        inputDirection = _playerInputControl.GamePlay.Move.ReadValue<Vector2>();
        CheckState();
    }


    private void OnDisable()
    {
        _playerInputControl.Disable();
    }

    public void Move()
    {
        Rb2D.velocity = new Vector2(inputDirection.x * MoveSpeed * Time.deltaTime, Rb2D.velocity.y);
        int FaceDir = (int)transform.localScale.x;
        if(inputDirection.x>0)
        {
            FaceDir = 1;
        }
        if(inputDirection.x<0)
        {
            FaceDir = -1;
        }

        transform.localScale = new Vector3(FaceDir, 1, 1);
    }
    private void Jump(InputAction.CallbackContext obj)
    {
        if (_physicsCheck.ifOnGround)
        {
            Rb2D.AddForce(transform.up*JumpForce,ForceMode2D.Impulse);
        }
       
    }
    
    private void Attack(InputAction.CallbackContext obj)
    {
        if (!_physicsCheck.ifOnGround)
        {
           return;
        }
        IfAttack = true;
        _playAnimation.PlayerAttack();  
    }

    public void GetHurt(Transform attacker)
    {
        IfHurt = true;
        Rb2D.velocity=Vector2.zero;
        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
        Rb2D.AddForce(dir*HurtForce,ForceMode2D.Impulse);
        
    }

    public void PlayerDead()
    {
        IfDead = true;
        _playerInputControl.GamePlay.Disable();
    }

    private void CheckState()
    {
        coll.sharedMaterial = _physicsCheck.ifOnGround ? Normal: Wall;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("take");
    }
}
