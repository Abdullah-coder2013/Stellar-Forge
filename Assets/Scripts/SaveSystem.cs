using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class SaveSystem
{
    private const string Savename = "/astromash.dat";
    public static void SaveData(Data data) {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + Savename;

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static Data LoadData() {
        var path = Application.persistentDataPath + Savename;
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Data data = formatter.Deserialize(stream) as Data;
            stream.Close();
            return data;
        } else {
            return null;
        }
    }

    public static void SaveTimeData(TimeData timeData) {
        var filename = Application.persistentDataPath + "/timedata" + "/" + timeData.task.name + ".json";
        if (File.Exists(filename)) {
            File.Delete(filename);
        }
        var json = JsonUtility.ToJson(timeData);
        File.WriteAllText(filename, json);
        
    }

    public static TimeData LoadTimeData(Task task) {
        if (!File.Exists(Application.persistentDataPath + "/timedata" + "/" + task.name + ".json")) {
            return null;
        }
        var json = File.ReadAllText(Application.persistentDataPath + "/timedata" + "/" + task.name + ".json");
        var timeData = new TimeData(null,0f,"");
        JsonUtility.FromJsonOverwrite(json, timeData);
        return timeData;
    }

    public static void SaveTask(Task task) {
        var filename = Application.persistentDataPath + "/tasks" + "/" + task.name + ".json";
        if (File.Exists(filename)) {
            File.Delete(filename);
        }
        var json = JsonUtility.ToJson(task);
        File.WriteAllText(filename, json);
    }

    public static List<Task> LoadTasks(List<string> names) {
        foreach (string name in names) {
            if (!File.Exists(Application.persistentDataPath + "/tasks" + "/" + name + ".json")) {
                return null;
            }
        }
        List<Task> tasks = new List<Task>();
        foreach (string name in names) {
            var json = File.ReadAllText(Application.persistentDataPath + "/tasks" + "/" + name + ".json");
            var task = ScriptableObject.CreateInstance<Task>();
            JsonUtility.FromJsonOverwrite(json, task);
            tasks.Add(task);
        }
        return tasks;
    }

    public static Task LoadTask(string name) {
        if (!File.Exists(Application.persistentDataPath + "/tasks" + "/" + name + ".json")) {
            return null;
        }
        var json = File.ReadAllText(Application.persistentDataPath + "/tasks" + "/" + name + ".json");
        var task = ScriptableObject.CreateInstance<Task>();
        JsonUtility.FromJsonOverwrite(json, task);
        return task;
    }

    public static void SavePlanet(Planet planet) {
        var filename = Application.persistentDataPath + "/planets" + "/" + planet.name + ".json";
        if (File.Exists(filename)) {
            File.Delete(filename);
        }
        var json = JsonUtility.ToJson(planet);
        File.WriteAllText(filename, json);
    }

    public static Planet LoadPlanet(string name) {
        var filename = Application.persistentDataPath + "/planets" + "/" + name + ".json";
        if (File.Exists(filename)) {
            var json = File.ReadAllText(filename);
            var planet = ScriptableObject.CreateInstance<Planet>();
            JsonUtility.FromJsonOverwrite(json, planet);
            return planet;
        } else {
            return null;
        }
    }

    public static void SaveUpgrade(SerializedUpgrade upgrade) {
        var filename = Application.persistentDataPath + "/upgrades" + "/" + upgrade.upgradeName + ".json";
        if (File.Exists(filename)) {
            File.Delete(filename);
        }
        var json = JsonUtility.ToJson(upgrade);
        File.WriteAllText(filename, json);
    }

    public static SerializedUpgrade LoadUpgrade(string upgradeName) {
        var filename = Application.persistentDataPath + "/upgrades" + "/" + upgradeName + ".json";
        if (File.Exists(filename)) {
            var json = File.ReadAllText(filename);
            var upgrade = new SerializedUpgrade(upgradeName, "0");
            JsonUtility.FromJsonOverwrite(json, upgrade);
            return upgrade;
        } else {
            return null;
        }
    }

    
}
