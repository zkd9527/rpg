using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemEffect : ScriptableObject
{


    public bool effectUsed { get; set; }
    public float effectLastUseTime { get; set; }
    public float effectCooldown;

    [TextArea]
    public string effectDescription;

    [TextArea]
    public string effectDescription_Chinese;

    public virtual void ExecuteEffect(Transform _spawnTransform)
    {
        //Debug.Log("Effect Executed");
    }
    

    public virtual void ReleaseSwordArcane()
    {

    }

}
