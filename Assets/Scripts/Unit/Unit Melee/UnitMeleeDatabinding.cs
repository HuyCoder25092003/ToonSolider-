using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMeleeDatabinding : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
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
    public bool Attack
    {
        set
        {
            int index = Random.Range(1, 3);
            animator.Play("Attack "+index.ToString(), 0, 0);
        }
    }
    private int Anim_K_Speed;
    void Start()
    {
        Anim_K_Speed = Animator.StringToHash("Speed");
    }
}
