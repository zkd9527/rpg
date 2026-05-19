#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
生成DrawIO格式的系统架构图
"""

import os
from xml.dom import minidom

output_dir = "D:\\论文优化文档"
os.makedirs(output_dir, exist_ok=True)

def create_drawio_xml(title, content_xml):
    """创建DrawIO格式的XML文件"""
    xml_template = f'''<?xml version="1.0" encoding="UTF-8"?>
<mxfile host="app.diagrams.net" modified="2026-05-19" agent="Mozilla/5.0" etag="drawio" version="14.0">
  <diagram id="diagram-{title}" name="{title}">
    <mxGraphModel dx="1200" dy="700" grid="1" gridSize="10" guides="1" tooltips="1" connect="1" arrows="1" fold="1" page="1" pageScale="1" pageWidth="827" pageHeight="1169" math="0" shadow="0">
      <root>
        <mxCell id="0"/>
        <mxCell id="1" parent="0"/>
        {content_xml}
      </root>
    </mxGraphModel>
  </diagram>
</mxfile>'''
    return xml_template

# 1. 系统架构图
architecture_xml = """
        <!-- 主标题 -->
        <mxCell id="title" value="游戏系统整体架构" style="text;html=1;fontSize=24;fontStyle=1;verticalAlign=middle" vertex="1" parent="1">
          <mxGeometry x="300" y="20" width="200" height="40" as="geometry"/>
        </mxCell>
        
        <!-- GameManager -->
        <mxCell id="gm" value="GameManager" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#90EE90;fontSize=12;fontStyle=1" vertex="1" parent="1">
          <mxGeometry x="350" y="100" width="120" height="60" as="geometry"/>
        </mxCell>
        
        <!-- 五大系统 -->
        <mxCell id="ps" value="Player System&#10;玩家系统" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#87CEEB;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="50" y="220" width="100" height="60" as="geometry"/>
        </mxCell>
        
        <mxCell id="es" value="Enemy System&#10;敌人系统" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#87CEEB;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="180" y="220" width="100" height="60" as="geometry"/>
        </mxCell>
        
        <mxCell id="ss" value="Skill System&#10;技能系统" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#87CEEB;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="310" y="220" width="100" height="60" as="geometry"/>
        </mxCell>
        
        <mxCell id="is" value="Item System&#10;物品系统" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#87CEEB;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="440" y="220" width="100" height="60" as="geometry"/>
        </mxCell>
        
        <mxCell id="ms" value="Manager System&#10;管理器体系" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#87CEEB;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="570" y="220" width="100" height="60" as="geometry"/>
        </mxCell>
        
        <!-- 连接线 -->
        <mxCell id="edge1" edge="1" parent="1" source="gm" target="ps">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        <mxCell id="edge2" edge="1" parent="1" source="gm" target="es">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        <mxCell id="edge3" edge="1" parent="1" source="gm" target="ss">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        <mxCell id="edge4" edge="1" parent="1" source="gm" target="is">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        <mxCell id="edge5" edge="1" parent="1" source="gm" target="ms">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
