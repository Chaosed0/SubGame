
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Breach : MonoBehaviour {
    public Level level;
    public Tile tile;
    public float workLeft = 5.0f;


    public float timeUntilRoomPermanentlyDisabled;//divide this by 3 and use those values to determine when it shows the different animations
    public bool permanent;//if this is true, you can't fix this room anymore, and you can't use it
    public Sprite[] floodSprites;

    public float oneThirdTimeLeft;
    public float twoThirdsTimeLeft;

    public UnityEvent onBreachFixed = new UnityEvent();

    public UnityEvent onBreachCompletelyFloodsRoom = new UnityEvent();

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = floodSprites[0];
        oneThirdTimeLeft = timeUntilRoomPermanentlyDisabled / 3;
        twoThirdsTimeLeft = (timeUntilRoomPermanentlyDisabled / 3) * 2;

    }

    void Update()
    {
        if (timeUntilRoomPermanentlyDisabled > 0)
        {
            timeUntilRoomPermanentlyDisabled -= Time.deltaTime;

            if (timeUntilRoomPermanentlyDisabled < twoThirdsTimeLeft)
            {
                GetComponent<SpriteRenderer>().sprite = floodSprites[1];
            }

            if (timeUntilRoomPermanentlyDisabled < oneThirdTimeLeft)
            {
                GetComponent<SpriteRenderer>().sprite = floodSprites[2];
            }

        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = floodSprites[3];

            //room permanently disabled
            permanent = true;


        }
    }

    public void doWorkOnBreach(float repairMulitplier) {
        workLeft -= Time.deltaTime * repairMulitplier;

        if (workLeft <= 0.0f) {
            level.SetTraversable(tile.transform.position, true);
            if (onBreachFixed != null) {
                onBreachFixed.Invoke();
            }
        }
    }
}
