using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGunImpact : MonoBehaviour
{
    public string name_pool = "Impact";
    public ParticleSystem particleSystem_;
    // Start is called before the first frame update
    public void OnSpawn()
    {
        particleSystem_.Play();
        StopCoroutine("Delay");
        StartCoroutine("Delay");
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        BYPoolManager.instance.GetPool(name_pool).Despawn(transform);
    }
}
