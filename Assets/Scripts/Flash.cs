using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    private float alpha;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        alpha = Mathf.Lerp(alpha, 0f, Time.deltaTime * 5f);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
    }

    public void FlashEffect()
    {
        alpha = 1f;
    }
}
