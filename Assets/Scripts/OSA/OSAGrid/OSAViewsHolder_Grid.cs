using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// OSA Grid子项视图, 中转到OSAElementBase（中转，业务层不用关心）
/// </summary>
public class OSAViewsHolder_Grid : CellViewsHolder
{
    private OSAElementBase _element;

    public override void CollectViews()
    {
        views = root;
        rootLayoutElement = root.GetComponent<LayoutElement>();
        _element = root.GetComponent<OSAElementBase>();
    }

    public void UpdateCell(int index, MonoBehaviour parent)
    {
        _element.UpdateCell(index, parent);
    }

    /// <summary>
    /// 分区域的grid子项刷新
    /// </summary>
    public virtual void UpdateCellByCategory(
        OSAAdapter_GridWithCategory.CellModel model, 
        int groupIndex, int index, 
        List<object> datas, 
        MonoBehaviour parent)
    {
        switch (model.type)
        {
            // 有效的数据刷新ui
            case OSAAdapter_GridWithCategory.CellModel.CellType.VALID:
                _element.VisualContent(true);
                _element.UpdateCellByCategory(model.data,groupIndex, index,datas, parent);
                break;
    
            // 占位的ui关闭显示
            case OSAAdapter_GridWithCategory.CellModel.CellType.FOR_ROW_COMPLETION:
            case OSAAdapter_GridWithCategory.CellModel.CellType.IN_ROW_SEPARATING_CATEGORIES:
                _element.VisualContent(false);
                break;
        }
            
            
            
    }
}
