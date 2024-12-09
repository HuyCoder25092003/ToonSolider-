using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeViewAnimation : BaseViewAnimation
{
    public Animator anim;
    private Action callback;
    public override void OnHideAnimation(Action callback)
    {
        this.callback = callback;
        anim.Play("Hide", 0, 0);
    }
    public override void OnShowAnimation(Action callback)
    {
        this.callback = callback;
        anim.Play("Show", 0, 0);
    }
    public void ShowEnd()
    {
        callback();
    }
    public void HideEnd()
    {
        callback();
    }
}