"""

# 2. 玩家状态机
player_state_xml = """
        <mxCell id="title" value="玩家状态机" style="text;html=1;fontSize=24;fontStyle=1;verticalAlign=middle" vertex="1" parent="1">
          <mxGeometry x="300" y="20" width="200" height="40" as="geometry"/>
        </mxCell>
        
        <!-- 状态节点 -->
        <mxCell id="idle" value="Idle&#10;待机" style="ellipse;whiteSpace=wrap;html=1;fillColor=#FFFFE0;fontSize=11;fontStyle=1" vertex="1" parent="1">
          <mxGeometry x="100" y="150" width="80" height="60" as="geometry"/>
        </mxCell>
        
        <mxCell id="move" value="Move&#10;移动" style="ellipse;whiteSpace=wrap;html=1;fillColor=#FFFFE0;fontSize=11;fontStyle=1" vertex="1" parent="1">
          <mxGeometry x="250" y="150" width="80" height="60" as="geometry"/>
        </mxCell>
        
        <mxCell id="jump" value="Jump&#10;跳跃" style="ellipse;whiteSpace=wrap;html=1;fillColor=#FFFFE0;fontSize=11;fontStyle=1" vertex="1" parent="1">
          <mxGeometry x="400" y="150" width="80" height="60" as="geometry"/>
        </mxCell>
        
        <mxCell id="air" value="Air&#10;空中" style="ellipse;whiteSpace=wrap;html=1;fillColor=#FFFFE0;fontSize=11;fontStyle=1" vertex="1" parent="1">
          <mxGeometry x="550" y="150" width="80" height="60" as="geometry"/>
        </mxCell>
        
        <mxCell id="dash" value="Dash&#10;冲刺" style="ellipse;whiteSpace=wrap;html=1;fillColor=#FFB6C1;fontSize=11;fontStyle=1" vertex="1" parent="1">
          <mxGeometry x="175" y="280" width="80" height="60" as="geometry"/>
        </mxCell>
        
        <mxCell id="attack" value="Attack&#10;攻击" style="ellipse;whiteSpace=wrap;html=1;fillColor=#FFB6C1;fontSize=11;fontStyle=1" vertex="1" parent="1">
          <mxGeometry x="325" y="280" width="80" height="60" as="geometry"/>
        </mxCell>
        
        <!-- 状态转换 -->
        <mxCell id="e1" edge="1" parent="1" source="idle" target="move">
          <mxGeometry relative="1" as="geometry">
            <mxPoint as="sourcePoint" x="180" y="180"/>
            <mxPoint as="targetPoint" x="250" y="180"/>
          </mxGeometry>
        </mxCell>
        
        <mxCell id="e2" edge="1" parent="1" source="move" target="jump">
          <mxGeometry relative="1" as="geometry">
            <mxPoint as="sourcePoint" x="330" y="180"/>
            <mxPoint as="targetPoint" x="400" y="180"/>
          </mxGeometry>
        </mxCell>
        
        <mxCell id="e3" edge="1" parent="1" source="jump" target="air">
          <mxGeometry relative="1" as="geometry">
            <mxPoint as="sourcePoint" x="480" y="180"/>
            <mxPoint as="targetPoint" x="550" y="180"/>
          </mxGeometry>
        </mxCell>
        
        <mxCell id="e4" edge="1" parent="1" source="idle" target="dash">
          <mxGeometry relative="1" as="geometry">
            <mxPoint as="sourcePoint" x="140" y="210"/>
            <mxPoint as="targetPoint" x="215" y="280"/>
          </mxGeometry>
        </mxCell>
        
        <mxCell id="e5" edge="1" parent="1" source="move" target="attack">
          <mxGeometry relative="1" as="geometry">
            <mxPoint as="sourcePoint" x="290" y="210"/>
            <mxPoint as="targetPoint" x="365" y="280"/>
          </mxGeometry>
        </mxCell>
"""

# 3. 敌人AI决策流程
enemy_ai_xml = """
        <mxCell id="title" value="敌人AI决策流程" style="text;html=1;fontSize=24;fontStyle=1;verticalAlign=middle" vertex="1" parent="1">
          <mxGeometry x="300" y="20" width="200" height="40" as="geometry"/>
        </mxCell>
        
        <!-- 决策流程 -->
        <mxCell id="detect" value="玩家在视野内?" style="rhombus;whiteSpace=wrap;html=1;fillColor=#FFE4B5;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="300" y="100" width="120" height="80" as="geometry"/>
        </mxCell>
        
        <mxCell id="idle" value="待机状态" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#FFB6C1;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="520" y="120" width="100" height="50" as="geometry"/>
        </mxCell>
        
        <mxCell id="move" value="追踪移动" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#87CEEB;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="100" y="120" width="100" height="50" as="geometry"/>
        </mxCell>
        
        <mxCell id="canAttack" value="攻击范围&amp;冷却完成?" style="rhombus;whiteSpace=wrap;html=1;fillColor=#FFE4B5;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="80" y="250" width="140" height="80" as="geometry"/>
        </mxCell>
        
        <mxCell id="attack" value="攻击状态" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#90EE90;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="100" y="390" width="100" height="50" as="geometry"/>
        </mxCell>
        
        <!-- 连接 -->
        <mxCell id="e1" value="Yes" edge="1" parent="1" source="detect" target="move">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        
        <mxCell id="e2" value="No" edge="1" parent="1" source="detect" target="idle">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        
        <mxCell id="e3" edge="1" parent="1" source="move" target="canAttack">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        
        <mxCell id="e4" value="Yes" edge="1" parent="1" source="canAttack" target="attack">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        
        <mxCell id="e5" value="No" edge="1" parent="1" source="canAttack" target="move">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
