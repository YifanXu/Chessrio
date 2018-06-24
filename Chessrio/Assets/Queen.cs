using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    class Queen: Piece
    {
        public override IList<Coords> GetValidLocations(Piece[,] board, out IList<Coords> specials)
        {
            specials = new List<Coords>();
            int x = location.x;
            int y = location.y;
            List<Coords> coords = new List<Coords>();
            //X++Y++
            Coords newCoord = new Coords(x + 1, y + 1);
            while (newCoord.IsValid)
            {
                if (board[newCoord.x, newCoord.y] == null)
                {
                    coords.Add(new Coords(newCoord.x, newCoord.y));
                }
                else if (board[newCoord.x, newCoord.y].side != this.side)
                {
                    coords.Add(new Coords(newCoord.x, newCoord.y));
                    break;
                }
                else
                {
                    break;
                }
                newCoord.x++;
                newCoord.y++;
            }
            //X++Y--
            newCoord = new Coords(x + 1, y - 1);
            while (newCoord.IsValid)
            {
                if (board[newCoord.x, newCoord.y] == null)
                {
                    coords.Add(new Coords(newCoord.x, newCoord.y));
                }
                else if (board[newCoord.x, newCoord.y].side != this.side)
                {
                    coords.Add(new Coords(newCoord.x, newCoord.y));
                    break;
                }
                else
                {
                    break;
                }
                newCoord.x++;
                newCoord.y--;
            }
            //X--Y++
            newCoord = new Coords(x - 1, y + 1);
            while (newCoord.IsValid)
            {
                if (board[newCoord.x, newCoord.y] == null)
                {
                    coords.Add(new Coords(newCoord.x, newCoord.y));
                }
                else if (board[newCoord.x, newCoord.y].side != this.side)
                {
                    coords.Add(new Coords(newCoord.x, newCoord.y));
                    break;
                }
                else
                {
                    break;
                }
                newCoord.x--;
                newCoord.y++;
            }
            //X--Y--
            newCoord = new Coords(x - 1, y - 1);
            while (newCoord.IsValid)
            {
                if (board[newCoord.x, newCoord.y] == null)
                {
                    coords.Add(new Coords(newCoord.x, newCoord.y));
                }
                else if (board[newCoord.x, newCoord.y].side != this.side)
                {
                    coords.Add(new Coords(newCoord.x, newCoord.y));
                    break;
                }
                else
                {
                    break;
                }
                newCoord.x--;
                newCoord.y--;
            }
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
