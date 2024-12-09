using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoDialogStatControl : MonoBehaviour
{
    public TMP_Text stat_lb;
    public Slider progress_val;
    public void SetupStat(int cur_val, int max_val)
    {
        stat_lb.text = $"{cur_val}/{max_val}";
        progress_val.value = (float)cur_val / (float)max_val;
    }
    public void SetupStatInvert(int cur_val, int max_val)
    {
        stat_lb.text = $"{cur_val}/{max_val}";
        progress_val.value = (float)max_val / (float)cur_val;
    }
    public void SetupStatInvert(float cur_val, float max_val)
    {
        stat_lb.text = $"{cur_val}/{max_val}";
        progress_val.value = max_val / cur_val;
    }
}
