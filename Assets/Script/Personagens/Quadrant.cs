using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quadrant
{
    public static Vector3Int GetVectorByQuadrant(int quadrant){

        switch(quadrant){
            case 0:
                return new Vector3Int(0, -1, 0);
            case 1: 
                return new Vector3Int(1, -1, 0);
            case 2: 
                return new Vector3Int(1, 0, 0);
            case 3: 
                return new Vector3Int(1, 1, 0);
            case 4: 
                return new Vector3Int(0, 1, 0);
            case 5: 
                return new Vector3Int(-1, 1, 0);
            case 6: 
                return new Vector3Int(-1, 0, 0);
            case 7: 
                return new Vector3Int(-1, -1, 0);
        }
        return Vector3Int.zero;
    }

    public static int GetQuadrantByAngle(Vector2 reference, Vector2 target){
        Vector2 vAngle = new Vector2(reference.x, reference.y) - target;
        float angle = Mathf.Atan2(vAngle.y, vAngle.x) * Mathf.Rad2Deg;

        if((angle > 0) && (angle <= 22.5)){
            return 6;
        } else {
            if((angle > 22.5) && (angle <= 67.5)){
                return 7;
            } else {
                if((angle > 67.5) && (angle <= 112.5)){
                    return 0;
                } else {
                    if((angle > 112.5) && (angle <= 157.5)){
                        return 1;
                    } else {
                        if((angle > 157.5) && (angle <= 180)){
                            return 2;
                        } else {
                            if((angle > -22.5) && (angle <= 0)){
                                return 6;
                            } else {
                                if((angle > -67.5) && (angle <= -22.5)){
                                    return 5;
                                } else {
                                    if((angle > -112.5) && (angle <= -67.5)){
                                        return 4;
                                    } else {
                                        if((angle > -157.5) && (angle <= -112.5)){
                                            return 3;
                                        } else {
                                            return 2;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

}
