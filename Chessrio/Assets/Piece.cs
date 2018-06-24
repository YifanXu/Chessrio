using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public abstract class Piece
    {
        public Coords location;
        public GameObject piece;
        public Player side;
        public bool moved = false;

        public Piece()
        {

        }
        
        public Piece(int xLoc, int y, GameObject piece, Player side)
        {
            location = new Coords(xLoc, y);
            if(!location.IsValid)
            {
                throw new ArgumentException("Location provided is invalid");
            }
            this.piece = piece;
            this.side = side;
        }

        public virtual IList<Coords> GetValidLocations(Piece[,] board, out IList<Coords> specialMoves)
        {
            specialMoves = new List<Coords>();
            return null;
        }

        
    }
}
