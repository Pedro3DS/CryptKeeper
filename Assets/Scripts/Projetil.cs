using UnityEngine;

public class Projetil : MonoBehaviour {
    private Renderer rend;

    void Start() {
        rend = GetComponent<Renderer>();
    }

    void Update() {
        if (!rend.isVisible) {
            Destroy(gameObject);
        }
    }
}
