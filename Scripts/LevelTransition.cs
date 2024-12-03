using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public string nextLevelName; // Nome da próxima cena

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Delegar a transição ao GameManager
            GameManager.instance.TransitionToNextLevel(nextLevelName);
        }
    }
}