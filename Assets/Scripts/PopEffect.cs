using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopEffect : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;

    private float verticalSpeed;

    private float horizontalSpeed;

    [SerializeField] private float speed;

    [SerializeField] private float gravity;

    // Start is called before the first frame update
    void Start()
    {
        verticalSpeed = speed;
        horizontalSpeed = Random.Range(-40f, 40f);
        transform.rotation = Quaternion.Euler(0f, 0f, -horizontalSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        verticalSpeed -= gravity * Time.deltaTime;

        transform.position += (Vector3)new Vector2(horizontalSpeed, verticalSpeed) * Time.deltaTime;
    }

    public void ChangeColor(PieceColor color)
    {
        GetComponent<SpriteRenderer>().sprite = sprites[(int)color];
    }
}
