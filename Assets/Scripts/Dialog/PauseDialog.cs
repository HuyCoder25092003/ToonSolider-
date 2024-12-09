using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseDialog : BaseDialog
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
    public void OnClose()
    {
        DialogManager.instance.HideDialog(DialogIndex.PauseDialog);
    }
    public void OnQuit()
    {
        DialogManager.instance.HideDialog(dialogIndex);
        BYPoolManager.instance.GetPool("HPHub").DeSpawnAll();
        LoadSceneManager.instance.LoadSceneByIndex(1, () =>
        {
            ViewManager.instance.SwitchView(ViewIndex.HomeView);
        });
    }
    public void OnRestartGame()
    {
        LoadSceneManager.instance.LoadSceneByName(GameController.instance.cf_mission.SceneName, () =>
        {
            ViewManager.instance.SwitchView(ViewIndex.HomeView);
        });
    }
    public void OnSettingsGame()
    {
        DialogManager.instance.ShowDialog(DialogIndex.SettingsDialog);
    }
}
