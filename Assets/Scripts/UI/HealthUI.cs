using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    [SerializeField]
    private GameObject user;
    
    [SerializeField]
    private GameObject healthObjectPrefab;
    
    private GameObject healthBarObject;
    private IHurtable userHurtable;
    private GameObject healthObject;
    private Image healthBarImage;
    private TextMeshProUGUI healthProgress;
    private string healthProgressFormat = "{0} / {1}";
    
    // ��� ������� ������� 
    [SerializeField]
    public Color decreaseColor = Color.red;
    [SerializeField]
    public Color increaseColor = Color.green;
    [SerializeField]
    private Color originalColor= Color.white;
    public float flashDuration = 0.5f;
    private Color currentColor;
    
    void Awake()
    {
        userHurtable = user.GetComponent<IHurtable>();
        healthObject = Instantiate(healthObjectPrefab, transform);
        
        healthBarImage = healthObject.GetComponent<Image>();
        healthProgress = healthObject.GetComponentInChildren<TextMeshProUGUI>();
        
        SetHealthUI(userHurtable.Health, userHurtable.MaxHealth);
        healthBarImage.color = originalColor;
        
        // �������
        userHurtable.OnHealthDecrease.AddListener(DecreaseHealthUI);
        // ��������
        userHurtable.OnHealthIncrease.AddListener(IncreaseHealthUI);
    }
    
    private void SetHealthUI(float health, float maxHealth)
    {
        healthBarImage.fillAmount = health / maxHealth;
        healthProgress.text = string.Format(healthProgressFormat, health, maxHealth);
    }
    
    private void DecreaseHealthUI(float health, float maxHealth)
        => ChangeHealthUI(health, maxHealth, decreaseColor);
    
    private void IncreaseHealthUI(float health, float maxHealth)
        => ChangeHealthUI(health, maxHealth, increaseColor);
    
    
    private void ChangeHealthUI(float health, float maxHealth, Color flashColor)
    {
        SetHealthUI(health, maxHealth);
        if (currentColor != originalColor)
        {
            StopCoroutine(FlashHealthBar(currentColor));
        }
        StartCoroutine(FlashHealthBar(flashColor));
    }
    
    private IEnumerator FlashHealthBar(Color flashColor)
    {
        healthBarImage.color = flashColor;
        currentColor = flashColor;
        yield return new WaitForSeconds(flashDuration);

        healthBarImage.color = originalColor; 
    }
}
