using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitGunDeadState : FSM_State
{
    [NonSerialized]
    public UnitGunControl parent;
    public override void Enter()
    {
        base.Enter();
        parent.databinding.Dead = true; 
    }
    public override void OnAnimMiddle()
    {
        parent.OnDead();

    }
}
