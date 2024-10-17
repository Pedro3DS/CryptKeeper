using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour {
    public static void SaveScore(string playerName, int score) {
        List<HighScore> highScores = LoadHighScores();

        HighScore newScore = new HighScore(playerName, score);
        highScores.Add(newScore);
        highScores.Sort((x, y) => y.score.CompareTo(x.score));

        if (highScores.Count > 10) {
            highScores.RemoveAt(10);
        }

        for (int i = 0; i < highScores.Count; i++) {
            PlayerPrefs.SetString("HighScoreName" + i, highScores[i].name);
            PlayerPrefs.SetInt("HighScore" + i, highScores[i].score);
        }
    }

    public static List<HighScore> LoadHighScores() {
        List<HighScore> highScores = new List<HighScore>();
        for (int i = 0; i < 10; i++) {
            string name = PlayerPrefs.GetString("HighScoreName" + i, "");
            int score = PlayerPrefs.GetInt("HighScore" + i, 0);
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
