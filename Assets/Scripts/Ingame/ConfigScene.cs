using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConfigScene : BYSingletonMono<ConfigScene>
{
    [SerializeField] Transform range_mark_Unit;
    [SerializeField]
    private List<Transform> enemy_spawns;
    [SerializeField]
    private List<Transform> targets;
    private void Start()
    {
        range_mark_Unit.gameObject.SetActive(false);
    }
    public Transform GetEnemySpawnPoint()
    {
        return enemy_spawns.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
    }
    public Transform GetRandomTarget()
    {
      
        return targets.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
    }

    public void SetMarkUnitRange(Vector3 pos,int range,bool isValid)
    {
        range_mark_Unit.gameObject.SetActive(isValid);
        range_mark_Unit.position = pos;
        range_mark_Unit.localScale = range * Vector3.one*2;
     
    }
}
