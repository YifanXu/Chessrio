using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    class Knight: Piece
    {
        public override IList<Coords> GetValidLocations(Piece[,] board, out IList<Coords> specials)
        {
            specials = new List<Coords>();
            int x = this.location.x;
            int y = this.location.y;
            List<Coords> coords = new List<Coords>()
            {
                new Coords(x + 1, y + 2),
                new Coords(x + 2, y + 1),
                new Coords(x + 1, y - 2),
                new Coords(x + 2, y - 1),
                new Coords(x - 1, y + 2),
                new Coords(x - 2, y + 1),
                new Coords(x - 1, y - 2),
                new Coords(x - 2, y - 1),
            };
            for(int i = coords.Count - 1; i >= 0; i--)
            {
                if(!coords[i].IsValid || (board[coords[i].x,coords[i].y] != null && board[coords[i].x, coords[i].y].side == this.side))
                {
                    coords.RemoveAt(i);
                }
            }
            return coords;
        }
    }
}
