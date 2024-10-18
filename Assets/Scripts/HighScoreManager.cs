using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour {
    
        public static void SaveScore(string playerName, int score) {
            List<HighScore> highScores = LoadHighScores();

            // Verifica se o jogador já existe na lista de pontuações
            bool playerExists = false;
            for (int i = 0; i < highScores.Count; i++) {
                if (highScores[i].name == playerName) {
                    // Se o jogador já existe e a nova pontuação for maior, atualiza a pontuação
                    if (score > highScores[i].score) {
                        highScores[i].score = score;
                    }
                    playerExists = true;
                    break;
                }
            }

            // Se o jogador não existir, adiciona a nova pontuação
            if (!playerExists) {
                highScores.Add(new HighScore(playerName, score));
            }

            // Ordena as pontuações em ordem decrescente
            highScores.Sort((x, y) => y.score.CompareTo(x.score));

            // Mantém apenas as 3 melhores pontuações
            if (highScores.Count > 3) {
                highScores.RemoveAt(3); // Remove a menor pontuação
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

            // Checa se os dados existem e carrega as pontuações
            for (int i = 0; i < 3; i++) {
                string name = PlayerPrefs.GetString("TopPlayer" + i, "");
                int score = PlayerPrefs.GetInt("TopScore" + i, 0);
                if (!string.IsNullOrEmpty(name)) {
                    highScores.Add(new HighScore(name, score));
                }
            }

            // Se não houver pontuações, inicializa com valores padrão (opcional)
            if (highScores.Count == 0) {
                highScores.Add(new HighScore("Player1", 0));
                highScores.Add(new HighScore("Player2", 0));
                highScores.Add(new HighScore("Player3", 0));
                SaveInitialScores(highScores);
            }

            return highScores;
        }

        private static void SaveInitialScores(List<HighScore> highScores) {
            for (int i = 0; i < highScores.Count; i++) {
                PlayerPrefs.SetString("TopPlayer" + i, highScores[i].name);
                PlayerPrefs.SetInt("TopScore" + i, highScores[i].score);
            }
            PlayerPrefs.Save();
        }

        public class HighScore {
            public string name;
            public int score;

            public HighScore(string name, int score) {
                this.name = name;
                this.score = score;
            }
        }
}