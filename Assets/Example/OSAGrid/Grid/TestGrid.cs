using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestGrid : MonoBehaviour
{
    public OSAAdapter_Grid adapterGrid;
    public int initNum = 100;

    private List<int> _data;

    private void Start()
    {
        _data = new List<int>();
        for (int i = 0; i < initNum; i++)
        {
            _data.Add(Random.Range(0, 1000));
        }
        adapterGrid.RefillCell(_data, this);
    }

    public int GetData(int index)
    {
        return _data[index];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            adapterGrid.ScrollToIndex(50);
        }
    }
}
