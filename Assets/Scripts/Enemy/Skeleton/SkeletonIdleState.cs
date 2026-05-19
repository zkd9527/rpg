using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.instance.StopSFX(14);
        stateTimer = enemy.patrolStayTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //如果骨骼已经不在空闲状态，它将不会执行剩余的代码
        if (stateMachine.currentState != enemy.idleState)
        {
            return;
        }

        enemy.SetVelocity(0, rb.velocity.y);

        if (stateTimer < 0)
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.moveState);
        }
    }
}
