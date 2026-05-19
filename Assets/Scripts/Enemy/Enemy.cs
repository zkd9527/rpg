using System.Collections;
using UnityEngine;

/// <summary>
/// 敌人基类，继承自Entity，负责处理敌人的基本行为和状态
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(EntityFX))]
[RequireComponent(typeof(ItemDrop))]
public class Enemy : Entity
{
    [Header("移动信息")]
    /// <summary>
    /// 巡逻时的移动速度
    /// </summary>
    public float patrolMoveSpeed;
    
    /// <summary>
    /// 巡逻时停留的时间
    /// </summary>
    public float patrolStayTime;

    /// <summary>
    /// 默认巡逻移动速度
    /// </summary>
    private float defaultPatrolMoveSpeed;

    [Header("侦察信息")]
    /// <summary>
    /// 玩家扫描距离
    /// </summary>
    public float playerScanDistance = 10;
    
    /// <summary>
    /// 玩家听觉距离
    /// </summary>
    public float playerHearDistance = 3;
    
    /// <summary>
    /// 什么是玩家的层掩码
    /// </summary>
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("战斗/攻击信息")]
    /// <summary>
    /// 战斗时的移动速度
    /// </summary>
    public float battleMoveSpeed;
    
    /// <summary>
    /// 攻击时间
    /// </summary>
    public float aggressiveTime = 7;

    /// <summary>
    /// 默认战斗移动速度
    /// </summary>
    private float defaultBattleMoveSpeed;

    [Header("攻击信息")]
    /// <summary>
    /// 攻击距离
    /// </summary>
    public float attackDistance = 2;
    
    /// <summary>
    /// 攻击冷却时间
    /// </summary>
    public float attackCooldown = 1.5f;
    
    /// <summary>
    /// 最小攻击冷却时间
    /// </summary>
    public float minAttackCooldown = 1;
    
    /// <summary>
    /// 最大攻击冷却时间
    /// </summary>
    public float maxAttackCooldown = 2;
    
    /// <summary>
    /// 上次攻击的时间
    /// </summary>
    [HideInInspector] public float lastTimeAttacked;

    [Header("眩晕信息")]
    /// <summary>
    /// 眩晕持续时间
    /// </summary>
    public float stunDuration = 1;
    
    /// <summary>
    /// 眩晕时的移动向量
    /// </summary>
    public Vector2 stunMovement = new Vector2(3, 3);
    
    /// <summary>
    /// 是否可以被眩晕
    /// </summary>
    protected bool canBeStunned;
    
    /// <summary>
    /// 反击提示图像
    /// </summary>
    [SerializeField] protected GameObject counterPromptImage;

    /// <summary>
    /// 敌人状态机
    /// </summary>
    public EnemyStateMachine stateMachine { get; private set; }
    
    /// <summary>
    /// 玩家对象
    /// </summary>
    protected Player player { get; private set; }
    
    /// <summary>
    /// 敌人特效管理器
    /// </summary>
    public EntityFX fx { get; private set; }

    /// <summary>
    /// 最后一个动画布尔值名称
    /// </summary>
    public string lastAnimBoolName { get; private set; }

    /// <summary>
    /// 唤醒时初始化状态机和各种组件
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        // 初始化状态机
        stateMachine = new EnemyStateMachine();
        // 获取特效组件
        fx = GetComponent<EntityFX>();

        // 保存默认速度值
        defaultBattleMoveSpeed = battleMoveSpeed;
        defaultPatrolMoveSpeed = patrolMoveSpeed;
    }

    /// <summary>
    /// 开始时获取玩家引用
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // 获取玩家实例
        player = PlayerManager.instance.player;
    }

    /// <summary>
    /// 每帧更新，处理状态更新
    /// </summary>
    protected override void Update()
    {
        base.Update();

        // 更新当前状态
        stateMachine.currentState.Update();
    }

    /// <summary>
    /// 绘制 gizmos，显示攻击距离
    /// </summary>
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // 设置 gizmos 颜色为黄色
        Gizmos.color = Color.yellow;
        // 绘制攻击距离线
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDirection, transform.position.y));
    }
    
    /// <summary>
    /// 检测玩家是否被发现
    /// </summary>
    /// <returns>如果检测到玩家返回RaycastHit2D，否则返回false</returns>
    public virtual RaycastHit2D IsPlayerDetected()
    {
        // 从墙检测位置发射射线，检测玩家
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, playerScanDistance, whatIsPlayer);
    }

    /// <summary>
    /// 动画触发方法，用于通知当前状态动画已完成
    /// </summary>
    public void AnimationTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }

    /// <summary>
    /// 特殊攻击触发方法
    /// </summary>
    public virtual void SpecialAttackTrigger()
    {

    }

    /// <summary>
    /// 冻结敌人的方法
    /// </summary>
    /// <param name="_freeze">是否冻结</param>
    public virtual void FreezeEnemy(bool _freeze)
    {
        if (_freeze)
        {
            // 冻结时设置速度为0
            battleMoveSpeed = 0;
            patrolMoveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            // 解冻时恢复默认速度
            battleMoveSpeed = defaultBattleMoveSpeed;
            patrolMoveSpeed = defaultPatrolMoveSpeed;
            anim.speed = 1;
        }
    }

    /// <summary>
    /// 冻结敌人一段时间的协程
    /// </summary>
    /// <param name="_seconds">冻结持续时间（秒）</param>
    /// <returns>协程迭代器</returns>
    protected virtual IEnumerator FreezeEnemyForTime_Coroutine(float _seconds)
    {
        // 冻结敌人
        FreezeEnemy(true);

        // 等待指定时间
        yield return new WaitForSeconds(_seconds);

        // 解冻敌人
        FreezeEnemy(false);
    }

    /// <summary>
    /// 冻结敌人一段时间的方法
    /// </summary>
    /// <param name="_seconds">冻结持续时间（秒）</param>
    public virtual void FreezeEnemyForTime(float _seconds)
    {
        // 启动冻结协程
        StartCoroutine(FreezeEnemyForTime_Coroutine(_seconds));
    }

    #region 反击相关方法
    /// <summary>
    /// 打开反击窗口
    /// </summary>
    public void OpenCounterAttackWindow()
    {
        // 设置可以被眩晕
        canBeStunned = true;
        // 显示反击提示图像
        if(counterPromptImage!=null)
        counterPromptImage.SetActive(true);
    }

    /// <summary>
    /// 关闭反击窗口
    /// </summary>
    public void CloseCounterAttackWindow()
    {
        // 设置不可以被眩晕
        canBeStunned = false;
        // 隐藏反击提示图像
        if (counterPromptImage != null)
            counterPromptImage.SetActive(false);
    }

    /// <summary>
    /// 检查是否可以被反击眩晕
    /// </summary>
    /// <returns>如果可以被眩晕返回true，否则返回false</returns>
    public virtual bool CanBeStunnedByCounterAttack()
    {
        if (canBeStunned)
        {
            // 关闭反击窗口
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }
    #endregion

    /// <summary>
    /// 分配最后一个动画布尔值名称
    /// </summary>
    /// <param name="_animBoolName">动画布尔值名称</param>
    public virtual void AssignLastAnimBoolName(string _animBoolName)
    {
        lastAnimBoolName = _animBoolName;
    }

    /// <summary>
    /// 按百分比减慢速度的方法
    /// </summary>
    /// <param name="_percentage">减慢的百分比（0-1）</param>
    /// <param name="_duration">减慢持续时间（秒）</param>
    public override void SlowSpeedBy(float _percentage, float _duration)
    {
        // 减慢巡逻速度
        patrolMoveSpeed = patrolMoveSpeed * (1 - _percentage);
        // 减慢战斗速度
        battleMoveSpeed = battleMoveSpeed * (1 - _percentage);
        // 减慢动画速度
        anim.speed = anim.speed * (1 - _percentage);

        // 一段时间后恢复默认速度
        Invoke("ReturnDefaultSpeed", _duration);
    }

    /// <summary>
    /// 恢复默认速度的方法
    /// </summary>
    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        // 恢复默认巡逻速度
        patrolMoveSpeed = defaultPatrolMoveSpeed;
        // 恢复默认战斗速度
        battleMoveSpeed = defaultBattleMoveSpeed;
    }

    /// <summary>
    /// 进入战斗状态的方法
    /// </summary>
    public virtual void GetIntoBattleState()
    {

    }

    /// <summary>
    /// 伤害闪烁效果方法
    /// </summary>
    public override void DamageFlashEffect()
    {
        // 启动闪烁特效协程
        fx.StartCoroutine("FlashFX");
    }

    /// <summary>
    /// 初始化最后时间信息的方法
    /// </summary>
    protected virtual void InitializeLastTimeInfo()
    {

    }

}
