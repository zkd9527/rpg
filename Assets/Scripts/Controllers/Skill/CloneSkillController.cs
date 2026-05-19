using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CloneSkillController : MonoBehaviour 
{
    private SpriteRenderer sr;
    private Animator anim;

    private float cloneDuration;//分身持续时间
    private float cloneTimer;
    private float colorLosingSpeed;//分身消失速度

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius;
    private Transform closestEnemy;

    private bool canDuplicateClone;
    private float duplicatePossibility;

    private bool cloneFacingRight = true;
    private float cloneFacingDirection = 1;

    private float cloneAttackDamageMultiplier;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLosingSpeed));

            if (sr.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }

    }

    public void SetupClone(float _cloneDuration, float _colorLosingSpeed, bool _canAttack, Transform _closestEnemy, bool _canDuplicateClone, float _duplicatePossibility, float _cloneAttackDamageMultiplier)
    {
        if (_canAttack)
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 4));
        }

        cloneDuration = _cloneDuration;
        colorLosingSpeed = _colorLosingSpeed;

        cloneTimer = cloneDuration;

        closestEnemy = _closestEnemy;

        FaceClosestTarget();

        canDuplicateClone = _canDuplicateClone;
        duplicatePossibility = _duplicatePossibility;

        cloneAttackDamageMultiplier = _cloneAttackDamageMultiplier;
    }

    private void AnimationTrigger()
    {
        cloneTimer = -0.1f;
    }

    private void AttackTrigger()
    {

        if (SkillManager.instance.clone.aggressiveCloneCanApplyOnHitEffect)
        {
            Inventory.instance.ReleaseSwordArcane_ConsiderCooldown();
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Enemy enemy = hit.GetComponent<Enemy>();


                PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();


                if(playerStats != null)
                {
                    playerStats.CloneDoDamage(enemy.GetComponent<CharacterStats>(), cloneAttackDamageMultiplier, transform);
                }

                if (SkillManager.instance.clone.aggressiveCloneCanApplyOnHitEffect)
                {
                    Inventory.instance.UseSwordEffect_ConsiderCooldown(enemy.transform); ;
                }

                if (canDuplicateClone)
                {

                    if (Random.Range(0, 100) < duplicatePossibility  && SkillManager.instance.clone.currentDuplicateCloneAmount < SkillManager.instance.clone.maxDuplicateCloneAmount)
                    {
                        SkillManager.instance.clone.CreateDuplicateClone(new Vector3(hit.transform.position.x + 1f * cloneFacingDirection, hit.transform.position.y));
                    }
                }

            }
        }
    }

    private void FaceClosestTarget()
    {

        if(closestEnemy != null)
        {

            if(transform.position.x > closestEnemy.position.x)
            {
                CloneFlip();
            }
        }

    }

    private void CloneFlip()
    {
        transform.Rotate(0, 180, 0);

        cloneFacingRight = !cloneFacingRight;
        cloneFacingDirection = -cloneFacingDirection;
    }

}
