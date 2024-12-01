using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    

    [SerializeField] private ParticleSystem particles;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private CircleCollider2D cc;
    [SerializeField] private Sprite bigAsteroid;
    [SerializeField] private Sprite smallAsteroid;
    public int materialincluded = 10;
    public float energyincluded;
    public int asteroidDamage = 10;
    private bool once = true;
    private float asteroidSpeed;

    
    // Start is called before the first frame update
    void Start()
    {
        int num = Random.Range(1, 3);
        if (num == 1)
        {
            sr.sprite = smallAsteroid;
        }
        else
        {
            sr.sprite = bigAsteroid;
        }
        particles.Stop();
        rb.linearVelocity = Vector2.left * asteroidSpeed;
        rb.angularVelocity = Random.Range(-200f, 200f);
        energyincluded = rb.angularVelocity/asteroidSpeed;
    }

    public void SelfDestruct()
    {
        if (once) {
            var em = particles.emission;
            var dur = particles.main.duration;
            em.enabled = true;
            particles.Play();
            once = false;
            Destroy(sr);
            Destroy(cc);
            Invoke(nameof(DestroySelf), dur);
        }
        
        
    }
    
    void OnTriggerEnter2D(Collider2D hitinfo)
    {
        SelfDestruct();
    }
    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void SetSpeed(float speed) {
        asteroidSpeed = speed;
    }

    
}
