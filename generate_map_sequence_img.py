#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
生成地图随机生成的时序图 PNG 预览
"""
import matplotlib.pyplot as plt
from matplotlib.patches import FancyArrowPatch, Rectangle
import os

plt.rcParams['font.sans-serif'] = ['SimHei', 'DejaVu Sans']
plt.rcParams['axes.unicode_minus'] = False

output_dir = r"D:\论文优化文档"
os.makedirs(output_dir, exist_ok=True)

def draw_sequence():
    fig, ax = plt.subplots(figsize=(14,6))
    ax.axis('off')
    participants = [
        ('MapGenerator\n地图生成器', 0.05),
        ('NoiseGenerator\n噪声生成', 0.22),
        ('TileManager\n瓦片管理', 0.39),
        ('RoomPlacer\n房间放置', 0.56),
        ('NavMeshBuilder\n导航网格', 0.73),
        ('GameWorld\n游戏世界', 0.90),
    ]

    # 画参与者盒子
    for name, x in participants:
        rect = Rectangle((x,0.75), 0.12, 0.18, facecolor='#FFE4B5', edgecolor='black')
        ax.add_patch(rect)
        ax.text(x+0.06, 0.84, name, ha='center', va='center', fontsize=10, fontweight='bold')
        # lifeline
        ax.plot([x+0.06, x+0.06], [0.7, 0.1], color='gray', linestyle='--')

    # 顺序箭头
    def arrow(from_x, to_x, y, label):
        start = (from_x+0.06, y)
        end = (to_x+0.06, y)
        ax.add_patch(FancyArrowPatch(start, end, arrowstyle='->', mutation_scale=20, linewidth=2, color='black'))
        ax.text((start[0]+end[0])/2, y+0.03, label, ha='center', va='bottom', fontsize=9, bbox=dict(boxstyle='round,pad=0.2', facecolor='white', alpha=0.8))

    y = 0.65
    arrow(0.9, 0.05, y, 'Initialize()')
    y -= 0.08
    arrow(0.05, 0.22, y, 'GenerateNoise(width, height)')
    y -= 0.08
    arrow(0.22, 0.39, y, 'CreateTiles(noiseData)')
    y -= 0.08
    arrow(0.39, 0.56, y, 'PlaceRooms(tiles)')
    y -= 0.08
    arrow(0.56, 0.39, y, 'CarveCorridors()')
    y -= 0.08
    arrow(0.39, 0.73, y, 'BuildNavMesh()')
    y -= 0.08
    arrow(0.05, 0.90, y, 'Finalize() / SpawnEntities')

    ax.set_xlim(0,1)
    ax.set_ylim(0,1)
    plt.tight_layout()
    out_path = os.path.join(output_dir, '7-地图随机生成时序图.png')
    plt.savefig(out_path, dpi=150)
    plt.close()
    print('✓ 已生成：7-地图随机生成时序图.png ->', out_path)

if __name__ == '__main__':
    draw_sequence()
