using System.Collections.Generic;
using UnityEngine;

public class Contestant : MonoBehaviour
{
    public static List<Contestant> All =
        new List<Contestant>();

    [SerializeField]
    private string displayName = "Character";

    [SerializeField]
    private bool isPlayer = false;

    public string DisplayName
    {
        get { return displayName; }
    }

    public bool IsPlayer
    {
        get { return isPlayer; }
    }

    void OnEnable()
    {
        if (!All.Contains(this))
        {
            All.Add(this);
        }
    }

    void OnDisable()
    {
        All.Remove(this);
    }
}