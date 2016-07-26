using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour {

    [SerializeField]
    private RectTransform healthBarRectangle;
    [SerializeField]
    private Text healthText;

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

    public void SetHealth(int _cur, int _max)
    {
        float _value = (float)_cur / _max;

        healthBarRectangle.localScale = new Vector3(_value, healthBarRectangle.localScale.y, healthBarRectangle.localScale.z);
        healthText.text = _cur + "/" + _max + " HP";
    }
}
