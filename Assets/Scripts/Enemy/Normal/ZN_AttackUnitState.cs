using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class ZN_AttackUnitState : FSM_State
{
    [NonSerialized]
    public ZombieNormal parent;
    private Transform target;
    public float speed;
    private float cur_speed_anim;
    private float delayCheck = 0;
    private bool isAttacking;
    public override void Enter(object data)
    {
        base.Enter(data);
        target = (Transform)data;
        parent.agent.isStopped = false;
        parent.agent.speed = speed;
        cur_speed_anim = 0;
        delayCheck = 0;
        parent.agent.stoppingDistance = parent.range_attack;
    }
    public override void Update()
    {
        delayCheck += Time.deltaTime;
        base.Update();

        if (target == null)
        {
            parent.GotoState(parent.moveState);
            return;
        }
        if(!isAttacking)
        {
            if(Vector3.Distance(parent.trans.position, target.position) > parent.range_detect )
            {
                parent.GotoState(parent.moveState);
                return;
            }
            parent.agent.SetDestination(target.position);
            UpdateRotation();
            float speed_anim = parent.agent.velocity.magnitude / parent.agent.speed;
            cur_speed_anim = Mathf.Lerp(cur_speed_anim, speed_anim * speed, Time.deltaTime * 5);

            if (delayCheck > 0.5f)
            {
                if (parent.agent.remainingDistance <= parent.range_attack + 0.1f)
                {
                    if (parent.timeAttack >= parent.cf.Attack_rate)
                    {
                        parent.dataBinding.Attack = true;
                        parent.agent.isStopped = true;
                        parent.timeAttack = 0;
                    }


                }
                else
                {
                    parent.dataBinding.Speed = cur_speed_anim;
                }
            }
            else
            {
                parent.dataBinding.Speed = cur_speed_anim;
            }
        }

      
      
    }
    public override void OnAnimEnter()
    {
        base.OnAnimEnter();
        isAttacking = true;
    }
    public override void OnAnimMiddle()
    {
        base.OnAnimMiddle();
        if(Vector3.Distance(parent.trans.position,target.position)<=parent.range_attack+0.1f)
        {
            target.GetComponent<UnitControl>().OnDamage(parent.damageData);

        }
    }
    public override void OnAnimExit()
    {
        base.OnAnimExit();
        parent.agent.isStopped = false;
        isAttacking = false;
    }
    private void UpdateRotation()
    {
        Vector3 dir = parent.agent.steeringTarget - parent.trans.position;
        dir.Normalize();
        if (dir != Vector3.zero)
        {
            Quaternion q = Quaternion.LookRotation(dir, Vector3.up);

            parent.trans.rotation = Quaternion.Slerp(parent.trans.rotation, q, Time.deltaTime * 30);
        }

    }
    public override void Exit()
    {
        base.Exit();
        parent.agent.stoppingDistance =0;
        parent.agent.isStopped = true;
        parent.dataBinding.Speed = 0;
    }
}
