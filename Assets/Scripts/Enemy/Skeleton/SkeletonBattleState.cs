using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Enemy_Skeleton enemy;
    private Transform player;

    private int moveDirection;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        //进入 battleState 会设置默认的敌人攻击时间
        //为了防止玩家从敌人背后接近时，敌人在 idleState 和 battleState 之间切换时卡住
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

        AudioManager.instance.StopSFX(24);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.stateMachine.currentState != this)
        {
            return;
        }

        AudioManager.instance.PlaySFX(24, enemy.transform);


        //敌人在战斗状态下总是面对玩家
        //防止敌人在地面边缘卡住
        FacePlayer();

        if (enemy.IsPlayerDetected())
        {
            //如果敌人能看到玩家，那么它总是处于攻击模式
            stateTimer = enemy.aggressiveTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {

                if (CanAttack())
                {
                    anim.SetBool("Idle", false);
                    anim.SetBool("Move", false);
                    stateMachine.ChangeState(enemy.attackState);
                    return;
                }
            }
        }
        else  //若敌人不能看到玩家
        {
            //如果敌人看不到玩家或玩家不在敌人的扫描范围内，敌人将切换回巡逻模式
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > enemy.playerScanDistance)
            {
                stateMachine.ChangeState(enemy.idleState);
                return;
            }
        }

        //敌人只有在玩家远离敌人的攻击范围时才向玩家移动
        //或者玩家在敌人身后时
        //当敌人接近玩家时，它将停止
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

        if(!enemy.IsGroundDetected())
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
        //骷髅只有在地面上才能攻击
        if (Time.time - enemy.lastTimeAttacked >= enemy.attackCooldown && !enemy.isKnockbacked && rb.velocity.y <= 0.1f && rb.velocity.y >= -0.1f)
        {
            //敌人的lastTimeAttacked在attackState设置
            //敌人的攻击频率将是随机的
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            return true;
        }

        return false;
    }

    private void ChangeToIdleAnimation()
    {
        anim.SetBool("Move", false);
        anim.SetBool("Idle", true);
    }

    private void ChangeToMoveAnimation()
    {
        anim.SetBool("Idle", false);
        anim.SetBool("Move", true);
    }
    //面向玩家方法
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
