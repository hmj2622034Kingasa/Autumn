using UnityEngine;

public class PlayerCandy : MonoBehaviour
{
    [SerializeField]
    private int candyCount = 0;

    public int CandyCount
    {
        get { return candyCount; }
    }

    public void AddCandy(int amount)
    {
        candyCount += amount;

        Debug.Log(
            "Candy : " + candyCount
        );
    }

    public bool HasCandy(int amount)
    {
        return candyCount >= amount;
    }

    public bool SpendCandy(int amount)
    {
        if (candyCount < amount)
        {
            return false;
        }

        candyCount -= amount;

        Debug.Log(
            "Candy Used : " + amount
        );

        Debug.Log(
            "Candy Left : " + candyCount
        );

        return true;
    }

    // キャンディ交換などで使用
    public void SetCandy(int amount)
    {
        candyCount =
            Mathf.Max(0, amount);

        Debug.Log(
            "Candy : " + candyCount
        );
    }
}