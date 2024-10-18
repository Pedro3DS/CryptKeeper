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
    private bool _canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        _rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        FixScreenBounds();
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
        if (Time.time >= _nextShoot && (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.Joystick1Button2)))
        {

            _nextShoot = Time.time + shootCadence;

            Vector2 direction = Vector2.up;
            // if(Input.GetKeyDown(KeyCode.Keypad1)){
            //     direction = Vector2.left;
            // }
            if(Input.GetKey(KeyCode.Keypad2) && Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKey(KeyCode.Joystick1Button2) && Input.GetKeyDown(KeyCode.Joystick1Button1)){
                direction = Vector2.down + Vector2.right;
            }
            else if(Input.GetKey(KeyCode.Keypad2) && Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKey(KeyCode.Joystick1Button2) && Input.GetKeyDown(KeyCode.Joystick1Button0)){
                direction = Vector2.up + Vector2.right;
            }
            else if(Input.GetKey(KeyCode.Keypad1) && Input.GetKeyDown(KeyCode.Keypad4)  || Input.GetKey(KeyCode.Joystick1Button3) && Input.GetKeyDown(KeyCode.Joystick1Button1)){
                direction = Vector2.up + Vector2.left;
            }
            else if(Input.GetKey(KeyCode.Keypad1) && Input.GetKeyDown(KeyCode.Keypad5)  || Input.GetKey(KeyCode.Joystick1Button3) && Input.GetKeyDown(KeyCode.Joystick1Button0)){
                direction = Vector2.down + Vector2.left;
            }
            else if(Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Joystick1Button1)){
                direction = Vector2.left;
            }
            else if(Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Joystick1Button2)){
                direction = Vector2.right;
            }
            else if(Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Joystick1Button3)){
                direction = Vector2.down;
            }

            

            Vector3 spawnPosition = transform.position + new Vector3(direction.x, direction.y, 0f);


            GameObject newBullet = Instantiate(shoot, spawnPosition, Quaternion.identity);

            Rigidbody2D bulletRB = newBullet.GetComponent<Rigidbody2D>();
            bulletRB.velocity = direction * shootSpeed;

            Destroy(bulletRB, 2);
        }
        
    }

    void FixScreenBounds(){
        if(_rb2d.transform.position.x < Camera.main.ScreenToWorldPoint(Vector3.zero).x || _rb2d.transform.position.x > Camera.main.ScreenToWorldPoint(Vector3.zero).x * -1){
            _rb2d.velocity = Vector2.zero;
        }
        // UnityEngine.Debug.Log( + " -- " +  + " -- " + Camera.main.ScreenToWorldPoint(Vector3.zero).x * -1);
        // // UnityEngine.Debug.Log(Camera.main.ScreenToWorldPoint(Vector3.zero).x + " -- " + Camera.main.ScreenToWorldPoint(Vector3.zero).x * -1);
        // float currentX = transform.position.x;

        // float newX = _rb2d.transform.position.x + currentX;

        // if (newX < Camera.main.ScreenToWorldPoint(Vector3.zero).x)
        // {
        //     newX = Camera.main.ScreenToWorldPoint(Vector3.zero).x * -1;
        //     // UnityEngine.Debug.Log(Mathf.Abs(Camera.main.ScreenToWorldPoint(Vector3.zero).x));
        //     // UnityEngine.Debug.Log();
        // }
        // else if (newX > (Camera.main.ScreenToWorldPoint(Vector3.zero).x * -1))
        // {
        //     newX = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        // }
    }
}
