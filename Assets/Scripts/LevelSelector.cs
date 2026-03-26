using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Slider levelSlider;

    public TMP_InputField levelInputField;

    public int minLevel = 1;
    public int maxUnlockedLevel = 40;

    int currentLevel;

    void Start()
    {
        levelSlider.minValue = minLevel;
        levelSlider.maxValue = maxUnlockedLevel;

        levelInputField.contentType = TMP_InputField.ContentType.IntegerNumber;

        currentLevel = (int)levelSlider.value;

        UpdateUI();
    }

    public void OnSliderChanged()
    {
        currentLevel = (int)levelSlider.value;
        UpdateUI();
    }

    public void IncreaseLevel()
    {
        currentLevel += 5;

        if (currentLevel > maxUnlockedLevel) currentLevel = maxUnlockedLevel;

        levelSlider.value = currentLevel;

        UpdateUI();
    }

    public void DecreaseLevel()
    {
        currentLevel -= 5;

        if (currentLevel < minLevel)
            currentLevel = minLevel;

        levelSlider.value = currentLevel;

        UpdateUI();
    }
    public void OnInputFieldChanged()
    {
        if (int.TryParse(levelInputField.text, out int value))
        {
            if (value < minLevel)
                value = minLevel;

            currentLevel = value;

            // Solo mover slider si está dentro del rango desbloqueado
            if (currentLevel <= maxUnlockedLevel)
            {
                levelSlider.value = currentLevel;
            }

            UpdateUI();
        }
    }

    void UpdateUI()
    {
        levelInputField.text = currentLevel.ToString();
    }

    public void ConfirmLevel()
    {
        GameManager.Instance.SelectedEnemyLevel = currentLevel;
        SceneManager.LoadScene("TowerDefense");
    }
}