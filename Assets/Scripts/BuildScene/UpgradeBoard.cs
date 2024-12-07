using UnityEngine;
using UnityEngine.UI;
public class UpgradeBoard : MonoBehaviour
{
    [SerializeField] private GameObject level2;
    [SerializeField] private GameObject level5;
    [SerializeField] private GameObject level10;
    [SerializeField] private GameObject level15;
    [SerializeField] private GameObject level20;
    [SerializeField] private TMPro.TMP_Text title;
    [SerializeField] private GameObject upgradeBoard;

    

    private void Start() {
        upgradeBoard.SetActive(false);
    }
    public void ShowUpgradeBoard(string name, Task constantTask) {
        var task = SaveSystem.LoadTask(name);
        title.text = task.name;
        upgradeBoard.SetActive(true);
        level2.transform.GetChild(1).GetComponent<Image>().sprite = constantTask.icon;
        level5.transform.GetChild(1).GetComponent<Image>().sprite = constantTask.icon;
        level10.transform.GetChild(1).GetComponent<Image>().sprite = constantTask.icon;
        level15.transform.GetChild(1).GetComponent<Image>().sprite = constantTask.icon;
        level20.transform.GetChild(1).GetComponent<Image>().sprite = constantTask.icon;
        
        level2.transform.GetChild(4).GetComponent<TMPro.TMP_Text>().text = (task.materialCost * 2).ToString()+"$";
        level5.transform.GetChild(4).GetComponent<TMPro.TMP_Text>().text = (task.materialCost * 5).ToString()+ "$";
        level10.transform.GetChild(4).GetComponent<TMPro.TMP_Text>().text = (task.materialCost * 10).ToString()+ "$";
        level15.transform.GetChild(4).GetComponent<TMPro.TMP_Text>().text = (task.materialCost * 15).ToString()+ "$";
        level20.transform.GetChild(4).GetComponent<TMPro.TMP_Text>().text = (task.materialCost * 20).ToString()+ "$";

        level2.GetComponent<Button>().onClick.AddListener(() => ReactToButton(task, 2));
        level5.GetComponent<Button>().onClick.AddListener(() => ReactToButton(task, 5));
        level10.GetComponent<Button>().onClick.AddListener(() => ReactToButton(task, 10));
        level15.GetComponent<Button>().onClick.AddListener(() => ReactToButton(task, 15));
        level20.GetComponent<Button>().onClick.AddListener(() => ReactToButton(task, 20));

        Refresh(task, 2, level2);
        Refresh(task, 5, level5);
        Refresh(task, 10, level10);
        Refresh(task, 15, level15);
        Refresh(task, 20, level20);

    }

    public void Back() {
        
         level2.transform.GetChild(2).gameObject.SetActive(false);
         level5.transform.GetChild(2).gameObject.SetActive(false);
         level10.transform.GetChild(2).gameObject.SetActive(false);
         level15.transform.GetChild(2).gameObject.SetActive(false);
         level20.transform.GetChild(2).gameObject.SetActive(false);
         level2.GetComponent<Button>().onClick.RemoveAllListeners();
         level5.GetComponent<Button>().onClick.RemoveAllListeners();
         level10.GetComponent<Button>().onClick.RemoveAllListeners();
         level15.GetComponent<Button>().onClick.RemoveAllListeners();
         level20.GetComponent<Button>().onClick.RemoveAllListeners();
         gameObject.SetActive(false);
    }

    private void ReactToButton(Task task, int level) {
        var stask = SaveSystem.LoadTask(task.name);
        if (SaveSystem.LoadData().money >= Cost(level, stask.materialCost)) {
            var exdata = SaveSystem.LoadData();
            exdata.money -= Cost(level, stask.materialCost);
            SaveSystem.SaveData(exdata);
            var newTask = TaskReturner(stask, level);
            Refresh(newTask, level, ButtonReturner(level));
            SaveSystem.SaveTask(newTask);
        }
    }

    private GameObject ButtonReturner(int level) {
        switch (level) {
            case 2:
                return level2;
            case 5:
                return level5;
            case 10:
                return level10;
            case 15:
                return level15;
            case 20:
                return level20;
            default:
                return level2;
        }
    }

    private void Refresh(Task task, int level, GameObject button) {
        if (level == 2) {
            if (task.level2) {
                level2.transform.GetChild(2).gameObject.SetActive(true);
            }
        }
        if (level == 10) {
            if (task.level10) {
                level10.transform.GetChild(2).gameObject.SetActive(true);
            }
        }
        if (level == 15) {
            if (task.level15) {
                level15.transform.GetChild(2).gameObject.SetActive(true);
            }
        }
        if (level == 5) {
            if (task.level5) {
                level5.transform.GetChild(2).gameObject.SetActive(true);
            }
        }
        if (level == 20) {
            if (task.level20) {
                level20.transform.GetChild(2).gameObject.SetActive(true);
            }
        }

    }

