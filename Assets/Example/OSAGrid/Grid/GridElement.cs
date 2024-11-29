using UnityEngine;
using UnityEngine.UI;

public class GridElement : OSAElementBase
{
    public Text txt;

    public override void UpdateCell(int index, MonoBehaviour parent)
    {
        base.UpdateCell(index, parent);
        var num = ((TestGrid)parent).GetData(index);
        txt.text = $"{index + 1}.随机数{num}";
    }
}
