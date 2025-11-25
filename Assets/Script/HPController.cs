using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    public Slider hpBar;
    public Text hpText;
    public int maxHP = 50;
    [HideInInspector] public int currentHP;

    void Start()
    {
        currentHP = maxHP;
        UpdateHPUI();
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0) currentHP = 0;
        UpdateHPUI();
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;
        UpdateHPUI();
    }

    public void UpdateHPUI()
    {
        hpBar.value = currentHP;
        hpText.text = $"{currentHP} / {maxHP}";
    }
}
