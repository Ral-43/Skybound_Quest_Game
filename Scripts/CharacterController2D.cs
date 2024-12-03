using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f; // Força aplicada ao pular.
    [Range(0, 1)][SerializeField] private float m_CrouchSpeed = .36f; // Redução da velocidade ao agachar (1 = 100% da velocidade).
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f; // Suavização do movimento.
    [SerializeField] private bool m_AirControl = false; // Permitir controle no ar.
    [SerializeField] private LayerMask m_WhatIsGround; // Define o que é considerado chão.
    [SerializeField] private Transform m_GroundCheck; // Posição para verificar se está no chão.
    [SerializeField] private Transform m_CeilingCheck; // Posição para verificar se há teto.
    [SerializeField] private Collider2D m_CrouchDisableCollider; // Collider a ser desativado ao agachar.

    const float k_GroundedRadius = .2f; // Raio do círculo para verificar se está no chão.
    private bool m_Grounded; // Indica se o personagem está no chão.
    const float k_CeilingRadius = .2f; // Raio do círculo para verificar se há teto.
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true; // Indica para qual lado o personagem está virado.
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

        // O personagem está no chão se o círculo na posição GroundCheck colidir com algo definido como chão.
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
        // Se não está agachado, verifica se pode levantar.
        if (!crouch)
        {
            // Se há algo acima impedindo de levantar, continua agachado.
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        // Controla o personagem somente se estiver no chão ou com controle no ar ativado.
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
                // Ativa o collider quando não estiver agachado.
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

            // Se o input move o personagem para a direita e ele está virado para a esquerda...
            if (move > 0 && !m_FacingRight)
            {
                // Inverte o personagem.
                Flip();
            }
            // Se o input move o personagem para a esquerda e ele está virado para a direita...
            else if (move < 0 && m_FacingRight)
            {
                // Inverte o personagem.
                Flip();
            }
        }

        // Se o personagem deve pular...
        if (m_Grounded && jump)
        {
            // Adiciona força vertical ao personagem.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }

    private void Flip()
    {
        // Inverte a direção que o personagem está olhando.
        m_FacingRight = !m_FacingRight;

        // Multiplica a escala local X por -1 para inverter o sprite.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
