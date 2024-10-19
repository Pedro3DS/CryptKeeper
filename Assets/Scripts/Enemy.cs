using UnityEngine;

public class Enemy : MonoBehaviour {
    public enum EnemyType { Terrestre, Voador, Especial };
    public EnemyType enemyType;

    public int maxHealth;
    public int scoreValue;
    private int currentHealth;

    private GameManager gameManager;

    [SerializeField] private GameObject[] dropItems;

    [SerializeField] private float dropChance = 20f;

    void Start() {
        currentHealth = maxHealth;
        gameManager = FindObjectOfType<GameManager>();
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            Die();
        }
    }

    public void Die() {
        // Adiciona pontua��o ao jogador
        if (gameManager != null) {
            gameManager.AddScore(scoreValue);
        }

        if(dropItems.Length >= 1){
            if(GameObject.FindGameObjectsWithTag("Shield").Length <= 0 && GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canTakeDamage != false){
                float randomValue = Random.Range(0f, 100f);

                if(randomValue <= dropChance)
                {
                    int randomIndex = Random.Range(0, dropItems.Length);
                    GameObject itemToDrop = dropItems[randomIndex];
                    Instantiate(itemToDrop, transform.position, Quaternion.identity);
                }

            }

        }

        Destroy(gameObject);
    }
}
