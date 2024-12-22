using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject Asteroid;
    [SerializeField] private float coolDown = 1f;
    private float attackSpeed = 4f;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private HealthBar healthBar;
    public event EventHandler isDead;
    [SerializeField] private AudioClip[] explosionSound;
    [SerializeField] private AudioClip[] bossAttackSound;
    [SerializeField] private AudioClip[] bossHitSound;
    public long health = 500;
    private bool once = true;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private CircleCollider2D cc;
    [SerializeField] private List<BossSo> bossSo;
    
    private void Start() {
        explosion.Stop();
        var rnd = new Random();
        var randindex = rnd.Next(bossSo.Count);
        var bossSelected = bossSo[randindex];
        sr.sprite = bossSelected.sprite;
        StartCoroutine(launchAttacks(bossSelected.attackSprite));
        healthBar.SetMaxHealth(health);
        

    }

    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator launchAttacks(Sprite attackSprite) {
        while (health > 0) {
            yield return new WaitForSeconds(coolDown);
            SoundManager.instance.PlayRandomSpecifiedSound(bossAttackSound, transform, 1f);
            var a1 =Instantiate(Asteroid, new Vector3(transform.position.x -3, -5, 0), Quaternion.identity);
            var a2 =Instantiate(Asteroid, new Vector3(transform.position.x -3, -3, 0), Quaternion.identity);
            var a3 =Instantiate(Asteroid, new Vector3(transform.position.x -3, 0, 0), Quaternion.identity);
            var a4 =Instantiate(Asteroid, new Vector3(transform.position.x -3, 3, 0), Quaternion.identity);
            var a5 =Instantiate(Asteroid, new Vector3(transform.position.x -3, 5, 0), Quaternion.identity);
            a1.GetComponent<Asteroid>().SetSpeed(attackSpeed);
            a2.GetComponent<Asteroid>().SetSpeed(attackSpeed);
            a3.GetComponent<Asteroid>().SetSpeed(attackSpeed);
            a4.GetComponent<Asteroid>().SetSpeed(attackSpeed);
            a5.GetComponent<Asteroid>().SetSpeed(attackSpeed);
            a1.GetComponent<Asteroid>().bigAsteroid = attackSprite;
            a2.GetComponent<Asteroid>().bigAsteroid = attackSprite;
            a3.GetComponent<Asteroid>().bigAsteroid = attackSprite;
            a4.GetComponent<Asteroid>().bigAsteroid = attackSprite;
            a5.GetComponent<Asteroid>().bigAsteroid = attackSprite;

        }
    }
    private void Die() {
        
        if (once) {
            SoundManager.instance.PlayRandomSpecifiedSound(explosionSound, transform, 1f);
            var em = explosion.emission;
            var dur = explosion.main.duration;
            em.enabled = true;
            explosion.Play();
            once = false;
            Destroy(sr);
            Destroy(cc);
            Destroy(healthBar.gameObject);
            isDead?.Invoke(this, EventArgs.Empty);
            Invoke(nameof(DestroySelf), dur);
        }
        
        
    }
    public void TakeDamage(long damage) {
        SoundManager.instance.PlayRandomSpecifiedSound(bossHitSound, transform, 1f);
        health -= damage;
        healthBar.SetHealth(health);
        if (health < 0) {
            health = 0;
            Die();
        }
        else if (health == 0) {
            Die();
        }
    }
    private void DestroySelf() {
        Destroy(gameObject);
    }

    
}
