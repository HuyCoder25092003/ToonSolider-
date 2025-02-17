using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IngameView : BaseView
{
    public Text wave_lb;
    public List<DeckIngameItemControl> deck_items;
    public TMP_Text stamina_lb;
    public GameObject lockUIObject;
    public RectTransform m_DraggingPlane;
    private int stamina;
    public UnityEvent<int> OnStaminaChange;
    public RectTransform parent_hub;
    // 680
    public Image hp_base_fg;
    public override void Setup(ViewParam param)
    {
        lockUIObject.SetActive(false);
        OnStaminaChange.RemoveAllListeners();

        List<UnitData> decks = DataController.instance.GetDeck();
        for (int i = 0; i < decks.Count; i++)
        {
            deck_items[i].Setup(decks[i],this);
        }
        stamina = 0;
        stamina_lb.text = $"{stamina}";
    }
    public void OnPause()
    {
        DialogManager.instance.ShowDialog(DialogIndex.PauseDialog);
    }
    public override void OnShowView()
    {
        hp_base_fg.fillAmount = 1;
        GameController.instance.OnWaveChange.AddListener(OnWaveChange);
        StartCoroutine("LoopStamina");
        GameController.instance.OnBaseHpChange.AddListener(OnBaseHpChange) ;
    }

    private void OnBaseHpChange(int hp, int max_hp)
    {
        float val = (float)hp / (float)max_hp;
        hp_base_fg.fillAmount = val;
    }

    private void OnWaveChange(int arg1, int arg2)
    {
        wave_lb.text = $"WAVE: {arg1} / {arg2}";
    }

    public override void OnHideView()
    {
        OnStaminaChange.RemoveAllListeners();

        StopCoroutine("LoopStamina");
    }
    IEnumerator LoopStamina()
    {
        WaitForSeconds wait = new WaitForSeconds(1);
        while(true)
        {
            yield return wait;
            stamina++;
            stamina_lb.text = $"{stamina}";
            OnStaminaChange?.Invoke(stamina);
        }
    }
    public void OnDropUnit(UnitData unitData,ConfigUnitRecord configUnit)
    {
        stamina -= configUnit.Stamina;
    }
}
