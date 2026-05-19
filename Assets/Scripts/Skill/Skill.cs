using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能基类，所有技能都继承自此类
/// </summary>
public class Skill : MonoBehaviour
{
    /// <summary>
    /// 玩家引用
    /// </summary>
    protected Player player;

    /// <summary>
    /// 技能冷却时间
    /// </summary>
    public float cooldown;
    
    /// <summary>
    /// 技能冷却计时器
    /// </summary>
    protected float cooldownTimer;
    
    /// <summary>
    /// 技能最后使用时间
    /// </summary>
    public float skillLastUseTime { get; protected set; } = 0;

    /// <summary>
    /// 开始时初始化玩家引用和技能解锁状态
    /// </summary>
    protected virtual void Start()
    {
        // 获取玩家实例
        player = PlayerManager.instance.player;

        // 检查从保存数据中解锁的技能
        CheckUnlockFromSave();
    }

    /// <summary>
    /// 每帧更新，处理技能冷却
    /// </summary>
    protected virtual void Update()
    {
        // 减少冷却计时器
        cooldownTimer -= Time.deltaTime;
    }

    /// <summary>
    /// 检查从保存数据中解锁的技能
    /// </summary>
    protected virtual void CheckUnlockFromSave()
    {

    }

    /// <summary>
    /// 检查技能是否准备好使用
    /// </summary>
    /// <returns>如果技能准备好返回true，否则返回false</returns>
    public virtual bool SkillIsReadyToUse()
    {
        // 如果冷却计时器小于0，技能可以使用
        if (cooldownTimer < 0)
        {
            return true;
        }
        else
        {
            // 显示技能冷却提示（英文）
            if (LanguageManager.instance.localeID == 0)
            {
                player.fx.CreatePopUpText("Skill is in cooldown");

            }
            // 显示技能冷却提示（中文）
            else if (LanguageManager.instance.localeID == 1)
            {
                player.fx.CreatePopUpText("技能冷却中");
            }
            return false;
        }
    }

    /// <summary>
    /// 如果技能可用则使用技能
    /// </summary>
    /// <returns>如果成功使用技能返回true，否则返回false</returns>
    public virtual bool UseSkillIfAvailable()
    {
        // 如果冷却计时器小于0，可以使用技能
        if (cooldownTimer < 0)
        {
            // 使用技能
            UseSkill();
            // 重置冷却计时器
            cooldownTimer = cooldown;
            return true;
        }

        // 显示技能冷却提示（英文）
        if (LanguageManager.instance.localeID == 0)
        {
            player.fx.CreatePopUpText("Skill is in cooldown");

        }
        // 显示技能冷却提示（中文）
        else if (LanguageManager.instance.localeID == 1)
        {
            player.fx.CreatePopUpText("技能冷却中");
        }
        return false;
    }

    /// <summary>
    /// 使用技能的方法，由子类重写
    /// </summary>
    public virtual void UseSkill()
    {

    }

    /// <summary>
    /// 查找最近的敌人
    /// </summary>
    /// <param name="_searchCenter">搜索中心点</param>
    /// <returns>最近的敌人Transform</returns>
    protected virtual Transform FindClosestEnemy(Transform _searchCenter)
    {
        Transform closestEnemy = null;

        // 在范围内找到所有的碰撞体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_searchCenter.position, 12);

        float closestDistanceToEnemy = Mathf.Infinity;

        // 找到最近的敌人
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float currentDistanceToEnemy = Vector2.Distance(_searchCenter.position, hit.transform.position);

                if (currentDistanceToEnemy < closestDistanceToEnemy)
                {
                    closestDistanceToEnemy = currentDistanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }

    /// <summary>
    /// 随机选择一个敌人
    /// </summary>
    /// <param name="_searchCenter">搜索中心点</param>
    /// <param name="_targetSearchRadius">目标搜索半径</param>
    /// <returns>随机选择的敌人Transform</returns>
    protected virtual Transform ChooseRandomEnemy(Transform _searchCenter, float _targetSearchRadius)
    {
        Transform targetEnemy = null;

        // 在范围内找到所有的碰撞体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _targetSearchRadius);

        // 在搜索范围内找到所有的敌人
        List<Transform> enemies = new List<Transform>();

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                enemies.Add(hit.transform);
            }
        }

        // 如果成功找到范围内的敌人，随机选择一个作为目标敌人
        if (enemies.Count > 0)
        {
            targetEnemy = enemies[Random.Range(0, enemies.Count)];
        }

        return targetEnemy;
    }
}