"""

# 4. 伤害计算流程
damage_xml = """
        <mxCell id="title" value="伤害计算流程" style="text;html=1;fontSize=24;fontStyle=1;verticalAlign=middle" vertex="1" parent="1">
          <mxGeometry x="300" y="20" width="200" height="40" as="geometry"/>
        </mxCell>
        
        <mxCell id="start" value="攻击命中" style="ellipse;whiteSpace=wrap;html=1;fillColor=#90EE90;fontSize=11;fontStyle=1" vertex="1" parent="1">
          <mxGeometry x="350" y="60" width="100" height="50" as="geometry"/>
        </mxCell>
        
        <mxCell id="invincible" value="目标无敌?" style="rhombus;whiteSpace=wrap;html=1;fillColor=#FFE4B5;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="330" y="150" width="140" height="80" as="geometry"/>
        </mxCell>
        
        <mxCell id="evade" value="可以闪避?" style="rhombus;whiteSpace=wrap;html=1;fillColor=#FFE4B5;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="330" y="280" width="140" height="80" as="geometry"/>
        </mxCell>
        
        <mxCell id="calcDmg" value="计算基础伤害" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#87CEEB;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="330" y="410" width="140" height="50" as="geometry"/>
        </mxCell>
        
        <mxCell id="crit" value="检查暴击" style="rhombus;whiteSpace=wrap;html=1;fillColor=#FFE4B5;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="350" y="520" width="100" height="80" as="geometry"/>
        </mxCell>
        
        <mxCell id="armor" value="计算护甲减伤" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#87CEEB;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="330" y="650" width="140" height="50" as="geometry"/>
        </mxCell>
        
        <!-- 连接 -->
        <mxCell id="e1" edge="1" parent="1" source="start" target="invincible">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        
        <mxCell id="e2" value="No" edge="1" parent="1" source="invincible" target="evade">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        
        <mxCell id="e3" value="No" edge="1" parent="1" source="evade" target="calcDmg">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        
        <mxCell id="e4" edge="1" parent="1" source="calcDmg" target="crit">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        
        <mxCell id="e5" edge="1" parent="1" source="crit" target="armor">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
"""

# 5. 技能系统架构
skill_xml = """
        <mxCell id="title" value="技能系统架构" style="text;html=1;fontSize=24;fontStyle=1;verticalAlign=middle" vertex="1" parent="1">
          <mxGeometry x="300" y="20" width="200" height="40" as="geometry"/>
        </mxCell>
        
        <mxCell id="sm" value="SkillManager&#10;技能管理器" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#90EE90;fontStyle=1;fontSize=12" vertex="1" parent="1">
          <mxGeometry x="320" y="100" width="140" height="60" as="geometry"/>
        </mxCell>
        
        <mxCell id="dash" value="DashSkill&#10;冲刺" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#87CEEB;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="50" y="220" width="100" height="50" as="geometry"/>
        </mxCell>
        
        <mxCell id="sword" value="SwordSkill&#10;剑技" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#87CEEB;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="180" y="220" width="100" height="50" as="geometry"/>
        </mxCell>
        
        <mxCell id="bh" value="BlackholeSkill&#10;黑洞" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#87CEEB;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="310" y="220" width="100" height="50" as="geometry"/>
        </mxCell>
        
        <mxCell id="cry" value="CrystalSkill&#10;水晶" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#87CEEB;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="440" y="220" width="100" height="50" as="geometry"/>
        </mxCell>
        
        <mxCell id="pary" value="ParrySkill&#10;招架" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#87CEEB;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="570" y="220" width="100" height="50" as="geometry"/>
        </mxCell>
        
        <!-- 连接 -->
        <mxCell id="e1" edge="1" parent="1" source="sm" target="dash">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        <mxCell id="e2" edge="1" parent="1" source="sm" target="sword">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        <mxCell id="e3" edge="1" parent="1" source="sm" target="bh">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        <mxCell id="e4" edge="1" parent="1" source="sm" target="cry">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        <mxCell id="e5" edge="1" parent="1" source="sm" target="pary">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
