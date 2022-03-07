using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderInLayer : MonoBehaviour
{

    private SpriteRenderer sRender;

    // Start is called before the first frame update
    void Start()
    {
        sRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        sRender.sortingOrder =  999 - (int) Mathf.Round(transform.position.y);
    }
}
