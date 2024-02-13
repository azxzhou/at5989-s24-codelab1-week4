using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WhackADot : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("whack");

        transform.position = new Vector2(
            Random.Range(-7f, 7f),
            Random.Range(-4f, 4f)
        );

        GameManager.instance.Score++; //calls instance in gamemanager

    }
}
