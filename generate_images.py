#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
使用Matplotlib生成系统架构图的PNG版本
"""

import matplotlib.pyplot as plt
import matplotlib.patches as patches
from matplotlib.patches import FancyBboxPatch, FancyArrowPatch
import os

# 设置中文字体
plt.rcParams['font.sans-serif'] = ['SimHei', 'DejaVu Sans']
plt.rcParams['axes.unicode_minus'] = False

output_dir = "D:\\论文优化文档"
os.makedirs(output_dir, exist_ok=True)

def draw_architecture_diagram():
    """绘制系统架构图"""
    fig, ax = plt.subplots(1, 1, figsize=(14, 8))
    ax.set_xlim(0, 14)
    ax.set_ylim(0, 8)
    ax.axis('off')
    
    # 标题
    ax.text(7, 7.5, '游戏系统整体架构', fontsize=20, fontweight='bold', ha='center')
    
    # GameManager
    gm_box = FancyBboxPatch((5.5, 5.5), 3, 1, boxstyle="round,pad=0.1", 
                            edgecolor='black', facecolor='lightgreen', linewidth=2)
    ax.add_patch(gm_box)
    ax.text(7, 6, 'GameManager\n游戏管理器', fontsize=11, ha='center', va='center', fontweight='bold')
    
    # 五大系统
    systems = [
        (1, 3.5, 'Player System\n玩家系统'),
        (3.5, 3.5, 'Enemy System\n敌人系统'),
        (6, 3.5, 'Skill System\n技能系统'),
        (8.5, 3.5, 'Item System\n物品系统'),
        (11, 3.5, 'Manager System\n管理器体系'),
    ]
    
    for x, y, text in systems:
        box = FancyBboxPatch((x-1, y-0.4), 2, 0.8, boxstyle="round,pad=0.05",
                            edgecolor='black', facecolor='lightblue', linewidth=1.5)
        ax.add_patch(box)
        ax.text(x, y, text, fontsize=10, ha='center', va='center')
        
        # 连接线
        arrow = FancyArrowPatch((7, 5.5), (x, y+0.4), 
                              arrowstyle='->', mutation_scale=20, 
                              color='black', linewidth=1.5)
        ax.add_patch(arrow)
    
    # 子系统细节
    subtext = "• Player: 玩家控制、状态管理\n• Enemy: AI决策、状态机\n• Skill: 技能冷却、效果\n• Item: 背包、装备\n• Manager: 音频、相机、保存"
    ax.text(7, 1.5, subtext, fontsize=9, ha='center', va='top', 
            bbox=dict(boxstyle='round', facecolor='wheat', alpha=0.3))
    
    plt.tight_layout()
    plt.savefig(f"{output_dir}\\1-系统架构图.png", dpi=150, bbox_inches='tight')
    print("✓ 已生成：1-系统架构图.png")
    plt.close()

def draw_player_state_machine():
    """绘制玩家状态机图"""
    fig, ax = plt.subplots(1, 1, figsize=(14, 8))
    ax.set_xlim(0, 14)
    ax.set_ylim(0, 8)
    ax.axis('off')
    
    # 标题
    ax.text(7, 7.5, '玩家状态机', fontsize=20, fontweight='bold', ha='center')
    
    # 状态节点
    states = [
        (2, 5.5, 'Idle\n待机', 'lightyellow'),
        (4, 5.5, 'Move\n移动', 'lightyellow'),
        (6, 5.5, 'Jump\n跳跃', 'lightyellow'),
        (8, 5.5, 'Air\n空中', 'lightyellow'),
        (10, 5.5, 'Dash\n冲刺', 'lightpink'),
        (12, 5.5, 'Attack\n攻击', 'lightpink'),
    ]
    
    node_patches = []
    for x, y, text, color in states:
        circle = patches.Circle((x, y), 0.6, edgecolor='black', facecolor=color, linewidth=2)
        ax.add_patch(circle)
        ax.text(x, y, text, fontsize=9, ha='center', va='center', fontweight='bold')
        node_patches.append((x, y))
    
    # 状态转换箭头
    transitions = [
        (0, 1, 'xInput'),
        (1, 2, 'Jump'),
        (2, 3, '跳起'),
        (0, 4, 'Dash'),
        (1, 5, '攻击'),
    ]
    
    for start_idx, end_idx, label in transitions:
        x1, y1 = node_patches[start_idx]
        x2, y2 = node_patches[end_idx]
        
        # 计算箭头起点和终点
        dx = x2 - x1
        dy = y2 - y1
        distance = (dx**2 + dy**2)**0.5
        
        start_x = x1 + (dx/distance)*0.6
        start_y = y1 + (dy/distance)*0.6
        end_x = x2 - (dx/distance)*0.6
        end_y = y2 - (dy/distance)*0.6
        
        arrow = FancyArrowPatch((start_x, start_y), (end_x, end_y),
                              arrowstyle='->', mutation_scale=15,
                              color='darkblue', linewidth=1.5)
        ax.add_patch(arrow)
        
        # 添加标签
        mid_x, mid_y = (x1 + x2) / 2, (y1 + y2) / 2
        ax.text(mid_x, mid_y + 0.3, label, fontsize=8, ha='center',
               bbox=dict(boxstyle='round', facecolor='white', alpha=0.8))
    
    # 说明文字
    note = "状态机采用FSM模式，支持复杂的状态转换\n每个状态都有Enter、Update、Exit三个生命周期方法"
    ax.text(7, 1.5, note, fontsize=10, ha='center', va='top',
           bbox=dict(boxstyle='round', facecolor='lightyellow', alpha=0.5))
    
    plt.tight_layout()
    plt.savefig(f"{output_dir}\\2-玩家状态机.png", dpi=150, bbox_inches='tight')
    print("✓ 已生成：2-玩家状态机.png")
    plt.close()

def draw_enemy_ai_flow():
    """绘制敌人AI决策流程"""
    fig, ax = plt.subplots(1, 1, figsize=(12, 10))
    ax.set_xlim(0, 12)
    ax.set_ylim(0, 10)
    ax.axis('off')
    
    # 标题
    ax.text(6, 9.5, '敌人AI决策流程', fontsize=20, fontweight='bold', ha='center')
    
    # 菱形节点（决策）
    def draw_diamond(x, y, width, height, text, color='lightyellow'):
        diamond = patches.Polygon([(x, y+height/2), (x+width/2, y), 
                                  (x, y-height/2), (x-width/2, y)],
                                 closed=True, edgecolor='black', 
                                 facecolor=color, linewidth=1.5)
        ax.add_patch(diamond)
        ax.text(x, y, text, fontsize=9, ha='center', va='center', fontweight='bold')
    
    # 矩形节点（状态）
    def draw_rect(x, y, width, height, text, color='lightblue'):
        rect = FancyBboxPatch((x-width/2, y-height/2), width, height,
                             boxstyle="round,pad=0.05", edgecolor='black',
                             facecolor=color, linewidth=1.5)
        ax.add_patch(rect)
        ax.text(x, y, text, fontsize=9, ha='center', va='center', fontweight='bold')
    
    # 流程图
    draw_diamond(6, 8, 1.5, 0.8, '玩家在\n视野内?', 'wheat')
    
    draw_rect(2, 6.5, 1.2, 0.6, '待机', 'lightcoral')
    draw_rect(10, 6.5, 1.2, 0.6, '待机', 'lightcoral')
    
    draw_rect(6, 6.5, 1.2, 0.6, '追踪', 'lightblue')
    
    draw_diamond(6, 5, 1.5, 0.8, '攻击范围\n且冷却完成?', 'wheat')
    
    draw_rect(6, 3.5, 1.2, 0.6, '攻击', 'lightgreen')
    
    # 箭头
    arrow1 = FancyArrowPatch((5.2, 7.6), (2.6, 6.8), arrowstyle='->', 
                            mutation_scale=15, color='black')
    ax.add_patch(arrow1)
    ax.text(3.5, 7.3, 'No', fontsize=8, fontweight='bold')
    
    arrow2 = FancyArrowPatch((6.8, 7.6), (9.4, 6.8), arrowstyle='->', 
                            mutation_scale=15, color='black')
    ax.add_patch(arrow2)
    ax.text(8.5, 7.3, 'Yes', fontsize=8, fontweight='bold')
    
    arrow3 = FancyArrowPatch((6, 6.2), (6, 5.4), arrowstyle='->', 
                            mutation_scale=15, color='black')
    ax.add_patch(arrow3)
    
    arrow4 = FancyArrowPatch((5.2, 4.6), (2.6, 6.2), arrowstyle='->', 
                            mutation_scale=15, color='black')
    ax.add_patch(arrow4)
    ax.text(3.5, 5.2, 'No', fontsize=8, fontweight='bold')
    
    arrow5 = FancyArrowPatch((6.8, 4.6), (6.6, 3.8), arrowstyle='->', 
                            mutation_scale=15, color='black')
    ax.add_patch(arrow5)
    ax.text(6.8, 4.2, 'Yes', fontsize=8, fontweight='bold')
    
    # 说明
    note = "敌人AI采用分层决策机制：\n1. 检测玩家（视野+听觉）→ 2. 追踪移动 → 3. 判断攻击条件 → 4. 执行攻击"
    ax.text(6, 1.5, note, fontsize=10, ha='center', va='top',
           bbox=dict(boxstyle='round', facecolor='lightyellow', alpha=0.5))
    
    plt.tight_layout()
    plt.savefig(f"{output_dir}\\3-敌人AI决策.png", dpi=150, bbox_inches='tight')
    print("✓ 已生成：3-敌人AI决策.png")
    plt.close()

def draw_damage_calculation():
    """绘制伤害计算流程"""
    fig, ax = plt.subplots(1, 1, figsize=(10, 12))
    ax.set_xlim(0, 10)
    ax.set_ylim(0, 12)
    ax.axis('off')
    
    # 标题
    ax.text(5, 11.5, '伤害计算流程', fontsize=20, fontweight='bold', ha='center')
    
    # 流程框
    steps = [
        (5, 10.5, '① 攻击命中', 'lightgreen'),
        (5, 9.5, '② 检查无敌', 'wheat'),
        (5, 8.5, '③ 检查闪避', 'wheat'),
        (5, 7.5, '④ 计算基础伤害\n= damage + strength', 'lightblue'),
        (5, 6.3, '⑤ 检查暴击', 'wheat'),
        (5, 5.3, '⑥ 计算护甲减伤', 'lightblue'),
        (5, 4.3, '⑦ 检查易伤状态', 'lightblue'),
        (5, 3.3, '⑧ 应用状态效果\n(燃烧/冰冻/麻痹)', 'lightblue'),
        (5, 2.1, '⑨ 目标受伤\nHP -= finalDamage', 'lightcoral'),
    ]
    
    for x, y, text, color in steps:
        rect = FancyBboxPatch((x-1.5, y-0.35), 3, 0.7, boxstyle="round,pad=0.05",
                             edgecolor='black', facecolor=color, linewidth=1.5)
        ax.add_patch(rect)
        ax.text(x, y, text, fontsize=9, ha='center', va='center', fontweight='bold')
    
    # 连接箭头
    for i in range(len(steps)-1):
        y1 = steps[i][1] - 0.35
        y2 = steps[i+1][1] + 0.35
        arrow = FancyArrowPatch((5, y1), (5, y2), arrowstyle='->', 
                              mutation_scale=15, color='black', linewidth=1.5)
        ax.add_patch(arrow)
    
    # 公式说明
    formula = "伤害公式：\n最终伤害 = (基础伤害 × 暴击系数 - 护甲减伤) × 易伤系数"
    ax.text(5, 0.5, formula, fontsize=10, ha='center', va='top',
           bbox=dict(boxstyle='round', facecolor='lightyellow', alpha=0.6))
    
    plt.tight_layout()
    plt.savefig(f"{output_dir}\\4-伤害计算流程.png", dpi=150, bbox_inches='tight')
    print("✓ 已生成：4-伤害计算流程.png")
    plt.close()

def draw_skill_system():
    """绘制技能系统架构"""
    fig, ax = plt.subplots(1, 1, figsize=(14, 8))
    ax.set_xlim(0, 14)
    ax.set_ylim(0, 8)
    ax.axis('off')
    
    # 标题
    ax.text(7, 7.5, '技能系统架构', fontsize=20, fontweight='bold', ha='center')
    
    # SkillManager
    gm_box = FancyBboxPatch((5.5, 5.5), 3, 1, boxstyle="round,pad=0.1",
                           edgecolor='black', facecolor='lightgreen', linewidth=2)
    ax.add_patch(gm_box)
    ax.text(7, 6, 'SkillManager\n技能管理器', fontsize=11, ha='center', va='center', fontweight='bold')
    
    # 7种技能
    skills = [
        (1, 3.5, 'DashSkill\n冲刺'),
        (2.5, 3.5, 'CloneSkill\n分身'),
        (4, 3.5, 'SwordSkill\n剑技'),
        (5.5, 3.5, 'BlackholeSkill\n黑洞'),
        (7, 3.5, 'CrystalSkill\n水晶'),
        (8.5, 3.5, 'ParrySkill\n招架'),
        (10, 3.5, 'DodgeSkill\n闪避'),
    ]
    
    for x, y, text in skills:
        box = FancyBboxPatch((x-0.6, y-0.35), 1.2, 0.7, boxstyle="round,pad=0.05",
                            edgecolor='black', facecolor='lightblue', linewidth=1.5)
        ax.add_patch(box)
        ax.text(x, y, text, fontsize=8, ha='center', va='center')
        
        # 连接线
        arrow = FancyArrowPatch((7, 5.5), (x, y+0.35),
                              arrowstyle='->', mutation_scale=15,
                              color='black', linewidth=1.5)
        ax.add_patch(arrow)
    
    # 技能生命周期
    lifecycle = "技能生命周期：\n检查冷却 → 检查解锁 → 检查条件 → 执行技能 → 重置冷却"
    ax.text(7, 1.5, lifecycle, fontsize=10, ha='center', va='top',
           bbox=dict(boxstyle='round', facecolor='wheat', alpha=0.3))
    
    plt.tight_layout()
    plt.savefig(f"{output_dir}\\5-技能系统架构.png", dpi=150, bbox_inches='tight')
    print("✓ 已生成：5-技能系统架构.png")
    plt.close()

def draw_item_system():
    """绘制物品系统流程"""
    fig, ax = plt.subplots(1, 1, figsize=(12, 10))
    ax.set_xlim(0, 12)
    ax.set_ylim(0, 10)
    ax.axis('off')
    
    # 标题
    ax.text(6, 9.5, '物品系统流程', fontsize=20, fontweight='bold', ha='center')
    
    # 流程
    nodes = [
        (6, 8.5, '敌人掉落', 'wheat'),
        (6, 7.5, '地面物品', 'wheat'),
        (6, 6.5, '玩家拾取', 'lightblue'),
        (3, 5, '材料仓库', 'lightyellow'),
        (9, 5, '装备背包', 'lightyellow'),
        (9, 3.5, '选择装备', 'lightgreen'),
        (9, 2.5, '添加属性修正', 'lightblue'),
        (9, 1.5, '玩家属性更新', 'lightcoral'),
    ]
    
    for x, y, text, color in nodes:
        if y >= 5:
            # 矩形
            rect = FancyBboxPatch((x-0.8, y-0.3), 1.6, 0.6,
                                 boxstyle="round,pad=0.05", edgecolor='black',
                                 facecolor=color, linewidth=1.5)
            ax.add_patch(rect)
        else:
            # 矩形
            rect = FancyBboxPatch((x-0.8, y-0.3), 1.6, 0.6,
                                 boxstyle="round,pad=0.05", edgecolor='black',
                                 facecolor=color, linewidth=1.5)
            ax.add_patch(rect)
        ax.text(x, y, text, fontsize=9, ha='center', va='center', fontweight='bold')
    
    # 箭头
    arrows = [
        ((6, 8.2), (6, 7.8)),
        ((6, 7.2), (6, 6.8)),
        ((5.2, 6.2), (3.8, 5.3)),
        ((6.8, 6.2), (8.2, 5.3)),
        ((9, 4.7), (9, 3.8)),
        ((9, 3.2), (9, 2.8)),
        ((9, 2.2), (9, 1.8)),
    ]
    
    for start, end in arrows:
        arrow = FancyArrowPatch(start, end, arrowstyle='->', 
                              mutation_scale=15, color='black', linewidth=1.5)
        ax.add_patch(arrow)
    
    # 说明
    note = "物品系统采用背包+装备的二分法设计\n支持装备切换、属性动态调整、材料合成等功能"
    ax.text(6, 0.3, note, fontsize=10, ha='center', va='top',
           bbox=dict(boxstyle='round', facecolor='lightyellow', alpha=0.5))
    
    plt.tight_layout()
    plt.savefig(f"{output_dir}\\6-物品系统流程.png", dpi=150, bbox_inches='tight')
    print("✓ 已生成：6-物品系统流程.png")
    plt.close()

# 生成所有图表
print("开始生成PNG图表...\n")

draw_architecture_diagram()
draw_player_state_machine()
draw_enemy_ai_flow()
draw_damage_calculation()
draw_skill_system()
draw_item_system()

print(f"\n所有PNG图表已保存到：{output_dir}")
