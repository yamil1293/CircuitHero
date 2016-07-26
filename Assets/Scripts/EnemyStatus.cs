using UnityEngine;
using System.Collections;

public class EnemyStatus : MonoBehaviour {

    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;
       
        public int _currentHealth;
        public int currentHealth
        {
            get { return _currentHealth; }
			set { _currentHealth = Mathf.Clamp (value, 0, maxHealth); }
        }

        public int damage = 40;

    public void Init()
    {
        currentHealth = maxHealth;
    }
 }

    public EnemyStats stats = new EnemyStats();

    public Transform deathParticles;

    [SerializeField]
    private StatusIndicator statusIndicator = null;

    void Start()
    {
       stats.Init();

    if (statusIndicator != null)
        {
              statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }

        if (deathParticles == null)
        {
            Debug.LogError("No death particles regerenced on Enemy.");
        }
    }

    public void DamageEnemy(int damage)
    {
        stats.currentHealth -= damage;
        if (stats.currentHealth <= 0)
        {
            GameMaster.KillEnemy(this);
        }

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }

    }

    void OnCollisionEnter2D(Collision2D _colliderInfo)
    {
        PlayerStatus _player = _colliderInfo.collider.GetComponent<PlayerStatus>();
        if (_player != null)
        {
            _player.DamagePlayer(stats.damage);
            DamageEnemy(999999);
        }
    }
}
