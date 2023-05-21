using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefab = default;
    [SerializeField] private GameObject container = default;
    [SerializeField] private GameObject[] potionPrefab = default;

    private bool stopSpawn = false;
    private Transform target;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform.Find("PointCentral").transform;
    }


    void Start()
    {
        StartSpawning();
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
            int randomPosition = Random.Range(0, 100);
            float randomTime = Random.Range(2f, 10f);

            GameObject newEnemy = Instantiate(enemyPrefab[randomEnemy], positionSpawnArriere.x > -5f ? randomPosition < 70 ? positionSpawnAvant : positionSpawnArriere : positionSpawnAvant, Quaternion.identity);
            newEnemy.transform.parent = container.transform;
            yield return new WaitForSeconds(randomTime);
        }

    }

    public void SpawnPotion(Vector3 spawnPosition)
    {
        int randomPotion = Random.Range(0, potionPrefab.Length);
        int randomChance = Random.Range(0, 100);
        if (randomChance < 30)
        {
            Instantiate(potionPrefab[randomPotion], spawnPosition, Quaternion.identity);
        }
    }

    public void mortJoueur()
    {
        stopSpawn = true;
    }
}
