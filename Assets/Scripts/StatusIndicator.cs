using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour {

    [SerializeField] private RectTransform healthBarRectangle = null;
    [SerializeField] private Text healthText = null;

    void Start()
    {
        if (healthBarRectangle == null)
        {
            Debug.LogError("Status Indicator: No health bar object referenced!");
        }
        if (healthText == null)
        {
            Debug.LogError("Stauts Indicator: No health text object regerenced!");
        }
    }

    public void SetHealth(int _currentHealth, int _maxHealth)
    {
        float _value = (float)_currentHealth / _maxHealth;

        healthBarRectangle.localScale = new Vector3(_value, healthBarRectangle.localScale.y, healthBarRectangle.localScale.z);
        healthText.text = _currentHealth + "/" + _maxHealth + " HP";
    }
}
