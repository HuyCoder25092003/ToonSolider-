using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGunDatabinding : MonoBehaviour
{
    public Animator animator;
    public float Speed
    {
        set
        {
            animator.SetFloat(Anim_K_Speed, value);
        }
    }
    public bool Dead
    {
        set
        {
            animator.Play("Dead", 0, 0);
        }
    }
    public bool Fire
    {
        set
        {
            animator.Play("Fire", 0, 0);
        }
    }
    public bool Reload
    {
        set
        {
            animator.Play("Reload", 0, 0);
        }
    }
    private int Anim_K_Speed;
    void Start()
    {
        Anim_K_Speed = Animator.StringToHash("Speed");
    }
}
