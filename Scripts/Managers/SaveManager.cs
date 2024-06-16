using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : Manager {
    public Action OnSave;
    public Action OnLoad;

    public SaveData SaveData;
    [SerializeField] private string dataPath = "/SaveData";

    [SerializeField] private bool useEncryption = false;
    private const string encryptKeyword = "1111111111";

    public override void AwakeManager() {
        base.AwakeManager();
        var service = ServiceManager.Instance;
        service.RegisterManager<SaveManager>(this);

        dataPath = Application.persistentDataPath + dataPath + ".json";
    }

    private void Start() {
        var service = ServiceManager.Instance;
        var uiManager = service.GetManager<UIManager>();
        uiManager.OnPlay += Load;
        uiManager.OnLoseOpen += Delete;
    }

    [ContextMenu("Save")]
    public void Save() {
        OnSave?.Invoke();
        SaveToFile();
    }

    [ContextMenu("Load")]
    public void Load() {
        if (!IsFileExist()) return;
        LoadFromFile();
        OnLoad?.Invoke();
    }

    [ContextMenu("Delete")]
    public void Delete() {
        if (File.Exists(dataPath)) File.Delete(dataPath);
    }

    private void SaveToFile() {
        string json = JsonUtility.ToJson(SaveData, true);
        if (useEncryption) json = EncryptOrDecrypt(json);
        File.WriteAllText(dataPath, json);
    }

    private void LoadFromFile() {
        string json = File.ReadAllText(dataPath);
        if (useEncryption) json = EncryptOrDecrypt(json);
        SaveData = JsonUtility.FromJson<SaveData>(json);
    }

    private string EncryptOrDecrypt(string json) {
        string result = "";
        for (int i = 0; i < json.Length; i++) {
            result += (char)(json[i] ^ encryptKeyword[i % encryptKeyword.Length]);
        }
        return result;
    }

    public bool IsFileExist() => File.Exists(dataPath);
}

[Serializable]
public class SaveData {
    public ResourceData ResourceData;
    public List<GridData> GridDatas;
    public EventData EventData;
}
