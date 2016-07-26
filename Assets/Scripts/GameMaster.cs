using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

    public static GameMaster gameMasterVariable;

    void Awake() {
        if (gameMasterVariable == null) {
            gameMasterVariable = GameObject.FindGameObjectWithTag ("GameMaster").GetComponent<GameMaster>();
        }
    }

    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 2;
    public Transform spawnPrefab;

    public CameraShake cameraShake;

    void Start()
    {
        if (cameraShake == null)
        {
            Debug.LogError("No camera shake referenced in GameMaster.");
        }
    }
  
    public IEnumerator _RespawnPlayer() {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(spawnDelay);

        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
        Destroy(clone.gameObject, 3f);
    }

    public static void KillPlayer (PlayerStatus playerStatus) {
        Destroy(playerStatus.gameObject);
        gameMasterVariable.StartCoroutine (gameMasterVariable._RespawnPlayer());
    }

    public static void KillEnemy(EnemyStatus enemy)
    {
        gameMasterVariable._KillEnemy(enemy);
    }
    public void _KillEnemy(EnemyStatus _enemy)
    {
        GameObject _clone =Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity) as GameObject;
        Destroy(_clone, 5f);
        Destroy(_enemy.gameObject);
    }
}
