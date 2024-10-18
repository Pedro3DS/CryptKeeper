using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    private Rigidbody2D _rb2d;
    [SerializeField] private GameObject shoot;
    [SerializeField] private float moveSpeed;
    public float shootSpeed;

    [SerializeField] private float shootCadence = 0.5f;
    private float _nextShoot = 0f;

    [SerializeField] private int maxHealth = 3; 
    private int currentHealth;

    public GameObject[] heartSprites; 
    private bool isInvulnerable = false;
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;

    private GameManager gameManager;

    private Vector2 lastMoveDirection; 

    void Start() {
        _rb2d = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        playerCollider = gameObject.GetComponent<Collider2D>();
        currentHealth = maxHealth;
        UpdateHearts(); 
        gameManager = FindObjectOfType<GameManager>();
        lastMoveDirection = Vector2.right; 
    }

    void Update() {
        Movement();
        Shoot();
    }

    void Movement() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical).normalized;
        _rb2d.velocity = move * moveSpeed * Time.deltaTime;

        if (move != Vector2.zero) {
            lastMoveDirection = move; 
        }

        if (horizontal > 0) {
            spriteRenderer.flipX = false;
        } else if (horizontal < 0) {
            spriteRenderer.flipX = true;
        }

        if (horizontal != 0 || vertical != 0) {
            gameObject.GetComponent<Animator>().SetBool("walk", true);
        } else {
            gameObject.GetComponent<Animator>().SetBool("walk", false);
        }
    }

    void Shoot() {
        if (Time.time >= _nextShoot && Input.GetKeyDown(KeyCode.P)) {
            _nextShoot = Time.time + shootCadence;

            
            GameObject newShoot = Instantiate(shoot, transform.position, Quaternion.identity);
            newShoot.GetComponent<Rigidbody2D>().velocity = lastMoveDirection * shootSpeed;
        }
    }

    
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("InimigoEspecial") && !isInvulnerable) {
            TakeDamage();
        }
    }

    void TakeDamage() {
        currentHealth--;
        UpdateHearts();

        if (currentHealth <= 0) {
            Die(); 
        } else {
            StartCoroutine(BecomeInvulnerable()); 
        }
    }

    void UpdateHearts() {
        for (int i = 0; i < heartSprites.Length; i++) {
            heartSprites[i].SetActive(i < currentHealth);
        }
    }

    IEnumerator BecomeInvulnerable() {
        isInvulnerable = true;
        playerCollider.enabled = false;

        for (int i = 0; i < 5; i++) {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f); 
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f); 
            yield return new WaitForSeconds(0.2f);
        }

        playerCollider.enabled = true;
        isInvulnerable = false;
    }

    void Die() {
        gameManager.SaveScore(); 
        SceneManager.LoadScene("Menu"); 
    }

    public void TakeDamage(int amount) {
        if (!isInvulnerable) {
            currentHealth -= amount;
            UpdateHearts();

            if (currentHealth <= 0) {
                Die();
            } else {
                StartCoroutine(BecomeInvulnerable());
            }
        }
    }
}
