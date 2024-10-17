using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D _rb2d;
    [SerializeField] private GameObject shoot;
    [SerializeField] private float moveSpeed;
    public float shootSpeed;

    [SerializeField] private float shootCadence = 0.5f;
    [SerializeField] private int shootSamples;
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

        _rb2d.velocity = new Vector2(horizontal * moveSpeed, vertical * moveSpeed) * Time.deltaTime;
        if(horizontal > 0){
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }else{
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        if(horizontal != 0 || vertical != 0){
            gameObject.GetComponent<Animator>().SetBool("walk", true);
        }else{
            gameObject.GetComponent<Animator>().SetBool("walk", false);
        }
        
    }

    void Shoot(){
        if (Time.time >= _nextShoot && (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Keypad5)))
        {

            _nextShoot = Time.time + shootCadence;

            Vector2 direction = Vector2.left;
            // if(Input.GetKeyDown(KeyCode.Keypad1)){
            //     direction = Vector2.left;
            // }
            if(Input.GetKey(KeyCode.Keypad2) && Input.GetKeyDown(KeyCode.Keypad5)){
                direction = Vector2.down + Vector2.right;
            }
            else if(Input.GetKey(KeyCode.Keypad2) && Input.GetKeyDown(KeyCode.Keypad4)){
                direction = Vector2.up + Vector2.right;
            }
            else if(Input.GetKey(KeyCode.Keypad1) && Input.GetKeyDown(KeyCode.Keypad4)){
                direction = Vector2.up + Vector2.left;
            }
            else if(Input.GetKey(KeyCode.Keypad1) && Input.GetKeyDown(KeyCode.Keypad5)){
                direction = Vector2.down + Vector2.left;
            }
            else if(Input.GetKeyDown(KeyCode.Keypad2)){
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
