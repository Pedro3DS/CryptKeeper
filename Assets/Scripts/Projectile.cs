using UnityEngine;

public class Projectile : MonoBehaviour {
    public int damage;  // Dano que o projétil causará

    private void OnTriggerEnter2D(Collider2D other) {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.TakeDamage(damage);  // Aplica dano ao inimigo
            Destroy(gameObject);  // Destroi o projétil após a colisão
        }
    }

    private void Start() {
        Destroy(gameObject, 3.0f);  // Destroi o projétil após 3 segundos caso não colida
    }
}
