using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailDialog : BaseDialog
{
    public override void OnShowDialog()
    {
        base.OnShowDialog();
        Time.timeScale = 0;
    }
    public override void OnHideDialog()
    {
        base.OnHideDialog();
        Time.timeScale = 1;
    }
    public void OnBack()
    {
        DialogManager.instance.HideDialog(DialogIndex.FailDialog);
        ViewManager.instance.SwitchView(ViewIndex.EmptyView);
        LoadSceneManager.instance.LoadSceneByIndex(1, () =>
        {
            ViewManager.instance.SwitchView(ViewIndex.HomeView);
        });
    }
}
