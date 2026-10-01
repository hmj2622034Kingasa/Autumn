using UnityEngine;
using TMPro;

public class CandyUI : MonoBehaviour
{
    [SerializeField]
    private PlayerCandy playerCandy;

    [SerializeField]
    private TMP_Text candyText;

    void Update()
    {
        if (playerCandy == null || candyText == null)
        {
            return;
        }

        candyText.text = "Candy : " + playerCandy.CandyCount;
    }
}