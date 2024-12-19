using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Task", menuName = "Scriptable Objects/Task")]
public class Task : ScriptableObject
{
    public new string name;
    public string planetName;
    public string description;
    public Sprite icon;
    public long materialCost;
    public long energyCost;
    public long experienceGain;
    public int moneyGain;
    public long levelRequired;
    public Transform placeToBuild;
    public bool unlocked;
    public long oilCost;
    
    public bool completed;

    public bool level2;
    public bool level5;
    public bool level10;
    public bool level15;
    public bool level20;

    public bool isOilDepositer;
    public int oilGain;
    public long moneyCost;
    public List<Task> dependencies = new List<Task>();
    public float timeNeededinSeconds;
    public bool usable;

    public Task(string name, string planetName, string description, Sprite icon, long materialCost, long energyCost, long experienceGain, int moneyGain, long levelRequired, Transform placeToBuild, bool unlocked, bool completed, bool level2, bool level5, bool level10, bool level15, bool level20, bool isOilDepositer, int oilGain, long oilCost, long moneyCost, List<Task> dependencies, float timeNeededinSeconds, bool usable)
    {
        this.name = name;
        this.planetName = planetName;
        this.description = description;
        this.icon = icon;
        this.materialCost = materialCost;
        this.energyCost = energyCost;
        this.experienceGain = experienceGain;
        this.levelRequired = levelRequired;
        this.unlocked = unlocked;
        this.placeToBuild = placeToBuild;
        this.completed = completed;
        this.moneyGain = moneyGain;
        this.level2 = level2;
        this.level5 = level5;
        this.level10 = level10;
        this.level15 = level15;
        this.level20 = level20;
        this.isOilDepositer = isOilDepositer;
        this.oilGain = oilGain;
        this.oilCost = oilCost;
        this.moneyCost = moneyCost;
        this.dependencies = dependencies;
        this.timeNeededinSeconds = timeNeededinSeconds;
        this.usable = usable;
    }
    public bool Complete()
    {
        if (!completed) {
            completed = true;
            return true;
        }
        else {
            return false;
        }
    }
    public bool Unlock(long level)
    {
        if (levelRequired <= level) {
            unlocked = true;
            return true;
        }
        else {
            return false;
        }
    }
    public bool GetUsable() { this.usable = true;return usable; }
    public void SetPlaceToBuild(Transform placeToBuild) { this.placeToBuild = placeToBuild; }
}
