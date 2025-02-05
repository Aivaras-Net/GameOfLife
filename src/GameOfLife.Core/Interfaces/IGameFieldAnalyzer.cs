using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Core.Interfaces
{
    public interface IGameFieldAnalyzer
    {
        int CountLivingCells(bool[,] field);
    }
}
