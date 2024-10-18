using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    private Rigidbody2D _rb2d;
    [SerializeField] private GameObject shoot;
    [SerializeField] private float moveSpeed;
    public float shootSpeed;

    [SerializeField] private float shootCadence = 0.5f;
    [SerializeField] private int shootSamples;
    private float _nextShoot = 0f;

    [SerializeField] private int maxHealth = 3; // Vida máxima do jogador
    private int currentHealth;

    public GameObject[] heartSprites; // Corações na UI
    private bool isInvulnerable = false;
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;

    private GameManager gameManager;



    void Start() {
        _rb2d = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        playerCollider = gameObject.GetComponent<Collider2D>(); // Obtém o Collider2D do jogador
        currentHealth = maxHealth;
        UpdateHearts(); // Atualiza os corações no início do jogo
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update() {
        Movement();
        Shoot();
    }

    void Movement() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        _rb2d.velocity = new Vector2(horizontal * moveSpeed, vertical * moveSpeed) * Time.deltaTime;

        if (horizontal > 0) {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        } else if (horizontal < 0) {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }

        if (horizontal != 0 || vertical != 0) {
            gameObject.GetComponent<Animator>().SetBool("walk", true);
        } else {
            gameObject.GetComponent<Animator>().SetBool("walk", false);
        }
    }

    void Shoot() {
        if (Time.time >= _nextShoot && Input.anyKeyDown) { // Atira quando qualquer tecla é pressionada
            _nextShoot = Time.time + shootCadence;

            // Determinar direção do tiro
            Vector2 direction = Vector2.zero;

            if (Input.GetKey(KeyCode.Keypad1)) direction = new Vector2(-1, -1); // Diagonal esquerda baixo
            if (Input.GetKey(KeyCode.Keypad4)) direction = Vector2.left; // Esquerda
            if (Input.GetKey(KeyCode.Keypad2)) direction = Vector2.right; // Direita
            if (Input.GetKey(KeyCode.Keypad5)) direction = Vector2.down; // Baixo

            if (direction != Vector2.zero) {
                Vector3 spawnPosition = transform.position + new Vector3(direction.x, direction.y, 0f);
                GameObject newBullet = Instantiate(shoot, spawnPosition, Quaternion.identity);
                Rigidbody2D bulletRB = newBullet.GetComponent<Rigidbody2D>();
                bulletRB.velocity = direction * shootSpeed;
                Destroy(newBullet, 2); // Destruir após 2 segundos
            }
        }
    }

    // Detectar colisão com inimigos
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("InimigoEspecial") && !isInvulnerable) {
            TakeDamage();
        }
    }

    void TakeDamage() {
        currentHealth--;
        UpdateHearts();

        if (currentHealth <= 0) {
            Die(); // Se a vida chegar a zero, chama Die()
        } else {
            StartCoroutine(BecomeInvulnerable()); // Pisca e se torna invulnerável
        }
    }

    void UpdateHearts() {
        for (int i = 0; i < heartSprites.Length; i++) {
            heartSprites[i].SetActive(i < currentHealth);
        }
    }

    IEnumerator BecomeInvulnerable() {
        isInvulnerable = true;
        playerCollider.enabled = false; // Desativa o Collider para evitar colisões

        for (int i = 0; i < 5; i++) {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f); // Diminuir a opacidade
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f); // Voltar à opacidade total
            yield return new WaitForSeconds(0.2f);
        }

        playerCollider.enabled = true; // Reativa o Collider após o período de invulnerabilidade
        isInvulnerable = false;
    }

    void Die() {
        gameManager.SaveScore(); // Chama o método SaveScore do GameManager
        SceneManager.LoadScene("Menu"); // Muda para a cena do Menu
    }

}
