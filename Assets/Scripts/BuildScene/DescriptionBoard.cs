using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DescriptionBoard : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text materialCost;
    [SerializeField] private TMP_Text energyCost;
    [SerializeField] private TMP_Text moneyCost;
    [SerializeField] private TMP_Text oilCost;
    [SerializeField] private TMP_Text moneyGain;
    [SerializeField] private TMP_Text oilGain;
    [SerializeField] private TMP_Text experienceGain;
    [SerializeField] private TMP_Text levelRequired;
    [SerializeField] private TMP_Text timeNeededinSeconds;
    [SerializeField] private Button buyButton;
    [SerializeField] private InformationBoard informationBoard;
    public int indexOfTask;

    private void Start()
    {
        HideDescriptionBoard();
    }

    public void SetValues(Task task)
    {
        title.text = task.name;
        description.text = task.description;
        materialCost.text = task.materialCost.ToString() + "$"; 
        energyCost.text = task.energyCost.ToString() + "$";
        moneyCost.text = task.moneyCost.ToString() + "$";
        oilCost.text = task.oilCost.ToString() + "$";
        moneyGain.text = "Money Gain: " + task.moneyGain.ToString() + "$";
        oilGain.text = "Oil Gain: " + task.oilGain.ToString() + "$";
        experienceGain.text = task.experienceGain.ToString() + " exp";
        levelRequired.text = "Level Required: " + task.levelRequired.ToString();
        timeNeededinSeconds.text = "Time Needed: " + task.timeNeededinSeconds.ToString() + " seconds";
    }



    public void ShowDescriptionBoard(Task task, int indexOftask)
    {
        gameObject.SetActive(true);
        SetValues(task);
        indexOfTask = indexOftask;
    }
    public void HideDescriptionBoard()
    {
        gameObject.SetActive(false);
        indexOfTask = 0;
    }

    public void Buy()
    {
        informationBoard.TaskComplete(indexOfTask);
        HideDescriptionBoard();
    }
}
