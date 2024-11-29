using System.Collections.Generic;
using UnityEngine;

public class TestGridWithCategory : MonoBehaviour
{
    public OSAAdapter_GridWithCategory grid;

    private List<List<string>> _dataHeroName;

    private List<string> _dataTitleName;

    private void Start()
    {
        _dataHeroName = new List<List<string>>();
        _dataTitleName = new List<string>();

        var id = 1;
        for (int i = 0; i < 10; i++)
        {
            _dataTitleName.Add($"T{i}阵容");
            var list = new List<string>();
            for (int j = 0; j < i+2; j++)
            {
                list.Add($"{id}号英雄");
                id++;
            }
            _dataHeroName.Add(list);
        }
        grid.RefillCell(_dataHeroName, this);
    }
    
    public string GetTitleName(int index)
    {
        return _dataTitleName[index];
    }
    
    public string GetHeroName(int row, int col)
    {
        return _dataHeroName[row][col];
    }
}
