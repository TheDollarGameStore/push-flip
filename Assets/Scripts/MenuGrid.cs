using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGrid : MonoBehaviour
{
    private bool activated;
    private Wobble wobbler;

    [SerializeField] private AudioClip startSound;

    // Start is called before the first frame update
    void Start()
    {
        wobbler = GetComponent<Wobble>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(new Vector3(0f, 0f, activated ? -90f : 0f)), 10f * Time.deltaTime);

        if (activated)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            activated = true;
            wobbler.DoTheWobble();
            Transitioner.Instance.TransitionToScene(1);
            SoundManager.instance.PlayNormal(startSound);
        }
    }
}
