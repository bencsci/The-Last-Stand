using System.Collections;
using UnityEngine;
using TMPro;

public class WaveGeneration : MonoBehaviour
{
    public AudioSource audio;

    //holds spawned enemies in current wave
    public GameObject waveContainer;
    
    public GameObject[] bosses;

    public GameObject[] special;
    public float[] specialSpawnRates;

    public GameObject[] preventSpawnObjects;

    //holds all different enemy types
    public GameObject[] enemies;

    public float[] enemySpawnRates;

    //number of enimies to spawn in next wave
    public int firstWaveEnemyCount;

    //active wave number
    public static int waveNumber = 0;

    //radius of the circle where new enemies will not spawn
    public float noSpawnRadius = 8f;

    //how far out an enemy can spawn
    public float spawnRange;

    //how many enimies will spawn in the next wave
    private float enemiesToSpawn;

    //scalar to increase number of enemies to spawn
    public float enemyCountScalar;

    //counts total enemies spawn in game
    private int enemiesSpawned = 0;

    public int score = 0;

    private bool waveIsActive = false;

    public static GameObject player;

    private Vector3[] avoidPositions;

    public TextMeshProUGUI waveDisplay;

    private bool spawning = true;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();    
        enemiesToSpawn = firstWaveEnemyCount;

        avoidPositions = new Vector3[preventSpawnObjects.Length];

        for (int i = 0; i < preventSpawnObjects.Length; i++)
        {
            avoidPositions[i] = preventSpawnObjects[i].transform.position;
        }

