using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryTitleElement : OSAElementBase_CategoryTitle
{
    public Text txtTitle;

    public override void ShowHeader(int index, List<List<object>> datas, MonoBehaviour parent)
    {
        base.ShowHeader(index, datas, parent);
        txtTitle.text = ((TestGridWithCategory)parent).GetTitleName(index);
    }
}
