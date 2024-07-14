using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D myRigidbody; //declare rigidbody 2D pada bullet
    [SerializeField] float speedBullet = 5f; //declare speed pada bullet
    PlayerMovement player; //reference ke code PlayerMovement
    float xSpeed; 
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>(); //mengambil komponen rigidbody2D bullet
        player = FindObjectOfType<PlayerMovement>(); //mengambil objek player movement
        xSpeed = player.transform.localScale.x * speedBullet; //reference membuat bullet agar ssuai dengan localscale karakter
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody.velocity = new Vector2 (xSpeed, 0f); //gerakan peluru
    }

    void OnTriggerEnter2D(Collider2D other) //jika objek bersentuhan dengan collider objek lain
    {
        if(other.tag == "Enemy") //jika objek dengan tag Enemy tersentuh oleh peluru
        {
            Destroy(other.gameObject); //maka objek yang disentuh akan hancur
        }
        Destroy(other.gameObject); 
    }

    void OnCollisionEnter2D(Collision2D other) //jika peluru bersentuhan dengan collision objek lain 
    {
        Destroy(gameObject);    //maka peluru akan hancur
    }
}
