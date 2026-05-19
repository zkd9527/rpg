using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-999)]
public class KeyBindManager : MonoBehaviour, ISettingsSaveManager
{
    public static KeyBindManager instance;

    public Dictionary<string, KeyCode> keybindsDictionary;

    [SerializeField] private KeybindList_UI keybindList;

    private void Awake()
    {
        instance = this;
        keybindsDictionary = new Dictionary<string, KeyCode>();
        AddAllDefaultKeys();
    }

    private void AddAllDefaultKeys()
    {
        keybindsDictionary.Clear();

        // ?? ?? ?? 你项目所有键全部一次性补满！
        keybindsDictionary.Add("Attack", KeyCode.Mouse0);
        keybindsDictionary.Add("Dash", KeyCode.LeftShift);
        keybindsDictionary.Add("Parry", KeyCode.Q);
        keybindsDictionary.Add("Aim", KeyCode.Mouse1);
        keybindsDictionary.Add("Blackhole", KeyCode.R);
        keybindsDictionary.Add("Character", KeyCode.C);
        keybindsDictionary.Add("Crystal", KeyCode.F);
        keybindsDictionary.Add("Craft", KeyCode.X);
        keybindsDictionary.Add("Flask", KeyCode.Alpha1);
        keybindsDictionary.Add("Skill", KeyCode.K);      // 你现在缺的这个！
        keybindsDictionary.Add("Inventory", KeyCode.I);
        keybindsDictionary.Add("Interact", KeyCode.E);
        keybindsDictionary.Add("Map", KeyCode.M);
        keybindsDictionary.Add("Settings", KeyCode.Escape);
        keybindsDictionary.Add("Pause", KeyCode.Escape);
    }

    public void UpdateKeybindListLanguage() { }
    public string UniformKeybindName(string s) { return s; }
    public void LoadData(SettingsData data) { AddAllDefaultKeys(); }
    public void SaveData(ref SettingsData data) { }
}