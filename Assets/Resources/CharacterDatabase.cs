using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameData
{
    [CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Game/CharacterDatabase")]
    public class CharacterDatabase : ScriptableObject
    {
        // -----------------------------
        // Singleton
        // -----------------------------
        private static CharacterDatabase _instance;
        public static CharacterDatabase Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<CharacterDatabase>("CharacterDatabase");
                    if (_instance == null)
                        Debug.LogError("CharacterDatabase が Resources フォルダに見つかりません。");
                    else
                        _instance.Initialize(); // 自動初期化
                }
                return _instance;
            }
        }

        // -----------------------------
        // キャラクターリスト
        // -----------------------------
        public List<CharacterData> allCharacters;
        private string savePath;

        // -----------------------------
        // 初期化
        // -----------------------------
        public void Initialize()
        {
            if (string.IsNullOrEmpty(savePath))
                savePath = Path.Combine(Application.persistentDataPath, "CharacterDatabase.json");

            Load();
        }

        // -----------------------------
        // 名前でキャラクター取得
        // -----------------------------
        public CharacterData GetCharacterByName(string name)
        {
            EnsureInitialized();
            return allCharacters.FirstOrDefault(c => c.characterName == name);
        }

        // -----------------------------
        // キャラクター解放
        // -----------------------------
        public void UnlockCharacter(CharacterData data)
        {
            EnsureInitialized();

            if (!data.unlocked)
            {
                data.unlocked = true;
                Debug.Log($"{data.characterName} を図鑑に登録しました！");
                Save(); // 解放後に保存
            }
        }

        // -----------------------------
        // 解放済みキャラクター取得
        // -----------------------------
        public List<CharacterData> GetUnlocked()
        {
            EnsureInitialized();
            return allCharacters.FindAll(c => c.unlocked);
        }

        // -----------------------------
        // 未解放キャラクター取得
        // -----------------------------
        public List<CharacterData> GetLocked()
        {
            EnsureInitialized();
            return allCharacters.FindAll(c => !c.unlocked);
        }

        // -----------------------------
        // 保存
        // -----------------------------
        public void Save()
        {
            EnsureInitialized();

            var saveList = new List<CharacterSaveInfo>();
            foreach (var c in allCharacters)
            {
                saveList.Add(new CharacterSaveInfo()
                {
                    characterName = c.characterName,
                    unlocked = c.unlocked
                });
            }

            string json = JsonUtility.ToJson(new CharacterSaveList { characters = saveList }, true);
            File.WriteAllText(savePath, json);
            Debug.Log($"💾 CharacterDatabase Save Completed: {savePath}");
        }

        // -----------------------------
        // 読み込み
        // -----------------------------
        public void Load()
        {
            EnsureInitialized();

            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                CharacterSaveList saveList = JsonUtility.FromJson<CharacterSaveList>(json);

                foreach (var cData in allCharacters)
                {
                    var saved = saveList.characters.Find(x => x.characterName == cData.characterName);
                    if (saved != null)
                        cData.unlocked = saved.unlocked;
                }

                Debug.Log("📂 CharacterDatabase Loaded");
            }
            else
            {
                Debug.Log("🆕 CharacterDatabase No Save Found, initialized new.");
            }
        }

        // -----------------------------
        // 初期化確認
        // -----------------------------
        private void EnsureInitialized()
        {
            if (string.IsNullOrEmpty(savePath))
                Initialize();
        }

        // -----------------------------
        // JSON 保存用クラス
        // -----------------------------
        [System.Serializable]
        private class CharacterSaveInfo
        {
            public string characterName;
            public bool unlocked;
        }

        [System.Serializable]
        private class CharacterSaveList
        {
            public List<CharacterSaveInfo> characters;
        }
    }
}
