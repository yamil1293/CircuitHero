using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

    public static GameMaster gameMaster;
    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 2;
    public Transform spawnPrefab;
    public CameraShake cameraShake;

    
        void Awake() {
            if (gameMaster == null) {
                gameMaster = GameObject.FindGameObjectWithTag ("GameMaster").GetComponent<GameMaster>();
            }
        }

        void Start() {

            if (cameraShake == null) {
                Debug.LogError("No camera shake referenced in GameMaster.");
            }
        }

        public IEnumerator _RespawnPlayer() {
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(spawnDelay);

            Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);       
            GameObject clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as GameObject;
            Destroy(clone, 3f);
        }

        public static void KillPlayer (PlayerStatus player) {
            Destroy(player.gameObject);
            gameMaster.StartCoroutine (gameMaster._RespawnPlayer());
        }

        public static void KillEnemy(EnemyStatus enemy) {
            gameMaster._KillEnemy(enemy);
        }

        public void _KillEnemy(EnemyStatus _enemy) {
            GameObject _clone =Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity) as GameObject;
            Destroy(_clone, 5f);
            Destroy(_enemy.gameObject);
        }
        
    

    }
