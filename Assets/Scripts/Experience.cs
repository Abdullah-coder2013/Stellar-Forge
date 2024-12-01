using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Experience : MonoBehaviour
{
    [SerializeField] private AnimationCurve experienceCurve;
    public int currentlevel, totalExperience;

    public int previousLevelsExperience, nextLevelsExperience;

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private Image experienceFill;

    private void Start() {
        SaveChecker();
        UpdateLevel();
    }

    private void SaveChecker() {
        if (SaveSystem.LoadData() != null) {
            Data data = SaveSystem.LoadData();
            currentlevel = data.currentlevel;
            totalExperience = data.totalExperience;
            previousLevelsExperience = data.previousLevelsExperience;
            nextLevelsExperience = data.nextLevelsExperience;
            UpdateInterface();
        }
        else {
            totalExperience = 0;
        }
    }

    public void AddExperience(int experienceToAdd)
    {
        totalExperience += experienceToAdd;
        CheckforLevelUp();
        UpdateInterface();
        var existingData = SaveSystem.LoadData();
        SaveSystem.SaveData(new Data(existingData.material, existingData.energy, totalExperience, currentlevel, previousLevelsExperience, nextLevelsExperience, existingData.money, existingData.oil));
        
    }

    void CheckforLevelUp() {
        if(totalExperience >= nextLevelsExperience) {
            currentlevel++;
            UpdateLevel();
    }
    }

    void UpdateLevel() {
        previousLevelsExperience = (int)experienceCurve.Evaluate(currentlevel);
        nextLevelsExperience = (int)experienceCurve.Evaluate(currentlevel+1);
        UpdateInterface();
    }
    void UpdateInterface() {
        int start = totalExperience - previousLevelsExperience;
        
        int end = nextLevelsExperience - previousLevelsExperience;

        levelText.text = currentlevel.ToString();
        experienceText.text = start + " / " + end;
        experienceFill.fillAmount = (float)start / (float)end;
    }

}
