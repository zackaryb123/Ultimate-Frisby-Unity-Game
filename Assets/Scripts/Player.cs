using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject
{
    public int PlayerSpeed = 10;
    public int PointsPerCoin = 10;
    public float restartLevelDelay = 1f;
    public Text CoinText;
    public Text YardText;

    private int playerSpeed;
    private float yards;
    private int coins;

    //private Vector2 touchOrigin = -Vector2.one;

    protected override void Start ()
    {
        playerSpeed = 0;

        coins = GameManager.instance.PlayerCoinPoints;
        yards = GameManager.instance.PlayerYardDist;

        CoinText.text = "Coins\n" + coins;
        YardText.text = "Yards\n" + yards;

        base.Start();
	}

    private void OnDisable()
    {
        GameManager.instance.PlayerCoinPoints = coins;
        GameManager.instance.PlayerYardDist = yards;
    }

    void Update()
    {
        if (GameManager.instance.doingSetup) return;

        if (playerSpeed == PlayerSpeed)
        {
            int horizontal = 0;
            int vertical = 0;

#if UNITY_STANDALONE || UNITY_WEBPLAYER

            horizontal = (int)(Input.GetAxisRaw("Horizontal"));
            vertical = (int)(Input.GetAxisRaw("Vertical"));

            // Prevent diagnal movement 
            if (horizontal != 0)
                vertical = 0;
#else
            if (Input.touchCount > 0)
            {
                Touch myTouch = Input.touches[0];
                if(myTouch.phase == TouchPhase.Began)
                {
                    touchOrigin = myTouch.position;
                }
                else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
                {
                    Vector2 touchEnd = myTouch.position;
                    float x = touchEnd.x - touchOrigin.x;
                    float y = touchEnd.y - touchOrigin.y;
                    touchOrigin.x = -1;
                    if (Mathf.Abs(x) > Mathf.Abs(y))
                        horizontal = x > 0 ? 1 : -1;
                    else
                        vertical = y > 0 ? 1 : -1;
                }
            }
#endif

            if (horizontal != 0 || vertical != 0)
            {
                AttemptMove<Enemy>(horizontal, vertical);
                YardText.text = "Yards\n" + transform.position.y;
            }
            playerSpeed = 0;
        }
        else
        {
            playerSpeed++;
        }
    }


    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        base.AttemptMove<T>(xDir, yDir);
        RaycastHit2D hit;

        if (!Move(xDir, yDir, out hit))
        {
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            coins += PointsPerCoin;
            CoinText.text = "Coins\n" + coins;
            collision.gameObject.SetActive(false);
        }
    }
}
