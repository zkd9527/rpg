using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 游戏管理器类，负责管理游戏的核心功能，包括检查点、游戏状态和数据保存
/// </summary>
public class GameManager : MonoBehaviour, IGameProgressionSaveManager
{
    /// <summary>
    /// 游戏管理器的单例实例
    /// </summary>
    public static GameManager instance;

    /// <summary>
    /// 玩家引用
    /// </summary>
    private Player player;

    /// <summary>
    /// 场景中的所有检查点
    /// </summary>
    [SerializeField] private Checkpoint[] checkpoints;
    
    /// <summary>
    /// 最后激活的检查点ID
    /// </summary>
    public string lastActivatedCheckpointID { get; set; }

    [Header("掉落货币")]
    /// <summary>
    /// 死亡身体预制体
    /// </summary>
    [SerializeField] private GameObject deathBodyPrefab;
    
    /// <summary>
    /// 掉落的货币数量
    /// </summary>
    public int droppedCurrencyAmount;
    
    /// <summary>
    /// 死亡位置
    /// </summary>
    [SerializeField] private Vector2 deathPosition;

    //public List<ItemObject> pickedUpItemInMapList { get; set; }
    /// <summary>
    /// 已使用的地图元素ID列表
    /// </summary>
    public List<int> UsedMapElementIDList {  get; set; }

    /// <summary>
    /// 唤醒时初始化单例和组件
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

        // 查找场景中的所有检查点
        checkpoints = FindObjectsOfType<Checkpoint>();
        // 获取玩家实例
        player = PlayerManager.instance.player;

