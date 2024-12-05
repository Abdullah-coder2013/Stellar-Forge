using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

public class PlanetScript : MonoBehaviour
{
    [SerializeField] private Planet planet;
    [SerializeField] private Canvas informationBoard;
    [SerializeField] private Canvas upgradeBoard;
    private bool forTerraforming;

    [SerializeField] private GameObject TaskPrefab;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private GameObject lockStatus;
    [SerializeField] private Experience experience;

    private BuildUI buildUio;
    private SpriteRenderer sr;
    private List<Task> prefabs = new List<Task>();
    private UpgradeBoard upgradeBoardScript;
    private List<Task> savedTasks = new List<Task>();

    private void Awake() {
        upgradeBoardScript = upgradeBoard.GetComponent<UpgradeBoard>();
        sr = GetComponent<SpriteRenderer>();
        buildUio = GameObject.Find("UIController").GetComponent<BuildUI>();
        forTerraforming = planet.forTerraforming;
        if (SaveSystem.LoadData() == null) {
            SaveSystem.SaveData(new Data(0, 0, experience.totalExperience, experience.currentlevel, experience.previousLevelsExperience, experience.nextLevelsExperience, 0, 0));
        }
    }
    private void Start() {
        informationBoard.gameObject.SetActive(false);
        var informationBoardScript = informationBoard.GetComponent<InformationBoard>();
        informationBoardScript.TaskBuilt += InformationBoardScriptOnTaskBuilt;
        if (SaveSystem.LoadPlanet(planet.planetName) != null) {
            planet = SaveSystem.LoadPlanet(planet.planetName);
        }
        else {
            SaveSystem.SavePlanet(planet);
            planet = SaveSystem.LoadPlanet(planet.planetName);
        }
        foreach (var newTask in planet.tasks)
        {
            var loadedTask = SaveSystem.LoadTask(newTask.name);
            if (loadedTask)
            {
                savedTasks.Add(loadedTask);
            }
            else
            {
                savedTasks.Add(newTask);
            }
        }
        var names = new List<string>();
        foreach (var task in savedTasks) {
            names.Add(task.name);
        }

        foreach (var task in savedTasks)
        {
            CheckForCompletedTasksAndBuildThem(task);
        }
        StartCoroutine(GetMoneyFromTasks(names));
        StartCoroutine(UnlockPlanet());

        
    }

    private void InformationBoardScriptOnTaskBuilt(object sender, BuiltTaskEventArgs e)
    {
        if (planet == e.planet)
            CheckForCompletedTasksAndBuildThem(e.Task);
    }


    private void PlanetUnlocked(Planet planet) {
        if (SaveSystem.LoadData().currentlevel >= planet.levelNeededToUnlock) {
            planet.Unlocked(SaveSystem.LoadData().currentlevel);
            SaveSystem.SavePlanet(planet);
            
        }
        
    }

    private void CheckIfPlanetIsUnlocked(Planet planet) {
        if (SaveSystem.LoadData().currentlevel >= planet.levelNeededToUnlock) {
            PlanetUnlocked(planet);
        }
        if (planet.unlocked == false) {
            lockStatus.SetActive(true);
        }
        else if (planet.unlocked) {
            lockStatus.SetActive(false);
        }
        
    }

    public void CheckForCompletedTasksAndBuildThem(Task task) {
        if (task.completed)
        {
            var planetFromTask = SaveSystem.LoadPlanet(task.planetName);
            if (planetFromTask.forTerraforming == false) {
                if (!prefabs.Any(t => t.name == task.name))
                {
                    var spawnPoint = GameObject.Find(task.name).transform;
                    // Instantiate the prefab and add it to the list
                    var prefab = Instantiate(TaskPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation, transform);
                    prefab.transform.GetChild(0).GetComponent<Image>().sprite = task.icon;
                    prefab.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => {
                        if (task.usable){
                            upgradeBoardScript.ShowUpgradeBoard(task.name);
                        }
                    });
                    prefab.transform.GetChild(1).gameObject.SetActive(true);
                    prefab.transform.GetChild(1).GetComponent<TimeManager>().SetTask(prefab);
                    prefab.transform.GetChild(1).GetComponent<TimeManager>().StartTimer(task, task.timeNeededinSeconds);
                    experience.AddExperience(task.experienceGain);
                    prefabs.Add(task);
                }
            }
            else {
                if (!prefabs.Any(t => t.name == task.name)) {
                    GameObject.Find(planetFromTask.planetName+"_0").GetComponent<SpriteRenderer>().sprite = task.icon;
                }
            }
                    
                    
                    
        }
    }

    private IEnumerator GetMoneyFromTasks(List<string> names) {
        while (true) {
            yield return new WaitForSeconds(1f);
            var tasks = SaveSystem.LoadTasks(names);
            if (tasks == null) {
                break;
            }
            foreach (Task task in tasks) {
                if (task.completed) {
                    if (task.usable) {
                    if (task.isOilDepositer) {
                var oilgain = task.oilGain;
                buildUio.UpdateOil(oilgain);
            }
            else {
                var moneyGain = task.moneyGain;
                buildUio.UpdateMoney(moneyGain);
            }}
                }
            }
            
            
        }
    }

    private IEnumerator UnlockPlanet()
    {
        while (true) {
            yield return new WaitForSeconds(0.75f);
            CheckIfPlanetIsUnlocked(planet);
        }
    }

    private bool Unlocked() {
        if (SaveSystem.LoadData().currentlevel < planet.levelNeededToUnlock) {
            return false;
        }
        else {
            return true;
        }
    }

    public void ShowInformationBoard() {
        if (!Unlocked()) return;
        if (SaveSystem.LoadData().currentlevel < planet.levelNeededToUnlock) return;
        informationBoard.gameObject.SetActive(true);

        var informationBoardScript = informationBoard.GetComponent<InformationBoard>();
        informationBoardScript.SetTasks(savedTasks);
        informationBoardScript.SetPlanet(planet);
        informationBoardScript.ShowTasks();
        var names = new List<string>();
        foreach (Task task in savedTasks) {
            names.Add(task.name);
        }
        informationBoardScript.SetNames(names);
    }

}
