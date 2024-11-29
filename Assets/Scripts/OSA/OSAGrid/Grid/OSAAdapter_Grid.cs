using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using Com.TheFallenGames.OSA.DataHelpers;
using UnityEngine;

/// <summary>
/// 同尺寸格子或者列表
/// 列表由3部分代码组成：
/// 1. OSAAdapter_Grid 列表控制器
/// 2. OSAViewsHolder_Grid 子项视图（中转，业务层不用关心）
/// 3. OSAElementBase 子项页面逻辑基类
/// 业务逻辑中，
/// 页面持有OSAAdapter_Grid列表，调用RefillCell方法刷新列表
/// 子项页面继承OSAElementBase并重写UpdateCell方法即可
/// </summary>
public class OSAAdapter_Grid : GridAdapter<GridParams, OSAViewsHolder_Grid>
{
    /// <summary>
    /// 父UI，持有所有列表的数据
    /// </summary>
    private MonoBehaviour _uiOwner;
    
    /// <summary>
    /// 刷新element
    /// </summary>
    /// <param name="viewsHolder"></param>
    protected override void UpdateCellViewsHolder(OSAViewsHolder_Grid viewsHolder)
    {
        viewsHolder.UpdateCell(viewsHolder.ItemIndex, _uiOwner);
    }
    
    /// <summary>
    /// 刷新列表数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="uiOwner"></param>
    /// <typeparam name="T"></typeparam>
    public void RefillCell<T>(List<T> data, MonoBehaviour uiOwner)
    {
        _uiOwner = uiOwner;
        var Data = new SimpleDataHelper<T>(this);
        base.Start();
        Data.List.AddRange(data);
        Data.NotifyListChangedExternally();
    }
    
    /// <summary>
    /// 跳到指定位置
    /// </summary>
    /// <param name="index"></param>
    public void ScrollToIndex(int index)
    {
        ScrollTo(index);
    } 
}
