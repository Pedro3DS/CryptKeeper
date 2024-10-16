using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D _rb2d;
    [SerializeField] private GameObject shoot;

    // Start is called before the first frame update
    void Start()
    {
        _rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Shoot();
    }

    void Movement(){
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Debug.Log(horizontal);
        _rb2d.velocity = new Vector2(horizontal, vertical ) * 10f;
        
    }

    void Shoot(){
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            Debug.Log("ghjkl");
            GameObject newShoot =  Instantiate(shoot, gameObject.transform);
        }
        
    }
}
