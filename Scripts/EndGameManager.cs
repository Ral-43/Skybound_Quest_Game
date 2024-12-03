using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Referência para o texto da pontuação
    public TextMeshProUGUI deathCountText; // Referência ao texto de mortes

    private void Start()
    {
        // Obtém a pontuação final do GameManager
        int finalScore = GameManager.instance.currentCoins;

        // Atualiza o texto da pontuação na tela final
        if (scoreText != null)
        {
            scoreText.text = "Pontuação: " + finalScore;
        }

        // Exibe o número de mortes na tela final
        if (GameManager.instance != null)
        {
            deathCountText.text = "Mortes: " + GameManager.instance.deathCount;
        }
    }

    // Método para voltar ao menu
    public void ReturnToMainMenu()
    {
        // Carrega a cena do menu principal
        SceneManager.LoadScene("Main Menu");
    }
}
