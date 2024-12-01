using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float globalTime;
    private bool firstTime = true;

    private GameObject taskVisual;
    [SerializeField] private Image showAd;
    private Task task;
    [SerializeField] private TMPro.TMP_Text timeText;
    [SerializeField] private Play play;
    public void SetTask(GameObject task) { taskVisual = task; }

    private void Awake()
    {
        play = GameObject.Find("shipidle/Button/Start").GetComponent<Play>();
    }

    private void Start() {
        play.OnPlay += PlayOnOnPlay;
        showAd.gameObject.SetActive(false);
        
    }

    private void PlayOnOnPlay(object sender, EventArgs e)
    {
        SaveTimeData();
    }

    public void ShowAd() {
        SaveSystem.SaveTimeData(new TimeData(task, globalTime-=120, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
        showAd.gameObject.SetActive(false);
    }

    IEnumerator startTimer(Task task, float time) {
        while (true) {
            yield return new WaitForSeconds(1f);
            if (SaveSystem.LoadTimeData(task) != null) {
                var timer = SaveSystem.LoadTimeData(task).timeRemainingForTask;
                
                if (timer > 0) {
                    if (firstTime) {
                        int timePassed = StringToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).Subtract(StringToDateTime(SaveSystem.LoadTimeData(task).lastSavedTime)).Seconds;
            if (timePassed > timer) {
                timer = 0;
            }
            else {
                timer = timer - timePassed;
            }
            firstTime = false;
                }
            if (timer > 0) {
            timer -= 1f;
            }
            globalTime = timer;
            SaveSystem.SaveTimeData(new TimeData(task, globalTime, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            UpdateUI(timer);
        }
        else {
            task.GetUsable();
            SaveSystem.SaveTask(task);
            StopTimer(task, time);
            taskVisual.transform.GetChild(1).gameObject.SetActive(false);
        }
            }
            else {
                if (time > 0) {
            time -= 1f;
            globalTime = time;
            UpdateUI(time);
        }
        else {
            task.GetUsable();
            SaveSystem.SaveTask(task);
            StopTimer(task, time);
            taskVisual.transform.GetChild(1).gameObject.SetActive(false);
        }
            }
            
        }
    }

    private void OnApplicationQuit() {
        SaveTimeData();
    }

    private void SaveTimeData()
    {
        var timeData = new TimeData(task, globalTime, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        SaveSystem.SaveTimeData(timeData);
    }

    private DateTime StringToDateTime(string time) 
    { 
        DateTime dateTime = DateTime.ParseExact(time, "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);
        return dateTime;
    }

    
    
    public void StartTimer(Task task, float time) { StartCoroutine(startTimer(task, time)); this.task = task; }
    public void StopTimer(Task task, float time) { StopCoroutine(startTimer(task, time)); }

    private void UpdateUI(float time){
        if (time >= 60) {
            timeText.text = Math.Floor(time / 60).ToString() + ":" + (time % 60).ToString() + "m";
        }
        else if (time >= 3600) {
            timeText.text = Math.Floor(time / 3600).ToString() + ":" + Math.Floor(time % 3600 / 60).ToString() + ":" + (time % 60).ToString() + "h";
        }
        else {
            timeText.text = time.ToString() + "s";
        }
        
    }


}
