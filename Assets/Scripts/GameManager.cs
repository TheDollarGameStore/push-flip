using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    MOVE,
    POP,
    FLIP
}

public class GameManager : MonoBehaviour
{
    private long score;
    private long displayScore;

    private int combo;

    [SerializeField] Text scoreText;
    [SerializeField] Text comboText;

    public static GameManager instance = null;

    [HideInInspector] public GameState state;

    [SerializeField] public int boardSize;

    [SerializeField] public int tileSize;

    [SerializeField] private List<GameObject> piecePrefabs;

    private Piece[,] pieces;

    private float boardRotation;

    [SerializeField] private GameObject board;

    [HideInInspector] public float offset;

    private int depthCounter = 0;

    List<List<Piece>> groups;

    [SerializeField] private GameObject particle;

    [SerializeField] private Flipper flipper;

    [SerializeField] private CameraBehaviour cameraBehaviour;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        combo = 1;
        state = GameState.MOVE;

        pieces = new Piece[boardSize, boardSize];
        offset = (-(float)boardSize / 2f) + 0.5f;
        FillBoard();
    }

    void FillBoard()
    {
        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                if (pieces[y, x] == null)
                {
                    pieces[y, x] = Instantiate(GetRandomNonMatchingPiece(x, y), GetPositionByIndex(y, x), Quaternion.identity).GetComponent<Piece>();
                }
            }
        }
    }

    Vector2 GetPositionByIndex(int y, int x)
    {
        return new Vector2((offset + x) * tileSize, (offset + y) * tileSize);
    }

    // Update is called once per frame
    void Update()
    {
        LerpPieces();
        LerpBoard();
    }

    private void FixedUpdate()
    {
        LerpScore();
    }

    void LerpScore()
    {
        if (displayScore < score)
        {
            if (score - displayScore > 100)
            {
                displayScore += 50;
            }
            else if (score - displayScore > 50)
            {
                displayScore += 25;
            }
            else if (score - displayScore > 20)
            {
                displayScore += 20;
            }
            else if (score - displayScore > 10)
            {
                displayScore += 2;
            }
            else
            {
                displayScore++;
            }
        }

        scoreText.text = displayScore.ToString().PadLeft(8, '0');
    }

    void LerpBoard()
    {
        board.transform.rotation = Quaternion.Slerp(board.transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, boardRotation)), 25f * Time.deltaTime);
    }

    void LerpPieces()
    {
        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                if (pieces[y, x] != null)
                {
                    pieces[y, x].transform.position = Vector2.Lerp(pieces[y, x].transform.position, GetPositionByIndex(y, x), 25f * Time.deltaTime);
                }
            }
        }
    }

    public void Push(int pos)
    {
        Piece bottomPiece = pieces[boardSize - 1, pos];

        depthCounter--;
        bottomPiece.GetComponent<SpriteRenderer>().sortingOrder = depthCounter;

        for (int y = boardSize - 1; y > 0; y--)
        {
            pieces[y, pos] = pieces[y - 1, pos];
        }

        pieces[0, pos] = bottomPiece;
        flipper.Flip();
        IncreasePhase();
        cameraBehaviour.Nudge();
        CheckMatches(true);
    }

    public void Flip(int dir)
    {
        if (dir == -1)
        {
            RotateClockwise();
        }
        else
        {
            RotateCounterclockwise();
        }
        flipper.Flip();
        IncreasePhase();
    }

    void IncreasePhase()
    {
        int newState = (int)state + 1;

        if (newState >= 3)
        {
            newState = 0;
        }

        state = (GameState)newState;
    }

    public void RotateClockwise()
    {
        boardRotation += 90f;
        Piece[,] rotatedPieces = new Piece[boardSize, boardSize];

        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                rotatedPieces[x, boardSize - 1 - y] = pieces[y, x];
            }
        }

        pieces = rotatedPieces;
    }

    public void RotateCounterclockwise()
    {
        boardRotation -= 90f;
        Piece[,] rotatedPieces = new Piece[boardSize, boardSize];

        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                rotatedPieces[boardSize - 1 - x, y] = pieces[y, x];
            }
        }

        pieces = rotatedPieces;
    }



    //Matching Logic
    bool CheckMatches(bool pop)
    {
        Piece[,] piecesGridDuplicate = (Piece[,])pieces.Clone();

        groups = new List<List<Piece>>();

        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                Piece currentPiece = piecesGridDuplicate[y, x];

                if (currentPiece == null)
                {
                    continue;
                }

                List<Piece> group = new List<Piece> { currentPiece };
                SearchForGroup(currentPiece.color, x, y, group, piecesGridDuplicate);

                if (group.Count >= 4)
                {
                    groups.Add(group);
                }
            }
        }

        if (pop)
        {
            if (groups.Count > 0)
            {
                combo += 1;
                Invoke("ProcessMatches", 0.2f);
            }
            else
            {
                combo = 1;
                IncreasePhase();
            }
        }

        Invoke("UpdateComboDisplay", 0.2f);
        return groups.Count > 0;
    }

    void UpdateComboDisplay()
    {
        comboText.text = combo != 1 ? "X " + combo.ToString() : "";
    }

    void ProcessMatches()
    {
        int totalPops = 0;
        for (int i = 0; i < groups.Count; i++)
        {
            totalPops += groups[i].Count;

            for (int j = 0; j < groups[i].Count; j++)
            {
                Instantiate(particle, groups[i][j].transform.position, Quaternion.identity).GetComponent<PopEffect>().ChangeColor(groups[i][j].color);
                Vector2 gridPos = GetPosOnGrid(groups[i][j]);
                Destroy(groups[i][j].gameObject);
                pieces[(int)gridPos.y, (int)gridPos.x] = null;
            }
        }

        score += totalPops * 5 * totalPops * combo;

        Invoke("FillBoard", 0.2f);
        Invoke("IncreasePhase", 0.2f);
    }

    Vector2 GetPosOnGrid(Piece piece)
    {
        for (int y = 0; y < boardSize; y++)
        {
            for (int x = 0; x < boardSize; x++)
            {
                if (pieces[y, x] == piece)
                {
                    return new Vector2(x, y);
                }
            }
        }

        return new Vector2(-1f, -1f);
    }

    private void SearchForGroup(PieceColor color, int x, int y, List<Piece> group, Piece[,] piecesGridDuplicate)
    {
        if (x < 0 || x >= boardSize || y < 0 || y >= boardSize)
        {
            return;
        }

        Piece piece = piecesGridDuplicate[y, x];

        if (piece == null || piece.color != color)
        {
            return;
        }

        if (!group.Contains(piece))
        {
            group.Add(piece);
        }
        piecesGridDuplicate[y, x] = null;

        SearchForGroup(color, x + 1, y, group, piecesGridDuplicate);
        SearchForGroup(color, x - 1, y, group, piecesGridDuplicate);
        SearchForGroup(color, x, y + 1, group, piecesGridDuplicate);
        SearchForGroup(color, x, y - 1, group, piecesGridDuplicate);
    }

    GameObject GetRandomNonMatchingPiece(int x, int y)
    {
        List<GameObject> nonMatchingPieces = new List<GameObject>();
        foreach (GameObject prefab in piecePrefabs)
        {
            GameObject tempPiece = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            tempPiece.GetComponent<Piece>().color = (PieceColor)UnityEngine.Random.Range(0, Enum.GetNames(typeof(PieceColor)).Length);
            pieces[y, x] = tempPiece.GetComponent<Piece>();

            if (!CheckMatches(false))
            {
                nonMatchingPieces.Add(prefab);
            }

            Destroy(tempPiece);
        }

        if (nonMatchingPieces.Count > 0)
        {
            return nonMatchingPieces[UnityEngine.Random.Range(0, nonMatchingPieces.Count)];
        }
        else
        {
            return piecePrefabs[UnityEngine.Random.Range(0, piecePrefabs.Count)];
        }
    }
}
