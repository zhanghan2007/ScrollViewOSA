using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 列表类别标题的基类，所有的列表类别标题的业务逻辑都应该继承这个类
/// </summary>
public class OSAElementBase_CategoryTitle : MonoBehaviour
{
    /// <summary>
    /// 标题的显示内容
    /// </summary>
    public GameObject content;

    /// <summary>
    /// 显示标题
    /// </summary>
    public virtual void ShowHeader(int index, List<List<object>> datas, MonoBehaviour parent)
    {
        content.SetActiveSelf(true);
    }

    /// <summary>
    /// 关闭标题
    /// </summary>
    public virtual void ClearHeader()
    {
        content.SetActiveSelf(false);
    }
}