using UnityEngine;

/// <summary>
/// 技能管理器类，负责管理游戏中所有的技能
/// </summary>
public class SkillManager : MonoBehaviour
{
    /// <summary>
    /// 技能管理器的单例实例
    /// </summary>
    public static SkillManager instance;

    /// <summary>
    /// 冲刺技能
    /// </summary>
    public DashSkill dash { get; private set; }
    
    /// <summary>
    /// 克隆技能
    /// </summary>
    public CloneSkill clone { get; private set; }
    
    /// <summary>
    /// 剑技能
    /// </summary>
    public SwordSkill sword { get; private set; }
    
    /// <summary>
    /// 黑洞技能
    /// </summary>
    public BlackholeSkill blackhole { get; private set; }
    
    /// <summary>
    /// 水晶技能
    /// </summary>
    public CrystalSkill crystal { get; private set; }
    
    /// <summary>
    /// 招架技能
    /// </summary>
    public ParrySkill parry { get; private set; }
    
    /// <summary>
    /// 闪避技能
    /// </summary>
    public DodgeSkill dodge { get; private set; }

    /// <summary>
    /// 唤醒时初始化单例和技能组件
    /// </summary>
    private void Awake()
    {
        // 实现单例模式
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // 如果已经存在实例，则销毁当前对象
            Destroy(gameObject);
        }

        // 获取各个技能组件
        dash = GetComponent<DashSkill>();
        clone = GetComponent<CloneSkill>();
        sword = GetComponent<SwordSkill>();
        blackhole = GetComponent<BlackholeSkill>();
        crystal = GetComponent<CrystalSkill>();
        parry = GetComponent<ParrySkill>();
        dodge = GetComponent<DodgeSkill>();
    }
}
