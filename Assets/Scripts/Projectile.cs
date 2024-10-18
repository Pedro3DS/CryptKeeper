using UnityEngine;

public class Projectile : MonoBehaviour {
    public int damage;  // Dano que o proj�til causar�

    private void OnTriggerEnter2D(Collider2D other) {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.TakeDamage(damage);  // Aplica dano ao inimigo
            Destroy(gameObject);  // Destroi o proj�til ap�s a colis�o
        }
    }

    private void Start() {
        Destroy(gameObject, 3.0f);  // Destroi o proj�til ap�s 3 segundos caso n�o colida
    }
}
