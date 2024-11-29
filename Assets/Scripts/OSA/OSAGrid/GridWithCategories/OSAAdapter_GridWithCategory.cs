using System;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using UnityEngine;

/// <summary>
/// 分类格子列表，包括类别标题和格子子项
/// 列表由5部分代码组成：
/// 1. OSAAdapter_GridWithCategory 列表控制器
/// 2. OSAViewsHolder_CategoryTitle 类别标题视图（中转，业务层不用关心）
/// 3. OSAElementBase_CategoryTitle 类别标题逻辑基类
/// 4. OSAViewsHolder_Grid 格子子项视图（中转，业务层不用关心）
/// 5. OSAElementBase 格子子项逻辑基类
/// 业务逻辑中，
/// 页面持有OSAAdapter_GridWithCategory列表，调用RefillCell方法刷新列表
/// 类别标题页面继承OSAElementBase_CategoryTitle并重写ShowHeader方法即可
/// 格子子项页面继承OSAElementBase并重写UpdateCellByCategory方法即可
/// </summary>
public class OSAAdapter_GridWithCategory : GridAdapter<
    OSAAdapter_GridWithCategory.CategoriyGridParams,
    OSAViewsHolder_Grid>
{
    /// <summary>
    /// 类别标题数据模型（本框架内部使用，玩家不用关心这个类）
    /// </summary>
    public class CategoryModel
    {
        public int index;
        public List<CellModel> items;
    }

    /// <summary>
    /// 子项数据模型（其中包括了占位的子项，不只是玩家真正的数据，本框架内部使用，玩家不用关心这个类）
    /// </summary>
    public class CellModel
    {
        public CategoryModel parentCategory;
        public CellType type;

        public enum CellType
        {
            //有效的玩家数据
            VALID,

            //子项行的空数据，用于补全占位
            FOR_ROW_COMPLETION,

            //标题行的空数据，用于补全占位
            IN_ROW_SEPARATING_CATEGORIES
        }

        public object data;
        public int groupIndex;
        public int index;
    }

    [Serializable]
    public class CategoriyGridParams : GridParams
    {
        [SerializeField]
        GameObject _CellGroupPrefab = null;

        protected override GameObject CreateCellGroupPrefabGameObject()
        {
            return Instantiate(_CellGroupPrefab);
        }
    }

    /// <summary>
    /// 父UI，持有所有列表的数据
    /// </summary>
    private MonoBehaviour _uiOwner;

    /// <summary>
    /// 玩家真正的数据
    /// </summary>
    private List<List<object>> _realDatas;

    private List<CellModel> _cellModels;
    private List<CategoryModel> _categoryModels;

    protected override void Start()
    {
        _cellModels = new List<CellModel>(0);
        _realDatas = new List<List<object>>();
        base.Start();
    }

    /// <summary>
    /// 初始化列表
    /// </summary>
    /// <param name="datas">玩家的真实数据</param>
    /// <param name="uiOwner"></param>
    /// <typeparam name="T"></typeparam>
    public void RefillCell<T>(List<List<T>> datas, MonoBehaviour uiOwner) where T : class
    {
        _uiOwner = uiOwner;
        _realDatas.Clear();
        var categories = new List<CategoryModel>();
        for (int i = 0; i < datas.Count; i++)
        {
            var groupIdx = i;
            var datagroups = datas[i];
            var c = new CategoryModel { items = new List<CellModel>(), index = groupIdx };

            var realDataList = new List<object>();

            for (int j = 0; j < datagroups.Count; j++)
            {
                var itemIdx = j;
                c.items.Add(new CellModel
                {
                    parentCategory = c,
                    type = CellModel.CellType.VALID,
                    data = datagroups[j],
                    groupIndex = groupIdx,
                    index = itemIdx
                });
                realDataList.Add(datagroups[j]);
            }

            categories.Add(c);
            _realDatas.Add(realDataList);
        }

        ResetData(categories);
    }

    /// <summary>
    /// 刷新头部
    /// </summary>
    /// <param name="newOrRecycled"></param>
    protected override void UpdateViewsHolder(CellGroupViewsHolder<OSAViewsHolder_Grid> newOrRecycled)
    {
        base.UpdateViewsHolder(newOrRecycled);

        // This is a Row prefab. We're updating it as a header if it's the special kind of row (the empty row separating our categories)
        if (newOrRecycled.NumActiveCells > 0)
        {
            var firstCellVH = newOrRecycled.ContainingCellViewsHolders[0];
            int modelIndex = firstCellVH.ItemIndex;
            var model = _cellModels[modelIndex];
            var newOrRecycledCasted = newOrRecycled as OSAViewsHolder_CategoryTitle;
            if (model.type == CellModel.CellType.IN_ROW_SEPARATING_CATEGORIES)
            {
                newOrRecycledCasted?.ShowHeader(model.groupIndex, _realDatas, _uiOwner);
            }
            else
            {
                newOrRecycledCasted?.ClearHeader();
            }
        }

        // Constantly triggering a twin pass after the current normal pass, so the CSFs will be updated
        // This can be optimized by keeping track of what items already had their size calculated, but for our purposes it's enough
        ScheduleComputeVisibilityTwinPass();
    }

    /// <summary>
    /// 刷新子项
    /// </summary>
    /// <param name="viewsHolder"></param>
    protected override void UpdateCellViewsHolder(OSAViewsHolder_Grid viewsHolder)
    {
        var model = _cellModels[viewsHolder.ItemIndex];
        viewsHolder.UpdateCellByCategory(model, model.groupIndex, model.index, _realDatas[model.groupIndex], _uiOwner);
    }

    public void ResetData(List<CategoryModel> categories)
    {
        _categoryModels = categories;
        var cellsPerRow = _Params.CurrentUsedNumCellsPerGroup;
        var cellsList = ConvertCategoriesToListOfItemModels(cellsPerRow, _categoryModels);
        _cellModels = cellsList;

        // Since we don't use a DataHelper to notify OSA for us, we do it manually
        ResetItems(_cellModels.Count, false, true); // the last 2 params are not important. Can be omitted if you want
    }

    protected override CellGroupViewsHolder<OSAViewsHolder_Grid> GetNewCellGroupViewsHolder()
    {
        // Create cell group holders of our custom type (which stores the CSF component)
        return new OSAViewsHolder_CategoryTitle();
    }

    private List<CellModel> ConvertCategoriesToListOfItemModels(int itemSlotsPerRow, List<CategoryModel> categories)
    {
        var finalCells = new List<CellModel>();
        for (int i = 0; i < categories.Count; i++)
        {
            var cat = categories[i];

            // Insert an empty row of items to make room for the category's header
            for (int j = 0; j < itemSlotsPerRow; j++)
            {
                var m = CreateItemModelInRowSeparatingCategories(cat);
                var groupIdx = i;
                m.groupIndex = groupIdx;
                finalCells.Add(m);
            }

            // Add the actual cells
            for (int j = 0; j < cat.items.Count; j++)
                finalCells.Add(cat.items[j]);

            // If the category's last row is not full, fill it with empty slots, so they won't be occupied with the ones from the next category
            while (finalCells.Count % itemSlotsPerRow != 0)
                finalCells.Add(CreateItemModelForRowCompletion(cat));
        }

        return finalCells;
    }

    private CellModel CreateItemModelInRowSeparatingCategories(CategoryModel parentCategory)
    {
        return new CellModel { parentCategory = parentCategory, type = CellModel.CellType.IN_ROW_SEPARATING_CATEGORIES };
    }

    private CellModel CreateItemModelForRowCompletion(CategoryModel parentCategory)
    {
        return new CellModel { parentCategory = parentCategory, type = CellModel.CellType.FOR_ROW_COMPLETION };
    }
}