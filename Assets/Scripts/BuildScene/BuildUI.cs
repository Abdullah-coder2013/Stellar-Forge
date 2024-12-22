using System;
using System.Globalization;
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
        MaterialShower.text = ToAbbreviatedString(data.material);
        EnergyShower.text = ToAbbreviatedString(Mathf.RoundToInt(data.energy));
        MoneyShower.text = ToAbbreviatedString(data.money)+"$";
        OilShower.text = ToAbbreviatedString(data.oil)+" l";
    }
    private void LateUpdate(){
        Data data = SaveSystem.LoadData();
        MaterialShower.text = ToAbbreviatedString(data.material);
        EnergyShower.text = ToAbbreviatedString(Mathf.RoundToInt(data.energy));
        MoneyShower.text = ToAbbreviatedString(data.money)+"$";
        OilShower.text = ToAbbreviatedString(data.oil)+" l";
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
    private long clong(string text){
        return long.Parse(text);
    }

   private string ToAbbreviatedString(long number)
   {
       var quintillion = 1000000000000000000;
       var quadrillion = 1000000000000000;
       var trillion = 1000000000000;
       var billion = 1000000000;
       var million = 1000000;
       var thousand = 1000;
       if (number >= quintillion)
       {
           return MathF.Floor(number / quintillion).ToString(CultureInfo.CurrentCulture) + "." + (number%quintillion).ToString().Substring(0,2) + "AB";
       }
       else if (number < quintillion && number >= quadrillion)
           return MathF.Floor(number / quadrillion).ToString(CultureInfo.CurrentCulture) + "." + (number%quadrillion).ToString().Substring(0,2) + "AA";
       else if (number < quadrillion && number >= trillion)
           return MathF.Floor(number / trillion).ToString(CultureInfo.CurrentCulture) + "." + (number%trillion).ToString().Substring(0,2) + "T";
       else if (number < trillion && number >= billion)
           return MathF.Floor(number / billion).ToString(CultureInfo.CurrentCulture) + "." + (number%billion).ToString().Substring(0,2) + "B";
       else if (number < billion && number >= million)
           return MathF.Floor(number / million).ToString(CultureInfo.CurrentCulture) + "." + (number%million).ToString().Substring(0,2) + "M";
       else if (number < million && number >= thousand)
           return MathF.Floor(number / thousand).ToString(CultureInfo.CurrentCulture) + "." + (number%thousand).ToString().Substring(0,2) + "K";
       else
           return number.ToString();
   }

    public void UpdateMaterial(int amount){
        MaterialShower.text = ToAbbreviatedString(SaveSystem.LoadData().material + amount);
        var existingData = SaveSystem.LoadData();
        SaveSystem.SaveData(new Data(SaveSystem.LoadData().material, existingData.energy, existingData.totalExperience, existingData.currentlevel, existingData.previousLevelsExperience, existingData.nextLevelsExperience, existingData.money, existingData.oil, existingData.incomeMultiplier));
    }
    public void UpdateEnergy(int amount){
        var floatEnergy = Mathf.RoundToInt(SaveSystem.LoadData().energy);
        EnergyShower.text = ToAbbreviatedString(floatEnergy + amount);
        var existingData = SaveSystem.LoadData();
        SaveSystem.SaveData(new Data(existingData.material, clong(EnergyShower.text), existingData.totalExperience, existingData.currentlevel, existingData.previousLevelsExperience, existingData.nextLevelsExperience, existingData.money, existingData.oil, existingData.incomeMultiplier));
    }
    public void UpdateMoney(int amount){
        var money = SaveSystem.LoadData().money;
        MoneyShower.text = ToAbbreviatedString(money + amount) + "$";
        var existingData = SaveSystem.LoadData();
        SaveSystem.SaveData(new Data(existingData.material, existingData.energy, existingData.totalExperience, existingData.currentlevel, existingData.previousLevelsExperience, existingData.nextLevelsExperience, money + amount, existingData.oil, existingData.incomeMultiplier));
    }
    public void UpdateOil(int amount){
        var oil = SaveSystem.LoadData().oil;
        OilShower.text = ToAbbreviatedString(oil + amount) + " l";
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


