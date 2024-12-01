using UnityEngine;

[CreateAssetMenu(fileName = "Reward", menuName = "Scriptable Objects/Reward")]
public class Reward : ScriptableObject
{
    public Sprite icon;
    public string rewardName;
    public RewardType rewardType;
    public AnimationCurve rewardAmount;
    
    public bool rewardComplete;
    public bool rewardUnlocked;

    public enum RewardType
    {
        Material,
        Energy,
        Money,
        Oil
    }

    public Reward(Sprite icon, string rewardName, RewardType rewardType, bool rewardComplete,
        bool rewardUnlocked)
    {
        this.icon = icon;
        this.rewardName = rewardName;
        this.rewardType = rewardType;
        this.rewardComplete = rewardComplete;
        this.rewardUnlocked = rewardUnlocked;
    }
}