"""

# 6. 物品系统
item_xml = """
        <mxCell id="title" value="物品系统流程" style="text;html=1;fontSize=24;fontStyle=1;verticalAlign=middle" vertex="1" parent="1">
          <mxGeometry x="300" y="20" width="200" height="40" as="geometry"/>
        </mxCell>
        
        <mxCell id="drop" value="敌人掉落&#10;物品" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#FFE4B5;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="50" y="120" width="100" height="60" as="geometry"/>
        </mxCell>
        
        <mxCell id="ground" value="地面物品&#10;对象" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#FFE4B5;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="200" y="120" width="100" height="60" as="geometry"/>
        </mxCell>
        
        <mxCell id="pickup" value="玩家拾取" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#87CEEB;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="350" y="120" width="100" height="60" as="geometry"/>
        </mxCell>
        
        <mxCell id="inv" value="背包系统" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#90EE90;fontStyle=1;fontSize=12" vertex="1" parent="1">
          <mxGeometry x="350" y="250" width="100" height="60" as="geometry"/>
        </mxCell>
        
        <mxCell id="equip" value="装备物品" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#FFB6C1;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="200" y="380" width="100" height="60" as="geometry"/>
        </mxCell>
        
        <mxCell id="add" value="添加属性&#10;修正" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#87CEEB;fontSize=11" vertex="1" parent="1">
          <mxGeometry x="350" y="380" width="100" height="60" as="geometry"/>
        </mxCell>
        
        <mxCell id="stats" value="玩家属性&#10;更新" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#90EE90;fontSize=11;fontStyle=1" vertex="1" parent="1">
          <mxGeometry x="500" y="380" width="100" height="60" as="geometry"/>
        </mxCell>
        
        <!-- 连接 -->
        <mxCell id="e1" edge="1" parent="1" source="drop" target="ground">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        <mxCell id="e2" edge="1" parent="1" source="ground" target="pickup">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        <mxCell id="e3" edge="1" parent="1" source="pickup" target="inv">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        <mxCell id="e4" edge="1" parent="1" source="inv" target="equip">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        <mxCell id="e5" edge="1" parent="1" source="equip" target="add">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        <mxCell id="e6" edge="1" parent="1" source="add" target="stats">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
"""

# 创建所有DrawIO文件
diagrams = {
    "1-系统架构图": architecture_xml,
    "2-玩家状态机": player_state_xml,
    "3-敌人AI决策": enemy_ai_xml,
    "4-伤害计算流程": damage_xml,
    "5-技能系统架构": skill_xml,
    "6-物品系统流程": item_xml,
}

print("开始生成DrawIO图表...\n")

for name, xml_content in diagrams.items():
    try:
        file_path = f"{output_dir}\\{name}.drawio"
        drawio_xml = create_drawio_xml(name, xml_content)
        
        with open(file_path, 'w', encoding='utf-8') as f:
            f.write(drawio_xml)
        
        print(f"✓ 已生成：{name}.drawio")
        
    except Exception as e:
        print(f"✗ 生成 {name} 失败: {e}")

print(f"\n所有图表已保存到：{output_dir}")
print("\n提示：使用draw.io或DrawIO桌面版打开这些文件即可编辑和导出！")
