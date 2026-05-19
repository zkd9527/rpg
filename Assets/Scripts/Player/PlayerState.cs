using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家状态基类，所有玩家状态都继承自此类
/// </summary>
public class PlayerState
{
    /// <summary>
    /// 状态机引用
    /// </summary>
    protected PlayerStateMachine stateMachine;
    
    /// <summary>
    /// 玩家引用
    /// </summary>
    protected Player player;
    
    /// <summary>
    /// 刚体组件引用
    /// </summary>
    protected Rigidbody2D rb;

    /// <summary>
    /// 动画布尔值名称
    /// </summary>
    private string animBoolName;
    
    /// <summary>
    /// 水平输入值
    /// </summary>
    protected float xInput;
    
    /// <summary>
    /// 垂直输入值
    /// </summary>
    protected float yInput;

    /// <summary>
    /// 状态计时器
    /// </summary>
    protected float stateTimer;
    
    /// <summary>
    /// 动画触发是否被调用
    /// </summary>
    protected bool triggerCalled;

    /// <summary>
    /// 构造函数，初始化玩家状态
    /// </summary>
    /// <param name="_player">玩家引用</param>
    /// <param name="_stateMachine">状态机引用</param>
    /// <param name="_animBoolName">动画布尔值名称</param>
    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        player = _player;
        stateMachine = _stateMachine;
        animBoolName = _animBoolName;
    }

    /// <summary>
    /// 进入状态时的初始化
    /// </summary>
    public virtual void Enter()
    {
        // 设置对应的动画布尔值为true
        player.anim.SetBool(animBoolName, true);
        // 获取刚体组件
        rb = player.rb;
        // 重置触发标志
        triggerCalled = false;
    }

    /// <summary>
    /// 每帧更新，处理状态的行为
    /// </summary>
    public virtual void Update()
    {
        // 减少状态计时器
        stateTimer -= Time.deltaTime;

        // 获取水平和垂直输入
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        // 设置动画中的垂直速度参数
        player.anim.SetFloat("yVelocity", rb.velocity.y);
    }

    /// <summary>
    /// 退出状态时的清理
    /// </summary>
    public virtual void Exit()
    {
        // 设置对应的动画布尔值为false
        player.anim.SetBool(animBoolName, false);
    }

    /// <summary>
    /// 动画完成触发方法
    /// </summary>
    public virtual void AnimationFinishTrigger()
    {
        // 设置触发标志为true
        triggerCalled = true;
    }

}
