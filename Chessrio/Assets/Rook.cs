using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    class Rook : Piece
    {
        public override IList<Coords> GetValidLocations(Piece[,] board, out IList<Coords> specials)
        {
            specials = new List<Coords>();
            List<Coords> coords = new List<Coords>();
            //X++
            int newX = location.x + 1;
            while (newX <= 7)
            {
                if (board[newX, this.location.y] == null)
                {
                    coords.Add(new Coords(newX, this.location.y));
                }
                else if (board[newX, this.location.y].side != this.side)
                {
                    coords.Add(new Coords(newX, this.location.y));
                    break;
                }
                else
                {
                    break;
                }
                newX++;
            }
            //X--
            newX = location.x - 1;
            while (newX >= 0)
            {
                if (board[newX, this.location.y] == null)
                {
                    coords.Add(new Coords(newX, this.location.y));
                }
                else if (board[newX, this.location.y].side != this.side)
                {
                    coords.Add(new Coords(newX, this.location.y));
                    break;
                }
                else
                {
                    break;
                }
                newX--;
            }
            //Y++
            int newY = location.y + 1;
            while (newY <= 7)
            {
                if (board[this.location.x, newY] == null)
                {
                    coords.Add(new Coords(this.location.x, newY));
                }
                else if (board[this.location.x, newY].side != this.side)
                {
                    coords.Add(new Coords(this.location.x, newY));
                    break;
                }
                else
                {
                    break;
                }
                newY++;
            }
            //Y--
            newY = location.y - 1;
            while (newY >= 0)
            {
                if (board[this.location.x, newY] == null)
                {
                    coords.Add(new Coords(this.location.x, newY));
                }
                else if (board[this.location.x, newY].side != this.side)
                {
                    coords.Add(new Coords(this.location.x, newY));
                    break;
                }
                else
                {
                    break;
                }
                newY--;
            }

            return coords;
        }
    }
}
