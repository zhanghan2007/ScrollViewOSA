using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 类别标题视图, 中转到OSAElementBase_CategoryTitle（中转，业务层不用关心）
/// </summary>
public class OSAViewsHolder_CategoryTitle : CellGroupViewsHolder<OSAViewsHolder_Grid>
{
    private ContentSizeFitter contentSizeFitterComponent;
    private OSAElementBase_CategoryTitle groupElement;
        
    public override void CollectViews()
    {
        base.CollectViews();
        groupElement = root.GetComponent<OSAElementBase_CategoryTitle>();
        contentSizeFitterComponent = root.gameObject.AddComponent<ContentSizeFitter>();
        contentSizeFitterComponent.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        contentSizeFitterComponent.enabled = true;
    }
        
    public void ShowHeader(int index, List<List<object>> datas, MonoBehaviour parent)
    {
        groupElement.ShowHeader(index, datas, parent);
    }

    public void ClearHeader()
    {
        groupElement.ClearHeader();
    }
}
