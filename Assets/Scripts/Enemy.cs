using UnityEngine;

public class Enemy : MonoBehaviour {
    public enum EnemyType { Terrestre, Voador, Especial };
    public EnemyType enemyType;

    public int maxHealth;
    public int scoreValue;
    private int currentHealth;

    private GameManager gameManager;

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

    void Die() {
        // Adiciona pontuação ao jogador
        if (gameManager != null) {
            gameManager.AddScore(scoreValue);
        }

        // Destroi o inimigo
        Destroy(gameObject);
    }
}
