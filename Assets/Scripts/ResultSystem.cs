using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ResultSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject resultPanel;

    [SerializeField]
    private TMP_Text resultTitle;

    [SerializeField]
    private TMP_Text resultText;

    private bool resultShown = false;

    void Start()
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (resultShown)
        {
            return;
        }

        if (GameTimer.Instance == null)
        {
            return;
        }

        if (GameTimer.Instance.ElapsedTime
            >= 300.0f)
        {
            ShowResult();
        }
    }

    void ShowResult()
    {
        resultShown = true;

        List<Contestant> contestants =
            new List<Contestant>();

        foreach (
            Contestant contestant
            in Contestant.All
        )
        {
            if (contestant != null)
            {
                contestants.Add(
                    contestant
                );
            }
        }

        contestants =
            contestants
            .OrderByDescending(
                c =>
                c.GetComponent<PlayerCandy>()
                    != null
                ? c.GetComponent<PlayerCandy>()
                    .CandyCount
                : 0
            )
            .ToList();

        if (contestants.Count == 0)
        {
            return;
        }

        int highestCandy =
            GetCandy(
                contestants[0]
            );

        List<string> winners =
            new List<string>();

        foreach (
            Contestant contestant
            in contestants
        )
        {
            if (GetCandy(contestant)
                == highestCandy)
            {
                winners.Add(
                    contestant.DisplayName
                );
            }
        }

        // --------------------
        // タイトル
        // --------------------

        if (winners.Count == 1)
        {
            resultTitle.text =
                winners[0] +
                " WIN!";
        }
        else
        {
            resultTitle.text =
                "DRAW!";
        }

        // --------------------
        // ランキング
        // --------------------

        string text = "";

        for (
            int i = 0;
            i < contestants.Count;
            i++
        )
        {
            Contestant contestant =
                contestants[i];

            text +=
                (i + 1) +
                "st  " +
                contestant.DisplayName +
                "   Candy : " +
                GetCandy(contestant);

            if (GetCandy(contestant)
                == highestCandy)
            {
                text +=
                    "  WINNER";
            }

            text += "\n";
        }

        // 同率優勝者表示
        if (winners.Count > 1)
        {
            text +=
                "\nWINNERS : ";

            for (
                int i = 0;
                i < winners.Count;
                i++
            )
            {
                text += winners[i];

                if (i <
                    winners.Count - 1)
                {
                    text += " / ";
                }
            }
        }

        resultText.text =
            text;

        resultPanel.SetActive(
            true
        );

        // ゲーム終了
        Time.timeScale = 0.0f;
    }

    int GetCandy(
        Contestant contestant
    )
    {
        PlayerCandy candy =
            contestant.GetComponent<
                PlayerCandy
            >();

        if (candy == null)
        {
            return 0;
        }

        return candy.CandyCount;
    }
}