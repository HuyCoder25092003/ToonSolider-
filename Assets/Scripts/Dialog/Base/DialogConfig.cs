using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogIndex
{
    PauseDialog=1,
    WinDialog=2,
    FailDialog=3,
    DeckEquipDialog=4,
    InfoUnitDialog=5,
    SettingsDialog = 6,
}
public class DialogParam
{

}
public class WinDialogParam: DialogParam
{
    public ConfigMissionRecord cf_mission;
}
public class DeckEquipDialogParam : DialogParam
{
    public UnitData unitData;
}
public class InfoUnitDialogParam : DialogParam
{
    public ConfigUnitRecord cf_unit;
}
public class DialogConfig 
{
    public static DialogIndex[] dialogIndices =
    {
        DialogIndex.PauseDialog,
        DialogIndex.WinDialog,
        DialogIndex.FailDialog,
        DialogIndex.DeckEquipDialog,
        DialogIndex.InfoUnitDialog,
        DialogIndex.SettingsDialog,
    };
}
