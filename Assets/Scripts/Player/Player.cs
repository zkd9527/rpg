using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 玩家控制器类，继承自Entity，负责处理玩家的所有行为和状态
/// </summary>
public class Player : Entity
{
    /// <summary>
    /// 技能管理器实例，用于管理玩家的技能
    /// </summary>
    public SkillManager skill { get; private set; }
    
    /// <summary>
    /// 当前玩家持有的剑对象
    /// </summary>
    public GameObject sword { get; private set; }

    [Header("移动信息")]
    /// <summary>
    /// 移动速度
    /// </summary>
    public float moveSpeed;
    
    /// <summary>
    /// 跳跃力量
    /// </summary>
    public float jumpForce;
    
    /// <summary>
    /// 墙跳时的水平速度
    /// </summary>
    public float wallJumpXSpeed;
    
    /// <summary>
    /// 墙跳持续时间
    /// </summary>
    public float wallJumpDuration;
    
    /// <summary>
    /// 默认移动速度
    /// </summary>
    private float defaultMoveSpeed;
    
    /// <summary>
    /// 默认跳跃力量
    /// </summary>
    private float defaultJumpForce;

    [Header("跳跃计数")]
    /// <summary>
    /// 当前跳跃次数
    /// </summary>
    public int jumpcount;
    
    /// <summary>
    /// 最大跳跃次数
    /// </summary>
    public int maxjumpcount;

    [Header("攻击信息")]
    /// <summary>
    /// 攻击时的移动向量数组
    /// </summary>
    public Vector2[] attackMovement;
    
    /// <summary>
    /// 反击持续时间
    /// </summary>
    public float counterAttackDuration = 0.2f;

    [Header("冲刺信息")]
    /// <summary>
    /// 冲刺速度
    /// </summary>
    public float dashSpeed;
    
    /// <summary>
    /// 冲刺持续时间
    /// </summary>
    public float dashDuration;
    
    /// <summary>
    /// 冲刺方向
    /// </summary>
    public float dashDirection { get; private set; }
    
    /// <summary>
    /// 默认冲刺速度
    /// </summary>
    private float defaultDashSpeed;

    [Header("环境检测")]
    /// <summary>
    /// 检测是否靠近陷阱的碰撞器
    /// </summary>
    [SerializeField] private BoxCollider2D pitCheck;
    
    /// <summary>
    /// 检测是否站在可下落平台上的碰撞器
    /// </summary>
    [SerializeField] private BoxCollider2D downablePlatformCheck;

    /// <summary>
    /// 是否靠近陷阱
    /// </summary>
    public bool isNearPit { get; set; }
    
    /// <summary>
    /// 最后站过的平台
    /// </summary>
    public DownablePlatform lastPlatform { get; set; }
    
    /// <summary>
    /// 是否站在平台上
    /// </summary>
    public bool isOnPlatform { get; set; } = false;

    /// <summary>
    /// 是否忙碌（如攻击、技能释放等）
    /// </summary>
    public bool isBusy { get; private set; }
    
    /// <summary>
    /// 玩家特效管理器
    /// </summary>
    public PlayerFX fx { get; private set; }

    #region 状态和状态机
    /// <summary>
    /// 玩家状态机
    /// </summary>
    public PlayerStateMachine stateMachine { get; private set; }

    /// <summary>
    ///  idle状态
    /// </summary>
    public PlayerIdleState idleState { get; private set; }
    
    /// <summary>
    /// 移动状态
    /// </summary>
    public PlayerMoveState moveState { get; private set; }
    
    /// <summary>
    /// 跳跃状态
    /// </summary>
    public PlayerJumpState jumpState { get; private set; }
    
    /// <summary>
    /// 空中状态
    /// </summary>
    public PlayerAirState airState { get; private set; }
    
    /// <summary>
    /// 冲刺状态
    /// </summary>
    public PlayerDashState dashState { get; private set; }
    
    /// <summary>
    /// 墙滑状态
    /// </summary>
    public PlayerWallSlideState wallSlideState { get; private set; }
    
    /// <summary>
    /// 墙跳状态
    /// </summary>
    public PlayerWallJumpState wallJumpState { get; private set; }
    
    /// <summary>
    /// 主要攻击状态
    /// </summary>
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    
    /// <summary>
    /// 空中发射攻击状态
    /// </summary>
    public PlayerAirLaunchAttackState airLaunchAttackState { get; private set; }
    
    /// <summary>
    /// 下落攻击状态
    /// </summary>
    public PlayerDownStrikeState downStrikeState { get; private set; }
    
    /// <summary>
    /// 反击状态
    /// </summary>
    public PlayerCounterAttackState counterAttackState { get; private set; }
    
    /// <summary>
    /// 瞄准剑状态
    /// </summary>
    public PlayerAimSwordState aimSwordState { get; private set; }
    
