using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;

namespace PULSED
{
    class Database
    {
        SQLiteConnection dbCon;
        public Database()
        {

            if (!System.IO.File.Exists("pulsedb.sqlite"))
            {
                SQLiteConnection.CreateFile("pulsedb.sqlite");
                Console.WriteLine("Sqlite database not found, rebuilding database");
                createDB();
                dbCon = new SQLiteConnection("Data Source=pulsedb.sqlite;Version=3;");
                dbCon.Open();
            }
            else
            {
                dbCon = new SQLiteConnection("Data Source=pulsedb.sqlite;Version=3;");
                dbCon.Open();
            }
        }
        private void createDB()
        {
            SQLiteConnection dbBuildCon = new SQLiteConnection("Data Source=pulsedb.sqlite;Version=3;");
            SQLiteCommand command;
            String query = "";

            if (System.IO.File.Exists("database.sql"))
            {
                dbBuildCon.Open();
                string[] queryLines = System.IO.File.ReadAllLines("database.sql");
                foreach (string line in queryLines)
                {
                    if (!line.StartsWith("--"))
                    {
                        query = query + ' ' + line;
                    }
                }
                Console.WriteLine(query);
                command = new SQLiteCommand(query, dbBuildCon);
                command.ExecuteNonQuery();
                dbBuildCon.Close();

            }
            else
            {
                Console.WriteLine("DB not found!");
            }
        }
        public Dictionary<String, int> getHighScores(int highScoreCount)
        {
            if (dbCon.State.ToString().Equals("Open"))
            {
                Dictionary<String, int> highScoreList = new Dictionary<string, int>();
                string query = "SELECT u.username as username, h.score as score FROM highscores h INNER JOIN users u ON h.user_id = u.id ORDER BY score DESC LIMIT 0," + highScoreCount.ToString() + ";";
                SQLiteCommand command = new SQLiteCommand(query, dbCon);
                SQLiteDataReader reader = command.ExecuteReader();
                int salt = 0;
                while (reader.Read())
                {
                    highScoreList.Add((string)reader["username"] + salt.ToString(), int.Parse(reader["score"].ToString()));
                    salt += 1;
                }
                return highScoreList;
            }
            return null;
        }
        public bool insertUser(string username)
        {
            if (dbCon.State.ToString().Equals("Open"))
            {
                string query = "INSERT INTO users (username) VALUES('" + username + "');";
                SQLiteCommand command = new SQLiteCommand(query, dbCon);
                command.ExecuteNonQuery();
                return true;
            }
            return false;
        }
        public String getQuestion(int questionNumber)
        {
            if (dbCon.State.ToString().Equals("Open"))
            {
                string query = "SELECT question FROM questions WHERE id=" + questionNumber.ToString() + ";";
                SQLiteCommand command = new SQLiteCommand(query, dbCon);
                SQLiteDataReader reader = command.ExecuteReader();
                return reader["question"].ToString();
            }
            return null;
        }
        public Dictionary<int, String> getAnswersForQuestion(int questionId)
        {
            if (dbCon.State.ToString().Equals("Open"))
            {
                Dictionary<int, String> answerDictionary = new Dictionary<int, string>();
                string query = "SELECT id, answer FROM answers WHERE question_id=" + questionId.ToString() + ";";
                SQLiteCommand command = new SQLiteCommand(query, dbCon);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    answerDictionary.Add(int.Parse(reader["id"].ToString()), (string)reader["answer"]);
                }
                return answerDictionary;
            }
            return null;

        }

        public bool? isAnswerCorrect(int questionId, int answerId)
        {
            if (dbCon.State.ToString().Equals("Open"))
            {
                Dictionary<int, String> answerDictionary = new Dictionary<int, string>();
                string query = "SELECT answer_id FROM correct_answer WHERE question_id LIKE '" + questionId + "';";
                SQLiteCommand command = new SQLiteCommand(query, dbCon);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int correctAnswerId = int.Parse(reader["answer_id"].ToString());
                    //This works because every question has 3 answers, so calculating the mod results in 0..2
                    //which we can use to compare that to the input
                    //since we input 1..3 we need to substract 1 from that
                    Console.WriteLine((correctAnswerId % 3).ToString() + ":" + (answerId - 1).ToString());
                    if (correctAnswerId % 3 == answerId - 1)
                    {
                        return true;
                    }
                }
                return false;
            }
            return null;
        }
        public bool insertHighScore(int userId, int score)
        {
            if (dbCon.State.ToString().Equals("Open"))
            {
                string query = "INSERT INTO highscores (user_id, score) VALUES(" + userId.ToString() + ", " + score.ToString() + ");";
                SQLiteCommand command = new SQLiteCommand(query, dbCon);
                command.ExecuteNonQuery();
                Console.WriteLine("Write highscore");
                return true;
            }
            return false;
        }
        public int? getUserIdFromUserName(string userName)
        {
            if (dbCon.State.ToString().Equals("Open"))
            {
                string query = "SELECT id FROM users WHERE username LIKE '" + userName + "';";
                SQLiteCommand command = new SQLiteCommand(query, dbCon);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    return int.Parse(reader["id"].ToString());
                }
                return -1;
            }
            return null;
        }
        public string getUserNameFromId(int userId)
        {
            if (dbCon.State.ToString().Equals("Open"))
            {
                string query = "SELECT username FROM users WHERE id=" + userId.ToString() + ";";
                SQLiteCommand command = new SQLiteCommand(query, dbCon);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    return reader["username"].ToString();
                }
                return "";
            }
            return null;
        }
        public void close()
        {
            if (dbCon.State.ToString().Equals("Open"))
            {
                dbCon.Close();
            }
        }

    }
}
