using UnityEngine;

public class Enemy : MonoBehaviour {
    public enum EnemyType { Terrestre, Voador, Especial, Boss };
    public EnemyType enemyType;

    public int maxHealth;
    public int scoreValue;
    private int currentHealth;

    private GameManager gameManager;

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

    void Die() {
       
        if (gameManager != null) {
            gameManager.AddScore(scoreValue);
        }

       
        if (enemyType == EnemyType.Boss) {
            gameManager.BossDied();
        }

       
        Destroy(gameObject);
    }
}