    /// <summary>
    /// 投掷剑状态
    /// </summary>
    public PlayerThrowSwordState throwSwordState { get; private set; }
    
    /// <summary>
    /// 接住剑状态
    /// </summary>
    public PlayerCatchSwordState catchSwordState { get; private set; }
    
    /// <summary>
    /// 释放黑洞技能状态
    /// </summary>
    public PlayerReleaseBlackholeSkillState blackholeSkillState { get; private set; }
    
    /// <summary>
    /// 死亡状态
    /// </summary>
    public PlayerDeathState deathState { get; private set; }
    #endregion

    /// <summary>
    /// 唤醒时初始化状态机和各种状态
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        // 初始化状态机
        stateMachine = new PlayerStateMachine();
        // 获取玩家特效组件
        fx = GetComponent<PlayerFX>();

        // 初始化各种状态
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        airLaunchAttackState = new PlayerAirLaunchAttackState(this, stateMachine, "AirLaunchAttack");
        downStrikeState = new PlayerDownStrikeState(this, stateMachine, "DownStrike");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        throwSwordState = new PlayerThrowSwordState(this, stateMachine, "ThrowSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackholeSkillState = new PlayerReleaseBlackholeSkillState(this, stateMachine, "Jump");
        deathState = new PlayerDeathState(this, stateMachine, "Death");
    }

    /// <summary>
    /// 开始时初始化技能管理器和默认状态
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // 获取技能管理器实例
        skill = SkillManager.instance;

        // 初始化状态机到idle状态
        stateMachine.Initialize(idleState);

