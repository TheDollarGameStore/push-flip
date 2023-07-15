using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int pos;

    [SerializeField] private GameObject pushEffect;

    void Start()
    {
        pos = Mathf.RoundToInt(GameManager.instance.boardSize / 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (GameManager.instance.state == GameState.MOVE)
            {
                if (pos > 0)
                {
                    pos--;
                }
            }
            else if (GameManager.instance.state == GameState.FLIP)
            {
                GameManager.instance.Flip(-1);
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (GameManager.instance.state == GameState.MOVE)
            {
                if (pos < GameManager.instance.boardSize - 1)
                {
                    pos++;
                }
            }
            else if (GameManager.instance.state == GameState.FLIP)
            {
                GameManager.instance.Flip(1);
            }
        }

        transform.position = Vector2.Lerp(transform.position, new Vector2((pos + GameManager.instance.offset) * GameManager.instance.tileSize, transform.position.y), 10f * Time.deltaTime);

        if (GameManager.instance.state == GameState.MOVE)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                GameManager.instance.Push(pos);
                Instantiate(pushEffect, transform.position + (Vector3)(Vector2.up * 20f), Quaternion.identity);
            }
        }
    }
}
