using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;
    

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    // Sons
    public AudioSource hitSound;
    public AudioSource collectSound;
    public AudioSource jumpSound;
    public AudioSource deathSound;

    // Pisca o Player
    private SpriteRenderer sprite;
    private Color baseColor;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        baseColor = sprite.color;

        
        if (hitSound == null || collectSound == null || jumpSound == null || deathSound == null)
        {
            Debug.LogError("Um ou mais sons não estão configurados no Player!");
        }
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
            jumpSound.Play(); 
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = !crouch;
            animator.SetBool("IsCrouching", crouch);
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            GameManager.instance.UpdateCoins(10);
            Destroy(collision.gameObject);
            collectSound.Play(); 
        }

        if (collision.CompareTag("Enemy"))
        {
            // Calcula o dano com base no nível atual
            int currentLevel = GameManager.instance != null ? GameManager.instance.currentLevel : 1;
            int damage = 5 * currentLevel; // 10 na fase 1, 20 na fase 2, 30 na fase 3

            // Aplica o dano
            GameManager.instance.UpdateHealth(-damage);

            Debug.Log($"Dano causado: {damage} no nível {currentLevel}");
            hitSound.Play(); 
            StartCoroutine(BlinkRed());
        }

        if (collision.CompareTag("River"))
        {
            GameManager.instance.UpdateHealth(-100);
            deathSound.Play(); 
        }
    }

    // Corrotina para piscar em vermelho
    private IEnumerator BlinkRed()
    {
        sprite.color = Color.red; // Altera para vermelho
        yield return new WaitForSeconds(0.1f); // Aguarda 0,1 segundos
        sprite.color = baseColor; // Retorna à cor original
    }
}