        // 保存默认速度值
        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }

    /// <summary>
    /// 每帧更新，处理输入和状态更新
    /// </summary>
    protected override void Update()
    {
        // 如果游戏暂停，直接返回
        if (Time.timeScale == 0)
        {
            return;
        }

        base.Update();

        // 更新当前状态
        stateMachine.currentState.Update();

        // 如果玩家死亡，直接返回
        if (stats.isDead)
        {
            return;
        }

        // 检测冲刺输入
        CheckForDashInput();

        // 检测水晶技能输入
        if (Input.GetKeyDown(KeyBindManager.instance.keybindsDictionary["Crystal"]) && skill.crystal.crystalUnlocked)
        {
            skill.crystal.UseSkillIfAvailable();
        }

        // 检测药水使用输入
        if (Input.GetKeyDown(KeyBindManager.instance.keybindsDictionary["Flask"]))
        {
            Inventory.instance.UseFlask_ConsiderCooldown(null);
        }

        // 检测从平台下落输入
        if (Input.GetKeyDown(KeyCode.S))
            fallfromplatform();

    }

    /// <summary>
    /// 从平台下落的方法
    /// </summary>
    private void fallfromplatform()
    {
        // 如果在 Boss 房间
        if (BossRoom.Instance != null)
        {
            // 找到 Boss 对象
            GameObject aimed = GameObject.Find("Boss(Clone)");
            // 找到 Tilemaps 子对象
            Transform tilemaps = aimed.transform.Find("Tilemaps");
            // 找到 Platforms 游戏对象
            GameObject wall = tilemaps.transform.Find("Platforms").gameObject;
            // 获取 TilemapCollider2D 组件
            TilemapCollider2D tilecolider = wall.GetComponent<TilemapCollider2D>();
            // 禁用碰撞器
            tilecolider.enabled = false;
            // 0.5秒后恢复碰撞器
            Invoke("returnplatform", 0.5f);
        }
        else
        {
            // 找到生成的关卡
            GameObject aimed = GameObject.Find("Generated Level");
            if (aimed == null)
                return;

            // 找到 Tilemaps 子对象
            Transform tilemaps = aimed.transform.Find("Tilemaps");
            // 找到 Platforms 游戏对象
            GameObject wall = tilemaps.transform.Find("Platforms").gameObject;
            // 获取 TilemapCollider2D 组件
            TilemapCollider2D tilecolider = wall.GetComponent<TilemapCollider2D>();
            // 禁用碰撞器
            tilecolider.enabled = false;
            // 0.5秒后恢复碰撞器
            Invoke("returnplatform", 0.5f);
        }
    }
    
    /// <summary>
    /// 恢复平台碰撞器的方法
    /// </summary>
    private void returnplatform()
    {
        // 如果在 Boss 房间
        if (BossRoom.Instance != null)
        {
            // 找到 Boss 对象
            GameObject aimed = GameObject.Find("Boss(Clone)");
            // 找到 Tilemaps 子对象
            Transform tilemaps = aimed.transform.Find("Tilemaps");
            // 找到 Platforms 游戏对象
            GameObject wall = tilemaps.transform.Find("Platforms").gameObject;
            // 获取 TilemapCollider2D 组件
            TilemapCollider2D tilecolider = wall.GetComponent<TilemapCollider2D>();
            // 启用碰撞器
            tilecolider.enabled = true;
        }
        else
        {
            // 找到生成的关卡
            GameObject aimed = GameObject.Find("Generated Level");
            if (aimed == null)
                return;
            // 找到 Tilemaps 子对象
            Transform tilemaps = aimed.transform.Find("Tilemaps");
            // 找到 Platforms 游戏对象
            GameObject wall = tilemaps.transform.Find("Platforms").gameObject;
            // 获取 TilemapCollider2D 组件
            TilemapCollider2D tilecolider = wall.GetComponent<TilemapCollider2D>();
            // 启用碰撞器
            tilecolider.enabled = true;
        }
    }

    /// <summary>
    /// 检测冲刺输入的方法
    /// </summary>
    private void CheckForDashInput()
    {
        // 如果冲刺技能未解锁，直接返回
        if (skill.dash.dashUnlocked == false)
        {
            return;
        }

        // 如果检测到墙，直接返回
        if (IsWallDetected())
        {
            return;
        }

        // 如果按下冲刺键且技能可用
        if (Input.GetKeyDown(KeyBindManager.instance.keybindsDictionary["Dash"]) && SkillManager.instance.dash.UseSkillIfAvailable())
        {
            // 如果当前状态是瞄准剑或投掷剑状态，隐藏瞄准点
            if (stateMachine.currentState == aimSwordState || stateMachine.currentState == throwSwordState)
            {
                skill.sword.ShowDots(false);
            }

            // 获取水平输入作为冲刺方向
            dashDirection = Input.GetAxisRaw("Horizontal");

            // 如果没有水平输入，使用当前朝向作为冲刺方向
            if (dashDirection == 0)
            {
                dashDirection = facingDirection;
            }

            // 切换到冲刺状态
            stateMachine.ChangeState(dashState);
        }
    }

    /// <summary>
    /// 动画触发方法，用于通知当前状态动画已完成
    /// </summary>
    public void AnimationTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }

    /// <summary>
    /// 空中发射跳跃触发方法
    /// </summary>
    public void AirLaunchJumpTrigger()
    {
        airLaunchAttackState.SetAirLaunchJumpTrigger();
    }

    /// <summary>
    /// 下落攻击触发方法
    /// </summary>
    public void DownStrikeTrigger()
    {
        downStrikeState.SetFallingStrikeTrigger();
    }

    /// <summary>
    /// 下落攻击动画停止触发方法
    /// </summary>
    public void DownStrikeAnimStopTrigger()
    {
        downStrikeState.SetAnimStopTrigger();
    }

    /// <summary>
    /// 使玩家忙碌指定时间的协程
    /// </summary>
    /// <param name="_seconds">忙碌持续时间（秒）</param>
    /// <returns>协程迭代器</returns>
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    /// <summary>
    /// 分配新剑的方法
    /// </summary>
    /// <param name="_newSword">新剑对象</param>
    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    /// <summary>
    /// 接住剑的方法
    /// </summary>
    public void CatchSword()
    {
        // 切换到接住剑状态
        stateMachine.ChangeState(catchSwordState);
        // 销毁当前剑对象
        Destroy(sword);
    }

    /// <summary>
    /// 检查是否没有剑的方法
    /// </summary>
    /// <returns>如果没有剑返回true，否则返回false并尝试收回剑</returns>
    public bool HasNoSword()
    {
        if (!sword)
        {
            return true;
        }

        // 尝试收回剑
        sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;
    }

    /// <summary>
    /// 玩家死亡方法
    /// </summary>
    public override void Die()
    {
        base.Die();

        // 切换到死亡状态
        stateMachine.ChangeState(deathState);
    }

    /// <summary>
    /// 按百分比减慢速度的方法
    /// </summary>
    /// <param name="_percentage">减慢的百分比（0-1）</param>
    /// <param name="_duration">减慢持续时间（秒）</param>
    public override void SlowSpeedBy(float _percentage, float _duration)
    {
        // 减慢移动速度
        moveSpeed = moveSpeed * (1 - _percentage);
        // 减慢跳跃力量
        jumpForce = jumpForce * (1 - _percentage);
        // 减慢冲刺速度
        dashSpeed = dashSpeed * (1 - _percentage);
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

        // 恢复默认移动速度
        moveSpeed = defaultMoveSpeed;
        // 恢复默认跳跃力量
        jumpForce = defaultJumpForce;
        // 恢复默认冲刺速度
        dashSpeed = defaultDashSpeed;
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
    /// 从平台跳下的方法
    /// </summary>
    public void JumpOffPlatform()
    {
        if (isOnPlatform)
        {
            // 暂时关闭平台碰撞器0.5秒
            lastPlatform.TurnOffPlatformColliderForTime(0.5f);
        }
    }
}
