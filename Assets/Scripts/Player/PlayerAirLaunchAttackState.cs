using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家空中发射攻击状态类，负责处理玩家在空中进行发射攻击的行为
/// </summary>
public class PlayerAirLaunchAttackState : PlayerState
{
    /// <summary>
    /// 空中发射跳跃触发标志
    /// </summary>
    public bool airLaunchJumpTrigger { get; private set; } = false;

    /// <summary>
    /// 是否已经跳跃
    /// </summary>
    private bool hasJumped = false;

    /// <summary>
    /// 构造函数，初始化空中发射攻击状态
    /// </summary>
    /// <param name="_player">玩家引用</param>
    /// <param name="_stateMachine">状态机引用</param>
    /// <param name="_animBoolName">动画布尔值名称</param>
    public PlayerAirLaunchAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    /// <summary>
    /// 进入状态时的初始化
    /// </summary>
    public override void Enter()
    {
        base.Enter();

        // 重置触发标志和跳跃标志
        airLaunchJumpTrigger = false;
        hasJumped = false;
    }

    /// <summary>
    /// 退出状态时的清理
    /// </summary>
    public override void Exit()
    {
        base.Exit();
    }

    /// <summary>
    /// 每帧更新，处理空中发射攻击状态的行为
    /// </summary>
    public override void Update()
    {
        base.Update();

        // 如果当前状态不是这个状态，直接返回
        if (player.stateMachine.currentState != this)
        {
            return;
        }

        // 如果触发了空中发射跳跃且还没有跳跃
        if(airLaunchJumpTrigger && !hasJumped)
        {
            // 设置向上跳跃速度
            player.SetVelocity(0, 17);
            hasJumped = true;
        }

        // 如果动画触发被调用，切换到空中状态
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.airState);
        }
    }

    /// <summary>
    /// 设置空中发射跳跃触发标志
    /// </summary>
    public void SetAirLaunchJumpTrigger()
    {
        airLaunchJumpTrigger = true;
    }
}
