using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using Unity.VisualScripting;

public class PlanetScript : MonoBehaviour
{
    [SerializeField] private Planet planet;
    [SerializeField] private Canvas informationBoard;
    [SerializeField] private Canvas upgradeBoard;

    [SerializeField] private bool forTerraforming;

    [SerializeField] private GameObject TaskPrefab;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private GameObject lockStatus;
    [SerializeField] private Experience experience;

    private BuildUI buildUio;
    private SpriteRenderer sr;
    private List<Task> prefabs = new List<Task>();

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        buildUio = GameObject.Find("UIController").GetComponent<BuildUI>();
        forTerraforming = planet.forTerraforming;
        if (SaveSystem.LoadData() == null) {
            SaveSystem.SaveData(new Data(0, 0, experience.totalExperience, experience.currentlevel, experience.previousLevelsExperience, experience.nextLevelsExperience, 0, 0));
        }
    }
    private void Start() {
        informationBoard.gameObject.SetActive(false);
        if (SaveSystem.LoadPlanet(planet.planetName) != null) {
            planet = SaveSystem.LoadPlanet(planet.planetName);
        }
        else {
            SaveSystem.SavePlanet(planet);
            planet = SaveSystem.LoadPlanet(planet.planetName);
        }
        var names = new List<string>();
        foreach (Task task in planet.tasks) {
            names.Add(task.name);
        }
        StartCoroutine(getMoneyFromTasks(names));
        StartCoroutine(CheckForCompletedTasks());
        StartCoroutine(UnlockPlanet());

        
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
        else if (planet.unlocked == true) {
            lockStatus.SetActive(false);
        }
        
    }

    public void CheckForCompletedTasksAndBuildThem() {
        var names = new List<string>();
        foreach (Task task in planet.tasks) {
            names.Add(task.name);
        }
        List<Task> tasks = SaveSystem.LoadTasks(names);
        
            if (SaveSystem.LoadTasks(names) == null) {
                tasks = planet.tasks;
            }
            foreach (Task task in tasks) {
                if (task.completed == true) {
                    var namess = new List<string>();
                    foreach (Task d in task.dependencies) {
                        namess.Add(d.name);
                    }
                    if (SaveSystem.LoadTasks(namess) == null) {
                        
                    }
                    else {
                        if (planet.forTerraforming == false) {
                        if (!prefabs.Any(t => t.name == task.name))
                {
                    // Instantiate the prefab and add it to the list
                    var prefab = Instantiate(TaskPrefab, spawnPoints[tasks.IndexOf(task)].transform.position, spawnPoints[tasks.IndexOf(task)].transform.rotation, transform);
                    prefab.transform.GetChild(0).GetComponent<Image>().sprite = task.icon;
                    var upgradeBoardScript = upgradeBoard.GetComponent<UpgradeBoard>();
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
                            sr.sprite = task.icon;
                        }
                    }
                    }
                    
                    
                }
                
        }
    }

    IEnumerator getMoneyFromTasks(List<string> names) {
        while (true) {
            yield return new WaitForSeconds(1f);
            var tasks = SaveSystem.LoadTasks(names);
            if (tasks == null) {
                break;
            }
            foreach (Task task in tasks) {
                if (task.completed == true) {
                    if (task.usable == true) {
                    if (task.isOilDepositer == true) {
                var oilgain = SaveSystem.LoadTask(task.name).oilGain;
                buildUio.UpdateOil(oilgain);
            }
            else {
                var moneyGain = SaveSystem.LoadTask(task.name).moneyGain;
                buildUio.UpdateMoney(moneyGain);
            }}
                }
            }
            
            
        }
    }

    

    IEnumerator CheckForCompletedTasks() {
        while (true) {
            yield return new WaitForSeconds(0.6f);
            if (planet.unlocked == true) CheckForCompletedTasksAndBuildThem();
            
        }
    }

    IEnumerator UnlockPlanet()
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
        informationBoardScript.SetTasks(planet.tasks);
        informationBoardScript.SetPlanetName(planet.planetName);
        informationBoardScript.ShowTasks();
        var names = new List<string>();
        foreach (Task task in planet.tasks) {
            names.Add(task.name);
        }
        informationBoardScript.SetNames(names);
    }

}
