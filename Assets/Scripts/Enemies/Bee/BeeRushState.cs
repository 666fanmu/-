using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeRushState : BaseState
{
    public override void OnEnterState(Enemy enemy)
    {
        CurrentEnemy = enemy;
        CurrentEnemy.CurrentSpeed = CurrentEnemy.RushSpeed;
        CurrentEnemy._animator.SetBool("Rush",true);
    }

    public override void LogicUpdate()
    {
        if (CurrentEnemy.LostPlayerCount<=0)
        {
            CurrentEnemy.SwitchState(EnemyState.Patrol);
        }
        
    }

    public override void PhysicsUpdate()
    {
       
    }

    public override void OnExitState()
    {
        CurrentEnemy._animator.SetBool("Rush",false);
    }
}
