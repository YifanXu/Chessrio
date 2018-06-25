using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Assets;
using System;

public enum Player
{
    White,
    Black
}

public class GameControllerScript : MonoBehaviour
{

    public static GameControllerScript instance;
    public GameObject blueCover;
    public GameObject redCover;
    public GameObject purpleCover;
    public GameObject samplePiece;
    public GameObject pausingCover;
    public GameObject promotionMenu;
    public GameObject[] winMenus;
    public GameObject loadMenu;
    public GameObject loadInput;
    public Sprite[] pawn = new Sprite[2];
    public Sprite[] rook = new Sprite[2];
    public Sprite[] knight = new Sprite[2];
    public Sprite[] bishop = new Sprite[2];
    public Sprite[] queen = new Sprite[2];
    public Sprite[] king = new Sprite[2];

    public const float cellSize = 1.2f;
    private float boardCentreX;
    private float boardCentreY;
    private Piece[,] board;
    private List<Piece> pieces;
    public string fileLoadPath = "C:\\CodeStuff\\Chessrio\\pieceLocation.txt";

    public Player turn;
    private List<GameObject> selectorSquares;
    private Piece selected = null;
    private IList<Coords> validDestos;
    private IList<Coords> specials;
    private bool paused = false;

    private static Dictionary<string, Piece> pieceName = new Dictionary<string, Piece>()
    {
        {"p", new Pawn() },
        {"r", new Rook() },
        {"n", new Knight() },
        {"b", new Bishop() },
        {"q", new Queen() },
        {"k", new King() }
    };

    private static Dictionary<string, Sprite[]> pieceSprites;

