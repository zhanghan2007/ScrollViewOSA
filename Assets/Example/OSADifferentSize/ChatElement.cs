using UnityEngine;
using UnityEngine.UI;

public class ChatElement : OSAElementBase
{
    public GameObject goLeftRoot;
    public GameObject goRightRoot;
    public Text txtContentLeft;
    public Text txtContentRight;

    public override void UpdateCell(int index, MonoBehaviour parent)
    {
        base.UpdateCell(index, parent);
        var win = (TestChat)parent;
        var data = win.GetChatData(index);
        SetInfo(data.content, data.isLeft);
    }

    public void SetInfo(string content, bool isLeft)
    {
        goLeftRoot.SetActive(isLeft);
        goRightRoot.SetActive(!isLeft);
        var t = isLeft? txtContentLeft : txtContentRight;
        t.text = content;
    }
}
