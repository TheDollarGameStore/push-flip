using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private float x;

    [SerializeField] private float angle;
    private float speed = Mathf.PI;
    // Start is called before the first frame update
    void Start()
    {
        x = Mathf.PI / 3f;
    }

    // Update is called once per frame
    void Update()
    {
        x += speed * Time.deltaTime;

        transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Sin(x) * angle);
    }
}
