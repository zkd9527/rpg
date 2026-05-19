using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int maxItemDropAmount;  //这个敌人最多可以掉落多少物品
    [SerializeField] private ItemData[] possibleDropItemList;  //这个敌人可以掉落的物品
    private List<ItemData> actualDropList = new List<ItemData>(); //这个敌人实际掉落的物品

    [SerializeField] private GameObject dropItemPrefab;  


    public virtual void GenrateDrop()
    {
        //根据它们的概率将物品添加到 actualDropList
        for (int i = 0; i < possibleDropItemList.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleDropItemList[i].dropChance)
            {
                actualDropList.Add(possibleDropItemList[i]);
            }
        }

        //丢弃物品并从 actualDropList 中删除它们
        for (int i = 0; i < maxItemDropAmount && actualDropList.Count > 0; i++)
        {
            ItemData itemToDrop = actualDropList[Random.Range(0, actualDropList.Count - 1)];

            actualDropList.Remove(itemToDrop);
            DropItem(itemToDrop);
        }

    }


    //当敌人死亡时调用 DropItem
    protected void DropItem(ItemData _itemToDrop)
    {
        GameObject newDropItem = Instantiate(dropItemPrefab, transform.position, Quaternion.identity);

        Vector2 dropVelocity = new Vector2(Random.Range(-5, 5), Random.Range(12, 15));

        //设置掉落物品的名称和图标
        newDropItem.GetComponent<ItemObject>()?.SetupItemDrop(_itemToDrop, dropVelocity);
    }
}
