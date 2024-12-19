using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Experience : MonoBehaviour
{
    [SerializeField] private AnimationCurve experienceCurve;
    public long currentlevel, totalExperience;

    public long previousLevelsExperience, nextLevelsExperience;

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private Image experienceFill;

    private void Awake() {
        SaveChecker();
        UpdateLevel();
    }

    private void SaveChecker() {
        if (SaveSystem.LoadData() != null) {
            Data data = SaveSystem.LoadData();
            currentlevel = data.currentlevel;
            totalExperience += data.totalExperience;
            previousLevelsExperience = data.previousLevelsExperience;
            nextLevelsExperience = data.nextLevelsExperience;
            Updatelongerface();
        }
        else {
            totalExperience = 0;
        }
    }

    public void AddExperience(long experienceToAdd)
    {
        totalExperience += experienceToAdd;
        CheckforLevelUp();
        Updatelongerface();
        var existingData = SaveSystem.LoadData();
        SaveSystem.SaveData(new Data(existingData.material, existingData.energy, totalExperience, currentlevel, previousLevelsExperience, nextLevelsExperience, existingData.money, existingData.oil, existingData.incomeMultiplier));
        
    }

    void CheckforLevelUp() {
        if(totalExperience >= nextLevelsExperience) {
            currentlevel++;
            var exdata = SaveSystem.LoadData();
            if (exdata != null)
            {
                exdata.incomeMultiplier += 0.25f;
                SaveSystem.SaveData(exdata);
            }
            UpdateLevel();
    }
    }

    void UpdateLevel() {
        previousLevelsExperience = (long)experienceCurve.Evaluate(currentlevel);
        nextLevelsExperience = (long)experienceCurve.Evaluate(currentlevel+1);
        Updatelongerface();
    }
    void Updatelongerface() {
        long start = totalExperience - previousLevelsExperience;
        
        long end = nextLevelsExperience - previousLevelsExperience;

        levelText.text = currentlevel.ToString();
        experienceText.text = start + " / " + end;
        experienceFill.fillAmount = (float)start / (float)end;
    }

}
