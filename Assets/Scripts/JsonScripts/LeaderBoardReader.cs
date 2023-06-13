using UnityEngine;
using System.Collections.Generic;

public class LeaderBoardReader : MonoBehaviour
{
    public TextAsset LBText;
    public Root elementList = new Root();

    private void Awake()
    {
        elementList = JsonUtility.FromJson<Root>(LBText.text);
    }

    public Datum RequestElementData(int index)
    {
        if (index < 0) index = Mathf.Max(index, 0);
        else index = Mathf.Min(index, elementList.data.Count - 1);

        return elementList.data[index];
    }

}

[System.Serializable]
public class Datum
{
    public int rank;
    public string nickname;
    public int score;
}

[System.Serializable]
public class Root
{
    public int page;
    public bool is_last;
    public List<Datum> data;
}