using UnityEngine;

[System.Serializable]
public class Data
{
    public int material;
    public float energy;
    public int totalExperience, currentlevel;
    public int previousLevelsExperience, nextLevelsExperience;
    public int money;
    public int oil;
    public float incomeMultiplier = 1f;

    public Data(int material, float energy, int totalExperience, int currentlevel, int previousLevelsExperience, int nextLevelsExperience, int money, int oil, float incomeMultiplier)
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
