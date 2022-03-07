using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusChar : MonoBehaviour
{

    //public GameObject arrowCurrentChar;

    public GameObject barraVida;

    public GameObject barraEnergia;

    public SpriteRenderer iconeEnergia;

    //public SpriteRenderer fundoEnergia;

    public SpriteRenderer fundo;

    //public SpriteRenderer iconeVida;

    public void setOrder(int pos){
        iconeEnergia.sortingOrder = pos + 2;
        //iconeVida.sortingOrder = pos + 2;
        fundo.sortingOrder = pos;

        //arrowCurrentChar.GetComponent<SpriteRenderer>().sortingOrder = pos + 1;
        barraVida.GetComponent<SpriteRenderer>().sortingOrder = pos + 2;
        barraEnergia.GetComponent<SpriteRenderer>().sortingOrder = pos + 2;
    }


    public void enableStatusBar(bool value){
        fundo.enabled = value;
        iconeEnergia.enabled = value;
        barraEnergia.SetActive(value);
        barraVida.SetActive(value);

    }
}
