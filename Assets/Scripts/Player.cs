using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int pos;

    [SerializeField] private GameObject pushEffect;
    [SerializeField] private AudioClip move;
    [SerializeField] private AudioClip push;
    [SerializeField] private AudioClip flip;

    void Start()
    {
        pos = Mathf.RoundToInt(GameManager.instance.boardSize / 2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, new Vector2((pos + GameManager.instance.offset) * GameManager.instance.tileSize, transform.position.y), 10f * Time.deltaTime);

        if (GameManager.instance.gameOver)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                GameManager.instance.RestartGame();
            }
        }

        if (GameManager.instance.gameOver)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (GameManager.instance.state == GameState.MOVE)
            {
                if (pos > 0)
                {
                    pos--;
                    SoundManager.instance.PlayRandom(move);
                }
            }
            else if (GameManager.instance.state == GameState.FLIP)
            {
                SoundManager.instance.PlayRandom(flip);
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
                    SoundManager.instance.PlayRandom(move);
                }
            }
            else if (GameManager.instance.state == GameState.FLIP)
            {
                GameManager.instance.Flip(1);
                SoundManager.instance.PlayRandom(flip);
            }
        }

        if (GameManager.instance.state == GameState.MOVE)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                GameManager.instance.Push(pos);
                SoundManager.instance.PlayRandom(push);
                Instantiate(pushEffect, transform.position + (Vector3)(Vector2.up * 20f), Quaternion.identity);
            }
        }
    }
}
