using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieNormal : EnemyControl
{
    public ZombieNormalDataBinding dataBinding;
    public ZN_AttackbaseState attackBaseState;
    public ZN_AttackUnitState attackUnitState;
    public ZN_DeadState deadState;
    public ZN_MoveState moveState;
    public ZN_StartState startState;
    public override void Setup(EnemyInitData enemyInitData)
    {
        base.Setup(enemyInitData);
        attackBaseState.parent = this;
        attackUnitState.parent = this;
        moveState.parent = this;
        startState.parent = this;
        deadState.parent = this;
        GotoState(startState);
        StartCoroutine("LoopDetectUnit");
    }
    IEnumerator LoopDetectUnit()
    {
        WaitForSeconds wait = new WaitForSeconds(1);
        while (true)
        {
            yield return wait;
            if (cur_State == moveState)
            {
                Collider[] cols = Physics.OverlapSphere(trans_Detect.position, range_detect, maskUnit);
                int index = -1;
                if (cols.Length == 1)
                {
                    index = 0;

                }
                float distance = 100;
                for (int i = 0; i < cols.Length; i++)
                {
                    float dis = Vector3.Distance(trans.position, cols[i].transform.position);
                    if (dis < distance)
                    {
                        distance = dis;
                        index = i;
                    }
                }

                if (index != -1)
                {
                    GotoState(attackUnitState, cols[index].transform);
                }
            }
        }
    }

    public override void OnDamage(DamageData enemyOnDamageData)
    {
      
        hp -= enemyOnDamageData.damage;
        if (hp <= 0)
        {
            if(cur_State!=deadState)
            {
                GotoState(deadState);
            }
        }
        base.OnDamage(enemyOnDamageData);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range_detect);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range_attack);
    }
}