    private int Cost(int level, int cost) {
        switch (level) {
            case 2:
                return cost * 2;
            case 5:
                return cost * 5;
            case 10:
                return cost * 10;
            case 15:
                return cost * 15;
            case 20:
                return cost * 20;
            default:
                return cost;
        }
    }

    private Task TaskReturner(Task task, int level) {
        if (task.isOilDepositer == false) {
            switch (level) {
            case 2:
                return new Task(task.name, task.planetName, task.description, task.icon, task.materialCost, task.energyCost, task.experienceGain, task.moneyGain*2, task.levelRequired, task.placeToBuild, task.unlocked, task.completed, true, task.level5, task.level10, task.level15, task.level20, task.isOilDepositer, task.oilGain, task.oilCost, task.moneyCost, task.dependencies, task.timeNeededinSeconds, task.usable);
            case 5:
                return new Task(task.name, task.planetName, task.description, task.icon, task.materialCost, task.energyCost, task.experienceGain, task.moneyGain*2, task.levelRequired, task.placeToBuild, task.unlocked, task.completed, task.level2, true, task.level10, task.level15, task.level20, task.isOilDepositer, task.oilGain, task.oilCost, task.moneyCost, task.dependencies, task.timeNeededinSeconds, task.usable);
            case 10:
                return new Task(task.name, task.planetName, task.description, task.icon, task.materialCost, task.energyCost, task.experienceGain, task.moneyGain*2, task.levelRequired, task.placeToBuild, task.unlocked, task.completed, task.level2, task.level5, true, task.level15, task.level20, task.isOilDepositer, task.oilGain, task.oilCost, task.moneyCost, task.dependencies, task.timeNeededinSeconds, task.usable);
            case 15:
                return new Task(task.name, task.planetName, task.description, task.icon, task.materialCost, task.energyCost, task.experienceGain, task.moneyGain*2, task.levelRequired, task.placeToBuild, task.unlocked, task.completed, task.level2, task.level5, task.level10, true, task.level20, task.isOilDepositer, task.oilGain, task.oilCost, task.moneyCost, task.dependencies, task.timeNeededinSeconds, task.usable);
            case 20:
                return new Task(task.name, task.planetName, task.description, task.icon, task.materialCost, task.energyCost, task.experienceGain, task.moneyGain*2, task.levelRequired, task.placeToBuild, task.unlocked, task.completed, task.level2, task.level5, task.level10, task.level15, true, task.isOilDepositer, task.oilGain, task.oilCost, task.moneyCost, task.dependencies, task.timeNeededinSeconds, task.usable);
            default:
                return task;
            }
        }
        else {
            switch (level) {
            case 2:
                return new Task(task.name, task.planetName, task.description, task.icon, task.materialCost, task.energyCost, task.experienceGain, task.moneyGain, task.levelRequired, task.placeToBuild, task.unlocked, task.completed, true, task.level5, task.level10, task.level15, task.level20, task.isOilDepositer, task.oilGain*2, task.oilCost, task.moneyCost, task.dependencies, task.timeNeededinSeconds, task.usable);
            case 5:
                return new Task(task.name, task.planetName, task.description, task.icon, task.materialCost, task.energyCost, task.experienceGain, task.moneyGain, task.levelRequired, task.placeToBuild, task.unlocked, task.completed, task.level2, true, task.level10, task.level15, task.level20, task.isOilDepositer, task.oilGain*2, task.oilCost, task.moneyCost, task.dependencies, task.timeNeededinSeconds, task.usable);
            case 10:
                return new Task(task.name, task.planetName, task.description, task.icon, task.materialCost, task.energyCost, task.experienceGain, task.moneyGain, task.levelRequired, task.placeToBuild, task.unlocked, task.completed, task.level2, task.level5, true, task.level15, task.level20, task.isOilDepositer, task.oilGain*2, task.oilCost, task.moneyCost, task.dependencies, task.timeNeededinSeconds, task.usable);
            case 15:
                return new Task(task.name, task.planetName, task.description, task.icon, task.materialCost, task.energyCost, task.experienceGain, task.moneyGain, task.levelRequired, task.placeToBuild, task.unlocked, task.completed, task.level2, task.level5, task.level10, true, task.level20, task.isOilDepositer, task.oilGain*2, task.oilCost, task.moneyCost, task.dependencies, task.timeNeededinSeconds, task.usable);
            case 20:
                return new Task(task.name, task.planetName, task.description, task.icon, task.materialCost, task.energyCost, task.experienceGain, task.moneyGain, task.levelRequired, task.placeToBuild, task.unlocked, task.completed, task.level2, task.level5, task.level10, task.level15, true, task.isOilDepositer, task.oilGain*2, task.oilCost, task.moneyCost, task.dependencies, task.timeNeededinSeconds, task.usable);
            default:
                return task;
    }
        }
        
}
}
