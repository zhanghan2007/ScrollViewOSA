using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Com.TheFallenGames.OSA.DataHelpers;
using UnityEngine;

/// <summary>
/// 子项不规则列表
/// 列表由3部分代码组成：
/// 1. OSAAdapter_DifferentSize 列表控制器
/// 2. OSAViewsHolder_DifferentSize 子项视图（中转，业务层不用关心）
/// 3. OSAElementBase 子项页面逻辑基类
/// 业务逻辑中，
/// 页面持有OSAAdapter_DifferentSize列表，调用RefillCell方法刷新列表
/// 子项页面继承OSAElementBase并重写UpdateCell方法即可
/// </summary>
public class OSAAdapter_DifferentSize : OSA<BaseParamsWithPrefab, OSAViewsHolder_DifferentSize>
{
    private MonoBehaviour _uiOwner;
    private SimpleDataHelper<object> _datas;

    protected override void Awake()
    {
        base.Awake();
        _datas = new SimpleDataHelper<object>(this);
    }

    /// <summary>
    /// 初始化列表
    /// </summary>
    /// <param name="data"></param>
    /// <param name="uiOwner"></param>
    /// <typeparam name="T"></typeparam>
    public void RefillCell<T>(List<T> data, MonoBehaviour uiOwner) where T : class
    {
        _uiOwner = uiOwner;
        base.Start();
        _datas.List.Clear();
        _datas.List.AddRange(data);
        _datas.NotifyListChangedExternally();

        //非等距的情况下，要跳到初始位置需要跳转一次
        if (data.Count > 0)
        {
            if (_Params.Gravity == BaseParams.ContentGravity.END)
            {
                SetNormalizedPosition(0);
            }
            ScrollTo(_Params.Gravity == BaseParams.ContentGravity.END? _datas.Count - 1 : 0);    
        }
    }

    /// <summary>
    /// 创建子项
    /// </summary>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    protected override OSAViewsHolder_DifferentSize CreateViewsHolder(int itemIndex)
    {
        var instance = new OSAViewsHolder_DifferentSize();
        instance.Init(_Params.ItemPrefab, _Params.Content, itemIndex);
        return instance;
    }

    /// <summary>
    /// 刷新子项
    /// </summary>
    /// <param name="newOrRecycled"></param>
    protected override void UpdateViewsHolder(OSAViewsHolder_DifferentSize newOrRecycled)
    {
        newOrRecycled.UpdateCell(newOrRecycled.ItemIndex, _uiOwner);
        newOrRecycled.MarkForRebuild();
        ScheduleComputeVisibilityTwinPass(true);
    }

    /// <summary>
    /// 插入一项
    /// </summary>
    /// <param name="data"></param>
    /// <param name="freezeEndEdge"></param>
    /// <typeparam name="T"></typeparam>
    public void InsertOne<T>(T data, bool freezeEndEdge = false)
    {
        _datas.InsertOne(_datas.List.Count, data, freezeEndEdge);
        if (freezeEndEdge) _datas.NotifyListChangedExternally(true);
    }
    
    /// <summary>
    /// 跳到指定位置，非等距的情况下，要多跳一次
    /// </summary>
    /// <param name="index"></param>
    public void ScrollToIndex(int index)
    {
        ScrollTo(index);
        ScrollTo(index);
    }
    
    /// <summary>
    /// 当前视图是否已满
    /// </summary>
    /// <returns></returns>
    public bool IsViewportFull()
    {
        return GetViewportSize() < GetContentSize();
    }
}
