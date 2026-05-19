using System.Collections;
using UnityEngine;

/// <summary>
/// 实体基类，Player和Enemy都继承自此类，提供基本的实体功能
/// </summary>
public class Entity : MonoBehaviour
{
    [Header("碰撞信息")]
    /// <summary>
    /// 地面检测点
    /// </summary>
    [SerializeField] protected Transform groundCheck;
    
    /// <summary>
    /// 地面检测距离
    /// </summary>
    [SerializeField] protected float groundCheckDistance = 1;
    
    /// <summary>
    /// 墙壁检测点
    /// </summary>
    [SerializeField] protected Transform wallCheck;
    
    /// <summary>
    /// 墙壁检测距离
    /// </summary>
    [SerializeField] protected float wallCheckDistance = 0.6f;
    
    /// <summary>
    /// 什么是地面的层掩码
    /// </summary>
    [SerializeField] protected LayerMask whatIsGround;
    [Space]
    
    /// <summary>
    /// 攻击检测点
    /// </summary>
    public Transform attackCheck;
    
    /// <summary>
    /// 攻击检测半径
    /// </summary>
    public float attackCheckRadius = 1.2f;

    [Header("击退信息")]
    /// <summary>
    /// 击退移动向量
    /// </summary>
    public Vector2 knockbackMovement = new Vector2(5, 3);
    
    /// <summary>
    /// 随机击退移动偏移范围
    /// </summary>
    public Vector2 randomKnockbackMovementOffsetRange;
    
    /// <summary>
    /// 击退持续时间
    /// </summary>
    [SerializeField] protected float knockbackDuration = 0.2f;
    
    /// <summary>
    /// 是否被击退
    /// </summary>
    public bool isKnockbacked { get; set; }

    /// <summary>
    /// 面朝方向（1为右，-1为左）
    /// </summary>
    public int facingDirection { get; private set; } = 1;
    
    /// <summary>
    /// 是否面朝右
    /// </summary>
    protected bool facingRight = true;

    #region 组件
    /// <summary>
    /// 精灵渲染器
    /// </summary>
    public SpriteRenderer sr { get; private set; }
    
    /// <summary>
    /// 动画控制器
    /// </summary>
    public Animator anim { get; private set; }
    
    /// <summary>
    /// 刚体组件
    /// </summary>
    public Rigidbody2D rb { get; private set; }
    //public EntityFX fx { get; private set; }
    
    /// <summary>
    /// 角色属性
    /// </summary>
    public CharacterStats stats { get; private set; }
    
    /// <summary>
    /// 胶囊碰撞器
    /// </summary>
    public CapsuleCollider2D cd { get; private set; }
    #endregion

    /// <summary>
    /// 翻转时的回调事件
    /// </summary>
    public System.Action onFlipped;

    /// <summary>
    /// 唤醒时初始化组件
    /// </summary>
    protected virtual void Awake()
    {
        // 获取精灵渲染器
        sr = GetComponentInChildren<SpriteRenderer>();
        //fx = GetComponent<EntityFX>();
        // 获取动画控制器
        anim = GetComponentInChildren<Animator>();
        // 获取刚体组件
        rb = GetComponent<Rigidbody2D>();
        // 获取角色属性
        stats = GetComponent<CharacterStats>();
        // 获取胶囊碰撞器
        cd = GetComponent<CapsuleCollider2D>();
    }

    /// <summary>
    /// 开始时的初始化
    /// </summary>
    protected virtual void Start()
    {

    }
    
    /// <summary>
    /// 每帧更新
    /// </summary>
    protected virtual void Update()
    {

    }

    /// <summary>
    /// 伤害闪烁效果
    /// </summary>
    public virtual void DamageFlashEffect()
    {
        //fx.StartCoroutine("FlashFX");

    }

    /// <summary>
    /// 伤害击退效果
    /// </summary>
    /// <param name="_attacker">攻击者</param>
    /// <param name="_attackee">被攻击者</param>
    public virtual void DamageKnockbackEffect(Transform _attacker, Transform _attackee)
    {
        // 计算击退方向
        float _knockbackDirection = CalculateKnockbackDirection(_attacker, _attackee);

        // 启动击退协程
        StartCoroutine(HitKnockback(_knockbackDirection));
    }

    /// <summary>
    /// 击退协程
    /// </summary>
    /// <param name="_knockbackDirection">击退方向</param>
    /// <returns>协程迭代器</returns>
    protected virtual IEnumerator HitKnockback(float _knockbackDirection)
    {
        // 设置被击退标志
        isKnockbacked = true;

        // 计算随机偏移
        float xOffset = Random.Range(0, randomKnockbackMovementOffsetRange.x);
        float yOffset = Random.Range(0, randomKnockbackMovementOffsetRange.y);

        // 设置击退速度
        rb.velocity = new Vector2((knockbackMovement.x + xOffset) * _knockbackDirection, knockbackMovement.y + yOffset);
        //yield return new WaitForSeconds(0.1f);
        //rb.velocity = Vector2.zero;

        // 等待击退持续时间
        yield return new WaitForSeconds(knockbackDuration);

        // 恢复水平速度为0
        rb.velocity = new Vector2(0, rb.velocity.y);

        // 清除被击退标志
        isKnockbacked = false;
    }

