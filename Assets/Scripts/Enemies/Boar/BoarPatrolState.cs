using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarPatrolState : BaseState
{
    
    public override void OnEnterState(Enemy enemy)
    {
        CurrentEnemy = enemy;
        enemy.CurrentSpeed = enemy.NormalSpeed;
    }

    public override void LogicUpdate()
    {
        //发现玩家冲锋
        if (CurrentEnemy.LookingForPlayerBox())
        {
            CurrentEnemy.SwitchState(EnemyState.Chase);
        }
        //巡逻逻辑
        if (!CurrentEnemy._physicsCheck.ifOnGround||
            ((CurrentEnemy._physicsCheck.ifTorchLeftWall && CurrentEnemy.FaceDir.x < 0) || 
             (CurrentEnemy._physicsCheck.ifTorchRightWall && CurrentEnemy.FaceDir.x > 0)))
        {
            CurrentEnemy.ifWait = true;
            CurrentEnemy._animator.SetBool("Walk",false); 
        }
        else
        {
            CurrentEnemy._animator.SetBool("Walk",true);
        }


        
    }

    public override void PhysicsUpdate()
    {
      
    }

    public override void OnExitState()
    {
        CurrentEnemy._animator.SetBool("Walk",false);
    }
}
