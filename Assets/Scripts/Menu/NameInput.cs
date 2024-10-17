using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NameInput : MonoBehaviour {
    public TextMeshProUGUI[] letterTexts;
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI rankingText;

    private char[] letters = new char[5] { 'A', 'A', 'A', 'A', 'A' };
    private int currentIndex = 0;

    private void Start() {
        LoadLastSavedName();
        UpdateLetters();
        HighlightCurrentLetter();
        //DisplayPlayerScore();
        DisplayRanking();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            ChangeLetter(1);
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            ChangeLetter(-1);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            MoveToNextLetter();
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            MoveToPreviousLetter();
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            // Removido o comando de salvar o nome e a pontuação aqui.
            SceneManager.LoadScene("SampleScene");
        }
    }

    private void ChangeLetter(int direction) {
        char currentLetter = letters[currentIndex];
        currentLetter = (char)(currentLetter + direction);

        if (currentLetter > 'Z') {
            currentLetter = 'A';
        } else if (currentLetter < 'A') {
            currentLetter = 'Z';
        }

        letters[currentIndex] = currentLetter;
        UpdateLetters();
    }

    private void UpdateLetters() {
        for (int i = 0; i < letterTexts.Length; i++) {
            letterTexts[i].text = letters[i].ToString();
        }

        HighlightCurrentLetter();
    }

    private void HighlightCurrentLetter() {
        for (int i = 0; i < letterTexts.Length; i++) {
            if (i == currentIndex) {
                letterTexts[i].fontSize = 85;
            } else {
                letterTexts[i].fontSize = 60;
            }
        }
    }

    private void MoveToNextLetter() {
        currentIndex = (currentIndex + 1) % letters.Length;
        HighlightCurrentLetter();
    }

    private void MoveToPreviousLetter() {
        currentIndex = (currentIndex - 1 + letters.Length) % letters.Length;
        HighlightCurrentLetter();
    }

    private void LoadLastSavedName() {
        if (PlayerPrefs.HasKey("PlayerName")) {
            string savedName = PlayerPrefs.GetString("PlayerName");
            for (int i = 0; i < letters.Length; i++) {
                if (i < savedName.Length) {
                    letters[i] = savedName[i];
                }
            }
        }
    }

    private void DisplayPlayerScore() {
        if (playerScoreText != null) {
            playerScoreText.text = "Score: " + PlayerPrefs.GetInt("PlayerScore", 0).ToString(); // Carrega a pontuação do PlayerPrefs
        }
    }

    private void DisplayRanking() {
        if (rankingText != null) {
            string playerName = new string(letters);
            int playerScore = PlayerPrefs.GetInt("PlayerScore", 0);
            rankingText.text = "Ranking:\n1. " + playerName + " - " + playerScore;
        }
    }
}
