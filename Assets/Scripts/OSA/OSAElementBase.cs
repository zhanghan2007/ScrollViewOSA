using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 列表子项基类，所有的子项业务逻辑都应该继承这个类（除了类别标题，列表类别标题应该继承OSAElementBase_CategoryTitle）
/// 刷新的时候会触发UpdateCell方法
/// 子项代码执行顺序：Awake->OnEnable->UpdateCell->Start
/// </summary>
public class OSAElementBase : MonoBehaviour
{
    public LayoutElement layoutElement;
    
    //分类别的列表的格子子项才需要赋值的参数
    public GameObject content;
    

    protected void Awake()
    {
        if (layoutElement == null)
        {
            layoutElement = GetComponent<LayoutElement>();    
        }
    }

    /// <summary>
    /// 常规子项刷新
    /// </summary>
    /// <param name="index"></param>
    /// <param name="parent"></param>
    public virtual void UpdateCell(int index, MonoBehaviour parent)
    {
    }
    
    //
    /// <summary>
    /// 分类别的grid子项刷新
    /// </summary>
    public virtual void UpdateCellByCategory(object dm,int groupIndex,  int index,List<object> datas, MonoBehaviour parent)
    {
    }
    
    public void VisualContent(bool visual)
    {
        layoutElement.ignoreLayout = !visual;
        content.SetActiveSelf( visual);
    }
}
