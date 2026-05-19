using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class CloneSkill : Skill
{
    //将分身攻击伤害倍率分配给此变量
    private float currentCloneAttackDamageMultipler;

    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private float colorLosingSpeed;


    [Header("Mirage Attack Unlock Info")] //解锁克隆能力
    [SerializeField] private SkillTreeSlot_UI mirageAttackUnlockButton;
    [Range(0f, 1f)]
    [SerializeField] private float cloneAttackDamageMultiplier;  //克隆攻击的伤害应小于玩家的伤害
    public bool mirageAttackUnlocked { get; private set; }


    [Header("Aggressive Mirage Unlock Info")] //使克隆造成更多伤害并能够施加命中效果
    [SerializeField] private SkillTreeSlot_UI aggressiveMirageUnlockButton;
    [Range(0f, 1f)]
    [SerializeField] private float aggressiveCloneAttackDamageMultiplier;
    public bool aggressiveMirageUnlocked { get; private set; }
    public bool aggressiveCloneCanApplyOnHitEffect { get; private set; }


    [Header("Multiple Mirage Unlock Info")] //分身可以创建分身
    [SerializeField] private SkillTreeSlot_UI multipleMirageUnlockButton;
    [Range(0f, 1f)]
    [SerializeField] private float duplicateCloneAttackDamageMultiplier;  //重复克隆造成玩家30%的伤害
    public bool multipleMirageUnlocked { get; private set; }
    [SerializeField] private float duplicatePossibility;
    public int maxDuplicateCloneAmount; //分身存在最大数量
    [HideInInspector] public int currentDuplicateCloneAmount;


    [Header("Crystal Mirage Unlock Info")]
    [SerializeField] private SkillTreeSlot_UI crystalMirageUnlockButton;
    public bool crystalMirageUnlocked { get; private set; }


    protected override void Start()
    {
        base.Start();

        mirageAttackUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockMirageAttack);
        aggressiveMirageUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockAggressiveMirage);
        multipleMirageUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockMultipleMirage);
        crystalMirageUnlockButton.GetComponent<Button>()?.onClick.AddListener(UnlockCrystalMirage);
    }


    //防止创建无限的重复克隆
    public void RefreshCurrentDuplicateCloneAmount()
    {
        currentDuplicateCloneAmount = 0;
    }

    public void CreateClone(Vector3 _position)
    {

        if (crystalMirageUnlocked)
        {
 
            if (SkillManager.instance.crystal.SkillIsReadyToUse())
            {
                SkillManager.instance.crystal.UseSkillIfAvailable();
            }
            return;
        }

        //防止创建无限的重复克隆
        //或者无法创建重复克隆
        RefreshCurrentDuplicateCloneAmount();
        //创建分身实体
        GameObject newClone = Instantiate(clonePrefab, _position, Quaternion.identity);
        CloneSkillController newCloneScript = newClone.GetComponent<CloneSkillController>();
        //设置分身参数
        newCloneScript.SetupClone(cloneDuration, colorLosingSpeed, mirageAttackUnlocked, FindClosestEnemy(newClone.transform), multipleMirageUnlocked, duplicatePossibility, currentCloneAttackDamageMultipler);
    }

    public void CreateDuplicateClone(Vector3 _position)
    {
        GameObject newClone = Instantiate(clonePrefab, _position, Quaternion.identity);
        CloneSkillController newCloneScript = newClone.GetComponent<CloneSkillController>();

        newCloneScript.SetupClone(cloneDuration, colorLosingSpeed, mirageAttackUnlocked, FindClosestEnemy(newClone.transform), multipleMirageUnlocked, duplicatePossibility, currentCloneAttackDamageMultipler);

        currentDuplicateCloneAmount++;
    }


    public void CreateCloneWithDelay(Vector3 _position, float _delay)
    {
        StartCoroutine(CreateCloneWithDelay_Coroutine(_position, _delay));
    }

    private IEnumerator CreateCloneWithDelay_Coroutine(Vector3 _position, float _seconds)
    {
        yield return new WaitForSeconds(_seconds);

        CreateClone(_position);
    }


    protected override void CheckUnlockFromSave()
    {
        UnlockMirageAttack();
        UnlockAggressiveMirage();
        UnlockCrystalMirage();
        UnlockMultipleMirage();
    }

    #region Unlock Skill
    private void UnlockMirageAttack()
    {
        if (mirageAttackUnlocked)
        {
            return;
        }

        if (mirageAttackUnlockButton.unlocked)
        {
            mirageAttackUnlocked = true;
            currentCloneAttackDamageMultipler = cloneAttackDamageMultiplier;
        }
    }

    private void UnlockAggressiveMirage()
    {
        if (aggressiveMirageUnlocked)
        {
            return;
        }

        if (aggressiveMirageUnlockButton.unlocked)
        {
            aggressiveMirageUnlocked = true;
            aggressiveCloneCanApplyOnHitEffect = true;
            currentCloneAttackDamageMultipler = aggressiveCloneAttackDamageMultiplier;
        }
    }

    private void UnlockMultipleMirage()
    {
        if (multipleMirageUnlocked)
        {
            return;
        }

        if (multipleMirageUnlockButton.unlocked)
        {
            multipleMirageUnlocked = true;
            currentCloneAttackDamageMultipler = duplicateCloneAttackDamageMultiplier;
        }
    }

    private void UnlockCrystalMirage()
    {
        if (crystalMirageUnlocked)
        {
            return;
        }

        if (crystalMirageUnlockButton.unlocked)
        {
            crystalMirageUnlocked = true;
        }
    }
    #endregion
}
