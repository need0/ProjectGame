using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{

    public List<AudioClip> playerWalking;
    public AudioClip playerJumping;
    public AudioClip playerdead;
    public AudioClip playerAttcak;
    public AudioClip playerHeal;
    public AudioClip playerFlash;
    private AudioSource playerSource;
    public int pos;

    public bool isCooldown = false;
    public float cooldownTime = 5f;

    public static PlayerSound instance;

    private void Awake()
    {
        instance = this;
    }

    public void playWalking()
    {
        pos = (int)Mathf.Floor(Random.Range(0, playerWalking.Count));
        playerSource.PlayOneShot(playerWalking[pos]);
    }

    public void playJumping() 
    {
        playerSource.PlayOneShot(playerJumping);
    }
    public void playDead()
    {
        playerSource.PlayOneShot(playerdead);
    }

    public void playAttack()
    {
        playerSource.PlayOneShot(playerAttcak);
    }


    // Start is called before the first frame update
    void Start()
    {
        playerSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isCooldown)
        {

            playerSource.PlayOneShot(playerHeal);
            StartCoroutine(Cooldown());

        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            playerSource.PlayOneShot(playerFlash);
        }

    }

    IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTime * 3);
        isCooldown = false;
    }
}
