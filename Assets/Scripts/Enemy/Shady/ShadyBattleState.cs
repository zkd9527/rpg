using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShadyBattleState : ShadyState
{
    private Transform player;

    private int moveDirection;

    public ShadyBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //进入 battleState 会设置默认的敌人攻击时间
        //为了防止玩家从背后接近敌人时，敌人在 idleState 和 battleState 之间切换时卡住
        stateTimer = enemy.aggressiveTime;

        player = PlayerManager.instance.player.transform;

        //如果玩家从背后攻击敌人，敌人会立即转向玩家的一侧
        FacePlayer();

        if (player.GetComponent<PlayerStats>().isDead)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void Update()
    {
        base.Update();

        if (enemy.stateMachine.currentState != this)
        {
            return;
        }


        FacePlayer();

        if (enemy.IsPlayerDetected())
        {

            stateTimer = enemy.aggressiveTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {

                if (CanAttack())
                {
                    anim.SetBool("Idle", false);
                    anim.SetBool("BattleMove", false);
                    stateMachine.ChangeState(enemy.explosionState);
                    return;
                }
            }
        }
        else  
        {
           
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > enemy.playerScanDistance)
            {
                stateMachine.ChangeState(enemy.idleState);
                return;
            }
        }


        if (enemy.IsPlayerDetected() && Vector2.Distance(enemy.transform.position, player.transform.position) < enemy.attackDistance)
        {
            ChangeToIdleAnimation();
            return;
        }
        if (Vector2.Distance(PlayerManager.instance.player.gameObject.transform.position, enemy.transform.position) < 1)
            return;

        if (player.position.x > enemy.transform.position.x)
        {
            moveDirection = 1;
        }
        else if (player.position.x < enemy.transform.position.x)
        {
            moveDirection = -1;
        }

        if (!enemy.IsGroundDetected())
        {
            enemy.SetVelocity(0, rb.velocity.y);
            ChangeToIdleAnimation();
            return;
        }

        enemy.SetVelocity(enemy.battleMoveSpeed * moveDirection, rb.velocity.y);
        ChangeToMoveAnimation();

    }

    private bool CanAttack()
    {
        
        if (Time.time - enemy.lastTimeAttacked >= enemy.attackCooldown && !enemy.isKnockbacked && rb.velocity.y <= 0.1f && rb.velocity.y >= -0.1f)
        {

            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            return true;
        }

        return false;
    }

    private void ChangeToIdleAnimation()
    {
        anim.SetBool("BattleMove", false);
        anim.SetBool("Idle", true);
    }

    private void ChangeToMoveAnimation()
    {
        anim.SetBool("Idle", false);
        anim.SetBool("BattleMove", true);
    }

    private void FacePlayer()
    {
        if (player.transform.position.x < enemy.transform.position.x)
        {
            if (enemy.facingDirection != -1)
            {
                enemy.Flip();
            }
        }

        if (player.transform.position.x > enemy.transform.position.x)
        {
            if (enemy.facingDirection != 1)
            {
                enemy.Flip();
            }
        }
    }
}
