using UnityEngine;

[CreateAssetMenu(fileName = "BossSo", menuName = "Scriptable Objects/BossSo")]
public class BossSo : ScriptableObject
{
    public Sprite sprite;
    public Sprite attackSprite;
    
    public BossSo(Sprite sprite, Sprite attackSprite)
    {
        this.sprite = sprite; 
        this.attackSprite = attackSprite;
    }
}
