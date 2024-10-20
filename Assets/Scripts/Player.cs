using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    private Rigidbody2D _rb2d;
    [SerializeField] private GameObject shoot;
    [SerializeField] private float moveSpeed;
    public float shootSpeed;
    [SerializeField] private float shootCadence = 0.5f;
    private float _nextShoot = 0f;
    public bool canTakeDamage = true;

    [SerializeField] private int maxHealth = 3; 
    private int currentHealth;
    public GameObject[] heartSprites; 
    private bool isInvulnerable = false;
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;

    private GameManager gameManager;

    [SerializeField] private Camera mainCamera;
    private Vector2 _screenBounds;
    private Vector2 lastMoveDirection; 

    [Header("Power Ups")]
    private bool _canUseSuper = true;
    [SerializeField] private float superDuration;
    [SerializeField] private Slider superSlider;
    [SerializeField] private Image powerUpField;
    [SerializeField] private GameObject shield;
    [SerializeField] private Sprite shieldSprite;

    void Start() {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        _rb2d = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        playerCollider = gameObject.GetComponent<Collider2D>();
        currentHealth = maxHealth;
        UpdateHearts(); 
        gameManager = FindObjectOfType<GameManager>();
        lastMoveDirection = Vector2.right; 
    }
    void UpdateHearts() {
        for (int i = 0; i < heartSprites.Length; i++) {
            heartSprites[i].SetActive(i < currentHealth);
        }
    }

    void Update()
    {
        
        FixScreenBounds();
        Movement();
        Shoot();
        if(Input.GetKeyDown(KeyCode.Space)){
            StartCoroutine(UseSuper());
        }
    }
    void Movement() {
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(horizontal, vertical).normalized;
        _rb2d.velocity = move * moveSpeed;

        if (move != Vector3.zero) {
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
    void FixScreenBounds(){
        _screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        Vector3 playerPosition = transform.position;
        playerPosition.x = Mathf.Clamp(playerPosition.x, _screenBounds.x * -1 + spriteRenderer.bounds.extents.x, _screenBounds.x - spriteRenderer.bounds.extents.x);
        playerPosition.y = Mathf.Clamp(playerPosition.y, _screenBounds.y * -1 + spriteRenderer.bounds.extents.y, _screenBounds.y - spriteRenderer.bounds.extents.y);
        _rb2d.transform.position = playerPosition;
    }

    IEnumerator UseSuper(){
        if(_canUseSuper){
            _canUseSuper = false;
            superSlider.value = 0;
            foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("InimigoEspecial")){
                enemy.GetComponent<Enemy>().Die();
            }
            for(int i = 0; i <= superDuration; i++){
                superSlider.value = i;
                yield return new WaitForSeconds(1);
            }
            _canUseSuper = true;

        }

    }

    void CreateShield(){
        Instantiate(shield, gameObject.transform);

    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("InimigoEspecial") && !isInvulnerable) {
            TakeDamage();
        }
        
    }
    
    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Shield")) {
            Destroy(other.gameObject);
            CreateShield();
            // ChangePowerUpImage(shieldSprite);
        }
    }

    void TakeDamage() {
        if(canTakeDamage){
            currentHealth--;
            UpdateHearts();

            if (currentHealth <= 0) {
                Die(); 
            } else {
                StartCoroutine(BecomeInvulnerable()); 
            }
        }
    }
    void ChangePowerUpImage(Sprite powerUpSprite){
        powerUpField.preserveAspect = true;
        powerUpField.sprite = powerUpSprite;
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