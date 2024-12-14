

[System.Serializable]
public class TimeData
{
    public Task task;
    public float timeRemainingForTask;
    public string lastSavedTime;

    public TimeData(Task task, float timeRemainingForTask, string lastSavedTime) {
        this.task = task;
        this.timeRemainingForTask = timeRemainingForTask;
        this.lastSavedTime = lastSavedTime;
    }
}
