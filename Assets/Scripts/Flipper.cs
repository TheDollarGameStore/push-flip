using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    [SerializeField] private Sprite push;
    [SerializeField] private Sprite flip;

    private SpriteRenderer sr;

    private float dir = 1;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(dir, transform.localScale.y, transform.localScale.z), 25f * Time.deltaTime);
    }

    public void Flip()
    {
        dir *= -1f;
        sr.sprite = dir == 1f ? push : flip;
    }
}
