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
    private bool _canMove = true;

    [SerializeField] private int maxHealth = 3; // Vida m�xima do jogador
    private int currentHealth;

    public GameObject[] heartSprites; // Cora��es na UI
    private bool isInvulnerable = false;
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;

    private GameManager gameManager;



    void Start() {
        _rb2d = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        playerCollider = gameObject.GetComponent<Collider2D>(); // Obt�m o Collider2D do jogador
        currentHealth = maxHealth;
        UpdateHearts(); // Atualiza os cora��es no in�cio do jogo
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        FixScreenBounds();
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


    void Shoot(){
        if (Time.time >= _nextShoot && (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.Joystick1Button2)))
        {

            _nextShoot = Time.time + shootCadence;

            Vector2 direction = Vector2.up;
            // if(Input.GetKeyDown(KeyCode.Keypad1)){
            //     direction = Vector2.left;
            // }
            if(Input.GetKey(KeyCode.Keypad2) && Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKey(KeyCode.Joystick1Button2) && Input.GetKeyDown(KeyCode.Joystick1Button1)){
                direction = Vector2.down + Vector2.right;
            }
            else if(Input.GetKey(KeyCode.Keypad2) && Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKey(KeyCode.Joystick1Button2) && Input.GetKeyDown(KeyCode.Joystick1Button0)){
                direction = Vector2.up + Vector2.right;
            }
            else if(Input.GetKey(KeyCode.Keypad1) && Input.GetKeyDown(KeyCode.Keypad4)  || Input.GetKey(KeyCode.Joystick1Button3) && Input.GetKeyDown(KeyCode.Joystick1Button1)){
                direction = Vector2.up + Vector2.left;
            }
            else if(Input.GetKey(KeyCode.Keypad1) && Input.GetKeyDown(KeyCode.Keypad5)  || Input.GetKey(KeyCode.Joystick1Button3) && Input.GetKeyDown(KeyCode.Joystick1Button0)){
                direction = Vector2.down + Vector2.left;
            }
            else if(Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Joystick1Button1)){
                direction = Vector2.left;
            }
            else if(Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Joystick1Button2)){
                direction = Vector2.right;
            }
            else if(Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Joystick1Button3)){
                direction = Vector2.down;
            }
        }
    }

    // Detectar colis�o com inimigos
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
            StartCoroutine(BecomeInvulnerable()); // Pisca e se torna invulner�vel
        }
    }

    void UpdateHearts() {
        for (int i = 0; i < heartSprites.Length; i++) {
            heartSprites[i].SetActive(i < currentHealth);
        }
    }

    IEnumerator BecomeInvulnerable() {
        isInvulnerable = true;
        playerCollider.enabled = false; // Desativa o Collider para evitar colis�es

        for (int i = 0; i < 5; i++) {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f); // Diminuir a opacidade
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f); // Voltar � opacidade total
            yield return new WaitForSeconds(0.2f);
        }

        playerCollider.enabled = true; // Reativa o Collider ap�s o per�odo de invulnerabilidade
        isInvulnerable = false;
    }


    void Die() {
        gameManager.SaveScore(); // Chama o m�todo SaveScore do GameManager
        SceneManager.LoadScene("Menu"); // Muda para a cena do Menu
    }

    void FixScreenBounds(){
        if(_rb2d.transform.position.x < Camera.main.ScreenToWorldPoint(Vector3.zero).x || _rb2d.transform.position.x > Camera.main.ScreenToWorldPoint(Vector3.zero).x * -1){
            _rb2d.velocity = Vector2.zero;
        }
        // UnityEngine.Debug.Log( + " -- " +  + " -- " + Camera.main.ScreenToWorldPoint(Vector3.zero).x * -1);
        // // UnityEngine.Debug.Log(Camera.main.ScreenToWorldPoint(Vector3.zero).x + " -- " + Camera.main.ScreenToWorldPoint(Vector3.zero).x * -1);
        // float currentX = transform.position.x;

        // float newX = _rb2d.transform.position.x + currentX;

        // if (newX < Camera.main.ScreenToWorldPoint(Vector3.zero).x)
        // {
        //     newX = Camera.main.ScreenToWorldPoint(Vector3.zero).x * -1;
        //     // UnityEngine.Debug.Log(Mathf.Abs(Camera.main.ScreenToWorldPoint(Vector3.zero).x));
        //     // UnityEngine.Debug.Log();
        // }
        // else if (newX > (Camera.main.ScreenToWorldPoint(Vector3.zero).x * -1))
        // {
        //     newX = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        // }
    }

}
