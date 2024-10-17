using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour {
    public static void SaveScore(string playerName, int score) {
        List<HighScore> highScores = LoadHighScores();

        HighScore newScore = new HighScore(playerName, score);
        highScores.Add(newScore);
        highScores.Sort((x, y) => y.score.CompareTo(x.score)); 

        
        if (highScores.Count > 3) {
            highScores.RemoveAt(3); 
        }

        // Salva os dados no PlayerPrefs
        for (int i = 0; i < highScores.Count; i++) {
            PlayerPrefs.SetString("TopPlayer" + i, highScores[i].name);
            PlayerPrefs.SetInt("TopScore" + i, highScores[i].score);
        }

        PlayerPrefs.Save(); 
    }

    public static List<HighScore> LoadHighScores() {
        List<HighScore> highScores = new List<HighScore>();
        for (int i = 0; i < 3; i++) {
            string name = PlayerPrefs.GetString("TopPlayer" + i, "");
            int score = PlayerPrefs.GetInt("TopScore" + i, 0);
            if (!string.IsNullOrEmpty(name)) {
                highScores.Add(new HighScore(name, score));
            }
        }
        return highScores;
    }
}

public class HighScore {
    public string name;
    public int score;

    public HighScore(string name, int score) {
        this.name = name;
        this.score = score;
    }
}
