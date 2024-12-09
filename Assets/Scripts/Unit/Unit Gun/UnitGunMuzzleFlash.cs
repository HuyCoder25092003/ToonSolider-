using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGunMuzzleFlash : MonoBehaviour
{
    public ParticleSystem particleSystem_;
    public void Fire()
    {
        particleSystem_.Play();
    }
}
