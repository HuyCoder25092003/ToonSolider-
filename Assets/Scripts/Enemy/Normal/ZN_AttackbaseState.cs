using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class ZN_AttackbaseState : FSM_State
{
    [NonSerialized]
    public ZombieNormal parent;

    public override void Enter()
    {
        base.Enter();
       
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if(parent.timeAttack>=parent.cf.Attack_rate)
        {
            parent.dataBinding.Attack = true;
            parent.timeAttack = 0;
        }
    }
    public override void OnAnimMiddle()
    {

        base.OnAnimMiddle();
        GameController.instance.OnDamage(parent.damageData);

    }
    public override void Exit()
    {
        base.Exit();
    }
}