    // Use this for initialization
    void Start()
    {
        fileLoadPath = PlayerPrefs.GetString("loadPath", fileLoadPath);
        instance = this;
        boardCentreX = this.transform.position.x;
        boardCentreY = this.transform.position.y;
        turn = Player.White;
        selectorSquares = new List<GameObject>();

        pieceSprites = new Dictionary<string, Sprite[]>()
        {
            {"p", pawn },
            {"r", rook },
            {"n", knight },
            {"b", bishop },
            {"q", queen },
            {"k", king }
        };
        board = new Piece[8, 8];
        pieces = new List<Piece>();

        
        if (File.Exists(fileLoadPath))
        {
            Load();
        }
        else
        {
            pausingCover.SetActive(true);
            loadMenu.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !pausingCover.activeInHierarchy)
        {
            Coords pressedCoord = GetMouseCoord();
            if (pressedCoord.IsValid)
            {
                ClearCoverSquares();
                var attemptSelected = board[pressedCoord.x, pressedCoord.y];
                //If the player selected their own piece
                if (attemptSelected != null && attemptSelected.side == turn)
                {
                    selected = attemptSelected;
                    validDestos = selected.GetValidLocations(board, out specials);
                    for (int i = 0; i < validDestos.Count; i++)
                    {
                        GameObject template;
                        if (board[validDestos[i].x, validDestos[i].y] != null)
                        {
                            template = redCover;
                        }
                        else
                        {
                            template = blueCover;
                        }
                        Vector3 loc = new Vector3(cellSize * (validDestos[i].x - 3.5f), cellSize * (validDestos[i].y - 3.5f));
                        var newSquare = Instantiate(template, loc, default(Quaternion));
                        selectorSquares.Add(newSquare);
                    }
                    for (int i = 0; i < specials.Count; i++)
                    {
                        Vector3 loc = new Vector3(cellSize * (specials[i].x - 3.5f), cellSize * (specials[i].y - 3.5f));
                        var newSquare = Instantiate(purpleCover, loc, default(Quaternion));
                        selectorSquares.Add(newSquare);
                    }
                }
                //If the player clicked a square or an enemy as destination while one of their units is selected
                else if (selected != null)
                {
                    //If the player has selected a valid "normal" move
                    foreach (Coords coordinate in validDestos)
                    {
                        if (pressedCoord.x == coordinate.x && pressedCoord.y == coordinate.y)
                        {
                            if (selected is Pawn)
                            {
                                Pawn p = selected as Pawn;
                                if (Mathf.Abs(pressedCoord.y - selected.location.y) == 2)
                                {
                                    p.doubleStepped = 2;
                                }
                            }
                            board[selected.location.x, selected.location.y] = null;
                            selected.location = pressedCoord;
                            Vector3 newPieceLoc = new Vector3(cellSize * (pressedCoord.x - 3.5f), cellSize * (pressedCoord.y - 3.5f));
                            selected.piece.transform.position = newPieceLoc;
                            Piece destinationPiece = board[pressedCoord.x, pressedCoord.y];
                            board[pressedCoord.x, pressedCoord.y] = selected;
                            selected.moved = true;
                            if (destinationPiece != null)
                            {
                                if (destinationPiece is King)
                                {
                                    pausingCover.SetActive(true);
                                    winMenus[(int)turn].SetActive(true);
                                    return;
                                }
                                pieces.Remove(destinationPiece);
                                Destroy(destinationPiece.piece);
                            }
                            SwitchTurn();
                            return;
                        }
                    }
                    //If the player has selected a valid "special" move (En passant. Castling, Pawn Promotion)
                    foreach (Coords coordinate in specials)
                    {
                        if (pressedCoord.x == coordinate.x && pressedCoord.y == coordinate.y)
                        {
                            board[selected.location.x, selected.location.y] = null;
                            selected.location = pressedCoord;
                            Vector3 newPieceLoc = new Vector3(cellSize * (pressedCoord.x - 3.5f), cellSize * (pressedCoord.y - 3.5f));
                            selected.piece.transform.position = newPieceLoc;
                            Piece destinationPiece = board[pressedCoord.x, pressedCoord.y];
                            board[pressedCoord.x, pressedCoord.y] = selected;
                            selected.moved = true;
                            if (destinationPiece != null)
                            {
                                if (destinationPiece is King)
                                {
                                    pausingCover.SetActive(true);
                                    winMenus[(int)turn].SetActive(true);
                                    return;
                                }
                                pieces.Remove(destinationPiece);
                                Destroy(destinationPiece.piece);
                            }
                            //Castle?
                            if (selected is King)
                            {
                                //QueenSide Castle?
                                if (pressedCoord.x == 1)
                                {
                                    Piece castlingKnight = board[0, pressedCoord.y];
                                    castlingKnight.location = new Coords(2, pressedCoord.y);
                                    board[0, pressedCoord.y] = null;
                                    board[2, pressedCoord.y] = castlingKnight;
                                    castlingKnight.piece.transform.position = new Vector3(-1.5f * cellSize, castlingKnight.piece.transform.position.y);
                                    castlingKnight.moved = true;
                                }
                                //KingSideCastle?
                                else if (pressedCoord.x == 6)
                                {
                                    Piece castlingKnight = board[7, pressedCoord.y];
                                    castlingKnight.location = new Coords(5, pressedCoord.y);
                                    board[7, pressedCoord.y] = null;
                                    board[5, pressedCoord.y] = castlingKnight;
                                    castlingKnight.piece.transform.position = new Vector3(1.5f * cellSize, castlingKnight.piece.transform.position.y);
                                    castlingKnight.moved = true;
                                }
                            }
                            //Promotion or En Passe? 
                            else if (selected is Pawn)
                            {
                                //Promotion?
                                if (pressedCoord.y == 0 || pressedCoord.y == 7)
                                {
                                    pausingCover.SetActive(true);
                                    promotionMenu.SetActive(true);
                                    return;
                                }
                                //En passant?
                                else
                                {
                                    Piece capturedPawn = board[pressedCoord.x, pressedCoord.y - 1];
                                    board[pressedCoord.x, pressedCoord.y - 1] = null;
                                    Destroy(capturedPawn.piece);
                                    pieces.Remove(capturedPawn);
                                }
                            }
                            SwitchTurn();
                        }
                    }
                }
            }
        }
    }

    public void TryLoad()
    {
        fileLoadPath = loadInput.GetComponent<InputField>().text;
        if(File.Exists(fileLoadPath))
        {
            PlayerPrefs.SetString("loadPath", fileLoadPath);
            Load();
            pausingCover.SetActive(false);
            loadMenu.SetActive(false);
        }
        else
        {
            loadInput.GetComponent<InputField>().text = string.Empty;
            loadInput.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Invalid location, try again";
        }
    }

