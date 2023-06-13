using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardElement : MonoBehaviour
{
    public TextMeshProUGUI rankTXT;
    public TextMeshProUGUI nickNameTXT;
    public TextMeshProUGUI scoreTXT;

    public void SetFetchedData(Datum data)
    {
        rankTXT.text = "Rank : " + data.rank.ToString();
        nickNameTXT.text = data.nickname;
        scoreTXT.text = "Score : " + data.score.ToString();
    }
}
