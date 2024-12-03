using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton para facilitar o acesso.

    // Referências para UI
    public TextMeshProUGUI CoinsText;
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI LevelText;

    // Dados do jogador
    public int currentCoins = 0;
    public int currentHealth = 100;
    public int currentLevel;
    public int deathCount = 0;

    public AudioSource deathSound;
    public AudioSource saveSound;
    public AudioSource loadSound;

    private IEnumerator RestartLevelAfterDelay(float delay)
    {
        // Aguarda o tempo especificado antes de reiniciar
        yield return new WaitForSeconds(delay);

        // Reinicia a cena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Awake()
    {
        // Verifica se já existe um GameManager na cena
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre cenas
        }
        else
        {
            Destroy(gameObject); // Evita duplicatas
            return; // Sai do método para não executar o restante
        }
    }

    private void Start()
    {
        // Reseta os dados salvos ao iniciar
        PlayerPrefs.DeleteAll();

        // Localiza os elementos da UI na nova cena
        CoinsText = GameObject.Find("CoinIcon/CoinText")?.GetComponent<TextMeshProUGUI>();
        HealthText = GameObject.Find("HealthIcon/HealthText")?.GetComponent<TextMeshProUGUI>();
        LevelText = GameObject.Find("LevelIcon/LevelText")?.GetComponent<TextMeshProUGUI>();
        LoadGame(); // Carrega os dados padrão ou resetados
        Invoke("UpdateUI", 0.1f); // Chama UpdateUI após 0.1 segundos
    }

    public void UpdateCoins(int value)
    {
        currentCoins += value;
        UpdateUI();
    }

    public void UpdateHealth(int value)
    {
        currentHealth += value;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            GameOver();
        }
        UpdateUI();
    }

    public void UpdateLevel(int level)
    {
        currentLevel = level;
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Atualiza os textos da interface apenas se os elementos existirem
        if (CoinsText != null) CoinsText.text = "" + currentCoins;
        if (HealthText != null) HealthText.text = "" + currentHealth;
        if (LevelText != null) LevelText.text = "Level" + currentLevel;
    }

    public void ReassignUI(TextMeshProUGUI coins, TextMeshProUGUI health, TextMeshProUGUI level)
    {
        CoinsText = coins;
        HealthText = health;
        LevelText = level;
        UpdateUI(); // Atualiza os valores exibidos na interface
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("Coins", currentCoins);
        PlayerPrefs.SetInt("Health", currentHealth > 0 ? currentHealth : 100);
        PlayerPrefs.SetInt("Level", currentLevel);
        PlayerPrefs.SetInt("DeathCount", deathCount);
        PlayerPrefs.Save();

        if (saveSound != null)
        {
            saveSound.Play();
        }

        Debug.Log("Progresso salvo! Level: " + currentLevel + ", Moedas: " + currentCoins + ", Vida: " + currentHealth + ", Mortes: " + deathCount);
    }

    public void LoadGame()
    {
        currentCoins = PlayerPrefs.GetInt("Coins", 0);
        currentHealth = PlayerPrefs.GetInt("Health", 100);
        currentLevel = PlayerPrefs.GetInt("Level", 1);
        deathCount = PlayerPrefs.GetInt("DeathCount", 0); 

        UpdateUI();

        if (loadSound != null)
        {
            loadSound.Play();
        }

        Debug.Log("Progresso carregado! Level: " + currentLevel + ", Moedas: " + currentCoins + ", Vida: " + currentHealth + ", Mortes: " + deathCount);
    }

    public void IncrementDeathCount()
    {
        deathCount++;
        Debug.Log("Número de mortes: " + deathCount);
    }

    private void GameOver()
    {
        Debug.Log("Fim de jogo!");

        if (deathSound != null)
        {
            deathSound.Play();
        }

        // Incrementa o contador de mortes
        IncrementDeathCount();

        // Restaura a vida ao reiniciar a fase
        currentHealth = 100;

        // Reinicia a cena
        StartCoroutine(RestartLevelAfterDelay(2f)); // 2 segundos de atraso
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; 
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; 
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Cena carregada: " + scene.name);

        // Atualiza a UI
        CoinsText = GameObject.Find("CoinIcon/CoinText")?.GetComponent<TextMeshProUGUI>();
        HealthText = GameObject.Find("HealthIcon/HealthText")?.GetComponent<TextMeshProUGUI>();
        LevelText = GameObject.Find("LevelIcon/LevelText")?.GetComponent<TextMeshProUGUI>();

        // Extrai o número do nome da cena (ex: "Level 1" -> 1)
        string[] parts = scene.name.Split(' ');
        if (parts.Length > 1 && int.TryParse(parts[1], out int levelNumber))
        {
            currentLevel = levelNumber;
        }
        else
        {
            Debug.LogWarning("Não foi possível determinar o nível a partir do nome da cena: " + scene.name);
            currentLevel = 1; // Valor padrão
        }

        // Restaura a vida se necessário
        if (currentHealth <= 0)
        {
            currentHealth = 100;
        }

        UpdateUI();
    }

    public void TransitionToNextLevel(string nextLevelName)
    {
        Debug.Log("Iniciando transição para o próximo nível: " + nextLevelName);

        // Salva o progresso antes de carregar a nova cena
        SaveGame();

        // Carrega a próxima cena
        SceneManager.LoadScene(nextLevelName);

        Debug.Log("Transição concluída para: " + nextLevelName);
    }

}