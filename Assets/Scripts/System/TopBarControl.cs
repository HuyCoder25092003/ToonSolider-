using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TopBarControl : MonoBehaviour
{
    public RectTransform parent;
    public TMP_Text level_lb;
    public TMP_Text nick_lb;
    public TMP_Text gem_lb;
    public TMP_Text gold_lb;
    private int gold,gem;
    private Tweener tween_gold;
    private Tweener tween_gem;
    [SerializeField] float moveUp;
    [SerializeField] float moveDown;
    [SerializeField] float durationMoveUp;
    [SerializeField] float durationMoveDown;
    void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        ViewManager.instance.OnViewShow += ViewManager_OnViewShow;
        ViewManager.instance.OnViewHide += ViewManager_OnViewHide;
        DialogManager.instance.OnDialogShow += DialogManager_OnDialogShow;
        DialogManager.instance.OnDialogHide += DialogManager_OnDialogHide;
    }
    private void DialogManager_OnDialogHide(BaseDialog obj)
    {
        SettingsParent(false);
    }

    private void DialogManager_OnDialogShow(BaseDialog obj)
    {
        SettingsParent(true);
    }

    private void ViewManager_OnViewHide(BaseView obj)
    {
        if (obj.viewIndex == ViewIndex.HomeView)
        {
           //parent.DOAnchorPosY(500, 1);

        }
    }

    private void ViewManager_OnViewShow(BaseView obj)
    {
        if (obj.viewIndex == ViewIndex.HomeView)
        {
            SettingsParent(false);
        }
        if (obj.viewIndex == ViewIndex.IngameView)
        {
            SettingsParent(true);
        }
    }

    void SettingsParent(bool playGame)
    {
        if (playGame)
            parent.DOAnchorPosY(moveUp, durationMoveUp);
        else
            parent.DOAnchorPosY(moveDown, durationMoveDown);
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    { 
        if (arg0.buildIndex == 1)
        {
            PlayerInfo playerInfo = DataController.instance.GetPlayerInfo();
            nick_lb.text = playerInfo.nickname;
            level_lb.text = $"{playerInfo.level}";
            gold = DataController.instance.GetGold();
            gold_lb.text = $"{gold}";
            gem = DataController.instance.GetGem();
            gem_lb.text = $"{gem}";
            DataTrigger.RegisterValueChange(DataSchema.INVENTORY, DataGoldChange);
        }

    }
    private void DataGoldChange(object data)
    {
        int cur_gold = gold;
        gold = DataController.instance.GetGold();
        tween_gold?.Kill();
        tween_gold= DOTween.To(() => cur_gold, x => cur_gold = x, gold, 0.5f).OnUpdate(() =>
        {
            gold_lb.text = $"{cur_gold}";
        });

        int cur_gem = gem;
        gem = DataController.instance.GetGem();
        tween_gem?.Kill();
        tween_gem = DOTween.To(() => cur_gem, x => cur_gem = x, gem, 0.5f).OnUpdate(() =>
        {
            gem_lb.text = $"{cur_gem}";
        });

    }
    public void AddGold()
    {
        DialogManager.instance.HideAllDialog();
        ViewManager.instance.SwitchView(ViewIndex.ShopView);
    }
    public void OnDicTest()
    {
        DataController.instance.UpdateUnitLevel(3);
    }
    private void OnDisable()
    {
        DataTrigger.UnRegisterValueChange(DataSchema.INVENTORY, DataGoldChange);
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        ViewManager.instance.OnViewShow -= ViewManager_OnViewShow;
        ViewManager.instance.OnViewHide -= ViewManager_OnViewHide;
        DialogManager.instance.OnDialogShow -= DialogManager_OnDialogShow;
        DialogManager.instance.OnDialogHide -= DialogManager_OnDialogHide;
    }
    public void OnSettings()
    {
        DialogManager.instance.ShowDialog(DialogIndex.SettingsDialog);
    }
}
