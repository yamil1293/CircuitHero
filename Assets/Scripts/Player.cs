using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public class Player : MonoBehaviour {

    [System.Serializable]
    public class PlayerStats {
       public int maxHealth = 100;

        private int _currentHealth;
        public int currentHealth {
            get { return _currentHealth; }
            set { _currentHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init()
        {
            currentHealth = maxHealth;
        }
    }

    public PlayerStats stats = new PlayerStats();

    public int fallBoundary = -20;

    [SerializeField]
    private StatusIndicator statusIndicator;

    void Start() {

        stats.Init();

        if (statusIndicator == null)
        {
            Debug.LogError("No status indicator referenced on Player");
        }
        else
        {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }
    }

    void Update() {
        if (transform.position.y <= fallBoundary)
        {
            DamagePlayer(9999999);

        }
    }

    public void DamagePlayer(int damage) {
        stats.currentHealth -= damage;
        if (stats.currentHealth <= 0)
        {
            GameMaster.KillPlayer(this);
        }

        statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
    }

}
