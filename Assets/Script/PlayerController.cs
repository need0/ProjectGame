using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private Camera followCamera;

    [SerializeField] private float rotationSpeed = 10f;

    private Vector3 playerVelocity;
    [SerializeField] private float gravityValue = -13f;

    public bool groundedPlayer;
    [SerializeField] private float jumpHeight = 2.5f;

    [SerializeField] private float delayTime = 1.0f;

    public Animator animator;

    public bool isDead;
    public bool isCooldown = false;
    public bool isInvincible = false;
    public Light FlashL;
    public Light HoleL;

    public float cooldownTime = 5f;

    public ParticleSystem damageParticle;
    public ParticleSystem deadParticle;
    public ParticleSystem Heal;


    public static PlayerController instance;
    public GameObject[] attackHitBox;
    public  Collider HitBox;

    private void Awake()
    {
        instance = this;
        damageParticle.Stop();
        deadParticle.Stop();
        Heal.Stop();
        FlashL.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        damageParticle.Stop();
        deadParticle.Stop();
        Heal.Stop();
        HoleL.enabled = false;
        FlashL.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isCooldown && !isDead) 
        {
            Invincible();
        }

        if (Input.GetKeyDown(KeyCode.F) && !isDead)
        {
            FlashLight();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isDead)
        {
            Attack();
        }

        if (isInvincible)
        {
            switch (CheckWinner.instance.isWinner)
            {
                case true:
                    animator.SetBool("Victory", CheckWinner.instance.isWinner);
                    break;
                case false:
                    Movement();
                    break;
            }
        }
        else 
        {
            switch (CheckWinner.instance.isWinner || isDead)
            {
                case true:
                    animator.SetBool("Victory", CheckWinner.instance.isWinner);
                    animator.SetBool("Dead", true);
                    fixGravityWhenPlayerDead();
                    break;
                case false:
                    Movement();
                    break;
            }
        }
        

        if (Input.GetKeyDown(KeyCode.R)) 
        {
            ReScene();
        }


    }

    void ReScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void Movement()
    {
        groundedPlayer = characterController.isGrounded;
        if (characterController.isGrounded && playerVelocity.y < -2f)
        {
            playerVelocity.y = -1f;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y,0)
                                * new Vector3 (horizontalInput, 0, verticalInput);
        Vector3 movementDirection = movementInput.normalized;
       
        characterController.Move(movementDirection * playerSpeed * Time.deltaTime);

        if (movementDirection != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {

            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3f * gravityValue);
            animator.SetTrigger("Jumping");

        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

        animator.SetFloat("Speed", Mathf.Abs(movementDirection.x) + Mathf.Abs(movementDirection.z));
        animator.SetBool("Ground", characterController.isGrounded);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BoxDamage"))
        {
            ShowDamageParticle();
            isDead = true;
        }
    }


    public void ShowDamageParticle()
    {
        if (!isInvincible)
        {
            TogglerSlowMotion();
            damageParticle.Play();
            deadParticle.Play();
            StartCoroutine(delaySlow());
        }
    }

    void TogglerSlowMotion()
    {
        Time.timeScale = 0.5f;
    }

    IEnumerator delaySlow()
    {
        yield return new WaitForSeconds(2f);
        Time.timeScale = 1;
    }

    void fixGravityWhenPlayerDead()
    {
        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    void Invincible()
    {
       
        Heal.Play();
        isInvincible = true;
        StartCoroutine(Cooldown());
        isDead = false;
    }

    IEnumerator Cooldown()
    {
        isCooldown = true;
        HoleL.enabled = true;
        yield return new WaitForSeconds(cooldownTime);
        HoleL.enabled = false;
        isDead = false;
        isInvincible = false;
        Heal.Stop();
        yield return new WaitForSeconds(cooldownTime*2);
        isCooldown = false;
        
    }

    void FlashLight()
    {
        FlashL.enabled = !FlashL.enabled;
    }

    private IEnumerator DelayedDestroy(GameObject objectToDestroy, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(objectToDestroy);
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");

        /*if (HitBox.CompareTag("Enemy")|| gameObject.name.Equals("Cube(1)"))
        {
             StartCoroutine(DelayedDestroy(HitBox.gameObject, delayTime));
        }*/
    }


}
