using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可下落平台检测类，用于检测玩家是否站在可下落平台上
/// </summary>
public class DownablePlatformCheck : MonoBehaviour
{
    /// <summary>
    /// 玩家引用
    /// </summary>
    private Player player;

    /// <summary>
    /// 开始时获取玩家引用
    /// </summary>
    private void Start()
    {
        // 获取玩家实例
        player = PlayerManager.instance.player;
    }

    /// <summary>
    /// 每帧更新，处理相机在可下落平台上的移动
    /// </summary>
    private void Update()
    {
        // 调用相机管理器的可下落平台移动方法
        CameraManager.instance.CameraMovementOnDownablePlatform();
    }

    /// <summary>
    /// 当触发进入时的回调方法
    /// </summary>
    /// <param name="collision">碰撞到的对象</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查碰撞对象是否是可下落平台
        if (collision.gameObject.GetComponent<DownablePlatform>() != null)
        {
            // 更新玩家的最后平台引用
            player.lastPlatform = collision.gameObject.GetComponent<DownablePlatform>();
            // 设置玩家在平台上的标志
            player.isOnPlatform = true;
        }
    }

    /// <summary>
    /// 当触发退出时的回调方法
    /// </summary>
    /// <param name="collision">碰撞到的对象</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 检查碰撞对象是否是可下落平台
        if (collision.gameObject.GetComponent<DownablePlatform>() != null)
        {
            // 更新玩家的最后平台引用
            player.lastPlatform = collision.gameObject.GetComponent<DownablePlatform>();
            // 设置玩家不在平台上的标志
            player.isOnPlatform = false;
        }
    }
}
