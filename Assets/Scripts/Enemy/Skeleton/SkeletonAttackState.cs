using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    Enemy_Skeleton enemy;
    public SkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        //在攻击开始时设定时间，让敌人先移动一点
        stateTimer = 0.1f;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            //如果敌人被击退，那么它就不会再向前移动了
            if (enemy.isKnockbacked)
            {
                stateTimer = 0;
                return;
            }

        }
        else
        {
            enemy.SetVelocity(0, rb.velocity.y);
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.battleState);
        }

    }
}
