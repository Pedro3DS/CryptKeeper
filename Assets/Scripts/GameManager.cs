using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public GameObject inimigoTerrestrePrefab;
    public GameObject inimigoVoadorPrefab;
    public GameObject inimigoEspecialPrefab;
    public TextMeshProUGUI name; 
    public int hordaAtual = 1;
    public int inimigosPorHorda = 5;
    public float spawnInterval = 1.0f;
    public float screenWidth = 20f;
    public float screenHeight = 10f;
    private bool hordaAtiva = false;
    private bool inimigoEspecialSpawnado = false;
    private int inimigosEspeciaisRestantes = 0;
    private int playerScore = 0; 
    void Start() {
        LoadPlayerName(); 
        StartCoroutine(GerenciarHordas());
    }

    void LoadPlayerName() {
        if (PlayerPrefs.HasKey("PlayerName")) {
            string playerName = PlayerPrefs.GetString("PlayerName");
            name.text = playerName; 
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            AddScore(10); //TODO: apagar isso que foi para teste.
        }

        if (Input.GetKeyDown(KeyCode.B)) {
            PlayerDied(); //TODO: apagar isso que foi para teste.
        }
    }
    // Logica de matar e aumentar os pontos
    private void AddScore(int points) {
        playerScore += points; 
        Debug.Log("Pontua��o atual: " + playerScore);
    }
    //Logica de morte coloque aqui
    private void PlayerDied() {
        SaveScore(); 
        SceneManager.LoadScene("Menu");
    }

    private void SaveScore() {
        string playerName = PlayerPrefs.GetString("PlayerName", "AAA");
        Debug.Log("Salvando pontua��o: " + playerName + " - " + playerScore);  
        HighScoreManager.SaveScore(playerName, playerScore); 
        PlayerPrefs.Save(); 
    }

    IEnumerator GerenciarHordas() {
        while (true) {
            hordaAtiva = true;

            int terrestres = inimigosPorHorda;
            int voadores = 0;

            if (hordaAtual >= 4) {
                voadores = inimigosPorHorda / 2;
                terrestres = inimigosPorHorda - voadores;
            }

            for (int i = 0; i < terrestres; i++) {
                SpawnInimigoTerrestre();
                yield return new WaitForSeconds(spawnInterval);
            }

            for (int i = 0; i < voadores; i++) {
                SpawnInimigoVoador();
                yield return new WaitForSeconds(spawnInterval);
            }

            if (hordaAtual >= 7 && !inimigoEspecialSpawnado) {
                StartCoroutine(GerenciarInimigosEspeciais());
            }

            yield return new WaitForSeconds(5.0f);

            hordaAtual++;
            inimigosPorHorda += 3;
        }
    }

    void SpawnInimigoTerrestre() {
        float ladoSpawn = Random.Range(0, 2) == 0 ? -screenWidth / 2 : screenWidth / 2;
        float randomY = Random.Range(-screenHeight / 2, screenHeight / 2);
        Vector3 spawnPosition = new Vector3(ladoSpawn, randomY, 0);
        Instantiate(inimigoTerrestrePrefab, spawnPosition, Quaternion.identity);
    }

    void SpawnInimigoVoador() {
        float randomX = Random.Range(-screenWidth / 2, screenWidth / 2);
        Vector3 spawnPosition = new Vector3(randomX, screenHeight / 2 + 2, 0);
        Instantiate(inimigoVoadorPrefab, spawnPosition, Quaternion.identity);
    }

    IEnumerator GerenciarInimigosEspeciais() {
        inimigoEspecialSpawnado = true;

        while (true) {
            if (inimigosEspeciaisRestantes == 0) {
                inimigosEspeciaisRestantes = Random.Range(1, 5);

                for (int i = 0; i < inimigosEspeciaisRestantes; i++) {
                    SpawnInimigoEspecial();
                    yield return new WaitForSeconds(spawnInterval);
                }
            }

            yield return null;

            if (GameObject.FindGameObjectsWithTag("InimigoEspecial").Length == 0) {
                yield return new WaitForSeconds(Random.Range(1, 5));
                inimigosEspeciaisRestantes = 0;
            }
        }
    }

    void SpawnInimigoEspecial() {
        float randomX = Random.Range(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x, Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x);
        float randomY = Random.Range(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y, Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y);
        Vector3 spawnPosition = new Vector3(randomX, randomY, 0);
        Instantiate(inimigoEspecialPrefab, spawnPosition, Quaternion.identity);
    }
}
