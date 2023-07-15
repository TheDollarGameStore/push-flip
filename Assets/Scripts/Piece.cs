using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceColor
{
    GREEN,
    BLUE,
    RED,
    YELLOW
}

public class Piece : MonoBehaviour
{
    // Start is called before the first frame update
    public PieceColor color;
}
