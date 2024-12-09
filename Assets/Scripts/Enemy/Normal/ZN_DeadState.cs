using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class ZN_DeadState : FSM_State
{
    [NonSerialized]
    public ZombieNormal parent;
    public override void Enter()
    {
        base.Enter();
        parent.dataBinding.Dead = true;
    }
    public override void OnAnimExit()
    {
        base.OnAnimExit();
        parent.OnDead();
    }
}
