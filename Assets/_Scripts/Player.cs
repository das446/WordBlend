using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int points;
    [SerializeField] int[] pointVals;
    
    void Start(){
        SubmitMode.Submit+=GetPoints;
    }

    private void GetPoints(Word word)
    {
        throw new NotImplementedException();
    }
}
