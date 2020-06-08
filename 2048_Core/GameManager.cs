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

        internal event EventHandler WinGame;
        internal event EventHandler LooseGame;
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

        public byte GameFieldWidth { get => _currentGamefield.Width; }

        public byte GameFieldHeight { get => _currentGamefield.Height; }

        public uint[,] Field { get => _currentGamefield.Field; }

        /// <summary>
        /// сброс рекорда
        /// </summary>
        internal void ResetHighScore()
        {
            _highScore = 0;
        }

        /// <summary>
        /// шаг назад
        /// </summary>
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

        public uint HighScore { get => _highScore; }

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

        internal void MoveDown()
        {
            bool canPlaceNewValue = false;
            Gamefield tempArr = _currentGamefield.Clone();

            for (int j = 0; j < _currentGamefield.Width; j++)
            {
                int maxEmptyRow = _currentGamefield.Height - 1;
                for (int i = _currentGamefield.Width - 1; i >= 0; i--)
                {
                    if (tempArr[i, j] == 0)
                    {
                        if (maxEmptyRow < i) maxEmptyRow = i;
                        continue;
                    }
                    else
                    {
                        if (maxEmptyRow < i) maxEmptyRow = i;
                        uint neededValue = tempArr[i, j];
                        if (i == 0 || i < maxEmptyRow)
                        {
                            tempArr[maxEmptyRow, j] = tempArr[i, j];
                            if (i != maxEmptyRow)
                            {
                                tempArr[i, j] = 0;
                                canPlaceNewValue = true;
                                i = maxEmptyRow + 1;
                                continue;
                            }
                            if (i == 0) break;
                        }
                        int initI = i;
                        for (int k = i - 1; k >= 0; k--)
                        {
                            if (tempArr[k, j] == neededValue)
                            {
                                tempArr[maxEmptyRow, j] = neededValue * 2;
                                _previousScore = _currentScore;
                                _currentScore += neededValue * 2;
                                if (i != maxEmptyRow) tempArr[i, j] = 0;
                                tempArr[k, j] = 0;
                                i = k;
                                canPlaceNewValue = true;
                                maxEmptyRow--;
                                break;
                            }
                            else if (tempArr[k, j] > 0)
                            {
                                tempArr[maxEmptyRow - 1, j] = tempArr[k, j];
                                i = maxEmptyRow;
                                if (maxEmptyRow - 1 != k)
                                {
                                    tempArr[k, j] = 0;
                                    canPlaceNewValue = true;
                                }
                                maxEmptyRow -= 2;
                                break;
                            }
                            else
                            {
                                if (k == 0)
                                {
                                    tempArr[maxEmptyRow, j] = tempArr[i, j];
                                    if (i != maxEmptyRow) tempArr[i, j] = 0;
                                    if (initI != maxEmptyRow && (maxEmptyRow - i) > 0) canPlaceNewValue = true;
                                    i = k;
                                }
                                else if (tempArr[maxEmptyRow, j] == 0 && tempArr[k, j] > 0)
                                {
                                    tempArr[maxEmptyRow, j] = tempArr[i, j];
                                    i++;
                                }
                            }
                        }
                    }
                }
            }
            if (_highScore > MAX_SCORE)
            {
                WinGame?.Invoke(this, EventArgs.Empty);
            }
            if (_highScore < _currentScore)
            {
                _highScore = _currentScore;
            }
            if (canPlaceNewValue)
            {
                _previousGamefield = _currentGamefield;
                _currentGamefield = tempArr;
                PlaceNewValue();
            }
        }

        internal void MoveUp()
        {
            bool canPlaceNewValue = false;
            Gamefield tempArr = _currentGamefield.Clone();

            for (int j = 0; j < _currentGamefield.Height; j++)
            {
                int maxEmptyRow = 0;
                for (int i = 0; i <= _currentGamefield.Width - 1; i++)
                {
                    if (tempArr[i, j] == 0)
                    {
                        if (maxEmptyRow > i) maxEmptyRow = i;
                        continue;
                    }
                    else
                    {
                        if (maxEmptyRow > i) maxEmptyRow = i;
                        uint neededValue = tempArr[i, j];
                        if (i == _currentGamefield.Width - 1 || i > maxEmptyRow)
                        {
                            tempArr[maxEmptyRow, j] = tempArr[i, j];
                            if (i != maxEmptyRow)
                            {
                                tempArr[i, j] = 0;
                                canPlaceNewValue = true;
                                i = maxEmptyRow - 1; continue;
                            }
                            if (i == _currentGamefield.Width - 1) break;
                        }
                        int initI = i;
                        for (int k = i + 1; k <= _currentGamefield.Width - 1; k++)
                        {
                            if (tempArr[k, j] == neededValue)
                            {
                                tempArr[maxEmptyRow, j] = neededValue * 2;
                                _previousScore = _currentScore;
                                _currentScore += neededValue * 2;
                                if (i != maxEmptyRow) tempArr[i, j] = 0;
                                tempArr[k, j] = 0;
                                i = k;
                                canPlaceNewValue = true;
                                maxEmptyRow++;
                                break;
                            }
                            else if (tempArr[k, j] > 0)
                            {
                                tempArr[maxEmptyRow + 1, j] = tempArr[k, j];
                                i = maxEmptyRow;
                                if (maxEmptyRow + 1 != k)
                                {
                                    tempArr[k, j] = 0;
                                    canPlaceNewValue = true;
                                }
                                maxEmptyRow += 2;
                                break;
                            }
                            else
                            {
                                if (k == _currentGamefield.Width - 1)
                                {
                                    tempArr[maxEmptyRow, j] = tempArr[i, j];
                                    if (i != maxEmptyRow) tempArr[i, j] = 0;
                                    if (initI != maxEmptyRow && (i - maxEmptyRow) > 0) canPlaceNewValue = true;
                                    i = k;
                                }
                                else if (tempArr[maxEmptyRow, j] == 0 && tempArr[k, j] > 0)
                                {
                                    tempArr[maxEmptyRow, j] = tempArr[i, j];
                                    i--;
                                }
                            }
                        }
                    }
                }
            }
            if (_highScore > MAX_SCORE)
            {
                WinGame?.Invoke(this, EventArgs.Empty);
            }
            if (_highScore < _currentScore)
            {
                _highScore = _currentScore;
            }
            if (canPlaceNewValue)
            {
                _previousGamefield = _currentGamefield;
                _currentGamefield = tempArr;
                PlaceNewValue();
            }
        }

        internal void MoveRight()
        {
            bool canPlaceNewValue = false;
            Gamefield tempArr = _currentGamefield.Clone();

            for (int i = 0; i < _currentGamefield.Width; i++)
            {
                int maxEmptyColumn = _currentGamefield.Width - 1;
                for (int j = _currentGamefield.Height - 1; j >= 0; j--)
                {
                    if (tempArr[i, j] == 0)
                    {
                        if (maxEmptyColumn < j) maxEmptyColumn = j;
                        continue;
                    }
                    else
                    {
                        if (maxEmptyColumn < j) maxEmptyColumn = j;
                        uint neededValue = tempArr[i, j];
                        if (j == 0 || j < maxEmptyColumn)
                        {
                            tempArr[i, maxEmptyColumn] = tempArr[i, j];
                            if (j != maxEmptyColumn)
                            {
                                tempArr[i, j] = 0;
                                canPlaceNewValue = true;
                                j = maxEmptyColumn + 1; continue;
                            }
                            if (j == 0) break;
                        }
                        int initJ = j;
                        for (int k = j - 1; k >= 0; k--)
                        {
                            if (tempArr[i, k] == neededValue)
                            {
                                tempArr[i, maxEmptyColumn] = neededValue * 2;
                                _previousScore = _currentScore;
                                _currentScore += neededValue * 2;
                                if (j != maxEmptyColumn) tempArr[i, j] = 0;
                                tempArr[i, k] = 0;
                                j = k;
                                canPlaceNewValue = true;
                                maxEmptyColumn--;
                                break;
                            }
                            else if (tempArr[i, k] > 0)
                            {
                                tempArr[i, maxEmptyColumn - 1] = tempArr[i, k];
                                j = maxEmptyColumn;

                                if (maxEmptyColumn - 1 != k)
                                {
                                    tempArr[i, k] = 0;
                                    canPlaceNewValue = true;
                                }
                                maxEmptyColumn -= 2;
                                break;
                            }
                            else
                            {
                                if (k == 0)
                                {
                                    tempArr[i, maxEmptyColumn] = tempArr[i, j];
                                    if (j != maxEmptyColumn) tempArr[i, j] = 0;
                                    if (initJ != maxEmptyColumn && (maxEmptyColumn - j) > 0) canPlaceNewValue = true;
                                    j = k;
                                }
                                else if (tempArr[i, maxEmptyColumn] == 0 && tempArr[i, k] > 0)
                                {
                                    tempArr[i, maxEmptyColumn] = tempArr[i, j];
                                    j++;
                                }
                            }
                        }
                    }
                }
            }
            if (_highScore > MAX_SCORE)
            {
                WinGame?.Invoke(this, EventArgs.Empty);
            }
            if (_highScore < _currentScore)
            {
                _highScore = _currentScore;
            }
            if (canPlaceNewValue)
            {
                _previousGamefield = _currentGamefield;
                _currentGamefield = tempArr;
                PlaceNewValue();
            }
        }

        internal void MoveLeft()
        {
            bool canPlaceNewValue = false;
            Gamefield tempArr = _currentGamefield.Clone();

            for (int i = 0; i < _currentGamefield.Width; i++)
            {
                int maxEmptyColumn = 0;
                for (int j = 0; j <= _currentGamefield.Height - 1; j++)
                {
                    if (tempArr[i, j] == 0)
                    {
                        if (maxEmptyColumn > j) maxEmptyColumn = j;
                        continue;
                    }
                    else
                    {
                        if (maxEmptyColumn > j) maxEmptyColumn = j;
                        uint neededValue = tempArr[i, j];
                        if (j == _currentGamefield.Height - 1 || j > maxEmptyColumn)
                        {
                            tempArr[i, maxEmptyColumn] = tempArr[i, j];
                            if (j != maxEmptyColumn)
                            {
                                tempArr[i, j] = 0;
                                canPlaceNewValue = true;
                                j = maxEmptyColumn - 1; continue;
                            }
                            if (j == _currentGamefield.Height - 1) break;
                        }
                        int initJ = j;
                        for (int k = j + 1; k <= _currentGamefield.Height - 1; k++)
                        {
                            if (tempArr[i, k] == neededValue)
                            {
                                tempArr[i, maxEmptyColumn] = neededValue * 2;
                                _previousScore = _currentScore;
                                _currentScore += neededValue * 2;
                                if (j != maxEmptyColumn) tempArr[i, j] = 0;
                                tempArr[i, k] = 0;
                                j = k;
                                canPlaceNewValue = true;
                                maxEmptyColumn++;
                                break;
                            }
                            else if (tempArr[i, k] > 0)
                            {
                                tempArr[i, maxEmptyColumn + 1] = tempArr[i, k];
                                j = maxEmptyColumn;

                                if (maxEmptyColumn + 1 != k)
                                {
                                    tempArr[i, k] = 0;
                                    canPlaceNewValue = true;
                                }
                                maxEmptyColumn += 2;
                                break;
                            }
                            else
                            {
                                if (k == _currentGamefield.Height - 1)
                                {
                                    tempArr[i, maxEmptyColumn] = tempArr[i, j];
                                    if (j != maxEmptyColumn) tempArr[i, j] = 0;
                                    if (initJ != maxEmptyColumn && (j - maxEmptyColumn) > 0) canPlaceNewValue = true;
                                    j = k;
                                }
                                else if (tempArr[i, maxEmptyColumn] == 0 && tempArr[i, k] > 0)
                                {
                                    tempArr[i, maxEmptyColumn] = tempArr[i, j];
                                    j--;
                                }
                            }
                        }
                    }
                }
            }
            if (_highScore > MAX_SCORE)
            {
                WinGame?.Invoke(this, EventArgs.Empty);
            }
            if (_highScore < _currentScore)
            {
                _highScore = _currentScore;
            }
            if (canPlaceNewValue)
            {
                _previousGamefield = _currentGamefield;
                _currentGamefield = tempArr;
                PlaceNewValue();
            }
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
            if (!GamefieldHasMove())
            {
                /*throw new NoEmptyCellException()*/
                ;//все ячейки заполнены - проигрыш
                LooseGame?.Invoke(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// есть ли доступный ход
        /// </summary>
        /// <returns>true если есть доступный ход</returns>
        private bool GamefieldHasMove()
        {
            if (_currentGamefield.HasEmptyCell()) // если есть пустая ячейка - ход есть
            {
                return true;
            };
            //если есть рядом две одинаковые ячейки - ход есть
            for (int i = 0; i < _currentGamefield.Width - 1; i++)
            {
                for (int j = 0; j < _currentGamefield.Height; j++)
                {
                    if (_currentGamefield[i, j] == _currentGamefield[i + 1, j])
                    {
                        return true;
                    }
                }
            }
            for (int i = 0; i < _currentGamefield.Width; i++)
            {
                for (int j = 0; j < _currentGamefield.Height - 1; j++)
                {
                    if (_currentGamefield[i, j] == _currentGamefield[i, j + 1])
                    {
                        return true;
                    }
                }
            }

            return false;
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
