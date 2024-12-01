using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationBoard : MonoBehaviour
{
    [SerializeField] private Transform spawnRect;
    private float firstXSpawnPosition = -519;
    private float widthOfButton = 233.4507f;
    private int spaceWidth = 10;
    public List<Task> Tasks = new List<Task>();
    private List<GameObject> Buttons = new List<GameObject>();
    [SerializeField] private GameObject ButtonPrefab;
    public List<string> names = new List<string>();
    [SerializeField] private TMP_Text planetName;

    public void SetTasks(List<Task> tasks) { Tasks = tasks; }
    public void SetNames(List<string> names) { this.names = names; }
    public void SetPlanetName(string name) {planetName.text = name;}

    private void Update() {
        if (Tasks == null) {
            return;
        }
        foreach (Task task in Tasks) {
            TaskUnlocked(task);
        }
        
    }
    

    /// <summary>
    /// Instantiates and displays buttons for each task in the Tasks list. 
    /// The buttons are arranged based on their index and display the task's name and icon.
    /// The button's appearance and click behavior change based on the task's unlocked and completed status.
    /// </summary>
    public void ShowTasks() {
        if (SaveSystem.LoadTasks(names) != null) {
            Tasks = SaveSystem.LoadTasks(names);
        }
        else {
            foreach (Task task in Tasks) {
                SaveSystem.SaveTask(task);
            }
            Tasks = SaveSystem.LoadTasks(names);
        }

        foreach (Task task in Tasks) {
            var xpos = firstXSpawnPosition;
            if (spawnRect.childCount == 0) {
                xpos = firstXSpawnPosition;
            }
            else {
                var totalButtonWidth = spawnRect.childCount * widthOfButton;
                var totalSpaceWidth = spawnRect.childCount * spaceWidth;
                xpos = firstXSpawnPosition + totalButtonWidth + totalSpaceWidth;

            }
            
            var button = Instantiate(ButtonPrefab, new Vector3(spawnRect.position.x + xpos, spawnRect.position.y, 0), Quaternion.identity, spawnRect);
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = task.name;
            button.transform.GetChild(1).GetComponent<Image>().sprite = task.icon;
            button.transform.GetChild(2).gameObject.SetActive(true);
            button.transform.GetChild(3).gameObject.SetActive(true);
            if (task.unlocked == true && task.completed == false) {
                button.transform.GetChild(3).gameObject.SetActive(false);
                button.transform.GetChild(2).gameObject.SetActive(false);
                
            }
            else if (task.unlocked == true && task.completed == true) {
                button.transform.GetChild(3).gameObject.SetActive(false);
            }
            else if (task.unlocked == false) {
                button.transform.GetChild(2).gameObject.SetActive(false);
            }
            button.GetComponent<Button>().onClick.AddListener(() => {

                TaskComplete(Tasks.IndexOf(task));
                
                
            });
            Buttons.Add(button);
        }
    }
    public void Back() {
         foreach (GameObject button in Buttons) {
             Destroy(button);
         }
         Buttons.Clear();
         gameObject.SetActive(false);
    }

    private void Refresh(Task task) {
        foreach (GameObject button in Buttons) {
            if (button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == task.name) {
                if (task.unlocked == true && task.completed == false) {
                    button.transform.GetChild(3).gameObject.SetActive(false);
                    button.transform.GetChild(2).gameObject.SetActive(false);
                    
                }
                else if (task.unlocked == true && task.completed == true) {
                    button.transform.GetChild(3).gameObject.SetActive(false);
                }
                else if (task.unlocked == false) {
                    button.transform.GetChild(2).gameObject.SetActive(false);
                }
            }
            
            
        }
    }

    

    public void TaskUnlocked(Task task) {
        var level = SaveSystem.LoadData().currentlevel;
        
            if (task.unlocked == false) {
            if (task.levelRequired <= level) {
                task.Unlock(level);
                SaveSystem.SaveTask(task);
                Refresh(task);
                
            }
        }
    }

    public void TaskComplete(int index)
    {
        if (!Tasks[index].completed)
        {
            if (Tasks[index].unlocked)
            {
                if (Tasks[index] != Tasks[0] && Tasks[index - 1].completed)
                {
                    if (Tasks[index].materialCost <= SaveSystem.LoadData().material)
                    {
                        if (Tasks[index].energyCost <= SaveSystem.LoadData().energy)
                        {
                            var dependencies = new List<string>();
                            foreach (var dependency in Tasks[index].dependencies)
                            {
                                dependencies.Add(dependency.name);
                            }

                            if (SaveSystem.LoadTasks(dependencies) == null)
                            {
                                NotificationManager.AddNotificationToQueue("You must complete tasks: " +
                                                                           string.Join(", ", dependencies));
                                return;
                            }

                            ;
                            var existingData = SaveSystem.LoadData();
                            existingData.material = existingData.material - Tasks[index].materialCost;
                            existingData.energy = existingData.energy - Tasks[index].energyCost;
                            SaveSystem.SaveData(existingData);
                            Tasks[index].Complete();
                            SaveSystem.SaveTask(Tasks[index]);
                            Back();
                        }
                    }
                }
                else if (Tasks[index] == Tasks[0])
                {
                    if (Tasks[index].materialCost <= SaveSystem.LoadData().material)
                    {
                        if (Tasks[index].energyCost <= SaveSystem.LoadData().energy)
                        {
                            var existingData = SaveSystem.LoadData();
                            existingData.material = existingData.material - Tasks[index].materialCost;
                            existingData.energy = existingData.energy - Tasks[index].energyCost;
                            SaveSystem.SaveData(existingData);
                            Tasks[index].Complete();
                            SaveSystem.SaveTask(Tasks[index]);
                            Back();
                        }
                    }
                }
                else
                {
                    NotificationManager.AddNotificationToQueue("You need to complete the previous task first");
                }


            }
            else
            {
                NotificationManager.AddNotificationToQueue("You need to unlock the task first");
            }

        }
        else
        {
            NotificationManager.AddNotificationToQueue("You have already completed this task!");
        }

    }
    
}
