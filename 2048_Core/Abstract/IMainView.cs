using _2048_Core.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048_Core.Abstract
{
    public interface IMainView
    {
        uint CurrentScore { set; }
        uint HighScore { set; }

        event EventHandler ResetHighscore;
        event EventHandler AboutClick;
        event EventHandler RestartClick;
        event EventHandler<GameParametersEventArgs> NewGameClick;

        /// <summary>
        /// откат на один ход назад
        /// </summary>
        event EventHandler BackClick;
        event EventHandler<ExitGameEventArgs> ExitClick;
        event EventHandler<MousePositionEventArgs> MouseDownView;
        event EventHandler<MousePositionEventArgs> MouseUpView;

        /// <summary>
        /// отрисовать пустое игровое поле под заданное количество ячеек
        /// </summary>
        /// <param name="width">количество ячеек по ширине</param>
        /// <param name="height">количество ячеек по высоте</param>
        void DrawGameField(int width, int height);
        /// <summary>
        /// закрыть представление
        /// </summary>
        void Close();

        /// <summary>
        /// установить значение '0' по указанному индексу
        /// </summary>
        void Set0(int i, int j);
        void Set2(int i, int j);
        void Set4(int i, int j);
        void Set8(int i, int j);
        void Set16(int i, int j);
        void Set32(int i, int j);
        void Set64(int i, int j);
        void Set128(int i, int j);
        void Set256(int i, int j);
        void Set512(int i, int j);
        void Set1024(int i, int j);
        void Set2048(int i, int j);
        void Set4096(int i, int j);
        void Set8192(int i, int j);
        void Set16384(int i, int j);
        void Set32768(int i, int j);
        void Set65536(int i, int j);
        void Set131072(int i, int j);
    }
}
