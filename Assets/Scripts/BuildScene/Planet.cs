using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Planet", menuName = "Scriptable Objects/Planet")]
public class Planet : ScriptableObject
{
    public int levelNeededToUnlock;
    public string planetName;
    public Sprite planetSprite;

    public List<Task> tasks;

    public bool unlocked;
    public bool forTerraforming;

    public Planet(string name, Sprite icon, int levelRequired, bool forTerraforming, bool unlocked)
    {
        this.name = name;
        planetSprite = icon;
        levelNeededToUnlock = levelRequired;
        tasks = new List<Task>();
        this.forTerraforming = forTerraforming;
    }
    public bool Unlocked(int level) {
        if (level >= levelNeededToUnlock) {
            unlocked = true;
            return true;
        } else {
            return false;
        }
    }
}
