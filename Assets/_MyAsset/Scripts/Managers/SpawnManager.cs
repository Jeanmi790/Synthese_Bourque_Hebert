using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefab = default;
    [SerializeField] private GameObject container = default;
    [SerializeField] private GameObject[] potionPrefab = default;

    private bool stopSpawn = false;
    private Transform target;
    private GameManager gameInfo;
    private int scoreToIncreaseDifficulty = 50;
    private float timeReduction = 0.4f;
    private float maxTimeReduction = 10f;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform.Find("PointCentral").transform;
    }


    void Start()
    {
        StartSpawning();
        gameInfo = FindObjectOfType<GameManager>();
    }

    private void StartSpawning()
    {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(1f);
        while (!stopSpawn)
        {
            Vector3 positionSpawnAvant = new Vector3(target.position.x + 20f, 7.8f, 0f);
            Vector3 positionSpawnArriere = new Vector3(target.position.x + -20f, 7.8f, 0f);
            Vector3[] positionSpawn = new Vector3[] { positionSpawnAvant, positionSpawnArriere };

            int randomEnemy = Random.Range(0, enemyPrefab.Length);
            float randomTime = Random.Range(1f, 10f);

            // if score is above threshold, reduce randomTime
            if (gameInfo.GetScore() >= scoreToIncreaseDifficulty)
            {
                randomTime -= timeReduction;
                scoreToIncreaseDifficulty += 50;
                timeReduction = Mathf.Min(timeReduction * 2f, maxTimeReduction);
            }

            GameObject newEnemy = Instantiate(enemyPrefab[randomEnemy], target.position.x > 545f ? positionSpawnArriere : positionSpawnAvant, Quaternion.identity);
            newEnemy.transform.parent = container.transform;
            yield return new WaitForSeconds(randomTime);
        }
    }

    public void SpawnPotion(Vector3 spawnPosition)
    {
        int randomPotion = Random.Range(0, potionPrefab.Length);
        int randomChance = Random.Range(0, 100);
        if (randomChance < 20)
        {
            Instantiate(potionPrefab[randomPotion], spawnPosition, Quaternion.identity);
        }
    }

    public void mortJoueur()
    {
        stopSpawn = true;
    }
}
