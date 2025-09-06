using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections;
using GameData;

[System.Serializable]
public class ElementVisual
{
    public GameData.Element element;
    public Color color = Color.white;
    public Sprite sprite;
}

public class WordBox : MonoBehaviour
{
    [Header("単語・データ参照")]
    public WordDatabase database;
    private WordData wordData;
    private WordEffect effect;

    [Header("英単語設定")]
    public string word;
    public string shownMeaning;
    private bool isCorrectMeaning;

    [Header("属性設定")]
    public GameData.Element element;
    public Material correctMat;
    public Material wrongMat;

    [Header("属性別ビジュアル")]
    public ElementVisual[] elementVisuals;
    public SpriteRenderer backgroundRenderer;
    public SpriteRenderer glowRenderer;

    [Header("単語画像表示用")]
    public SpriteRenderer wordImageRenderer;

    [Header("TextMeshPro")]
    public TextMeshPro textMesh;
    public TextMeshPro meaningText;

    [Header("演出設定")]
    public AudioClip hitSound;
    public ParticleSystem hitParticlesPrefab;

    private Renderer rend;
    private AudioSource audioSource;
    private bool hasTriggered = false;

    // -----------------------------
    // Spawn 直後に必ず呼ぶリセットメソッド
    // -----------------------------
    public void ResetBox()
    {
        hasTriggered = false;
        isCorrectMeaning = false;
        wordData = null;
        word = "";
        shownMeaning = "";

        if (glowRenderer != null)
        {
            glowRenderer.enabled = false;
            Color glowColor = glowRenderer.color;
            glowColor.a = 0f;
            glowRenderer.color = glowColor;
        }

        if (backgroundRenderer != null)
        {
            backgroundRenderer.sprite = null;
            Color bgColor = Color.white;
            bgColor.a = 1f;
            backgroundRenderer.color = bgColor;
        }

        if (wordImageRenderer != null)
            wordImageRenderer.sprite = null;

        if (textMesh != null)
            textMesh.text = "";

        if (meaningText != null)
            meaningText.text = "";
    }

    private void OnEnable()
    {
        // Prefab が有効化されるたびにリセット
        ResetBox();
    }

    // -----------------------------
    // データセット・初期化
    // -----------------------------
    public void Initialize(WordDatabase db)
    {
        database = db;

        rend = GetComponent<Renderer>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (database != null)
        {
            wordData = database.GetRandomWordByWeight();
            if (wordData != null)
            {
                word = wordData.englishWord;
                element = wordData.element;
                effect = wordData.effect;

                if (textMesh != null)
                    textMesh.text = word;

                if (wordImageRenderer != null && wordData.wordImage != null)
                    wordImageRenderer.sprite = wordData.wordImage;

                var meanings = wordData.GetAllMeanings().Where(m => !string.IsNullOrEmpty(m)).ToArray();
                if (meanings.Length > 0 && meaningText != null)
                {
                    int index = Random.Range(0, meanings.Length);
                    shownMeaning = meanings[index];
                    meaningText.text = shownMeaning;
                    isCorrectMeaning = (shownMeaning == wordData.correctMeaning);
                }
            }
        }

        ApplyVisual();
    }

    private void ApplyVisual()
    {
        if (backgroundRenderer != null && elementVisuals != null && elementVisuals.Length > 0)
        {
            ElementVisual visual = elementVisuals[Random.Range(0, elementVisuals.Length)];
            backgroundRenderer.sprite = visual.sprite;
            Color color = visual.color;
            color.a = 1f;
            backgroundRenderer.color = color;
            element = visual.element;
        }

        if (glowRenderer != null)
        {
            glowRenderer.enabled = false;
            Color glowColor = glowRenderer.color;
            glowColor.a = 0f;
            glowRenderer.color = glowColor;
        }
    }

    private void ShowHitEffect()
    {
        if (glowRenderer != null)
        {
            glowRenderer.enabled = true;
            StartCoroutine(GlowCoroutine());
        }

        if (audioSource != null && hitSound != null)
            audioSource.PlayOneShot(hitSound);

        if (hitParticlesPrefab != null)
        {
            ParticleSystem particles = Instantiate(hitParticlesPrefab, transform.position, Quaternion.identity);
            particles.Play();
            Destroy(particles.gameObject, particles.main.duration + particles.main.startLifetime.constantMax);
        }
    }

    private IEnumerator GlowCoroutine()
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Color color = glowRenderer.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 0.5f, elapsed / duration);
            glowRenderer.color = color;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        elapsed = 0f;
        float fadeOutDuration = 0.3f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(0.5f, 0f, elapsed / fadeOutDuration);
            glowRenderer.color = color;
            yield return null;
        }

        glowRenderer.enabled = false;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Players")) return;
        hasTriggered = true;

        CharacterStats stats = other.GetComponent<CharacterStats>();
        if (stats == null) return;

        ShowHitEffect();

        if (isCorrectMeaning && effect != null)
        {
            if (rend != null && correctMat != null)
                rend.material = correctMat;

            float multiplier = stats.IsSameAttribute(element) ? effect.sameElementMultiplier : 1f;

            stats.AddAttack(Mathf.RoundToInt(effect.attackChange * multiplier));
            stats.AddDefense(Mathf.RoundToInt(effect.defenseChange * multiplier));
            stats.Heal(Mathf.RoundToInt(effect.healAmount * multiplier));
            stats.ChargeSpecial(Mathf.RoundToInt(effect.specialCharge * multiplier));

            stats.AddAttack(effect.attackChangeNegative);
            stats.AddDefense(effect.defenseChangeNegative);
        }
        else
        {
            if (rend != null && wrongMat != null)
                rend.material = wrongMat;

            stats.TakeDamage(effect != null ? effect.attackChangeNegative : 30);
        }
    }

    public bool IsCorrect() => isCorrectMeaning;
}
