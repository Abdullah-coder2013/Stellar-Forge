using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InformationBoard : MonoBehaviour
{
    [SerializeField] private Transform spawnRect;
    private float firstXSpawnPosition = -519;
    private float widthOfButton = 233.4507f;
    private long spaceWidth = 10;
    public List<Task> tasks;
    private List<GameObject> buttons = new List<GameObject>();
    [FormerlySerializedAs("ButtonPrefab")] [SerializeField] private GameObject buttonPrefab;
    public List<string> names = new List<string>();
    public List<Task> constantTasks;
    public TMP_Text planetName;
    public Planet planet;
    private AudioSource audioSource;
    public AudioClip buttonClicked;
    [SerializeField] private DescriptionBoard descriptionBoard;

    public event System.EventHandler<BuiltTaskEventArgs> TaskBuilt;

    private void Awake()
    {
        audioSource = GameObject.Find("UI SFX").GetComponent<AudioSource>();
    }

    private void Update() {
        if (tasks == null) {
            return;
        }
        foreach (Task task in tasks) {
            TaskUnlocked(task);
        }
        
    }
    

    /// <summary>
    /// Instantiates and displays buttons for each task in the Tasks list. 
    /// The buttons are arranged based on their index and display the task's name and icon.
    /// The button's appearance and click behavior change based on the task's unlocked and completed status.
    /// </summary>
    public void ShowTasks() {
        
        foreach (var task in tasks) {
            var constantTask = constantTasks.Find(x => x.name == task.name);
            var xpos = firstXSpawnPosition;
            if (spawnRect.childCount == 0) {
                xpos = firstXSpawnPosition;
            }
            else {
                var totalButtonWidth = spawnRect.childCount * widthOfButton;
                var totalSpaceWidth = spawnRect.childCount * spaceWidth;
                xpos = firstXSpawnPosition + totalButtonWidth + totalSpaceWidth;

            }
            
            var button = Instantiate(buttonPrefab, new Vector3(spawnRect.position.x + xpos, spawnRect.position.y, 0), Quaternion.identity, spawnRect);
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = task.name;
            button.transform.GetChild(1).GetComponent<Image>().sprite = constantTask.icon;
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

                descriptionBoard.ShowDescriptionBoard(task, tasks.IndexOf(task));
                audioSource.PlayOneShot(buttonClicked);
                
            });
            buttons.Add(button);
        }
    }
    public void Back() {
         foreach (GameObject button in buttons) {
             Destroy(button);
         }
         buttons.Clear();
         gameObject.SetActive(false);
    }

    private void Refresh(Task task) {
        foreach (GameObject button in buttons) {
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
        if (!tasks[index].completed)
        {
            if (tasks[index].unlocked)
            {
                if (tasks[index] != tasks[0] && tasks[index - 1].completed)
                {
                    if (tasks[index].levelRequired != SaveSystem.LoadData().currentlevel)
                    {
                        NotificationManager.AddNotificationToQueue("You must reach level " +
                                                                   tasks[index].levelRequired + " to complete this task");
                        return;
                    }
                    if (tasks[index].materialCost <= SaveSystem.LoadData().material)
                    {
                        if (tasks[index].energyCost <= SaveSystem.LoadData().energy)
                        {
                            var dependencies = new List<string>();
                            foreach (var dependency in tasks[index].dependencies)
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
                            existingData.material = existingData.material - tasks[index].materialCost;
                            existingData.energy = existingData.energy - tasks[index].energyCost;
                            existingData.money = existingData.money - tasks[index].moneyCost;
                            existingData.oil = existingData.oil - tasks[index].oilCost;
                            SaveSystem.SaveData(existingData);
                            tasks[index].Complete();
                            SaveSystem.SaveTask(tasks[index]);
                            TaskBuilt?.Invoke(this, new BuiltTaskEventArgs(tasks[index], planet, constantTasks[index]));
                            Back();
                        }
                    }
                }
                else if (tasks[index] == tasks[0])
                {
                    if (tasks[index].levelRequired != SaveSystem.LoadData().currentlevel)
                    {
                        NotificationManager.AddNotificationToQueue("You must reach level " +
                                                                   tasks[index].levelRequired + " to complete this task");
                        return;
                    }
                    if (tasks[index].materialCost <= SaveSystem.LoadData().material)
                    {
                        if (tasks[index].energyCost <= SaveSystem.LoadData().energy)
                        {
                            var existingData = SaveSystem.LoadData();
                            existingData.material = existingData.material - tasks[index].materialCost;
                            existingData.energy = existingData.energy - tasks[index].energyCost;
                            SaveSystem.SaveData(existingData);
                            tasks[index].Complete();
                            SaveSystem.SaveTask(tasks[index]);
                            TaskBuilt?.Invoke(this, new BuiltTaskEventArgs(tasks[index], planet, constantTasks[index]));
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
