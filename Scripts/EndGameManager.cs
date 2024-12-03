using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Refer�ncia para o texto da pontua��o
    public TextMeshProUGUI deathCountText; // Refer�ncia ao texto de mortes

    private void Start()
    {
        // Obt�m a pontua��o final do GameManager
        int finalScore = GameManager.instance.currentCoins;

        // Atualiza o texto da pontua��o na tela final
        if (scoreText != null)
        {
            scoreText.text = "Pontua��o: " + finalScore;
        }

        // Exibe o n�mero de mortes na tela final
        if (GameManager.instance != null)
        {
            deathCountText.text = "Mortes: " + GameManager.instance.deathCount;
        }
    }

    // M�todo para voltar ao menu
    public void ReturnToMainMenu()
    {
        // Carrega a cena do menu principal
        SceneManager.LoadScene("Main Menu");
    }
}