    /// <summary>
    /// 计算击退方向
    /// </summary>
    /// <param name="_attacker">攻击者</param>
    /// <param name="_attackee">被攻击者</param>
    /// <returns>击退方向（1为右，-1为左）</returns>
    public virtual float CalculateKnockbackDirection(Transform _attacker, Transform _attackee)
    {
        float _knockbackDirection = 0;

        // 如果攻击者在被攻击者左侧，向右击退
        if (_attacker.position.x < _attackee.position.x)
        {
            _knockbackDirection = 1;
        }
        // 如果攻击者在被攻击者右侧，向左击退
        else if (_attacker.position.x > _attackee.position.x)
        {
            _knockbackDirection = -1;
        }

        return _knockbackDirection;
    }

    /// <summary>
    /// 设置击退移动向量
    /// </summary>
    /// <param name="_knockbackMovement">击退移动向量</param>
    public virtual void SetupKnockbackMovement(Vector2 _knockbackMovement)
    {
        knockbackMovement = _knockbackMovement;
    }

    /// <summary>
    /// 设置零击退移动
    /// </summary>
    public virtual void SetupZeroKnockbackMovement()
    {

    }

    #region 速度控制
    /// <summary>
    /// 设置速度
    /// </summary>
    /// <param name="_xVelocity">X轴速度</param>
    /// <param name="_yVelocity">Y轴速度</param>
    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {
        // 如果被击退，不设置速度
        if (isKnockbacked)
        {
            return;
        }

        // 设置刚体速度
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        // 控制翻转
        FlipController(_xVelocity);
    }

    /// <summary>
    /// 设置零速度
    /// </summary>
    public virtual void SetZeroVelocity()
    {
        // 如果被击退，不设置速度
        if (isKnockbacked)
        {
            return;
        }

        // 设置速度为0
        rb.velocity = new Vector2(0, 0);
    }
    #endregion

    #region 碰撞检测
    /// <summary>
    /// 绘制调试用的Gizmos
    /// </summary>
    protected virtual void OnDrawGizmos()
    {
        // 绘制地面检测线
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        // 绘制墙壁检测线
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y));
        // 绘制攻击检测球
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    /// <summary>
    /// 检测是否在地面
    /// </summary>
    /// <returns>如果在地面返回true，否则返回false</returns>
    public virtual bool IsGroundDetected()
    {
        // 从地面检测点向下发射射线
        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    /// <summary>
    /// 检测是否碰到墙
    /// </summary>
    /// <returns>如果碰到墙返回true，否则返回false</returns>
    public virtual bool IsWallDetected()
    {
        // 从墙壁检测点向面朝方向发射射线
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);

    }
    #endregion

    #region 翻转控制
    /// <summary>
    /// 翻转面朝方向
    /// </summary>
    public virtual void Flip()
    {
        // 反转面朝方向
        facingDirection = -facingDirection;
        // 反转面朝右标志
        facingRight = !facingRight;
        // 旋转180度
        transform.Rotate(0, 180, 0);

        // 如果有翻转回调，调用回调
        if (onFlipped != null)
        {
            onFlipped();
        }
    }

    /// <summary>
    /// 翻转控制器
    /// </summary>
    /// <param name="_x">X轴速度</param>
    public virtual void FlipController(float _x)
    {
        // 如果向右移动且面朝左，翻转
        if (_x > 0 && !facingRight)
        {
            Flip();
        }
        // 如果向左移动且面朝右，翻转
        else if (_x < 0 && facingRight)
        {
            Flip();
        }
    }

    /// <summary>
    /// 设置默认面朝方向
    /// </summary>
    /// <param name="_facingDirection">面朝方向（1为右，-1为左）</param>
    public void SetupDefaultFacingDirection(int _facingDirection)
    {
        facingDirection = _facingDirection;

        // 如果面朝左，设置面朝右标志为false
        if (facingDirection == -1)
        {
            facingRight = false;
        }
    }
    #endregion

    /// <summary>
    /// 死亡方法
    /// </summary>
    public virtual void Die()
    {
        
    }

    /// <summary>
    /// 按百分比减慢速度
    /// </summary>
    /// <param name="_percentage">减慢的百分比（0-1）</param>
    /// <param name="_duration">减慢持续时间（秒）</param>
    public virtual void SlowSpeedBy(float _percentage, float _duration)
    {

    }

    /// <summary>
    /// 恢复默认速度
    /// </summary>
    protected virtual void ReturnDefaultSpeed()
    {
        // 恢复动画速度为1
        anim.speed = 1;
    }

}
