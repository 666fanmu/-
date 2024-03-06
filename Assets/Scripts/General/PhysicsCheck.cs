using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D coll;
    //是否自动判断左右
    public bool manual;
    
    [Header("检测参数")]
    //检测范围
    public float checkRaduis;
    //脚底的位置偏差
    public Vector2 buttonOffset;
    public Vector2 LeftWallOffset;
    public Vector2 RightWallOffset;
    
    public LayerMask GroundLayer;
    
    [Header("状态")]
    public bool ifOnGround=true;
    public bool ifTorchLeftWall;
    public bool ifTorchRightWall;

    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();
        if (!manual)
        {
            RightWallOffset = new Vector2((coll.bounds.size.x + coll.offset.x)/2, coll.bounds.size.y/2);
            LeftWallOffset = new Vector2(-RightWallOffset.x, RightWallOffset.y);
        }
    }

    private void Update()
    {
        Check();
    }

    public void Check()
    {
        //检测地面
        ifOnGround = Physics2D.OverlapCircle((Vector2)transform.position+new Vector2(buttonOffset.x*transform.localScale.x,buttonOffset.y),checkRaduis,GroundLayer);
        //检测墙壁
        ifTorchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(LeftWallOffset.x,LeftWallOffset.y),checkRaduis, GroundLayer);
        ifTorchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(RightWallOffset.x,RightWallOffset.y),checkRaduis, GroundLayer);
    }
    
 
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position+new Vector2(buttonOffset.x*transform.localScale.x,buttonOffset.y),checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(LeftWallOffset.x,LeftWallOffset.y),checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(RightWallOffset.x,RightWallOffset.y),checkRaduis);
    }
}
