using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    [SerializeField]
    private float gameTime = 300.0f;

    private float elapsedTime = 0.0f;

    public float ElapsedTime
    {
        get { return elapsedTime; }
    }

    public float RemainingTime
    {
        get { return Mathf.Max(0.0f, gameTime - elapsedTime); }
    }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > gameTime)
        {
            elapsedTime = gameTime;
        }
    }
}