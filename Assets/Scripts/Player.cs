using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D _rb2d;
    [SerializeField] private GameObject shoot;
    public float shootSpeed;

    [SerializeField] private float shootCadence = 0.5f;
    private float _nextShoot = 0f;

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

        _rb2d.velocity = new Vector2(horizontal, vertical ) * 5f;
        
    }

    void Shoot(){
        if (Time.time >= _nextShoot && (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Keypad5)))
        {

            _nextShoot = Time.time + shootCadence;

            Vector2 direction = Vector2.left;
            // if(Input.GetKeyDown(KeyCode.Keypad1)){
            //     direction = Vector2.left;
            // }
            if(Input.GetKeyDown(KeyCode.Keypad2)){
                direction = Vector2.right;
            }
            else if(Input.GetKeyDown(KeyCode.Keypad4)){
                direction = Vector2.up;
            }
            else if(Input.GetKeyDown(KeyCode.Keypad5)){
                direction = Vector2.down;
            }

            

            Vector3 spawnPosition = transform.position + new Vector3(direction.x, direction.y, 0f);


            GameObject newBullet = Instantiate(shoot, spawnPosition, Quaternion.identity);


            Rigidbody2D bulletRB = newBullet.GetComponent<Rigidbody2D>();
            bulletRB.velocity = direction * shootSpeed;

            Destroy(bulletRB, 2);
        }
        
    }
}
