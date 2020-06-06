using _2048_Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048_Core.Presenter
{
    public sealed class Presenter
    {
        private GameManager _gameManager;
        IMainView _mainView;
        private IMessageService _messageService;
        public Presenter(IMainView mainView, IMessageService messageService)
        {
            _gameManager = GameManager.Instance;
            _mainView = mainView;
            _messageService = messageService;
            _mainView.ResetHighscore += MainView_ResetHighscore;
            _mainView.RestartClick += MainView_RestartClick;
            _mainView.NewGameClick += MainView_NewGameClick;
            _mainView.BackClick += MainView_BackClick;
            _mainView.AboutClick += MainView_AboutClick;
            _mainView.ExitClick += MainView_ExitClick;
            _mainView.MouseDownView += MainView_MouseDownView;
            _mainView.MouseUpView += MainView_MouseUpView;

            _gameManager.LaunchGame();
            InitView();
        }

        private void MainView_RestartClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MainView_ResetHighscore(object sender, EventArgs e)
        {
            _gameManager.HighScore = 0;
            _mainView.HighScore = 0;
        }


        private void MainView_NewGameClick(object sender, GameParametersEventArgs e)
        {
            _gameManager.StartNewGame(e.Width, e.Height);
            InitView();
        }

        private void MainView_BackClick(object sender, EventArgs e)
        {
            int prevWidth = _gameManager.GameFieldWidth;
            int prevHeight = _gameManager.GameFieldHeight;
            _gameManager.StepBack();
            if (prevWidth != _gameManager.GameFieldWidth || prevHeight != _gameManager.GameFieldHeight)
            {
                _mainView.DrawGameField(_gameManager.GameFieldWidth, _gameManager.GameFieldHeight);
            }

            DrawCellsOnView(_gameManager.Field);
            _mainView.CurrentScore = _gameManager.CurrentScore;
            _mainView.HighScore = _gameManager.HighScore;
        }

        private void MainView_AboutClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MainView_ExitClick(object sender, ExitGameEventArgs e)
        {
            _gameManager.SaveGame();
            if (e.NeedCloseView)
            {
                _mainView.Close();
            }
        }
        private void MainView_MouseUpView(object sender, MousePositionEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MainView_MouseDownView(object sender, MousePositionEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void InitView()
        {
            _mainView.DrawGameField(_gameManager.GameFieldWidth, _gameManager.GameFieldHeight);
            DrawCellsOnView(_gameManager.Field);
            _mainView.CurrentScore = _gameManager.CurrentScore;
            _mainView.HighScore = _gameManager.HighScore;
        }

        private void DrawCellsOnView(uint[,] cells)
        {
            for (int i = 0; i < _gameManager.GameFieldWidth; i++)
            {
                for (int j = 0; j < _gameManager.GameFieldHeight; j++)
                {
                    switch (cells[i, j])
                    {
                        case 0: _mainView.Set0(i, j); break;
                        case 2: _mainView.Set2(i, j); break;
                        case 4: _mainView.Set4(i, j); break;
                        case 8: _mainView.Set8(i, j); break;
                        case 16: _mainView.Set16(i, j); break;
                        case 32: _mainView.Set32(i, j); break;
                        case 64: _mainView.Set64(i, j); break;
                        case 128: _mainView.Set128(i, j); break;
                        case 256: _mainView.Set256(i, j); break;
                        case 512: _mainView.Set512(i, j); break;
                        case 1024: _mainView.Set1024(i, j); break;
                        case 2048: _mainView.Set2048(i, j); break;
                        case 4096: _mainView.Set4096(i, j); break;
                        case 8192: _mainView.Set8192(i, j); break;
                        case 16384: _mainView.Set16384(i, j); break;
                        case 32768: _mainView.Set32768(i, j); break;
                        case 65536: _mainView.Set65536(i, j); break;
                        case 131072: _mainView.Set131072(i, j); break;
                    }
                }
            }
        }
    }
}
