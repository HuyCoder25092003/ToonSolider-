using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitGunAttackState : FSM_State
{
    [NonSerialized]
    public UnitGunControl parent;
    [SerializeField]
    private Transform trans_e;
    private EnemyControl enemy_control;
    public UnitGunWeaponBehaviour weaponBehaviour;
    public bool is_InitGun = false;
    public override void Enter(object data)
    {
         trans_e = (Transform)data;
        enemy_control = trans_e.GetComponent<EnemyControl>();
        UnitGunData unitGunData = new UnitGunData();
        unitGunData.target = trans_e;
        unitGunData.unitControl = parent;
        ConfigUnitLevelRecord cf_level = parent.data.configUnit_lv;
        int lv = parent.data.unitData.level;
        unitGunData.damage = cf_level.GetDamage(lv);
        unitGunData.rof = cf_level.GetAttackSpeed(lv);
        if (!is_InitGun)
        {
            is_InitGun = true;
           
            weaponBehaviour.SetupGun(unitGunData);
        }
        else
        {
            weaponBehaviour.SetupGunData(unitGunData);
        }
        weaponBehaviour.enabled = true;

        weaponBehaviour.enemy_target = trans_e;
        weaponBehaviour.isFire = true;

    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (trans_e == null)
        {
            parent.GotoState(parent.guardState);
            
        }
        else
        {
            float dis = Vector3.Distance(parent.trans.position, trans_e.position);

            if (dis > parent.range)
            {
                parent.GotoState(parent.guardState);
            }

            else if (enemy_control.hp <= 0)
            {
                parent.GotoState(parent.guardState);
            }
            else
            {
                RotateToEnemy();

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
    public override void Exit()
    {
        trans_e = null;
        weaponBehaviour.enabled = false;

        weaponBehaviour.isFire = false;
        weaponBehaviour.enemy_target = null;
    }
}
