using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f; // For�a aplicada ao pular.
    [Range(0, 1)][SerializeField] private float m_CrouchSpeed = .36f; // Redu��o da velocidade ao agachar (1 = 100% da velocidade).
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f; // Suaviza��o do movimento.
    [SerializeField] private bool m_AirControl = false; // Permitir controle no ar.
    [SerializeField] private LayerMask m_WhatIsGround; // Define o que � considerado ch�o.
    [SerializeField] private Transform m_GroundCheck; // Posi��o para verificar se est� no ch�o.
    [SerializeField] private Transform m_CeilingCheck; // Posi��o para verificar se h� teto.
    [SerializeField] private Collider2D m_CrouchDisableCollider; // Collider a ser desativado ao agachar.

    const float k_GroundedRadius = .2f; // Raio do c�rculo para verificar se est� no ch�o.
    private bool m_Grounded; // Indica se o personagem est� no ch�o.
    const float k_CeilingRadius = .2f; // Raio do c�rculo para verificar se h� teto.
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true; // Indica para qual lado o personagem est� virado.
    private Vector3 m_Velocity = Vector3.zero;

    [Header("Eventos")]
    [Space]

    public UnityEvent OnLandEvent; // Evento para quando o personagem aterrissar.

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent; // Evento para quando o personagem se agachar.
    private bool m_wasCrouching = false; // Verifica se o personagem estava agachado.

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // O personagem est� no ch�o se o c�rculo na posi��o GroundCheck colidir com algo definido como ch�o.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }

    public void Move(float move, bool crouch, bool jump)
    {
        // Se n�o est� agachado, verifica se pode levantar.
        if (!crouch)
        {
            // Se h� algo acima impedindo de levantar, continua agachado.
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        // Controla o personagem somente se estiver no ch�o ou com controle no ar ativado.
        if (m_Grounded || m_AirControl)
        {
            // Se estiver agachado.
            if (crouch)
            {
                if (!m_wasCrouching)
                {
                    m_wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                // Reduz a velocidade usando o multiplicador CrouchSpeed.
                move *= m_CrouchSpeed;

                // Desativa o collider ao agachar.
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            }
            else
            {
                // Ativa o collider quando n�o estiver agachado.
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                if (m_wasCrouching)
                {
                    m_wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            // Movimenta o personagem calculando a velocidade alvo.
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.linearVelocity.y);
            // Suaviza o movimento e aplica ao personagem.
            m_Rigidbody2D.linearVelocity = Vector3.SmoothDamp(m_Rigidbody2D.linearVelocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // Se o input move o personagem para a direita e ele est� virado para a esquerda...
            if (move > 0 && !m_FacingRight)
            {
                // Inverte o personagem.
                Flip();
            }
            // Se o input move o personagem para a esquerda e ele est� virado para a direita...
            else if (move < 0 && m_FacingRight)
            {
                // Inverte o personagem.
                Flip();
            }
        }

        // Se o personagem deve pular...
        if (m_Grounded && jump)
        {
            // Adiciona for�a vertical ao personagem.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }

    private void Flip()
    {
        // Inverte a dire��o que o personagem est� olhando.
        m_FacingRight = !m_FacingRight;

        // Multiplica a escala local X por -1 para inverter o sprite.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
