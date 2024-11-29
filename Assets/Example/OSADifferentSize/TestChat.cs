using System.Collections.Generic;
using UnityEngine;

public class TestChat : MonoBehaviour
{
    public class ChatData
    {
        public string content;
        public bool isLeft;
    }

    public OSAAdapter_DifferentSize osaAdapterDifferentSize;
    public int initNum = 100;

    private List<string> _randomChatContent;
    private List<ChatData> _chatDatas;

    void Start()
    {
        _randomChatContent = new List<string>
        {
            "左侧短左侧短左侧短", 
            "左侧长左侧长左侧长左侧长左侧长左侧长左侧长左侧长左侧长左侧长左侧长左侧长左侧长左侧长左侧长左侧长左侧长", 
            "右侧短右侧短右侧短", 
            "右侧长右侧长右侧长右侧长右侧长右侧长右侧长右侧长右侧长右侧长右侧长右侧长右侧长右侧长右侧长右侧长右侧长"
        };
        
        _chatDatas = new List<ChatData>();
        for (int i = 0; i < initNum; i++)
        {
            var index = Random.Range(0, _randomChatContent.Count);
            var content = _randomChatContent[index];
            _chatDatas.Add(new ChatData {content = $"{i+1}.{content}", isLeft = index <= 1});
        }
        
        osaAdapterDifferentSize.RefillCell(_chatDatas, this);
    }
    
    /// <summary>
    /// 跳到最底部
    /// </summary>
    private void ScrollToBottom()
    {
        if (!osaAdapterDifferentSize.IsViewportFull()) return;
        osaAdapterDifferentSize.SetNormalizedPosition(0);
        if (_chatDatas.Count > 0)
        {
            osaAdapterDifferentSize.ScrollTo(_chatDatas.Count - 1);
        }
    }
    
    /// <summary>
    /// 新加一条聊天记录
    /// </summary>
    /// <param name="objectChat"></param>
    private void OnAddChat(ChatData objectChat)
    {
        var isBtm = osaAdapterDifferentSize.GetNormalizedPosition() == 0;
        _chatDatas.Add(objectChat);
        osaAdapterDifferentSize.InsertOne(objectChat, !osaAdapterDifferentSize.IsViewportFull());
        if (!objectChat.isLeft || isBtm)
        {
            ScrollToBottom();
        }
    }
    
    public ChatData GetChatData(int index)
    {
        return _chatDatas[index];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnAddChat(new ChatData {content = $"自己的消息：{_randomChatContent[Random.Range(0, _randomChatContent.Count)]}", 
                isLeft = false});
        }
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            OnAddChat(new ChatData {content = $"别人的消息：{_randomChatContent[Random.Range(0, _randomChatContent.Count)]}", 
                isLeft = true});
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            osaAdapterDifferentSize.ScrollToIndex(50);
        }
    }
}
