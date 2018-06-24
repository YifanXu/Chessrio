using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class Bishop: Piece
    {
        public override IList<Coords> GetValidLocations(Piece[,] board, out IList<Coords> specials)
        {
            specials = new List<Coords>();
            List<Coords> coords = new List<Coords>();
            int x = this.location.x;
            int y = this.location.y;
            //X++Y++
            Coords newCoord = new Coords(x + 1, y + 1);
            while(newCoord.IsValid)
            {
                if (board[newCoord.x,newCoord.y] == null)
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
            return coords;
        }
    }
}
