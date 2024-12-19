using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Transform p1;
    [SerializeField] private Transform p2;
    [SerializeField] private GameObject Asteroid;
    [SerializeField] private GameObject boss;
    [SerializeField] private float spawnRate = 3f;
    [SerializeField] private float increaseOfAsteroidSpeed = 15f;
    [SerializeField] private float increaseOfSpawnRate = 15f;
    [SerializeField] private long bossSpawnTime = 60;
    [SerializeField] private long bossSpawnChance = 0;
    public bool waveOnGoing;
    private long asteroidCount = 0;
    public long asteroidsPerWave = 10;
    [SerializeField] private GameObject bossSpawnPolong;
    public event System.EventHandler onWaveEnd;
    public float asteroidSpeed = 5f;
    // Update is called once per frame
    private void Start() {
        var bossScript = boss.GetComponent<Boss>();
        bossScript.isDead += Boss_onDestroy;
        
        StartCoroutine(increaseSpawnRate());
        StartCoroutine(increaseAsteroidSpeed());
    }
    private void Boss_onDestroy(object sender, System.EventArgs e) {
        bossSpawnChance = 0;
    }


    private Vector2 GetRandomAsteroidPosition(Transform p1, Transform p2){
        Vector3 polong1 = p1.transform.position;
        Vector3 polong2 = p2.transform.position;
        return new Vector2(Random.Range(polong1.x, polong2.x), Random.Range(polong1.y, polong2.y));
    }
    
    IEnumerator spawnWave()
    {
        while (waveOnGoing) {
            yield return new WaitForSeconds(spawnRate);
            var bossfind = GameObject.Find(boss.gameObject.name + "(Clone)");
            if (bossfind is null) {
                
                GameObject asteroid = Instantiate(Asteroid, GetRandomAsteroidPosition(p1, p2), Quaternion.identity);
                asteroid.GetComponent<Asteroid>().SetSpeed(asteroidSpeed);
                bossSpawnChance++;
                asteroidCount++;
                if (asteroidCount == asteroidsPerWave) {
                    asteroidCount = 0;
                    waveOnGoing = false;
                    onWaveEnd?.Invoke(this, System.EventArgs.Empty);
                    
                }
            }
            
        }
        
    }
    private void Update() {
        if (asteroidSpeed >= 8.5f) {
            bossSpawnTime = 100;
        }

        if (bossSpawnChance == bossSpawnTime) {
            var bossfind = GameObject.Find(boss.gameObject.name + "(Clone)");
            if (bossfind == null) {    
                Instantiate(boss, bossSpawnPolong.transform.position, bossSpawnPolong.transform.rotation);
            } else {
                bossSpawnChance = 0;
            }
                    
        }
    }
    IEnumerator increaseSpawnRate()
    {
        while (true) {
            yield return new WaitForSeconds(increaseOfSpawnRate);
            spawnRate -= 0.1f;
        }
        
    }
    IEnumerator increaseAsteroidSpeed()
    {
        while (true) {
            yield return new WaitForSeconds(increaseOfAsteroidSpeed);
            asteroidSpeed += 0.25f;
        }
        
    }

    public void SpawnWave() {
        waveOnGoing = true;
        StartCoroutine(spawnWave());
    }
}