    private void Load()
    {
        string[] pieceLocs = File.ReadAllLines(fileLoadPath);
        foreach (string line in pieceLocs)
        {
            string[] pars = line.Split(' ');
            Player side;
            if (pars[0] == "b")
            {
                side = Player.Black;
            }
            else
            {
                side = Player.White;
            }
            Piece type = pieceName[pars[1]];
            int x = int.Parse(pars[2]);
            int y = int.Parse(pars[3]);
            if (board[x, y] != null)
            {
                throw new ArgumentException(String.Format("Duplicate Piece detected at ({0},{1})", x, y));
            }
            System.Reflection.ConstructorInfo a = type.GetType().GetConstructor(new Type[0]);
            Piece newPiece = a.Invoke(new object[0]) as Piece;
            newPiece.location = new Coords(x, y);
            newPiece.side = side;

            board[x, y] = newPiece;
            pieces.Add(newPiece);

            Vector3 newPieceLoc = new Vector3(cellSize * (x - 3.5f), cellSize * (y - 3.5f));
            Sprite newSprite = pieceSprites[pars[1]][(int)side];
            newPiece.piece = Instantiate(samplePiece, newPieceLoc, default(Quaternion));
            newPiece.piece.GetComponent<SpriteRenderer>().sprite = newSprite;
        }
    }

    private Coords GetMouseCoord()
    {
        Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Coords(Mathf.FloorToInt((pz.x - boardCentreX) / 1.3f) + 4, Mathf.FloorToInt((pz.y - boardCentreY) / cellSize) + 4);
    }

    private void ClearCoverSquares()
    {
        foreach (GameObject square in selectorSquares)
        {
            Destroy(square);
        }
        selectorSquares = new List<GameObject>();
    }

    public void PromoteQueen()
    {
        Piece newPiece = new Queen();
        Sprite newSprite = queen[(int)turn];
        Promote(newPiece, newSprite);
    }

    public void PromoteKnight()
    {
        Piece newPiece = new Knight();
        Sprite newSprite = knight[(int)turn];
        Promote(newPiece, newSprite);
    }

    public void PromoteRook()
    {
        Piece newPiece = new Rook();
        Sprite newSprite = rook[(int)turn];
        Promote(newPiece, newSprite);
    }

    private void Promote(Piece piece, Sprite sprite)
    {
        promotionMenu.SetActive(false);
        pausingCover.SetActive(false);
        Destroy(selected.piece);
        if (turn == Player.White)
        {
            piece.location = new Coords(selected.location.x, 7);
            piece.side = Player.White;
        }
        else
        {
            piece.location = new Coords(selected.location.x, 0);
            piece.side = Player.Black;
        }
        pieces.Remove(selected);
        pieces.Add(piece);
        piece.location = selected.location;
        board[selected.location.x, selected.location.y] = null;
        board[selected.location.x, piece.location.y] = piece;
        piece.piece = Instantiate(samplePiece, new Vector3((piece.location.x - 3.5f) * cellSize, (piece.location.y - 3.5f) * cellSize), default(Quaternion));
        piece.piece.GetComponent<SpriteRenderer>().sprite = sprite;
        if (turn == Player.White)
        {
            turn = Player.Black;
            Debug.Log("Black Turn");
        }
        else
        {
            turn = Player.White;
            Debug.Log("White Turn");
        }
        selected = null;
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void SwitchTurn()
    {
        if (turn == Player.White)
        {
            turn = Player.Black;
            Debug.Log("Black Turn");
        }
        else
        {
            turn = Player.White;
            Debug.Log("White Turn");
        }
        foreach (Piece piece in pieces)
        {
            if (piece.doubleStepped > 0)
            {
                piece.doubleStepped--;
            }
        }
        selected = null;
    }

    private void Check()
    {

    }

    private List<Piece> Threatened(Coords coord)
    {
        List<Piece> threats = new List<Piece>();
        foreach (Piece p in pieces)
        {
            IList<Coords> valids;
            if (p.GetValidLocations(board, out valids).Contains(coord))
            {
                threats.Add(p);
            }
        }
        return threats;
    }
    private List<Piece> Thraetened(Coords coord, Type pieceType)
    {
        List<Piece> threats = new List<Piece>();
        foreach (Piece p in pieces)
        {
            IList<Coords> valids;
            if (p.GetType() == pieceType && p.GetValidLocations(board, out valids).Contains(coord))
            {
                threats.Add(p);
            }
        }
        return threats;
    }
}
