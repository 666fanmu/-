using UnityEngine;

public class BoarRushState : BaseState
{
    
    public override void OnEnterState(Enemy enemy)
    {
        CurrentEnemy = enemy;
        CurrentEnemy.CurrentSpeed = CurrentEnemy.RushSpeed;
        CurrentEnemy._animator.SetBool("Run",true);
    }

    public override void LogicUpdate()
    {
        if (CurrentEnemy.LostPlayerCount<=0)
        {
            CurrentEnemy.SwitchState(EnemyState.Patrol);
        }
        if (!CurrentEnemy._physicsCheck.ifOnGround||
            ((CurrentEnemy._physicsCheck.ifTorchLeftWall && CurrentEnemy.FaceDir.x < 0) || 
             (CurrentEnemy._physicsCheck.ifTorchRightWall && CurrentEnemy.FaceDir.x > 0)))
        {
            CurrentEnemy.transform.localScale = new Vector3(CurrentEnemy.FaceDir.x, 1, 1);

        }
       
    }

    public override void PhysicsUpdate()
    {
       
    }

    public override void OnExitState()
    {
        CurrentEnemy._animator.SetBool("Run",false);
        CurrentEnemy.CurrentSpeed = CurrentEnemy.NormalSpeed;
       
    }
}
