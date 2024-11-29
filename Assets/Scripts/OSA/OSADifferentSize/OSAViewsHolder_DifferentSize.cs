using Com.TheFallenGames.OSA.Core;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 不规则子项的视图，中转到OSAElementBase（中转，业务层不用关心）
/// </summary>
public class OSAViewsHolder_DifferentSize : BaseItemViewsHolder
{
    private OSAElementBase _element;
    private ContentSizeFitter _sizeFitter;
    
    public override void CollectViews()
    {
        base.CollectViews();
        _element = root.GetComponent<OSAElementBase>();
        _sizeFitter = root.GetComponent<ContentSizeFitter>();
        _sizeFitter.enabled = false;
    }

    /// <summary>
    /// 将刷新时机中转到OSAElementBase的UpdateCell方法
    /// </summary>
    /// <param name="index"></param>
    /// <param name="parent"></param>
    public void UpdateCell(int index, MonoBehaviour parent)
    {
        _element.UpdateCell(index, parent);
    }
        
    public override void MarkForRebuild()
    {
        base.MarkForRebuild();
        if (_sizeFitter)
            _sizeFitter.enabled = true;
    }

    public override void UnmarkForRebuild()
    {
        if (_sizeFitter)
            _sizeFitter.enabled = false;
        base.UnmarkForRebuild();
    }
}
