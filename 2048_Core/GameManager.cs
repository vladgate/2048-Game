#define CONTRACTS_FULL

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace _2048_Core
{
    public sealed class GameManager
    {
        private const uint MAX_SCORE = 131072;
        private const byte DEFAULT_WIDTH = 4;
        private const byte DEFAULT_HEIGHT = 4;

        private static GameManager _instance;

        private Gamefield _currentGamefield;
        private Gamefield _previousGamefield;
        private uint _currentScore;
        private uint _previousScore;
        private uint _highScore;

        internal string GameString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _currentGamefield.Width; i++)
            {
                for (int j = 0; j < _currentGamefield.Height; j++)
                {
                    sb.Append("\t");
                    sb.Append(_currentGamefield[i, j]);
                }
                sb.Append("\n");
            };
            return sb.ToString();
        }

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
                return _instance ?? (_instance = new GameManager());
            }
        }

        public int GameFieldWidth { get => _currentGamefield.Width; }
        public int GameFieldHeight { get => _currentGamefield.Height; }
        public uint[,] Field { get => _currentGamefield.Field; }

        internal void StepBack()
        {
            if (_currentGamefield == _previousGamefield) //некуда откатываться
            {
                return;
            }
            _currentGamefield = _previousGamefield;
            _currentScore = _previousScore;
        }

        public uint CurrentScore { get => _currentScore; }
        public uint PreviousScore { get => _previousScore; }
        public uint HighScore { get => _highScore; set => _highScore = value; }


        /// <summary>
        /// сохранить текущую игру
        /// </summary>
        /// <param name="gamefield">игровые данные</param>
        internal void SaveGame()
        {
            GameData gameData = new GameData(_currentScore, _previousScore, _highScore, _currentGamefield, _previousGamefield);
            SaveGame(gameData);
        }
        private void SaveGame(GameData gameData)
        {
            if (gameData == null)
            {
                throw new ArgumentNullException(nameof(gameData));
            }
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
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// загрузить последнюю игру
        /// </summary>
        /// <returns>null если не удалось загрузить игровые данные</returns>
        internal GameData LoadGame()
        {
            GameData gameData = null;
            string data = Properties.Settings.Default.Game;
            if (string.IsNullOrEmpty(data))
            {
                return gameData;
            }
            string[] s = data.Split('b');
            byte[] byteArray = new byte[s.Length - 1];
            for (int i = 0; i < byteArray.Length; i++)
            {
                byteArray[i] = Byte.Parse(s[i]);
            }
            try
            {
                using (MemoryStream ms = new MemoryStream(byteArray))
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    BinaryFormatter serializer = new BinaryFormatter();
                    gameData = serializer.Deserialize(ms) as GameData;
                }
            }
            catch (SerializationException)
            {
            }
            return gameData;
        }

        /// <summary>
        /// размещает новое значение (2 или 4) на игровом поле
        /// </summary>
        /// <exception cref="NoEmptyCellException">если нет свободных ячеек</exception>
        private void PlaceNewValue()
        {
            if (_currentGamefield.HasEmptyCell())
            {
                Random rnd = new Random();
                int newX, newY;
                do
                {
                    newX = rnd.Next(_currentGamefield.Width);
                    newY = rnd.Next(_currentGamefield.Height);
                }
                while (_currentGamefield[newX, newY] != 0);
                _currentGamefield[newX, newY] = (uint)(rnd.Next(0, 100) > 10 ? 2 : 4);
            }
            else  //все ячейки заполнены - проигрыш
            {
                throw new NoEmptyCellException();
            }
        }

        /// <summary>
        /// метод при запуске игры
        /// </summary>
        internal void LaunchGame()
        {
            GameData gameData = LoadGame();
            if (gameData == null) //нет сохранения
            {
                StartNewGame(DEFAULT_WIDTH, DEFAULT_HEIGHT);
            }
            else //есть сохранение
            {
                ContinueGame(gameData);
            }
        }

        /// <summary>
        /// запуск новой игры указанных размеров
        /// </summary>
        internal void StartNewGame(byte width, byte height)
        {
            _currentScore = _previousScore = 0;
            if (_currentGamefield == null) //данные о текущей игре отсутствуют
            {
                _previousGamefield = new Gamefield(width, height);
            }
            //изменились размеры игрового поля
            else if (_currentGamefield != null && _currentGamefield.Width == _previousGamefield.Width && _currentGamefield.Height == _previousGamefield.Width)
            {
                _previousGamefield = _currentGamefield;
            }
            else // размеры не поменялись
            {
                _previousGamefield = _currentGamefield;
            }
            _currentGamefield = new Gamefield(width, height);
            //добавляем две ячейки со значениями
            PlaceNewValue();
            PlaceNewValue();
        }


        /// <summary>
        /// продолжаем игру
        /// </summary>
        internal void ContinueGame(GameData gameData)
        {
            if (gameData == null)
            {
                throw new ArgumentNullException(nameof(gameData));
            }
            _currentGamefield = gameData.CurrentGamefield;
            _previousGamefield = gameData.PreviousGamefield;
            _currentScore = gameData.CurrentScore;
            _previousScore = gameData.PreviousScore;
            _highScore = gameData.HighScore;
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
        public Gamefield CurrentGamefield { get; set; }
        public Gamefield PreviousGamefield { get; set; }
        public GameData(uint currentScore, uint prevScore, uint highScore, Gamefield currentGamefield, Gamefield previousGamefield)
        {
            CurrentGamefield = currentGamefield ?? throw new ArgumentNullException(nameof(currentGamefield));
            PreviousGamefield = previousGamefield ?? throw new ArgumentNullException(nameof(previousGamefield));
            CurrentScore = currentScore;
            PreviousScore = prevScore;
            HighScore = highScore;
        }
    }

    /// <summary>
    /// вбрасывается при попытке разместить новое значение когда нет свободных ячеек
    /// </summary>
    internal sealed class NoEmptyCellException : Exception
    {
        public NoEmptyCellException() { }
        public NoEmptyCellException(string message) : base(message) { }
    }
}
