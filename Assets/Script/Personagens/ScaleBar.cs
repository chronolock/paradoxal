using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBar : MonoBehaviour
{
    public Transform refPoint;

    private float initW;

    private SpriteRenderer spRender;
    
    void Awake() {
        spRender = GetComponent<SpriteRenderer>();
        initW = spRender.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float newPosX = refPoint.position.x + ((initW /2) * transform.localScale.x);
        transform.position = new Vector3(newPosX, transform.position.y, 0);
    }
}
