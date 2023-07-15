using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float offset;
    [SerializeField] private Vector2 direction;

    private Vector2 startPos;

    private float x;

    // Start is called before the first frame update
    void Start()
    {
        x = offset;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        x += speed * Time.deltaTime;

        transform.position = startPos + (direction * Mathf.Sin(x));
    }
}
