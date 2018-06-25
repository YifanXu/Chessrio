using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class Pawn : Piece
    {

        public override IList<Coords> GetValidLocations(Piece[,] board, out IList<Coords> specials)
        {
            specials = new List<Coords>();
            List<Coords> coords = new List<Coords>();
            int x = this.location.x;
            int y = this.location.y;
            int rankMovement;
            if (side == Player.Black)
            {
                rankMovement = -1;
            }
            else
            {
                rankMovement = 1;
            }
            //move Straight Forward
            if (board[x, rankMovement + y] == null)
            {
                if ((y == 6 && side == Player.White) || (y == 1 && side == Player.Black))
                {
                    specials.Add(new Coords(x, rankMovement + y));
                }
                else
                {
                    if (!moved && board[x, y + rankMovement * 2] == null)
                    {
                        coords.Add(new Coords(x, rankMovement * 2 + y));
                    }
                    coords.Add(new Coords(x, rankMovement + y));
                }
            }
            //Sideways Eating (To right)
            if (x < 7 && board[x + 1, rankMovement + y] != null && board[x + 1, rankMovement + y].side != this.side)
            {
                if ((y == 6 && side == Player.White) || (y == 1 && side == Player.Black))
                {
                    specials.Add(new Coords(x + 1, rankMovement + y));
                }
                else
                {
                    coords.Add(new Coords(x + 1, rankMovement + y));
                }
            }
            // Enpassant (To right)
            else if (x < 7 && board[x + 1, y] != null && board[x + 1, y].side != side && board[x + 1, y].doubleStepped != 0)
            {
                specials.Add(new Coords(x + 1, rankMovement + y));
            }
            //Sideways Eating (To left)
            if (x > 0 && board[x - 1, rankMovement + y] != null && board[x - 1, rankMovement + y].side != this.side)
            {
                if ((y == 6 && side == Player.White) || (y == 1 && side == Player.Black))
                {
                    specials.Add(new Coords(x - 1, rankMovement + y));
                }
                else
                {
                    coords.Add(new Coords(x - 1, rankMovement + y));
                }
            }
            // Enpassant (To left)
            else if (x > 0 && board[x - 1, y] != null && board[x - 1, y].side != side && board[x - 1, y].doubleStepped != 0)
            {
                specials.Add(new Coords(x - 1, rankMovement + y));
            }


            return coords;
        }
    }
}
