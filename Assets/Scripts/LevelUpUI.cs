using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpUI : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private Button button1;

    [SerializeField]
    private Button button2;

    [SerializeField]
    private Button button3;

    [SerializeField]
    private TMP_Text text1;

    [SerializeField]
    private TMP_Text text2;

    [SerializeField]
    private TMP_Text text3;

    private LevelUpSystem levelUpSystem;

    private void Start()
    {
        if (panel != null) 
        {
            panel.SetActive(false);
        }
    }

    public void Open(
        LevelUpSystem system,
        string option1,
        string option2,
        string option3
    )
    {
        levelUpSystem = system;

        panel.SetActive(true);

        text1.text = option1;
        text2.text = option2;
        text3.text = option3;

        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
        button3.onClick.RemoveAllListeners();

        button1.onClick.AddListener(
            () => SelectOption(0)
        );

        button2.onClick.AddListener(
            () => SelectOption(1)
        );

        button3.onClick.AddListener(
            () => SelectOption(2)
        );
    }

    void SelectOption(int index)
    {
        if (levelUpSystem == null)
        {
            return;
        }

        panel.SetActive(false);

        levelUpSystem.SelectOption(index);
    }
}