using TMPro;
using UnityEngine;


public class BuildUI : MonoBehaviour
{
    public TMP_Text MaterialShower;
    public TMP_Text EnergyShower;
    public TMP_Text MoneyShower;
    public TMP_Text OilShower;
    [SerializeField] private Canvas shopUI;
    [SerializeField] private Canvas upgradeUI;

    [SerializeField] private Canvas inGameUI;
    [SerializeField] private Canvas roletteui;

    private void Start(){
        shopUI.gameObject.SetActive(false);
        upgradeUI.gameObject.SetActive(false);
        roletteui.gameObject.SetActive(false);
        Data data = SaveSystem.LoadData();
        MaterialShower.text = data.material.ToString();
        EnergyShower.text = data.energy.ToString();
        MoneyShower.text = data.money.ToString()+"$";
        OilShower.text = data.oil.ToString()+"l";
    }
    private void LateUpdate(){
        Data data = SaveSystem.LoadData();
        MaterialShower.text = data.material.ToString();
        EnergyShower.text = data.energy.ToString();
        MoneyShower.text = data.money.ToString()+"$";
        OilShower.text = data.oil.ToString()+"l";
    }

    public void ShowShop() {
        shopUI.gameObject.SetActive(true);
    }
    public void HideShop() {
        shopUI.gameObject.SetActive(false);
    }
    public void ShowUpgrades() {
        upgradeUI.gameObject.SetActive(true);
    }
    public void HideUpgrades() {
        upgradeUI.gameObject.SetActive(false);
    }
    private int cInt(string text){
        return int.Parse(text);
    }

    public void UpdateMaterial(int amount){
        print(cInt(MaterialShower.text) + amount);
        MaterialShower.text = (cInt(MaterialShower.text) + amount).ToString();
        var existingData = SaveSystem.LoadData();
        SaveSystem.SaveData(new Data(cInt(MaterialShower.text), existingData.energy, existingData.totalExperience, existingData.currentlevel, existingData.previousLevelsExperience, existingData.nextLevelsExperience, existingData.money, existingData.oil, existingData.incomeMultiplier));
    }
    public void UpdateEnergy(float amount){
        var floatEnergy = float.Parse(EnergyShower.text);
        EnergyShower.text = (floatEnergy + amount).ToString();
        var existingData = SaveSystem.LoadData();
        SaveSystem.SaveData(new Data(existingData.material, float.Parse(EnergyShower.text), existingData.totalExperience, existingData.currentlevel, existingData.previousLevelsExperience, existingData.nextLevelsExperience, existingData.money, existingData.oil, existingData.incomeMultiplier));
    }
    public void UpdateMoney(int amount){
        var money = SaveSystem.LoadData().money;
        MoneyShower.text = (money + amount).ToString() + "$";
        var existingData = SaveSystem.LoadData();
        SaveSystem.SaveData(new Data(existingData.material, existingData.energy, existingData.totalExperience, existingData.currentlevel, existingData.previousLevelsExperience, existingData.nextLevelsExperience, money + amount, existingData.oil, existingData.incomeMultiplier));
    }
    public void UpdateOil(int amount){
        var oil = SaveSystem.LoadData().oil;
        OilShower.text = (oil + amount).ToString() + "l";
        var existingData = SaveSystem.LoadData();
        SaveSystem.SaveData(new Data(existingData.material, existingData.energy, existingData.totalExperience, existingData.currentlevel, existingData.previousLevelsExperience, existingData.nextLevelsExperience, existingData.money, oil + amount, existingData.incomeMultiplier));
    }

    public void DisableInGameUI(){
        inGameUI.gameObject.SetActive(false);
    }
    public void EnableInGameUI(){
        inGameUI.gameObject.SetActive(true);
    }
    
}


