using UnityEngine;

/// <summary>
/// 玩家管理器类，负责管理玩家实例和游戏货币
/// </summary>
public class PlayerManager : MonoBehaviour, IGameProgressionSaveManager
{
    /// <summary>
    /// 玩家管理器的单例实例
    /// </summary>
    public static PlayerManager instance;
    
    /// <summary>
    /// 玩家引用
    /// </summary>
    public Player player;

    /// <summary>
    /// 游戏货币数量
    /// </summary>
    public int currency;

    /// <summary>
    /// 唤醒时初始化单例
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
    }

    /// <summary>
    /// 每帧更新，处理货币上限
    /// </summary>
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F1))
        //{
        //    Cheat_Get500Currency();
        //}

        // 限制货币最大值为999999
        if (currency >= 999999)
        {
            currency = 999999;
        }

    }

    /// <summary>
    /// 作弊方法：获得500货币
    /// </summary>
    private void Cheat_Get500Currency()
    {
        currency += 500;
    }

    /// <summary>
    /// 如果货币足够则购买
    /// </summary>
    /// <param name="_price">物品价格</param>
    /// <returns>如果购买成功返回true，否则返回false</returns>
    public bool BuyIfAvailable(int _price)
    {
        // 检查货币是否足够
        if (currency < _price)
        {
            Debug.Log("Not enough money!");
            return false;
        }

        // 扣除货币
        currency -= _price;
        return true;
    }

    /// <summary>
    /// 获取当前货币数量
    /// </summary>
    /// <returns>当前货币数量</returns>
    public int GetCurrentCurrency()
    {
        return currency;
    }

    /// <summary>
    /// 加载游戏数据
    /// </summary>
    /// <param name="_data">游戏数据</param>
    public void LoadData(GameData _data)
    {
        // 加载货币数量
        this.currency = _data.currecny;
    }

    /// <summary>
    /// 保存游戏数据
    /// </summary>
    /// <param name="_data">游戏数据</param>
    public void SaveData(ref GameData _data)
    {
        // 保存货币数量
        _data.currecny = this.currency;
    }
}
