using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{ 
    public Camera _camera;
    public float moveSpeed;
    public Rigidbody2D rb;
    public Vector2 moveDirection;
    private Animator anim;

    private Button[] buttons;
    GameObject buttonsGO;
    [SerializeField] AudioClip battleStart;
    static Texture2D screenShot;
    public static SpriteRenderer transitionImage;
    float lastMoveX = 0;
    float lastMoveY = 0;

    private float attackTime = .5f;
    private float attackCounter =  .5f;
    private bool isAttacking;
    void Start()
    {
        anim = GetComponent<Animator>();
        buttonsGO = GameObject.FindGameObjectsWithTag("MoveButtons")[0];
        buttons = buttonsGO.GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(delegate {moveUp();});
        buttons[1].onClick.AddListener(delegate {moveDown();});
        buttons[2].onClick.AddListener(delegate {moveRight();});
        buttons[3].onClick.AddListener(delegate {moveLeft();});
    }

    void Update()
    {
        ProcessInputs();
    }  
    
    void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if(Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 ) {
            anim.SetFloat("moveX",moveX);
            anim.SetFloat("moveY",0);
        }

        if(Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1 ) {
            anim.SetFloat("moveX",0);
            anim.SetFloat("moveY",moveY);
        }

        if(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0 ) {
            anim.SetFloat("moveX",0);
            anim.SetFloat("moveY",0);
        }
        

        if(Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1  || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1 )
        {
            anim.SetFloat("LastMoveX",Input.GetAxisRaw("Horizontal"));
            anim.SetFloat("LastMoveY",Input.GetAxisRaw("Vertical"));
        }
        if(isAttacking)
        {
            attackCounter -= Time.deltaTime;
            if(attackCounter <= 0)
            {
                anim.SetBool("isAttacking",false);
                isAttacking = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            attackCounter = attackTime;
            anim.SetBool("isAttacking",true);
            isAttacking = true;
        }
        

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void moveUp() 
    {
        moveDirection = new Vector2(0.0f,1.0f).normalized;
        Debug.Log("Moving up");
    }
    void moveDown()
    {
        moveDirection = new Vector2(0.0f,-1.0f).normalized;
    }
    void moveRight()
    {
        moveDirection = new Vector2(1.0f,0.0f).normalized;
    }
    void moveLeft()
    {
        moveDirection = new Vector2(-1.0f,0.0f).normalized;
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        Debug.Log(moveDirection);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {    
        if (collision.gameObject.tag == "Enemy")
        {
            Time.timeScale = 0;
            StartCoroutine(TakeScreenShot());
            //If the GameObject has the same tag as specified, output this message in the console
            //SceneManager.LoadScene(0);
        }
    }


    IEnumerator TakeScreenShot()
    {
    yield return new WaitForEndOfFrame();
        screenShot = ScreenCapture.CaptureScreenshotAsTexture();
        // do something with texture
        Rect rec = new Rect(0,0,screenShot.width,screenShot.height);
        TransitionImage.instance.image.sprite = Sprite.Create(screenShot, rec, new Vector2(0,0));
        
        SceneManager.LoadScene(2);
    }
}

