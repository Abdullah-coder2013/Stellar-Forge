using UnityEngine;

[System.Serializable]
public class Data
{
    public long material;
    public float energy;
    public long totalExperience, currentlevel;
    public long previousLevelsExperience, nextLevelsExperience;
    public long money;
    public long oil;
    public float incomeMultiplier = 1f;

    public Data(long material, float energy, long totalExperience, long currentlevel, long previousLevelsExperience, long nextLevelsExperience, long money, long oil, float incomeMultiplier)
    {
        this.material = material;
        this.energy = energy;
        this.totalExperience = totalExperience;
        this.currentlevel = currentlevel;
        this.previousLevelsExperience = previousLevelsExperience;
        this.nextLevelsExperience = nextLevelsExperience;
        this.money = money;
        this.oil = oil;
        this.incomeMultiplier = incomeMultiplier;
    }
    
}
