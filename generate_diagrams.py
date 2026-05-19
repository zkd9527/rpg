#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
使用graphviz生成系统架构图
"""

import subprocess
import os

# 确保输出目录存在
output_dir = "D:\\论文优化文档"
os.makedirs(output_dir, exist_ok=True)

# 1. 整体系统架构图
architecture_dot = """
digraph GameArchitecture {
    rankdir=TB;
    node [shape=box, style=filled, fillcolor=lightblue, fontname="Microsoft YaHei"];
    edge [fontname="Microsoft YaHei"];
    
    GameManager [fillcolor=lightgreen, label="GameManager\n游戏管理器"];
    
    PlayerSystem [label="Player System\n玩家系统"];
    EnemySystem [label="Enemy System\n敌人系统"];
    SkillSystem [label="Skill System\n技能系统"];
    ItemSystem [label="Item System\n物品系统"];
    ManagerSystem [label="Manager System\n管理器体系"];
    
    Player [label="Player\n玩家主类"];
    PlayerState [label="PlayerState\n玩家状态"];
    PlayerStats [label="CharacterStats\n玩家属性"];
    
    Enemy [label="Enemy\n敌人基类"];
    EnemyState [label="EnemyState\n敌人状态"];
    EnemyAI [label="EnemyAI\n敌人AI"];
    
    Skill [label="Skill\n技能基类"];
    DashSkill [label="DashSkill"];
    SwordSkill [label="SwordSkill"];
    BlackholeSkill [label="BlackholeSkill"];
    
    Inventory [label="Inventory\n背包系统"];
    ItemData [label="ItemData\n物品数据"];
    Equipment [label="Equipment\n装备"];
    
    Audio [label="AudioManager"];
    Camera [label="CameraManager"];
    Save [label="SaveManager"];
    
    GameManager -> PlayerSystem;
    GameManager -> EnemySystem;
    GameManager -> SkillSystem;
    GameManager -> ItemSystem;
    GameManager -> ManagerSystem;
    
    PlayerSystem -> Player;
    PlayerSystem -> PlayerState;
    PlayerSystem -> PlayerStats;
    
    EnemySystem -> Enemy;
    EnemySystem -> EnemyState;
    EnemySystem -> EnemyAI;
    
    SkillSystem -> Skill;
    Skill -> DashSkill;
    Skill -> SwordSkill;
    Skill -> BlackholeSkill;
    
    ItemSystem -> Inventory;
    ItemSystem -> ItemData;
    ItemData -> Equipment;
    
    ManagerSystem -> Audio;
    ManagerSystem -> Camera;
    ManagerSystem -> Save;
}
"""

# 2. 玩家状态机图
player_state_dot = """
digraph PlayerStateMachine {
    rankdir=LR;
    node [shape=circle, style=filled, fillcolor=lightyellow, fontname="Microsoft YaHei"];
    edge [fontname="Microsoft YaHei"];
    
    Idle [label="Idle\n待机"];
    Move [label="Move\n移动"];
    Jump [label="Jump\n跳跃"];
    Air [label="Air\n空中"];
    Dash [label="Dash\n冲刺"];
    Attack [label="Attack\n攻击"];
    Slide [label="Slide\n滑墙"];
    Death [label="Death\n死亡"];
    
    Idle -> Move [label="xInput != 0"];
    Move -> Idle [label="xInput == 0"];
    
    Idle -> Jump [label="Jump键"];
    Move -> Jump [label="Jump键"];
    
    Jump -> Air [label="跳跃完成"];
    Air -> Idle [label="着地"];
    Air -> Move [label="着地+移动"];
    
    Idle -> Dash [label="Dash技能"];
    Dash -> Idle [label="冲刺结束"];
    
    Move -> Attack [label="攻击键"];
    Attack -> Move [label="攻击结束"];
    
    Move -> Slide [label="碰墙检测"];
    Slide -> Jump [label="墙跳"];
    
    Idle -> Death [label="HP <= 0"];
    Move -> Death [label="HP <= 0"];
}
"""

# 3. 敌人AI决策流程
enemy_ai_dot = """
digraph EnemyAI {
    rankdir=TB;
    node [shape=box, style=filled, fillcolor=lightcoral, fontname="Microsoft YaHei"];
    edge [fontname="Microsoft YaHei"];
    
    Start [shape=ellipse, label="敌人AI\nUpdate"];
    
    DetectPlayer [label="玩家在视野内?"];
    Idle [label="待机状态"];
    Move [label="追踪状态"];
    CanAttack [label="在攻击范围\n且冷却完成?"];
    Attack [label="攻击状态"];
    GetHit [label="受伤判定"];
    Stun [label="眩晕状态"];
    Dead [label="死亡检查"];
    
    Start -> DetectPlayer;
    
    DetectPlayer -> Move [label="Yes"];
    DetectPlayer -> Idle [label="No"];
    
    Move -> CanAttack [label="持续检测"];
    CanAttack -> Attack [label="Yes"];
    CanAttack -> Move [label="No"];
    
    Attack -> Move [label="攻击结束"];
    
    Move -> DetectPlayer [label="玩家离开\n视野"];
    
    GetHit -> Stun [label="可以眩晕"];
    Stun -> Move [label="眩晕结束"];
    
    Idle -> Dead [label="HP<=0"];
    Move -> Dead [label="HP<=0"];
    Attack -> Dead [label="HP<=0"];
}
"""

# 4. 伤害计算流程
damage_dot = """
digraph DamageCalculation {
    rankdir=TB;
    node [shape=box, style=filled, fillcolor=lightsteelblue, fontname="Microsoft YaHei"];
    edge [fontname="Microsoft YaHei"];
    
    Start [shape=ellipse, label="攻击命中"];
    
    Invincible [label="目标无敌?"];
    Evade [label="目标可闪避?"];
    CalcDamage [label="基础伤害\n= damage+strength"];
    CheckCrit [label="暴击检定"];
    ApplyCrit [label="应用暴击倍数"];
    CheckArmor [label="计算护甲减伤"];
    ApplyAilments [label="应用状态效果"];
    TakeDamage [label="目标受伤"];
    End [shape=ellipse, label="伤害结束"];
    
    Start -> Invincible;
    Invincible -> End [label="Yes"];
    Invincible -> Evade [label="No"];
    
    Evade -> End [label="Yes\n伤害=0"];
    Evade -> CalcDamage [label="No"];
    
    CalcDamage -> CheckCrit;
    CheckCrit -> ApplyCrit [label="Yes"];
    CheckCrit -> CheckArmor [label="No"];
    ApplyCrit -> CheckArmor;
    
    CheckArmor -> ApplyAilments;
    ApplyAilments -> TakeDamage;
    TakeDamage -> End;
}
"""

# 5. 物品系统流程
item_system_dot = """
digraph ItemSystem {
    rankdir=TB;
    node [shape=box, style=filled, fillcolor=lightgreen, fontname="Microsoft YaHei"];
    edge [fontname="Microsoft YaHei"];
    
    ItemDrop [label="敌人掉落\n物品"];
    ItemGround [label="地面上的\n物品对象"];
    PlayerPickup [label="玩家拾取"];
    
    Inventory [label="背包系统"];
    Equipment [label="装备"];
    Material [label="材料"];
    
    EquipItem [label="装备物品"];
    RemoveModifier [label="移除旧装备\n属性修正"];
    AddModifier [label="添加新装备\n属性修正"];
    
    PlayerStats [label="玩家属性\n更新"];
    
    ItemDrop -> ItemGround;
    ItemGround -> PlayerPickup;
    PlayerPickup -> Inventory;
    
    Inventory -> Equipment;
    Inventory -> Material;
    
    Equipment -> EquipItem;
    EquipItem -> RemoveModifier;
    RemoveModifier -> AddModifier;
    AddModifier -> PlayerStats;
}
"""

# 6. 技能系统架构
skill_system_dot = """
digraph SkillSystem {
    rankdir=TB;
    node [shape=box, style=filled, fillcolor=lightyellow, fontname="Microsoft YaHei"];
    edge [fontname="Microsoft YaHei"];
    
    SkillManager [label="SkillManager\n技能管理器"];
    
    DashSkill [label="DashSkill\n冲刺技能"];
    CloneSkill [label="CloneSkill\n分身技能"];
    SwordSkill [label="SwordSkill\n剑技"];
    BlackholeSkill [label="BlackholeSkill\n黑洞"];
    CrystalSkill [label="CrystalSkill\n水晶"];
    ParrySkill [label="ParrySkill\n招架"];
    DodgeSkill [label="DodgeSkill\n闪避"];
    
    Cooldown [label="冷却管理"];
    Unlock [label="解锁检查"];
    Execute [label="技能执行"];
    
    SkillManager -> DashSkill;
    SkillManager -> CloneSkill;
    SkillManager -> SwordSkill;
    SkillManager -> BlackholeSkill;
    SkillManager -> CrystalSkill;
    SkillManager -> ParrySkill;
    SkillManager -> DodgeSkill;
    
    DashSkill -> Cooldown;
    SwordSkill -> Cooldown;
    BlackholeSkill -> Cooldown;
    CrystalSkill -> Cooldown;
    
    Cooldown -> Unlock;
    Unlock -> Execute;
}
"""

# 生成所有图表
diagrams = {
    "系统架构图": architecture_dot,
    "玩家状态机": player_state_dot,
    "敌人AI决策": enemy_ai_dot,
    "伤害计算流程": damage_dot,
    "物品系统流程": item_system_dot,
    "技能系统架构": skill_system_dot,
}

print("开始生成系统架构图...\n")

for name, dot_content in diagrams.items():
    try:
        # 创建临时dot文件
        dot_file = f"{output_dir}\\{name}.dot"
        png_file = f"{output_dir}\\{name}.png"
        svg_file = f"{output_dir}\\{name}.svg"
        
        # 写入dot文件
        with open(dot_file, 'w', encoding='utf-8') as f:
            f.write(dot_content)
        
        # 使用graphviz生成PNG
        try:
            subprocess.run([
                'dot', '-Tpng', dot_file, '-o', png_file
            ], check=True, capture_output=True)
            print(f"✓ 已生成：{name}.png")
        except:
            print(f"⚠ PNG生成失败（可能未安装Graphviz）：{name}")
        
        # 使用graphviz生成SVG
        try:
            subprocess.run([
                'dot', '-Tsvg', dot_file, '-o', svg_file
            ], check=True, capture_output=True)
            print(f"✓ 已生成：{name}.svg")
        except:
            pass
            
    except Exception as e:
        print(f"✗ 生成 {name} 失败: {e}")

print("\n图表生成完成！")
print(f"\n所有图表已保存到：{output_dir}")
