using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048_Core
{
    [Serializable]
    internal sealed class Gamefield
    {
        private uint[,] _field;
        private readonly byte _width;
        private readonly byte _height;

        public byte Width => _width;

        public byte Height => _height;

        /// <summary>
        /// Создает новое игровое поле заданных размеров
        /// </summary>
        /// <param name="width">Количество ячеек по ширине</param>
        /// <param name="height">Количество ячеек по высоте</param>
        public Gamefield(byte width, byte height)
        {
            _width = width;
            _height = height;
            _field = new uint[width, height];
        }

        /// <summary>
        /// индексатор
        /// </summary>
        /// <param name="i">индекс по х</param>
        /// <param name="j">индекс по у</param>
        /// <returns></returns>
        public uint this[int i, int j]
        {
            get
            {
                return _field[i, j];
            }
            private set
            {
                _field[i, j] = value;
            }
        }

        /// <summary>
        /// клонирует игровое поле
        /// </summary>
        /// <returns>игровое поле - клон текущего</returns>
        public Gamefield Clone()
        {
            Gamefield clone = new Gamefield(_width, _height);
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    clone[i, j] = _field[i, j];
                }
            }
            return clone;
        }

        /// <summary>
        /// проверка, есть ли на игровом поле пустая ячейка
        /// </summary>
        /// <returns>наличие пустой ячейки</returns>
        public bool HasEmptyCell()
        {
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    if (_field[i, j] == 0) return true;
                }
            }
            return false;
        }
    }
}
