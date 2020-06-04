using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048_Core.Presenter
{
    public sealed class Presenter
    {
        public Presenter(GameManager gameManager)
        {
            gameManager.SaveGame(null);
        }
    }
}
