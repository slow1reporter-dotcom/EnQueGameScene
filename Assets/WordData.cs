using UnityEngine;
using GameData;

namespace GameData
{
    public enum WordPartOfSpeech
    {
        Noun,
        Verb,
        Adjective,
        Adverb,
        Pronoun,
        Preposition,
        Conjunction,
        Interjection
    }

    [CreateAssetMenu(fileName = "NewWordData", menuName = "Game/WordData")]
    public class WordData : ScriptableObject
    {
        [Header("単語情報")]
        public string englishWord;
        public string correctMeaning;

        [Header("不正解の意味")]
        public string wrongMeaning1;
        public string wrongMeaning2;

        [Header("出現頻度")]
        public int spawnWeight = 1;

        [Header("品詞")]
        public WordPartOfSpeech partOfSpeech;

        [Header("属性・効果")]
        public Element element;
        public WordEffect effect;

        [Header("関連画像 (2D Sprite)")]
        public Sprite wordImage;  // ← 単語に関連する画像を設定可能

        /// <summary>
        /// 正解・不正解を含むすべての意味を配列で取得
        /// </summary>
        public string[] GetAllMeanings()
        {
            return new string[] { correctMeaning, wrongMeaning1, wrongMeaning2 };
        }
    }
}
