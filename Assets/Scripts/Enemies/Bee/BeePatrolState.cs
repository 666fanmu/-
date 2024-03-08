using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeePatrolState : BaseState
{
    public override void OnEnterState(Enemy enemy)
    {
        CurrentEnemy = enemy;
        CurrentEnemy.CurrentSpeed = CurrentEnemy.NormalSpeed;
    }

    public override void LogicUpdate()
    {
        if (CurrentEnemy.LookingForPlayerCircle())
        {
            CurrentEnemy.SwitchState(EnemyState.Chase);
        }
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExitState()
    {
        
    }
}
