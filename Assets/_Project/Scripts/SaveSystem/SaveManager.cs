using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private string savePath;

    public SaveData CurrentSaveData { get; private set; }

    // -----------------------------
    // Runtimeキャラクター管理
    // -----------------------------
    public List<CharacterRuntime> runtimeCharacters = new List<CharacterRuntime>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "save.json");
        Load();

        InitializeRuntimeCharacters();
    }

    // -----------------------------
    // 保存 / 読込
    // -----------------------------
    public void Save()
    {
        // Runtime → SaveData に変換
        CurrentSaveData.characters = runtimeCharacters.Select(r => r.ToSaveData()).ToList();

        string json = JsonUtility.ToJson(CurrentSaveData, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"💾 Save Completed: {savePath}");
    }

    public void Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            CurrentSaveData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("📂 Save Loaded");
        }
        else
        {
            CurrentSaveData = new SaveData();
            Debug.Log("🆕 New Save Created");
        }
    }

    // -----------------------------
    // Runtimeキャラクター初期化
    // -----------------------------
    private void InitializeRuntimeCharacters()
    {
        runtimeCharacters.Clear();

        foreach (var charSave in CurrentSaveData.characters)
        {
            var template = CharacterDatabase.Instance.GetCharacterByName(charSave.characterName);
            if (template != null)
            {
                runtimeCharacters.Add(new CharacterRuntime(template, charSave));
                Debug.Log($"🔄 Runtimeキャラクター初期化: {charSave.characterName}");
            }
        }
    }

    // -----------------------------
    // 初回生成 or 取得
    // -----------------------------
    public CharacterRuntime GetOrCreateRuntimeCharacter(CharacterData template)
    {
        // 既存Runtimeを検索
        var runtime = runtimeCharacters.Find(r => r.template.characterName == template.characterName);

        if (runtime == null)
        {
            // 新規Runtime作成
            runtime = new CharacterRuntime(template);
            runtimeCharacters.Add(runtime);

            // SaveData にも追加
            CurrentSaveData.characters.Add(runtime.ToSaveData());
            Save();
            Debug.Log($"✨ Runtimeキャラクター生成: {template.characterName}");
        }

        return runtime;
    }

    // -----------------------------
    // Runtime → SaveData 更新
    // -----------------------------
    public void UpdateCharacterRuntime(CharacterRuntime runtime)
    {
        var index = CurrentSaveData.characters.FindIndex(c => c.characterName == runtime.template.characterName);

        if (index >= 0)
            CurrentSaveData.characters[index] = runtime.ToSaveData();
        else
            CurrentSaveData.characters.Add(runtime.ToSaveData());

        Save();
        Debug.Log($"💾 Runtimeキャラクター更新: {runtime.template.characterName}");
    }

    // -----------------------------
    // 名前でRuntime取得
    // -----------------------------
    public CharacterRuntime GetRuntimeByName(string characterName)
    {
        return runtimeCharacters.Find(r => r.template.characterName == characterName);
    }
}
