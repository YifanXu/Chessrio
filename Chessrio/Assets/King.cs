using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    class King: Piece
    {
        public override IList<Coords> GetValidLocations(Piece[,] board, out IList<Coords> specials)
        {
            specials = new List<Coords>();
            int x = this.location.x;
            int y = this.location.y;
            List<Coords> coords = new List<Coords>()
            {
                new Coords(x + 1, y),
                new Coords(x + 1, y + 1),
                new Coords(x + 1, y - 1),
                new Coords(x - 1, y),
                new Coords(x - 1, y - 1),
                new Coords(x - 1, y + 1),
                new Coords(x, y + 1),
                new Coords(x, y - 1),
            };
            for (int i = coords.Count - 1; i >= 0; i--)
            {
                if (!coords[i].IsValid || (board[coords[i].x, coords[i].y] != null && board[coords[i].x, coords[i].y].side == this.side))
                {
                    coords.RemoveAt(i);
                }
            }
            //Can it: Castle?
            if (!this.moved)
            {
                //QueenSide Castle
                if (board[1, y] == null && board[2, y] == null && board[3, y] == null && board[0, y] != null && !board[0, y].moved)
                {
                    specials.Add(new Coords(1, y));
                }

                //KingSide Castle
                if (board[5, y] == null && board[6, y] == null && board[7, y] != null && !board[7, y].moved)
                {
                    specials.Add(new Coords(6, y));
                }
            }

            return coords;
        }
    }
}
