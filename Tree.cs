using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_1
{
    public class Tree : Cell
    {
        public Tree(int x, int y, string value, bool movable, bool crossable) :
            base(x, y, value, movable, crossable)
        {
        }

        public Tree(int x, int y) :
            base(x, y, "T", false, false)
        {
        }
    }
}
