using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Scriptable Objects/Upgrade")]
public class Upgrade : ScriptableObject
{
    public AnimationCurve priceAndIncomeCurve;
    public string upgradeName;

    public Upgrade(AnimationCurve priceAndIncome, string upgradeName) {
        this.priceAndIncomeCurve = priceAndIncome;
        this.upgradeName = upgradeName;
    }
}
