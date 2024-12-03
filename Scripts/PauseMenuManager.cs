using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager instance;
    public string mainMenuSceneName = "Main Menu"; // Nome da cena do menu principal

    private void Awake()
    {
        // Verifica se já existe uma instância
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Garante que o objeto persista entre as cenas
        }
        else
        {
            Destroy(gameObject); // Evita duplicatas
        }
    }

    void Update()
    {
        // Verifica se a tecla ESC foi pressionada
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMainMenu();
        }
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Voltando ao menu principal...");

        // Salva o progresso atual (opcional)
        if (GameManager.instance != null)
        {
            GameManager.instance.SaveGame();
        }

        // Carrega a cena do menu principal
        SceneManager.LoadScene(mainMenuSceneName);
    }
}