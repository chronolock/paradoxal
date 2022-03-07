using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena
{
    private Dictionary<int, Dictionary<int, float>> arenaTiles = new Dictionary<int, Dictionary<int, float>>();
    private Vector2Int lastPosHadSet;

    public Arena()
    {

    }

    public float GetValue(Vector3Int pos){
        return GetValue(pos.x, pos.y);
    }

    public float GetValue(Vector2Int pos){
        return GetValue(pos.x, pos.y);
    }

    public float GetValue(int x, int y){
        return arenaTiles[x][y];
    }

    public Vector3Int GetPosByValue(float value){
        Vector3Int result = new Vector3Int(int.MinValue, int.MinValue, 0);
        
        foreach(KeyValuePair<int, Dictionary<int, float>> i in arenaTiles){
            foreach(KeyValuePair<int, float> j in arenaTiles[i.Key]){
                if(j.Value == value){
                    result = new Vector3Int(i.Key, j.Key, 0);
                }
            }
        }

        return result;
    }

    public void SetValue(int x, int y, float value){
        if(!arenaTiles.ContainsKey(x)){
            arenaTiles.Add(x, new Dictionary<int, float>());
        }
        if(!arenaTiles[x].ContainsKey(y)){
            arenaTiles[x].Add(y, value);
        } else {
            arenaTiles[x][y] = value;
        }
        
        lastPosHadSet = new Vector2Int(x, y);
    }

    public void SetValue(Vector2Int pos, float value){
        SetValue(pos.x, pos.y, value);
    }

    public void SetValue(Vector3Int pos, float value){
        SetValue(pos.x, pos.y, value);
    }

    public Vector2Int GetLastPosHadSet(){
        return lastPosHadSet;
    }

    public void DebugArena(){
        foreach(KeyValuePair<int, Dictionary<int, float>> i in arenaTiles){
            foreach(KeyValuePair<int, float> j in arenaTiles[i.Key]){
                if(j.Value != 0){
                    Debug.Log("X: "+i.Key+" | Y:"+j.Key+"="+j.Value);
                }
            }
        }
    }

}
