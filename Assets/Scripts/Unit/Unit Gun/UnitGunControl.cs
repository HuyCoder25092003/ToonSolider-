using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGunControl : UnitControl
{
    public UnitGunDatabinding databinding;
    public UnitGunGuardState guardState;
    public UnitGunAttackState attackState;
    public UnitGunDeadState deadState;
    public override void Setup(UnitInitData unitInitData)
    {
        base.Setup(unitInitData);

        guardState.parent = this;
        attackState.parent = this;
        deadState.parent = this;
        GotoState(guardState);
    }
    public override void OnDamage(DamageData damageData)
    {
        hp -= damageData.damage;
        if(hp<=0)
        {
            if(cur_State!=deadState)
            {
                GotoState(deadState);
            }
        }
        base.OnDamage(damageData);
    }
}
