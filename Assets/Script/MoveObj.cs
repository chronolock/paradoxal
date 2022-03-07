using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObj : MonoBehaviour
{
    private Vector3 oldPos;
    private Vector3 nextPos;

    private bool animate = false;

    private float countTime = 0;

    private float maxTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(animate){
            countTime += Time.deltaTime;
            transform.position = Vector3.Lerp(oldPos, nextPos, countTime/maxTime);

            if(countTime > maxTime){
                animate = false;
                countTime = 0;
            }
        }
    }

    public void moveTo(Vector3 newPos){
        oldPos = gameObject.transform.position;
        nextPos = new Vector3(newPos.x, newPos.y, -10);
        animate = true;
    }
}
