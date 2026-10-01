using UnityEngine;

public class MidBossTag : MonoBehaviour
{
    public enum MidBossType
    {
        Melee,
        Ranged
    }

    [SerializeField]
    private MidBossType bossType;

    public MidBossType BossType
    {
        get { return bossType; }
    }
}