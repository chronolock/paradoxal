using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{

    public Sprite n0;
    public Sprite n1;
    public Sprite n2;
    public Sprite n3;
    public Sprite n4;
    public Sprite n5;
    
    private static Global instance;
    void Awake()
    {
        instance = this;
    }

    void Update() {
        
    }

    public static Sprite N0{
        get {
            return instance.n0;
        }
    }

    public static Sprite N1{
        get {
            return instance.n1;
        }
    }

    public static Sprite N2{
        get {
            return instance.n2;
        }
    }

    public static Sprite N3{
        get {
            return instance.n3;
        }
    }

    public static Sprite N4{
        get {
            return instance.n4;
        }
    }

    public static Sprite N5{
        get {
            return instance.n5;
        }
    }
}
