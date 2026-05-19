using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 相机管理器类，负责管理游戏相机的移动和视角调整
/// </summary>
public class CameraManager : MonoBehaviour
{
    /// <summary>
    /// 相机管理器的单例实例
    /// </summary>
    public static CameraManager instance;

    /// <summary>
    /// Cinemachine虚拟相机
    /// </summary>
    public CinemachineVirtualCamera cm;

    [Header("相机镜头信息")]
    /// <summary>
    /// 默认相机镜头大小
    /// </summary>
    public float defaultCameraLensSize;
    
    /// <summary>
    /// 目标相机镜头大小
    /// </summary>
    public float targetCameraLensSize;
    
    /// <summary>
    /// 相机镜头大小变化速度
    /// </summary>
    public float cameraLensSizeChangeSpeed;

    [Header("相机屏幕Y位置信息")]
    /// <summary>
    /// 默认相机Y位置
    /// </summary>
    public float defaultCameraYPosition;
    
    /// <summary>
    /// 目标相机Y位置
    /// </summary>
    public float targetCameraYPosition;
    
    /// <summary>
    /// 相机Y位置变化速度
    /// </summary>
    public float cameraYPositionChangeSpeed;

    //[Header("Camera Screen X Position Info")]
    //public float defaultCameraXPosition;
    //public float targetCameraXPositionOffset;
    //public float cameraXPositionChangeSpeed;

    /// <summary>
    /// 玩家引用
    /// </summary>
    private Player player;
    
    /// <summary>
    /// Cinemachine帧转置器组件
    /// </summary>
    public CinemachineFramingTransposer ft { get; set; }

    /// <summary>
    /// 唤醒时初始化单例和相机组件
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

        // 获取Cinemachine帧转置器组件
        ft = cm.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    /// <summary>
    /// 开始时获取玩家引用
    /// </summary>
    private void Start()
    {
        // 获取玩家实例
        player = PlayerManager.instance.player;
    }

    /// <summary>
    /// 可下落平台上的相机移动方法
    /// </summary>
    public void CameraMovementOnDownablePlatform()
    {
        // 如果玩家在可下落平台上，相机镜头大小会增加，同时相机Y位置会降低
        if (player.isOnPlatform)
        {
            // 平滑调整相机镜头大小到目标大小
            if (cm.m_Lens.OrthographicSize < targetCameraLensSize)
            {
                cm.m_Lens.OrthographicSize = Mathf.Lerp(cm.m_Lens.OrthographicSize, targetCameraLensSize, cameraLensSizeChangeSpeed * Time.deltaTime);

                // 当接近目标大小时，直接设置为目标大小
                if (cm.m_Lens.OrthographicSize >= targetCameraLensSize - 0.01f)
                {
                    cm.m_Lens.OrthographicSize = targetCameraLensSize;
                }
            }

            // 平滑调整相机Y位置到目标位置
            if (ft.m_ScreenY > targetCameraYPosition)
            {
                ft.m_ScreenY = Mathf.Lerp(ft.m_ScreenY, targetCameraYPosition, cameraYPositionChangeSpeed * Time.deltaTime);

                // 当接近目标位置时，直接设置为目标位置
                if (ft.m_ScreenY >= targetCameraYPosition + 0.01f)
                {
                    ft.m_ScreenY = targetCameraYPosition;
                }
            }
        }
        // 反之，恢复默认相机设置
        else
        {
            // 不要让这个影响陷阱上的相机移动
            if (player.isNearPit)
            {
                return;
            }

            // 平滑调整相机镜头大小到默认大小
            if (cm.m_Lens.OrthographicSize > defaultCameraLensSize)
            {
                cm.m_Lens.OrthographicSize = Mathf.Lerp(cm.m_Lens.OrthographicSize, defaultCameraLensSize, cameraLensSizeChangeSpeed * Time.deltaTime);

                // 当接近默认大小时，直接设置为默认大小
                if (cm.m_Lens.OrthographicSize <= defaultCameraLensSize + 0.01f)
                {
                    cm.m_Lens.OrthographicSize = defaultCameraLensSize;
                }
            }

            // 平滑调整相机Y位置到默认位置
            if (ft.m_ScreenY < defaultCameraYPosition)
            {
                ft.m_ScreenY = Mathf.Lerp(ft.m_ScreenY, defaultCameraYPosition, cameraYPositionChangeSpeed * Time.deltaTime);

                // 当接近默认位置时，直接设置为默认位置
                if (ft.m_ScreenY <= targetCameraYPosition - 0.01f)
                {
                    ft.m_ScreenY = targetCameraYPosition;
                }
            }
        }
    }
}
