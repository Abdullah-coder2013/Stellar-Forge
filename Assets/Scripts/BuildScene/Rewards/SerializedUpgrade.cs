using UnityEngine;

[System.Serializable]
public class SerializedUpgrade
{
    public string upgradeName;
    public string level;

    public SerializedUpgrade(string upgradeName, string level)
    {
        this.upgradeName = upgradeName;
        this.level = level;
    }
}
