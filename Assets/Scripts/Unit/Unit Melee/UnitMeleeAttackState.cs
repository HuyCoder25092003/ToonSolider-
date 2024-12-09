using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitMeleeAttackState : FSM_State
{
    [NonSerialized]
    public UnitMeleeControl parent;
    private Transform trans_e;
    private EnemyControl enemy_control;
    private DamageData damageData = new DamageData();
    public override void Enter(object data)
    {
        Debug.LogError("Attack Staste");
        trans_e = (Transform)data;
        enemy_control = trans_e.GetComponent<EnemyControl>();
        ConfigUnitLevelRecord cf_level = parent.data.configUnit_lv;
        int lv = parent.data.unitData.level;
        damageData.damage = cf_level.GetDamage(lv);
        parent.u_agent.Warp(parent.trans.position);
        parent.u_agent.isStopped = false;
        parent.u_agent.stoppingDistance = 1;
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (trans_e == null)
            parent.GotoState(parent.guardState);
        else
        {
            float dis = Vector3.Distance(parent.trans.position, trans_e.position);

            if (dis > parent.range)
                parent.GotoState(parent.guardState);
            else if (enemy_control.hp <= 0)
                parent.GotoState(parent.guardState);
            else
            {
                RotateToEnemy();
                if (dis <= 1.1f)
                    parent.databinding.Attack = true;
                else
                    parent.u_agent.SetDestination(trans_e.position);
            }
        }
    }
    private void RotateToEnemy()
    {
        Vector3 pos_e = trans_e.position;
        pos_e.y = parent.trans.position.y;
        Vector3 dir = pos_e - parent.trans.position;

        dir.Normalize();

        Quaternion q = Quaternion.LookRotation(dir, Vector3.up);

        parent.trans.rotation = q;
    }
    public override void OnAnimMiddle()
    {
        base.FixedUpdate();
        if (trans_e != null)
        {
            float dis = Vector3.Distance(parent.trans.position, trans_e.position);
            if (dis <= 1)
                enemy_control.OnDamage(damageData);
        }
    }
    public override void Exit()
    {
        parent.u_agent.stoppingDistance = 0;
        parent.u_agent.isStopped = true;
    }
}
