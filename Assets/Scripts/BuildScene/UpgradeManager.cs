using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private Upgrade upgrade;
    [SerializeField] private TMPro.TMP_Text title;
    [SerializeField] private TMPro.TMP_Text cost;
    public int level = 1;

    private void Start() {
        level = int.Parse(SaveSystem.LoadUpgrade(upgrade.upgradeName).level);
    }

    private void Update() {
        title.text = upgrade.upgradeName;
        cost.text = (int)upgrade.priceAndIncomeCurve.Evaluate(level) + "$";
    }

    public void BuyUpgrade() {
        level++;
        var cost = (int)upgrade.priceAndIncomeCurve.Evaluate(level);
        if (SaveSystem.LoadData().money >= cost) {
            var BuildUI = GameObject.Find("UIController").GetComponent<BuildUI>();
            BuildUI.UpdateMoney(-cost);
            var upgradeToSave = new SerializedUpgrade(upgrade.upgradeName, level.ToString());
            SaveSystem.SaveUpgrade(upgradeToSave);
        }
        else {
            level--;
        }
    }
}
