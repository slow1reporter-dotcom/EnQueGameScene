using UnityEngine;

namespace GameData
{
    [CreateAssetMenu(fileName = "NewWordDatabase", menuName = "Game/WordDatabase")]
    public class WordDatabase : ScriptableObject
    {
        [Header("単語リスト")]
        public WordData[] words;

        /// <summary>
        /// 単純ランダム取得
        /// </summary>
        public WordData GetRandomWord()
        {
            if (words == null || words.Length == 0) return null;
            int index = Random.Range(0, words.Length);
            return words[index];
        }

        /// <summary>
        /// 出現頻度を考慮したランダム取得
        /// </summary>
        public WordData GetRandomWordByWeight()
        {
            if (words == null || words.Length == 0) return null;

            int totalWeight = 0;
            for (int i = 0; i < words.Length; i++)
            {
                totalWeight += Mathf.Max(0, words[i].spawnWeight);
            }

            int randomValue = Random.Range(0, totalWeight);
            int currentSum = 0;

            for (int i = 0; i < words.Length; i++)
            {
                currentSum += Mathf.Max(0, words[i].spawnWeight);
                if (randomValue < currentSum)
                    return words[i];
            }

            // 万一到達しなかった場合は最後の単語を返す
            return words[words.Length - 1];
        }
    }
}
