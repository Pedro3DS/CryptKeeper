using UnityEngine;

public class Projectile : MonoBehaviour {
    public int damage;  

    private void OnTriggerEnter2D(Collider2D other) {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.TakeDamage(damage); 
            Destroy(gameObject);  
        }
    }

    private void Start() {
        Destroy(gameObject, 3.0f);  
    }
}
