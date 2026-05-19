using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.jumpcount = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // ===================== 安全按键读取（永远不报错） =====================
        KeyCode attackKey = KeyCode.Mouse0;
        if (KeyBindManager.instance != null && KeyBindManager.instance.keybindsDictionary.ContainsKey("Attack"))
            attackKey = KeyBindManager.instance.keybindsDictionary["Attack"];

        KeyCode parryKey = KeyCode.Q;
        if (KeyBindManager.instance != null && KeyBindManager.instance.keybindsDictionary.ContainsKey("Parry"))
            parryKey = KeyBindManager.instance.keybindsDictionary["Parry"];

        KeyCode blackholeKey = KeyCode.R;
        if (KeyBindManager.instance != null && KeyBindManager.instance.keybindsDictionary.ContainsKey("Blackhole"))
            blackholeKey = KeyBindManager.instance.keybindsDictionary["Blackhole"];

        KeyCode aimKey = KeyCode.Mouse1;
        if (KeyBindManager.instance != null && KeyBindManager.instance.keybindsDictionary.ContainsKey("Aim"))
            aimKey = KeyBindManager.instance.keybindsDictionary["Aim"];
        // ======================================================================


        if (Input.GetKeyDown(attackKey))
        {
            stateMachine.ChangeState(player.primaryAttackState);
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(attackKey))
        {
            stateMachine.ChangeState(player.airLaunchAttackState);
        }

        if (Input.GetKeyDown(parryKey) && player.skill.parry.parryUnlocked && player.skill.parry.SkillIsReadyToUse())
        {
            SkillManager.instance.parry.UseSkillIfAvailable();
        }

        if (Input.GetKeyDown(blackholeKey) && player.skill.blackhole.blackholeUnlocked && player.skill.blackhole.SkillIsReadyToUse())
        {
            stateMachine.ChangeState(player.blackholeSkillState);
        }

        if (Input.GetKeyDown(aimKey) && player.HasNoSword() && player.skill.sword.throwSwordSkillUnlocked)
        {
            stateMachine.ChangeState(player.aimSwordState);
        }

        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
        }

        if (player.IsGroundDetected() && player.isOnPlatform)
        {
            if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
            {
                player.JumpOffPlatform();
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.jumpState);
        }
    }
}