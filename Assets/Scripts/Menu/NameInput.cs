using TMPro;
using UnityEngine;

public class NameInput : MonoBehaviour {
    public TextMeshProUGUI[] letterTexts;
    public TextMeshProUGUI rankingText;   
    private char[] letters = new char[5]; 
    private int currentIndex = 0;

    private void Start() {
        LoadPlayerName(); 
        UpdateLetters();
        HighlightCurrentLetter();
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
            SavePlayerName(); 
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene"); 
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
            letterTexts[i].fontSize = i == currentIndex ? 85 : 60;
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

    private void SavePlayerName() {
        string playerName = new string(letters);
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();
        Debug.Log("Nome salvo: " + playerName); 
    }

    private void LoadPlayerName() {
        string savedName = PlayerPrefs.GetString("PlayerName", "AAAAA"); 
        letters = savedName.ToCharArray();
    }

    private void DisplayRanking() {
        rankingText.text = ""; 
        var highScores = HighScoreManager.LoadHighScores(); 
        for (int i = 0; i < highScores.Count; i++) {
            rankingText.text += (i + 1) + ". " + highScores[i].name + ": " + highScores[i].score + "\n"; 
        }
    }
}
