using System;
using System.Collections.Generic;
using System.Net.Mime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardsManager : MonoBehaviour
{
    [SerializeField] private Canvas rewardsCanvas;
    [SerializeField] private GameObject star;
    [SerializeField] private GameObject startStar;
    [SerializeField] private Transform startPosition;
    private List<GameObject> stars = new List<GameObject>();
    private Experience experience;
    private readonly float widthOfStar = 940.113f;
    private bool firstTimeOpen = true;
    [SerializeField] private Reward material;
    [SerializeField] private Reward energy;
    [SerializeField] private Reward money;
    [SerializeField] private Reward oil;

     private readonly int maxExpLevel = 99;
     private readonly int lowestExpLevel = 0;
     [SerializeField] private ScrollRect scrollRect;
     [SerializeField] private RectTransform contentPanel;
     [SerializeField] private BuildUI uiController;

     private void SnapTo(Transform target)
     {
         Canvas.ForceUpdateCanvases();

         contentPanel.anchoredPosition =
             (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
             - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
     }
     private void Awake()
     {
         experience = GetComponent<Experience>();
     }

     public void Close()
     {
         rewardsCanvas.gameObject.SetActive(false);
     }

     private void Start()
    {
        rewardsCanvas.gameObject.SetActive(false);
    }

     [System.Serializable]
    private class SerializedRewards
    {
        public List<string> rewards;
        public SerializedRewards(List<string> rewards) { this.rewards = rewards; }
    }
    private class SRewards
    {
        public List<string> serializedRewards;
        public SRewards(List<string> srewards) { this.serializedRewards = srewards; }
    }
    
    private void RewardEvaluater(string rewardStatus, Transform taskInStar, BuildUI uiController, Reward rewardtype, int iteration)
    {
        var stext = "";
        if (System.IO.File.ReadAllText(Application.persistentDataPath + "/SerializedRewards.json") == "" || System.IO.File.ReadAllText(Application.persistentDataPath + "/SerializedRewards.json") == "{}")
        {
            stext += "{\"serializedRewards\":[";
            for (var n = 0; n <= 99; n++)
            {
                if (n == 99)
                {
                    stext += "\"locked\"";
                }
                else
                {
                    stext += "\"locked\",";
                }
                
            }
            stext += "]}";
            System.IO.File.WriteAllText(Application.persistentDataPath + "/SerializedRewards.json", stext);
        }
        else
        {
            stext = System.IO.File.ReadAllText(Application.persistentDataPath + "/SerializedRewards.json");
        }
        var serializedRewards = new SRewards(new List<string>());
        JsonUtility.FromJsonOverwrite(stext, serializedRewards);
        if (rewardStatus != "unlocked" || rewardStatus == "completed") return;
        if (rewardtype == material)
        {
            uiController.UpdateMaterial((int)rewardtype.rewardAmount.Evaluate(iteration-1));
        }
        else if (rewardtype == energy)
        {
            uiController.UpdateEnergy((int)rewardtype.rewardAmount.Evaluate(iteration-1));
        }
        else if (rewardtype == money)
        {
            uiController.UpdateMoney((int)rewardtype.rewardAmount.Evaluate(iteration-1));
        }
        else if (rewardtype == oil)
        {
            if (iteration == 1)
                uiController.UpdateOil((int)rewardtype.rewardAmount.Evaluate(iteration));
            else
            {
                uiController.UpdateOil((int)rewardtype.rewardAmount.Evaluate(iteration-1));
            }
        }
        
        taskInStar.GetChild(2).gameObject.SetActive(true);
        serializedRewards.serializedRewards[iteration-1] = "completed";

        for (var i = 0; i < serializedRewards.serializedRewards.Count; i++)
        {
            serializedRewards.serializedRewards[i] = "\""+serializedRewards.serializedRewards[i]+"\"";
        }
        
        var prejson = new SRewards(new List<string>());
        var rewardsToSaveString = string.Join(",", serializedRewards.serializedRewards);
        rewardsToSaveString = "{\"serializedRewards\":[" + rewardsToSaveString + "]}";
        JsonUtility.FromJsonOverwrite(rewardsToSaveString, prejson);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/" + "SerializedRewards.json", JsonUtility.ToJson(prejson));
    }

    // Update is called once per frame
    public void ShowRewards()
    {
        var text = System.IO.File.ReadAllText(Application.persistentDataPath +"/LevelRewards.json");
        var rewards = new SerializedRewards(new List<string>());
        JsonUtility.FromJsonOverwrite(text, rewards);
        var rewardsl = rewards.rewards;
        var stext = "";
        if (System.IO.File.ReadAllText(Application.persistentDataPath + "/SerializedRewards.json") == "" || System.IO.File.ReadAllText(Application.persistentDataPath + "/SerializedRewards.json") == "{}")
        {
            stext += "{\"serializedRewards\":[";
            for (var n = 0; n <= 99; n++)
            {

                if (n <= experience.currentlevel)
                {
                    if (n == 99)
                    {
                        stext += "\"unlocked\"";
                    }
                    else
                    {
                        stext += "\"unlocked\",";
                    }
                }
                else {
                    if (n == 99)
                    {
                        stext += "\"locked\"";
                    }
                    else
                    {
                        stext += "\"locked\",";
                    }
                }
            }
            stext += "]}";
            System.IO.File.WriteAllText(Application.persistentDataPath + "/SerializedRewards.json", stext);
        }
        else
        {
            stext = System.IO.File.ReadAllText(Application.persistentDataPath + "/SerializedRewards.json");
        }
        
        var serializedRewards = new SRewards(new List<string>());
        JsonUtility.FromJsonOverwrite(stext, serializedRewards);
        var currentLevelStar = gameObject;
        rewardsCanvas.gameObject.SetActive(true);
        
            if (firstTimeOpen)
            {
                var startpos = startPosition.position;
                var rewardtosave = new List<string>();
                for (var n = maxExpLevel; n > lowestExpLevel; n--)
                {
                    if (n != lowestExpLevel)
                    {
                        var starr = Instantiate(star, startpos, Quaternion.identity,
                            rewardsCanvas.transform.Find("Scroll Area/Content"));

                        starr.transform.Find("StarContent/Level").GetComponent<TMP_Text>().text = n.ToString();
                        var taskInStar = starr.transform.Find("StarContent/Task");
                        taskInStar.GetChild(0).GetComponent<TextMeshProUGUI>().text = rewardsl[n - 1];
                        taskInStar.GetChild(2).gameObject.SetActive(false);

                        if (rewardsl[n - 1] == "Material")
                        {
                            taskInStar.GetChild(1).GetComponent<Image>().sprite = material.icon;
                            var nCopy = n;
                            taskInStar.GetComponent<Button>().onClick.AddListener(() =>
                            {
                                RewardEvaluater(serializedRewards.serializedRewards[nCopy - 1], taskInStar,
                                    uiController, material, nCopy);
                            });
                        }
                        else if (rewardsl[n - 1] == "Energy")
                        {
                            taskInStar.GetChild(1).GetComponent<Image>().sprite = energy.icon;
                            var nCopy = n;
                            taskInStar.GetComponent<Button>().onClick.AddListener(() =>
                            {
                                RewardEvaluater(serializedRewards.serializedRewards[nCopy - 1], taskInStar,
                                    uiController, energy, nCopy);
                            });
                        }
                        else if (rewardsl[n - 1] == "Money")
                        {
                            taskInStar.GetChild(1).GetComponent<Image>().sprite = money.icon;
                            var nCopy = n;
                            taskInStar.GetComponent<Button>().onClick.AddListener(() =>
                            {
                                RewardEvaluater(serializedRewards.serializedRewards[nCopy - 1], taskInStar,
                                    uiController, money, nCopy);
                            });
                        }
                        else if (rewardsl[n - 1] == "Oil")
                        {
                            taskInStar.GetChild(1).GetComponent<Image>().sprite = oil.icon;
                            var nCopy = n;
                            taskInStar.GetComponent<Button>().onClick.AddListener(() =>
                            {
                                RewardEvaluater(serializedRewards.serializedRewards[nCopy - 1], taskInStar,
                                    uiController, oil, nCopy);
                            });
                        }

                        startpos.x -= widthOfStar;

                        if (n - 1 == experience.currentlevel)
                        {
                            if (serializedRewards.serializedRewards[n] != "completed")
                                rewardtosave.Add("\"unlocked\"");
                            else
                            {
                                rewardtosave.Add("\"completed\"");
                            }
                            starr.transform.Find("StarContent/Fill").gameObject.GetComponent<Image>().fillAmount =
                                (float)(experience.totalExperience - experience.previousLevelsExperience) /
                                (float)(experience.nextLevelsExperience - experience.previousLevelsExperience);
                        }
                        else if (n - 1 <= experience.currentlevel)
                        {
                            if (serializedRewards.serializedRewards[n] != "completed")
                                rewardtosave.Add("\"unlocked\"");
                            else
                            {
                                rewardtosave.Add("\"completed\"");
                            }
                            taskInStar.GetChild(3).gameObject.SetActive(false);
                            starr.transform.Find("StarContent/Fill").gameObject.GetComponent<Image>()
                                .fillAmount = 1f;
                        }
                        else
                        {
                            rewardtosave.Add("\"locked\"");
                            starr.transform.Find("StarContent/Fill").gameObject.GetComponent<Image>()
                                .fillAmount = 0f;
                        }

                        if (serializedRewards.serializedRewards[n - 1] == "completed")
                        {
                            taskInStar.GetChild(2).gameObject.SetActive(true);
                            taskInStar.GetChild(3).gameObject.SetActive(false);
                        }

                        if (starr.transform.Find("StarContent/Level").GetComponent<TMP_Text>().text ==
                            experience.currentlevel.ToString())
                        {
                            currentLevelStar = starr;
                        }

                        stars.Add(starr);
                    }
                    else
                    {
                        var starr = Instantiate(startStar, startpos, Quaternion.identity, rewardsCanvas.transform.Find("Scroll Area/Content"));
                        starr.transform.Find("StarContent/Level").GetComponent<TMPro.TMP_Text>().text = n.ToString();
                        startpos.x -= widthOfStar;
                        if (starr.transform.Find("StarContent/Level").GetComponent<TMP_Text>().text == experience.currentlevel.ToString())
                        {
                            currentLevelStar = starr;
                        }
                        stars.Add(starr);
                    }
                }
                var prejson = new SRewards(new List<string>());
                rewardtosave.Reverse();
                var rewardsToSaveString = string.Join(",", rewardtosave);
                rewardsToSaveString = "{\"serializedRewards\":[" + rewardsToSaveString + "]}";
                JsonUtility.FromJsonOverwrite(rewardsToSaveString, prejson);
                System.IO.File.WriteAllText(Application.persistentDataPath + "/" + "SerializedRewards.json", JsonUtility.ToJson(prejson));
            }
            else
            {
                var rewardtosave = new List<string>();
                stars.Reverse();
                for (var n = maxExpLevel; n > lowestExpLevel; n--)
                {
                    var starr = stars[n - 1];
                    var taskInStar = stars[n - 1].transform.Find("StarContent/Task");
                    if (serializedRewards.serializedRewards[n - 1] == "unlocked")
                    {
                        taskInStar.GetChild(3).gameObject.SetActive(false);
                    }

                    if (serializedRewards.serializedRewards[n - 1] == "completed")
                    {
                        taskInStar.GetChild(2).gameObject.SetActive(true);
                        taskInStar.GetChild(3).gameObject.SetActive(false);
                    }

                    if (n - 1 == experience.currentlevel)
                    {
                        rewardtosave.Add("\"unlocked\"");
                        starr.transform.Find("StarContent/Fill").gameObject.GetComponent<Image>().fillAmount =
                            (float)(experience.totalExperience - experience.previousLevelsExperience) /
                            (float)(experience.nextLevelsExperience - experience.previousLevelsExperience);
                    }

                    else if (n - 1 < experience.currentlevel)
                    {
                        if (serializedRewards.serializedRewards[n] != "completed")
                            rewardtosave.Add("\"unlocked\"");
                        starr.transform.Find("StarContent/Fill").gameObject.GetComponent<Image>().fillAmount =
                            1f;
                    }
                    else
                    {
                        rewardtosave.Add("\"locked\"");
                        starr.transform.Find("StarContent/Fill").gameObject.GetComponent<Image>().fillAmount =
                            0f;
                    }

                    if (starr.transform.Find("StarContent/Level").GetComponent<TMP_Text>().text ==
                        experience.currentlevel.ToString())
                    {
                        currentLevelStar = starr;
                    }






                }
                var prejson = new SRewards(new List<string>());
                rewardtosave.Reverse();
                var rewardsToSaveString = string.Join(",", rewardtosave);
                rewardsToSaveString = "{\"serializedRewards\":[" + rewardsToSaveString + "]}";
                JsonUtility.FromJsonOverwrite(rewardsToSaveString, prejson);
                System.IO.File.WriteAllText(Application.persistentDataPath + "/" + "SerializedRewards.json", JsonUtility.ToJson(prejson));
            }

            
            

            SnapTo(currentLevelStar.transform);

        firstTimeOpen = false;
    }


}
