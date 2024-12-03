using UnityEngine;

public class Player : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform player;
    [SerializeField] private float speed = 10;
    [SerializeField] private Animator anim;
    private PlayerInputs inputActions;

    private void Awake() {
        inputActions = new PlayerInputs();
        inputActions.PlayerInputActions.Enable();
    }
    // Update is called once per frame
    private void Update()
    {
            Vector2 inputVector = inputActions.PlayerInputActions.Move.ReadValue<Vector2>();
            Vector3 direction = new Vector3(0, inputVector.y, 0);
            transform.position += direction * (Time.deltaTime * speed);
            anim.SetBool(IsMoving, direction != Vector3.zero);
    }
    public HealthBar healthBar;
    public GameObject ship;
    [SerializeField] private ParticleSystem sparks;
    [SerializeField] private ParticleSystem flash;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private EdgeCollider2D cc;
    public event System.EventHandler PlayerDeath;
    public int health = 100;
    private bool once = true;
    

    private void Start() {
        flash.Stop();
        sparks.Stop();
        healthBar.SetMaxHealth(health);
    }

    public void DisablePlayerInputActions() {
        inputActions.PlayerInputActions.Disable();
    }

    public void TakeDamage(int damage) {
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

    void OnTriggerEnter2D(Collider2D hitInfo) {
        Asteroid asteroid = hitInfo.GetComponent<Asteroid>();
        if (asteroid != null) {
            TakeDamage(asteroid.asteroidDamage);
            asteroid.SelfDestruct();
        }
    }

    

    private void Die() {     
        if (once) {
            var emf = flash.emission;
            var durf = flash.main.duration;
            emf.enabled = true;
            var ems = sparks.emission;
            ems.enabled = true;
            sparks.Play();
            flash.Play();
            once = false;
            Destroy(healthBar.gameObject);
            Destroy(sr);
            Destroy(cc);
            inputActions.PlayerInputActions.Disable();
            Invoke(nameof(DestroyObj), durf);
        }
        
    }
    private void DestroyObj() {
        PlayerDeath?.Invoke(this, System.EventArgs.Empty);
        Destroy(gameObject);
    }
}
