using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 15;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private int ExpContained = 13;
    private GameObject UiController;
        // Start is called before the first frame update
    void Awake() {
        UiController = GameObject.Find("UIController");
    }
    void Start()
    {
        if (SaveSystem.LoadUpgrade("Bullet Speed") != null) {
            speed += float.Parse(SaveSystem.LoadUpgrade("Bullet Speed").level);
        }

        if (SaveSystem.LoadUpgrade("Bullet Damage") != null) {
            damage += int.Parse(SaveSystem.LoadUpgrade("Bullet Damage").level);
        }
        rb.linearVelocity = Vector2.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitinfo)
    {
        Instantiate(hitEffect, transform.position, transform.rotation);
        Asteroid asteroid = hitinfo.GetComponent<Asteroid>();
        Boss boss = hitinfo.GetComponent<Boss>();
        if (boss != null) {
            boss.TakeDamage(damage);
        }

        if (asteroid != null) {
            UiController.GetComponent<UIController>().UpdateMaterial(asteroid.materialincluded);
            UiController.GetComponent<UIController>().UpdateEnergy(asteroid.energyincluded);
            UiController.GetComponent<UIController>().UpdateExp(ExpContained);
            asteroid.SelfDestruct();
        }
        Destroy(gameObject);
    }

}
