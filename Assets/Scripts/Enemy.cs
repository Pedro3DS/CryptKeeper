using UnityEngine;

public class Enemy : MonoBehaviour {
    public enum EnemyType { Terrestre, Voador, Especial, Boss };
    public EnemyType enemyType;

    public int maxHealth;
    public int scoreValue;
    private int currentHealth;

    private GameManager gameManager;

    [SerializeField] private GameObject[] dropItems;

    [SerializeField] private float dropChance = 20f;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
    }


    public void AjustarVidaInicial() {
        if (enemyType == EnemyType.Boss) {
            maxHealth = 10;  
            scoreValue = 10;
        } else {
            maxHealth = 1;   
            scoreValue = 1;  
        }
    }
  
    public void AumentarDificuldade(int vidaExtra, int pontosExtra) {
        maxHealth += vidaExtra;
        currentHealth = maxHealth; 
        scoreValue += pontosExtra;  
        Debug.Log($"Aumentando Dificuldade: Vida = {maxHealth}, Pontos = {scoreValue}");
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


        if (enemyType == EnemyType.Boss) {
            gameManager.BossDied();
        }


        if (dropItems.Length >= 1) {
            if (GameObject.FindGameObjectsWithTag("Shield").Length <= 0 && GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().canTakeDamage != false) {
                float randomValue = Random.Range(0f, 100f);

                if (randomValue <= dropChance) {
                    int randomIndex = Random.Range(0, dropItems.Length);
                    GameObject itemToDrop = dropItems[randomIndex];
                    Instantiate(itemToDrop, transform.position, Quaternion.identity);
                }

            }

        }

        Destroy(gameObject);
    }
}
