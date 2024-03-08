using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee :Enemy
{

   public float IdleRange;
   
   [HideInInspector]public bool ifMove;
   [HideInInspector]public bool ifRush;
   [HideInInspector]public Vector2 IdleDir;
   
    protected override void Awake()
    { 
       base.Awake();
       Patrolstate = new BeePatrolState(); 
       RushState = new BeeRushState();
    }

    protected override void Start()
    {
       base.Start();
       RandomPlace();
    }

    protected override void Update()
    {
      
       Currentstate.LogicUpdate();
       if (ifMove&&!ifHurt&&!ifDead&&!ifWait)
       {
          Move();
       }
       TimeCounter();
    }

   

    protected override void TimeCounter()
    {
       if (ifWait)
       {
          WaitCount -= Time.deltaTime;
          if (WaitCount<=0)
          {
             ifWait = false;
             WaitCount = WaitTime;
             transform.localScale = new Vector3(FaceDir.x, 1, 1);
             if (!ifMove)
             {
                RandomPlace();
             }
             ifMove = true;
          }
       }

       if (!ifWait)
       {
          WaitCount -= Time.deltaTime;
          if (WaitCount<=0)
          {
             ifWait = true;
             WaitCount = WaitTime;
             ifMove = false;
          }
       }

       if (!LookingForPlayerCircle()&& LostPlayerCount>0)
       {
          LostPlayerCount -= Time.deltaTime;
       }
       else if (LookingForPlayerCircle())
       {
          LostPlayerCount = LostPlayerTime;
       }
    }

    protected override void Move()
    {
       if (CurrentSpeed==NormalSpeed)
       {
          this.transform.localScale = new Vector3(FaceDir.x, 1, 1);
          transform.position = Vector2.MoveTowards(transform.position,
             IdleDir, CurrentSpeed * Time.deltaTime);
       }
       else if (CurrentSpeed==RushSpeed)
       {
          this.transform.localScale = new Vector3(FaceDir.x, 1, 1);
          transform.position = Vector2.MoveTowards(transform.position,
             Target.transform.position, CurrentSpeed * Time.deltaTime);
       }
    }

    private void RandomPlace()
   {
      IdleDir= new Vector2(Random.Range(this.transform.position.x - IdleRange, this.transform.position.x + IdleRange),
         Random.Range(this.transform.position.y - IdleRange, this.transform.position.y + IdleRange));
      FaceDir = new Vector3(transform.position.x-IdleDir.x!=0?transform.position.x-IdleDir.x:1, 0, 0).normalized;
   }

}
