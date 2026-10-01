using UnityEngine;

public class PlayerEquipmentEffects : MonoBehaviour
{
    private PlayerLoadout loadout;
    private PlayerHealth playerHealth;

    private float nextVampireHealTime = 0.0f;

    void Start()
    {
        loadout =
            GetComponent<PlayerLoadout>();

        playerHealth =
            GetComponent<PlayerHealth>();
    }

    // “G‚ğ“|‚µ‚½‚ÉŒÄ‚Î‚ê‚é
    public void OnEnemyDefeated()
    {
        if (loadout == null ||
            playerHealth == null)
        {
            return;
        }

        int level =
            loadout.GetEquipmentLevel(
                PlayerLoadout.EquipmentType.VampireCape
            );

        // –¢Š
        if (level <= 0)
        {
            return;
        }

        // ˜A‘±‰ñ•œ–h~
        if (Time.time <
            nextVampireHealTime)
        {
            return;
        }

        float healChance =
            GetHealChance(level);

        float random =
            Random.value;

        if (random <= healChance)
        {
            playerHealth.Heal(5.0f);

            nextVampireHealTime =
                Time.time + 0.75f;

            Debug.Log(
                "Vampire Cape Activated!"
            );
        }
    }

    float GetHealChance(int level)
    {
        switch (level)
        {
            case 1:
                return 0.05f;

            case 2:
                return 0.10f;

            default:
                return 0.20f;
        }
    }
}