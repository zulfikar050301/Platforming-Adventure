using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float enemySpeed = 5f;
    Rigidbody2D enemyRigidBody;
    // Start is called before the first frame update
    void Start()
    {
        enemyRigidBody = GetComponent<Rigidbody2D>(); //reference ke rigidbody musuh
    }

    // Update is called once per frame
    void Update()
    {
        enemyRigidBody.velocity = new Vector2 (enemySpeed, 0f); //membuat musuh bergerak secara horizontal dengan speed yang sudah ditentukan
    }

    void OnTriggerExit2D(Collider2D other) //jika musuh tersentuh dengan collider lain maka reaksi musuh 
    {
        enemySpeed = -enemySpeed; //speed musuh menghadap manapun nilainya sama
        FlipEnemyFacing();
    }

    void FlipEnemyFacing() //agar posisi musuh berubah sesuai arah jalannya ketika bersentuh dengan objek lain
    {
        transform.localScale = new Vector2(-(Mathf.Sign(enemyRigidBody.velocity.x)), 1f); //merubah arah menghadap musuh ke arah berjalannya
    }
}
