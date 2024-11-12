using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_1
{
    public class Rock : Cell
    {
        public Rock(int x, int y, string value, bool movable, bool crossable) :
            base(x, y, value, movable, crossable)
        {
        }

        // Дополнительный конструктор, по умолчанию делаем камень перемещаемым
        public Rock(int x, int y) :
            base(x, y, "R", true, false) // isMovable по умолчанию true
        {
        }
    }
}
