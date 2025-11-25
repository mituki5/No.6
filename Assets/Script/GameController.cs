using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("UI Elements")]
    public Image enemyImage;
    public Image backgroundImage;
    public Slider enemyHPBar;
    public Text enemyHPText;
    public Slider playerHPBar;
    public Text playerHPText;
    public Text displayText;       // ScrollView 内の Text
    public InputField inputField;

    [Header("Enemies")]
    public EnemyData[] enemies;
    private int currentEnemyIndex = 0;
    private EnemyData currentEnemy;

    [Header("Player Stats")]
    public int playerMaxHP = 50;
    private int playerCurrentHP;

    [Header("Scroll Settings")]
    public ScrollRect scrollRect;

    private string currentText;
    private int currentCharIndex = 0;
    private int mistakeCount = 0;

    void Start()
    {
        playerCurrentHP = playerMaxHP;
        playerHPBar.maxValue = playerMaxHP;
        UpdatePlayerHPUI();

        LoadEnemy();
        inputField.onValueChanged.AddListener(OnInputChanged);
        inputField.ActivateInputField();
    }

    void LoadEnemy()
    {
        if (currentEnemyIndex >= enemies.Length)
        {
            Debug.Log("All enemies defeated!");
            return;
        }

        currentEnemy = enemies[currentEnemyIndex];

        // 敵画像と背景
        if (enemyImage != null) enemyImage.sprite = currentEnemy.enemySprite;
        if (backgroundImage != null) backgroundImage.sprite = currentEnemy.backgroundSprite;

        // 敵HP設定
        enemyHPBar.maxValue = currentEnemy.maxHP;
        enemyHPBar.value = currentEnemy.maxHP;
        enemyHPText.text = $"{currentEnemy.maxHP} / {currentEnemy.maxHP}";

        // 文章読み込み
        if (currentEnemy.storyFile != null)
            currentText = currentEnemy.storyFile.text;
        else
            currentText = "";

        currentCharIndex = 0;
        mistakeCount = 0;
        displayText.text = "";
        inputField.text = "";
        inputField.ActivateInputField();
    }

    void OnInputChanged(string input)
    {
        if (string.IsNullOrEmpty(currentText)) return;
        if (input.Length == 0) return;

        char typedChar = input[input.Length - 1];
        char expectedChar = currentText[currentCharIndex];

        if (typedChar == expectedChar)
        {
            // 正確入力
            currentCharIndex++;
            displayText.text += $"<color=blue>{typedChar}</color>";
        }
        else
        {
            // ミス入力
            mistakeCount++;
        }

        inputField.text = "";
        inputField.ActivateInputField();

        // ScrollView 自動スクロール（現在行を中央に）
        Canvas.ForceUpdateCanvases();
        if (scrollRect != null)
        {
            float normalizedPos = Mathf.Clamp01((float)currentCharIndex / Mathf.Max(currentText.Length - 1, 1));
            scrollRect.verticalNormalizedPosition = 1 - normalizedPos;
        }

        // 文章完了判定
        if (currentCharIndex >= currentText.Length)
        {
            int correctCount = currentText.Length - mistakeCount;
            int enemyDamage = Mathf.RoundToInt(correctCount * 0.1f); // 例：10%でダメージ換算
            ApplyEnemyDamage(enemyDamage);
        }
    }

    void ApplyEnemyDamage(int damage)
    {
        int newHP = Mathf.Max((int)enemyHPBar.value - damage, 0);
        enemyHPBar.value = newHP;
        enemyHPText.text = $"{newHP} / {currentEnemy.maxHP}";

        // 敵攻撃（プレイヤーにミス分ダメージ）
        ApplyPlayerDamage(mistakeCount);

        // 次の敵へ
        currentEnemyIndex++;
        LoadEnemy();
    }

    void ApplyPlayerDamage(int damage)
    {
        playerCurrentHP -= damage;
        if (playerCurrentHP < 0) playerCurrentHP = 0;
        UpdatePlayerHPUI();

        if (playerCurrentHP <= 0)
        {
            Debug.Log("Game Over");
            // ゲームオーバー処理（Scene切替など）
        }
    }

    void UpdatePlayerHPUI()
    {
        playerHPBar.value = playerCurrentHP;
        playerHPText.text = $"{playerCurrentHP} / {playerMaxHP}";
    }
}
