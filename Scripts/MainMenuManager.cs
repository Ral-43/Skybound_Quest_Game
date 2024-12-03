using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public string firstLevelName = "Level 1"; // Nome da primeira cena do jogo

    public void StartNewGame()
    {
        Debug.Log("Iniciando um novo jogo...");

        // Reseta todos os dados salvos
        PlayerPrefs.DeleteAll();

        // Garante que o GameManager está resetado (opcional)
        if (GameManager.instance != null)
        {
            GameManager.instance.currentCoins = 0;
            GameManager.instance.currentHealth = 100;
            GameManager.instance.currentLevel = 1;
        }

        // Carrega a primeira cena do jogo
        SceneManager.LoadScene(firstLevelName);
    }


    public void StartGame()
    {
        // Carrega o Level 1 (ou o primeiro nível)
        SceneManager.LoadScene("Level 1");
    }

    public void LoadGame()
    {
        // Carrega o progresso salvo do GameManager
        GameManager.instance.LoadGame();
        SceneManager.LoadScene("Level 1"); // Ajuste para a última fase salva, se necessário
    }

    public void SaveGame()
    {
        // Salva o progresso atual do GameManager
        GameManager.instance.SaveGame();
    }

    public void ExitGame()
    {
        // Sai do jogo (funciona apenas na build, não no editor)
        Debug.Log("Sair do jogo");
        Application.Quit();
    }
}
