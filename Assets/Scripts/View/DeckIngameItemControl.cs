using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DeckIngameItemControl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject[] rare_objects;
    public Image icon;
    public TMP_Text name_lb;
    public TMP_Text level;
    public TMP_Text stamina_lb;
    public TMP_Text cool_down;

    private UnitData cur_UnitData;
    private ConfigUnitRecord config_unit;
    private IngameView parent;
    public GameObject item_drag;
    private RectTransform rect_item_drag;
    public Image bg_drag;
    public Image icon_drag;
    public GameObject valid_object;
    public GameObject in_valid_object;
    public GameObject cd_object;
    public Image clock_image;

    public GameObject lock_cd_object;
    public GameObject lock_object;
    public LayerMask mask;
    private bool isValid = false;
    private Camera cam;
    private Vector3 pos_drag;
    private bool isDraging;
    private Vector3 pos_CreateUnit;
    private ConfigUnitLevelRecord cf_level;


   public void OnBeginDrag(PointerEventData eventData)
    {
        if (lock_object.activeSelf)
            return;
        isDraging = true;
        isValid = false;
        parent.lockUIObject.SetActive(true);
        item_drag.SetActive(true);
        item_drag.transform.SetParent(parent.m_DraggingPlane, false);
        SetDraggedPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        parent.lockUIObject.SetActive(false);
        item_drag.SetActive(false);

        // drop card 
        if (isValid)
        {
            parent.OnDropUnit(cur_UnitData, config_unit);
            cd_object.SetActive(true);
            clock_image.fillAmount = 0;
            clock_image.DOFillAmount(1, config_unit.Cool_down).OnComplete(() =>
            {
                cd_object.SetActive(false);
            });
            GameController.instance.OnCreateUnit(cur_UnitData, config_unit, pos_CreateUnit);
        }
        valid_object.SetActive(false);
        in_valid_object.SetActive(false);
        isValid = false;
        isDraging = false;
        ConfigScene.instance.SetMarkUnitRange(Vector3.zero, 1, false);

    }

    private void SetDraggedPosition(PointerEventData data)
    {

        pos_drag = data.position;

        Vector2 pos_local;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parent.m_DraggingPlane, data.position, data.pressEventCamera, out pos_local);
        rect_item_drag.anchoredPosition = pos_local;
    }
    private void FixedUpdate()
    {
        if(isDraging)
        {
            Ray r = cam.ScreenPointToRay(pos_drag);
            Debug.DrawLine(r.origin, r.origin + r.direction * 5000, Color.cyan, 0.02f);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit, 100, mask))
            {
                valid_object.SetActive(true);
                in_valid_object.SetActive(false);
                isValid = true;
                pos_CreateUnit = hit.point;
                ConfigScene.instance.SetMarkUnitRange(pos_CreateUnit, cf_level.GetRange(cur_UnitData.level), true);
            }
            else
            {
                valid_object.SetActive(false);
                in_valid_object.SetActive(true);
                isValid = false;
                ConfigScene.instance.SetMarkUnitRange(pos_CreateUnit, cf_level.GetRange(cur_UnitData.level), false);
            }
        }
       
    }
    public void Setup(UnitData unitData, IngameView parent)
    {
        cam = Camera.main;
        this.parent = parent;
        cur_UnitData = unitData;
        List<UnitData> decks = DataController.instance.GetDeck();
        parent.OnStaminaChange.AddListener(OnStaminaChange);

        config_unit = ConfigManager.instance.configUnit.GetRecordBykeySearch(cur_UnitData.id);
        stamina_lb.text = $"{config_unit.Stamina}";
        cool_down.text = $"{config_unit.Cool_down}";
        name_lb.text = config_unit.Name;
         cf_level = ConfigManager.instance.configUnitLevel.GetRecordBykeySearch(config_unit.ID);
        if (cur_UnitData.level < cf_level.Maxlv)
            level.text = $"Lv {cur_UnitData.level}";
        else
            level.text = "MAX LV ";
        for (int i = 0; i < rare_objects.Length; i++)
        {
            rare_objects[i].SetActive(i + 1 == (int)config_unit.Rare);
            if (i + 1 == (int)config_unit.Rare)
            {
                bg_drag.overrideSprite = rare_objects[i].GetComponent<Image>().sprite;

            }
        }
        icon.overrideSprite = SpriteLibControl.instance.GetSpriteByName(config_unit.Prefab);
        icon_drag.overrideSprite = SpriteLibControl.instance.GetSpriteByName(config_unit.Prefab);
        rect_item_drag = item_drag.GetComponent<RectTransform>();
        lock_object.SetActive(true);
        lock_cd_object.SetActive(false);
    }
    private void OnStaminaChange(int stamina)
    {
        if (stamina >= config_unit.Stamina)
        {
            lock_object.SetActive(false);
        }
        else
        {
            lock_object.SetActive(true);
        }
    }

}
