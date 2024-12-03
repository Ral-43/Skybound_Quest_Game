using UnityEngine;

public class PlatformAttach : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o objeto que colidiu é o jogador.
        if (collision.gameObject.CompareTag("Player"))
        {
            // Define o jogador como filho da plataforma.
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Verifica se o jogador saiu da plataforma.
        if (collision.gameObject.CompareTag("Player"))
        {
            // Remove o jogador como filho da plataforma.
            collision.gameObject.transform.SetParent(null);
        }
    }
}