        // 初始化已使用的地图元素ID列表
        //pickedUpItemInMapList = new List<ItemObject>();
        UsedMapElementIDList = new List<int>();
    }

    /// <summary>
    /// 重启当前场景
    /// </summary>
    public void RestartScene()
    {
        // 获取当前场景
        Scene scene = SceneManager.GetActiveScene();

        // 重新加载当前场景
        SceneManager.LoadScene(scene.name);
    }

    /// <summary>
    /// 暂停或恢复游戏
    /// </summary>
    /// <param name="_pause">是否暂停</param>
    public void PauseGame(bool _pause)
    {
        if (_pause)
        {
            // 暂停游戏
            Time.timeScale = 0;
        }
        else
        {
            // 恢复游戏
            Time.timeScale = 1;
        }
    }

    /// <summary>
    /// 查找最近的已激活检查点
    /// </summary>
    /// <returns>最近的已激活检查点</returns>
    private Checkpoint FindClosestActivatedCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestActivatedCheckpoint = null;

        // 遍历所有检查点
        foreach (var checkpoint in checkpoints)
        {
            // 计算到检查点的距离
            float distanceToCheckpoint = Vector2.Distance(player.transform.position, checkpoint.transform.position);

            // 如果距离更近且检查点已激活
            if (distanceToCheckpoint < closestDistance && checkpoint.activated == true)
            {
                closestDistance = distanceToCheckpoint;
                closestActivatedCheckpoint = checkpoint;
            }
        }

        return closestActivatedCheckpoint;
    }

    /// <summary>
    /// 加载掉落的货币
    /// </summary>
    /// <param name="_data">游戏数据</param>
    private void LoadDroppedCurrency(GameData _data)
    {
        // 加载掉落的货币数量和死亡位置
        droppedCurrencyAmount = _data.droppedCurrencyAmount;
        deathPosition = _data.deathPosition;

        // 如果有掉落的货币，生成死亡身体
        if (droppedCurrencyAmount > 0)
        {
            GameObject deathBody = Instantiate(deathBodyPrefab, deathPosition, Quaternion.identity);
            deathBody.GetComponent<DroppedCurrencyController>().droppedCurrency = droppedCurrencyAmount;
        }

        // 防止在玩家没有死亡但有货币剩余的情况下在地面生成死亡身体
        // 当玩家选择保存并继续游戏时
        droppedCurrencyAmount = 0;
    }

    /// <summary>
    /// 加载检查点状态
    /// </summary>
    /// <param name="_data">游戏数据</param>
    private void LoadCheckpoints(GameData _data)
    {
        // 遍历保存的检查点字典
        foreach (var search in _data.checkpointsDictionary)
        {
            // 遍历场景中的检查点
            foreach (var checkpoint in checkpoints)
            {
                // 如果检查点ID匹配且状态为激活
                if (checkpoint.checkpointID == search.Key && search.Value == true)
                {
                    // 激活检查点
                    checkpoint.ActivateCheckpoint();
                }
            }
        }
    }

    /// <summary>
    /// 加载最后激活的检查点
    /// </summary>
    /// <param name="_data">游戏数据</param>
    private void LoadLastActivatedCheckpoint(GameData _data)
    {
        lastActivatedCheckpointID = _data.lastActivatedCheckpointID;
    }

    /// <summary>
    /// 在最近的已激活检查点生成玩家
    /// </summary>
    /// <param name="_data">游戏数据</param>
    private void SpawnPlayerAtClosestActivatedCheckpoint(GameData _data)
    {
        if (_data.closestActivatedCheckpointID == null)
        {
            return;
        }

        // 遍历所有检查点
        foreach (var checkpoint in checkpoints)
        {
            // 如果检查点ID匹配
            if (_data.closestActivatedCheckpointID == checkpoint.checkpointID)
            {
                // 在检查点位置生成玩家
                player.transform.position = checkpoint.transform.position;
            }
        }
    }

    /// <summary>
    /// 在最后激活的检查点生成玩家
    /// </summary>
    /// <param name="_data">游戏数据</param>
    private void SpawnPlayerAtLastActivatedCheckpoint(GameData _data)
    {
        if (_data.lastActivatedCheckpointID == null)
        {
            return;
        }

        // 遍历所有检查点
        foreach (var checkpoint in checkpoints)
        {
            // 如果检查点ID匹配
            if (_data.lastActivatedCheckpointID == checkpoint.checkpointID)
            {
                // 在检查点位置生成玩家
                player.transform.position = checkpoint.transform.position;
            }
        }
    }

    /// <summary>
    /// 加载已使用的地图元素ID列表
    /// </summary>
    /// <param name="_data">游戏数据</param>
    private void LoadPickedUpItemInMapIDList(GameData _data)
    {
        if (_data.UsedMapElementIDList != null)
        {
            // 遍历保存的已使用地图元素ID列表
            foreach (var seach in _data.UsedMapElementIDList)
            {
                // 添加到当前列表
                UsedMapElementIDList.Add(seach);
            }
        }
    }

    /// <summary>
    /// 加载游戏数据
    /// </summary>
    /// <param name="_data">游戏数据</param>
    public void LoadData(GameData _data)
    {
        // 加载掉落的货币
        LoadDroppedCurrency(_data);

        // 加载已使用的地图元素ID列表
        // 地图中的拾取物品会在ItemObject脚本中自动销毁
        LoadPickedUpItemInMapIDList(_data);
        //LoadPickedUpItemInMapList(_data);

        // 激活所有保存为已激活的检查点
        LoadCheckpoints(_data);

        // 加载最后激活的检查点
        LoadLastActivatedCheckpoint(_data);

        // 在最近的已激活检查点生成玩家
        //SpawnPlayerAtClosestActivatedCheckpoint(_data);

        // 在最后激活的检查点生成玩家
        SpawnPlayerAtLastActivatedCheckpoint(_data);
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="_data">游戏数据</param>
    public void SaveData(ref GameData _data)
    {
        // 保存死亡位置和掉落的货币
        _data.droppedCurrencyAmount = droppedCurrencyAmount;
        _data.deathPosition = player.transform.position;

        // 防止保存重复的检查点数据
        _data.checkpointsDictionary.Clear();

        // 保存最近的已激活检查点ID
        _data.closestActivatedCheckpointID = FindClosestActivatedCheckpoint()?.checkpointID;

        // 保存所有检查点的激活状态
        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpointsDictionary.Add(checkpoint.checkpointID, checkpoint.activated);
        }

        // 保存最后激活的检查点ID
        _data.lastActivatedCheckpointID = lastActivatedCheckpointID;

        // 保存已使用的地图元素ID列表
        _data.UsedMapElementIDList.Clear();
        foreach (var itemID in UsedMapElementIDList)
        {
            _data.UsedMapElementIDList.Add(itemID);
        }
    }
}
