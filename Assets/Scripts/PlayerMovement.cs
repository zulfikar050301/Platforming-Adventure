using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float jumpSpeed = 10f; //mengatur ktinggian lompatan
    [SerializeField] float runSpeed = 10f; //mengatur kecepatan karakter
    [SerializeField] float climbSpeed = 5f; //kecepatan manjat
    [SerializeField] Vector2 DeathKick = new Vector2 (10f, 10f);
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject gameOver;
    [SerializeField] Transform gun;
    Vector2 moveInput; //declare untuk movement horizontal
    CapsuleCollider2D myCapsuleCollider;
    BoxCollider2D myFeetCollider; //reference declare ke box collider kaki karakter
    Rigidbody2D myRigidbody; //declare reference ke rigidbody pada character/player
    Animator myAnimator; //declare animator player
    float gravityScaleAtStart; //membuat fungsi nilai gravity di awal sesuai default
    bool isAlive = true; //bool untuk karakter hidup atau tidak
    

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>(); //mengambil komponen rigidbody 2D pada character 
        myAnimator = GetComponent<Animator>(); //mengambil komponen animator pada character 
        myCapsuleCollider = GetComponent<CapsuleCollider2D>(); //mengambil komponen collider 2D untuk character
        myFeetCollider = GetComponent<BoxCollider2D>(); //mengambil komponen boxcollider pada character
        gravityScaleAtStart = myRigidbody.gravityScale; //mengambil nilai gravitasi default pada engine
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive) //if statement untuk status karakter
        {
            return; //mengembalikan nilai karakter ke hidup lagi
        }
        Run(); // mengambil void RUN dan memerintahkan charcater untuk lari ( mengapa diletakkan pada update karena fuungsi lari akan berubah di setiap frame)
        FlipSprite(); //untuk membalikkan tubuh karakter jika menghadap ke lawan arah
        ClimbLadder();
        Die();
    }

    void OnFire(InputValue value)
    {
        if(!isAlive) //if statement untuk status karakter
        {
            return; //mengembalikan nilai karakter ke hidup lagi
        }
        Instantiate(bullet, gun.position, transform.rotation);
    }

    void OnMove(InputValue value)
    {
        if(!isAlive) //if statement untuk status karakter
        {
            return; //mengembalikan nilai karakter ke hidup lagi
        }
        moveInput = value.Get<Vector2>(); //untuk mengambil komponen nilai pada movement vector2 (horizontal)
        Debug.Log(moveInput); //untuk mengecek pada console apakah komponen sudah terambil belum
    }

    void OnJump(InputValue value)
    {
        if(!isAlive) //if statement untuk status karakter
        {
            return; //mengembalikan nilai karakter ke hidup lagi
        }
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return; //logic if untuk mengembalikan nilai ke 0 pada value jump agar tidak melompat dua kali saat sudah menyentuh layer...
        }
        //untuk jump berbeda dengan move yg mengharuskan untuk membuat vector baru, ia hanya perlu membuat button statement jika diklik saja
        if(value.isPressed ) 
        {
            Debug.Log("lompat");
            myRigidbody.velocity += new Vector2  (0f, jumpSpeed); //untuk membuat character lompat pada sumbu y dengan variable jumpSpeed yang dibuaat.
        }

        
    }

    void Run() // void untuk character bergerak /lari
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y); //untuk membuat karakter hanya bergerak pada sumbu X dan mengatur kecepatannya dan membuat karakter agar nilai pada sumbu y dibiarkan sesuai rigidbodynya
        myRigidbody.velocity = playerVelocity; //kecepatan pada rigidbody akan sama moveinput
        
        bool playerHasHorizontalSpeed  = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon; // syntac untuk membuat animasi karakter agar ssuai dengan move
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed); //membuat bool animasi lari berjalan jika lari

        
    }

    void ClimbLadder() //untuk memanjat
    {
        if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) //fungsi untuk membuat karakter jika tidak ter interact dengan tangga maaka akan menjadi seperti smula
        {
            myRigidbody.gravityScale = gravityScaleAtStart; //mengembalikan nilai gravitasi ke default
            myAnimator.SetBool("isClimbing", false);
            return; //mengembalikan semua nilai ke awal
        }
        Vector2 climbVelocity = new Vector2 (myRigidbody.velocity.x, moveInput.y * climbSpeed); //fungsi untuk membuat chacarter bergerak ke atas saat menaiki tangga
        myRigidbody.velocity = climbVelocity; //declare jika rigidbody velocity sama dengan climb velocity
        myRigidbody.gravityScale = 0f; // membuat nilai gravitasi jadi nol jika ber-interact dengan tangga
        
        bool playerHasVerticalSpeed  = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon; // bool karakter bergerak secara vertikal
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed); //kecepatan karakter saat manjat
    }

    void FlipSprite() //mengubah arah gambar karakter
    {
        bool playerHasHorizontalSpeed  = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon; //syntac untuk membuat karakter agar tetap menghadap ke arah terakhir dia digerakkan
        if(playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f); //merubah scale arah pada player, dengan menggunakan mathf.sign untuk mengambil nilai =/- nya lewa rigidbody dan menjadi nilai 1
            //zul
        }
    }

    void Die() //fungsi karakter mati
    {
        if(myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazard"))) //karakter akan mati jika collider character menyentur layer enemy dan hazard
        {
            Instantiate(gameOver);
            isAlive = false; //bool hidup menjadi false
            myAnimator.SetTrigger("Dying"); //membuat animasi karakter ke animasi mati
            myRigidbody.velocity = DeathKick; //membuat karakter terlempar ketika mati
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