        AdvanceWave();
        if (waveNumber == 1)
        {
            StartCoroutine(beginingAnimation(0.25f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawning && waveIsActive && waveContainer.transform.childCount <= 0)
        {
            waveIsActive = false;
            StartCoroutine(displayWaveComplete(0.25f));
        }
    }

    private IEnumerator beginingAnimation(float delay)
    {
        waveDisplay.color = Color.white;
        yield return new WaitForSeconds(delay);
        waveDisplay.color = Color.red;
        yield return new WaitForSeconds(delay);
        waveDisplay.color = Color.white;
        yield return new WaitForSeconds(delay);
        waveDisplay.color = Color.red;
        yield return new WaitForSeconds(delay);
        waveDisplay.color = Color.white;
        yield return new WaitForSeconds(delay);
        waveDisplay.color = Color.red;
        yield return new WaitForSeconds(delay);
        waveDisplay.color = Color.white;
        yield return new WaitForSeconds(delay);
        waveDisplay.color = Color.red;
    }

    private IEnumerator displayWaveComplete(float delay)
    {
        waveDisplay.text = "Wave Complete";
        waveDisplay.color = Color.white;
        yield return new WaitForSeconds(delay);
        waveDisplay.color = Color.grey;
        yield return new WaitForSeconds(delay);
        waveDisplay.color = Color.white;
        yield return new WaitForSeconds(10f); // Time until wave starts
        StartCoroutine(startNextWave());
    }

    private IEnumerator startNextWave()
    {
        if (waveNumber >= 1)
        {
            audio.Play();
        }
        waveDisplay.text = "WAVE " + waveNumber.ToString();
        waveDisplay.color = Color.white;
        yield return new WaitForSeconds(0.3f);
        waveDisplay.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        waveDisplay.color = Color.white;
        yield return new WaitForSeconds(0.3f);
        waveDisplay.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        waveDisplay.color = Color.white;
        yield return new WaitForSeconds(0.3f);
        waveDisplay.color = Color.red;
        AdvanceWave();
    }


    private void AdvanceWave()
    {
        if (player != null)
        {
            // Increase wave number
            waveNumber++;
            waveDisplay.text = "WAVE " + waveNumber.ToString();

            // Set waveIsActive to true
            waveIsActive = true;

            // Calculate wave enemies to spawn
            if (waveNumber > 8)
            {
                enemiesToSpawn = Mathf.RoundToInt(enemiesToSpawn * enemyCountScalar);
            }
            else if (waveNumber != 1)
            {
                enemiesToSpawn += Mathf.RoundToInt(waveNumber * 0.3f);
            }

            enemiesSpawned = 0;


            //Spawn Boss
            if (waveNumber % 5 == 0 && waveNumber >= 15)
            {
                int randomBoss = Random.Range(0, bosses.Length);
                spawnEnemy(bosses[randomBoss], "Boss");
                enemiesSpawned++;
            } else if (waveNumber % 5 == 0)
            {
                int randomBoss = Random.Range(0, bosses.Length - 1);
                spawnEnemy(bosses[randomBoss], "Boss");
                enemiesSpawned++;
            }

            // Spawn wave enemies
            while (enemiesSpawned < enemiesToSpawn)
            {
                spawning = true;
                for (int i = 0; i < enemies.Length; i++)
                {
                    float chance = Random.Range(0f, 1f);
                    if (chance < enemySpawnRates[i])
                    {
                        StartCoroutine(randomSpawnTime(enemies[i], "Normal"));
                        enemiesSpawned++;
                    }
                    // Check if enemiesSpawned is equal to or greater than enemiesToSpawn
                    if (enemiesSpawned >= enemiesToSpawn)
                    {
                        break;
                    }
                }

                for (int i = 0; i < special.Length; i++)
                {
                    float chance = Random.Range(0f, 1f);
                    if (chance < specialSpawnRates[i])
                    {
                        specialSpawnConditions(i);
                        enemiesSpawned++;
                    }

                    if (enemiesSpawned >= enemiesToSpawn)
                    {
                        break;
                    }
                }
            }
        }
       
       
    }

    private void specialSpawnConditions(int i)
    {
        // Spawn special zombies depending on wave number
        if (waveNumber >= 3 && ( i == 0))
        {
            StartCoroutine(randomSpawnTime(special[i], "Normal"));
        }

        if (waveNumber >= 6 && (i == 1 || i == 2 || i == 3 ||i == 4))
        {
            StartCoroutine(randomSpawnTime(special[i], "Special"));
        }

        if (waveNumber >= 10 && (i == 5 || i == 6 || i == 7))
        {
            StartCoroutine(randomSpawnTime(special[i], "Special"));
        }
    }


    private void waveBalancer(GameObject enemy, string type)
    {
        // Adjust enemy health based on wave number
        if (waveNumber != 1)
        {
            switch (type)
            {
                case "Normal":
                    if (waveNumber <= 50)
                    {
                        enemy.GetComponent<BasicEnemy>().health += waveNumber * 10f;
                    } 
                    else
                    {
                        enemy.GetComponent<BasicEnemy>().health += 50 * 10f;
                    }
                    break;
                case "Special":
                    if (waveNumber <= 50)
                    {
                        enemy.GetComponent<BasicEnemy>().health += waveNumber * 25f;
                    }
                    else
                    {
                        enemy.GetComponent<BasicEnemy>().health += 50 * 25f;
                    }
                    break;
                case "Boss":
                    if (waveNumber <= 50)
                    {
                        enemy.GetComponent<BasicEnemy>().health += waveNumber * 75f;
                    }
                    else
                    {
                        enemy.GetComponent<BasicEnemy>().health += 50 * 75f;
                    }
                    break;
            }
            
        }
    }

    void spawnEnemy(GameObject enemy, string type)
    {
        if (player != null)
        {
            Vector3 enemyLocation = Vector3.zero;
            bool isValidSpawnPoint = false;

            while (!isValidSpawnPoint)
            {
                float xLocation = Random.Range(-spawnRange, spawnRange);
                float zLocation = Random.Range(-spawnRange, spawnRange);


                enemyLocation = new Vector3(xLocation, 1, zLocation) + player.transform.position;

                // Check if the enemy is outside the noSpawnRadius
                if (Vector3.Distance(enemyLocation, player.transform.position) > noSpawnRadius)
                {
                    // Check if the enemy position is within a valid spawn point
                    if (IsValidSpawnPoint(enemyLocation))
                    {
                        isValidSpawnPoint = true;
                    }
                }
            }

            // Spawn enemy at the valid spawn point
            GameObject spawned = Instantiate(enemy, enemyLocation, Quaternion.identity, waveContainer.transform);
            waveBalancer(spawned, type);
            spawning = false;
        } 
    }

    IEnumerator randomSpawnTime(GameObject enemy, string type)
    {
        float timeBetweenSpawns;
        if (waveNumber >= 20)
        {
            timeBetweenSpawns = 1f;
        } else
        {
            timeBetweenSpawns = 0.5f;
        }


        float offset = 0f;
        if (enemiesSpawned >= 5)
        {
            offset = Random.Range(0.5f, 5f);
        }
        float delay = (timeBetweenSpawns * enemiesSpawned) + offset;
        yield return new WaitForSeconds(delay);
        spawnEnemy(enemy, type);
    }


    bool IsValidSpawnPoint(Vector3 point)
    {
        float avoidRadius = 10f;

        foreach (Vector3 avoidPosition in avoidPositions)
        {
            // Check if the distance between the spawn point and the avoid position is less than the avoid radius
            if (Vector3.Distance(point, avoidPosition) < avoidRadius)
            {
                return false;
            }
        }
        return true;
    }
}
