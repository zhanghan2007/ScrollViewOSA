using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryGridElement : OSAElementBase
{
    public Text txtHeroName;

    public override void UpdateCellByCategory(object dm, int groupIndex, int index, List<object> datas, MonoBehaviour parent)
    {
        base.UpdateCellByCategory(dm, groupIndex, index, datas, parent);
        txtHeroName.text = (string)datas[index];
    }
}
