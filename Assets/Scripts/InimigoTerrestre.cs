using UnityEngine;

public class InimigoTerrestre : MonoBehaviour {
    public float velocidade = 3.0f;       
    private Transform player;            
    private SpriteRenderer spriteRenderer; 

    void Start() {
        
        player = GameObject.FindGameObjectWithTag("Player").transform;

       
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
       
        if (player != null) {
            
            Vector3 direcao = (player.position - transform.position).normalized;

            
            transform.position += direcao * velocidade * Time.deltaTime;

           
            if (direcao.x > 0 && spriteRenderer.flipX == true) {
                spriteRenderer.flipX = false;  
            } else if (direcao.x < 0 && spriteRenderer.flipX == false) {
                spriteRenderer.flipX = true;   
            }
        }
    }
}
