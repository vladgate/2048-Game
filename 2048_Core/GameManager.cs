using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace _2048_Core
{
    public sealed class GameManager
    {
        private const uint MAX_VALUE = 131072;
        private static GameManager _instance;

        private Gamefield _currentGamefield;
        private Gamefield _previousGamefield;
        private GameManager()
        {
        }

        /// <summary>
        /// singleton
        /// </summary>
        public static GameManager Instance
        {
            get
            {
                return _instance ?? new GameManager();
            }
        }

        /// <summary>
        /// сохранить текущую игру
        /// </summary>
        /// <param name="gamefield">игровые данные</param>
        internal void SaveGame(GameData gameData)
        {
            string data;
            BinaryFormatter serializer = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            using (StreamReader sr = new StreamReader(ms))
            {
                serializer.Serialize(ms, gameData);
                byte[] ar = ms.ToArray();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < ar.Length; i++)
                {
                    sb.Append(ar[i]);
                    sb.Append('b');
                }
                data = sb.ToString();
            }
            Properties.Settings.Default.Game = data;

        }
        /// <summary>
        /// загрузить последнюю игру
        /// </summary>
        /// <param name="gamefield">игровые данные</param>
        internal GameData LoadGame()
        {
            GameData gameData;
            string data = Properties.Settings.Default.Game;
            string[] s = data.Split('b');
            byte[] byteArray = new byte[s.Length - 1];
            for (int i = 0; i < byteArray.Length; i++)
            {
                byteArray[i] = Byte.Parse(s[i]);
            }

            using (MemoryStream ms = new MemoryStream(byteArray))
            using (StreamWriter sw = new StreamWriter(ms))
            {
                BinaryFormatter serializer = new BinaryFormatter();
                gameData = serializer.Deserialize(ms) as GameData;
            }
            return gameData;
        }


    }


    /// <summary>
    /// инкапсулирует данные для сохранения/загрузки
    /// </summary>
    [Serializable]
    internal sealed class GameData
    {
        /// <summary>
        /// текущий счет
        /// </summary>
        public uint CurrentScore { get; set; }
        /// <summary>
        /// счет на предыдущем ходу
        /// </summary>
        public uint PreviousScore { get; set; }
        /// <summary>
        /// рекорд
        /// </summary>
        public uint HighScore { get; set; }
        /// <summary>
        /// игровое поле
        /// </summary>
        public Gamefield Gamefield { get; set; }
        public GameData()
        {

        }
        public GameData(uint currentScore, uint prevScore, uint highScore, Gamefield gamefield)
        {
            CurrentScore = currentScore;
            PreviousScore = prevScore;
            HighScore = highScore;
            Gamefield = gamefield;
        }

    }
}
