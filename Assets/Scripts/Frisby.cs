using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Frisby : MonoBehaviour {

    [Serializable]
    public class Range
    {
        public int minimum;
        public int maximum;

        public Range(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }
    public GameObject FrisbyTile;
    public GameObject PlayerTile;
    public GameObject TargetTile;

    public Range LongPassRange;
    public Range ShortPassRange;
    public Range GoalPassRange = new Range(101, 104);

    public float Movetime = 0.4f;

    public bool LongPass;
    public bool ShortPass;
    public bool GoalPass;

    float inverseMovetime;
    int boardRows;

    GameObject playerPosY;
    Rigidbody2D rd2D;
    List<Vector3> TenYardAresList = new List<Vector3>();

    void Start ()
    {
        FrisbyTile = GameObject.FindGameObjectWithTag("Frisby");
        PlayerTile = GameObject.FindGameObjectWithTag("Player");
        TargetTile = GameObject.FindGameObjectWithTag("Target");

        boardRows = GameManager.instance.boardScript.rows;

        rd2D = GetComponent<Rigidbody2D>();
        inverseMovetime = 1f / Movetime;
    }

    public void GetFrisbyPass()
    {
        int PosY;
        float PosX = Random.Range(0,9);

        UpdatePassRange();

        if (LongPass)
        {
            PosY = Random.Range(LongPassRange.minimum, LongPassRange.maximum);

            if (LongPassRange.maximum > boardRows - 1)
            {
                PosY = Random.Range(GoalPassRange.minimum, GoalPassRange.maximum);
            }
            StartCoroutine(SmoothMovement(new Vector3(PosX, PosY, 0f)));
            TargetTile.transform.position = new Vector3(PosX, PosY, 0f);
        }

        else if (ShortPass)
        {
            PosY = Random.Range(ShortPassRange.minimum, ShortPassRange.maximum);

            if (ShortPassRange.maximum > boardRows - 1)
            {
                PosY = Random.Range(GoalPassRange.minimum, GoalPassRange.maximum);
            }
            StartCoroutine(SmoothMovement(new Vector3(PosX, PosY, 0f)));
            TargetTile.transform.position = new Vector3(PosX, PosY, 0f);
        }

        else if (GoalPass)
        {
            PosY = Random.Range(GoalPassRange.minimum, GoalPassRange.maximum);
            StartCoroutine(SmoothMovement(new Vector3(PosX, PosY, 0f)));
            TargetTile.transform.position = new Vector3(PosX, PosY, 0f);
        }
    }

    List<Vector3> GetTenYardAreaList(int PosY)
    {
        TenYardAresList.Clear();

        for (int x = 0; x < 9; x++)
            for (int y = PosY; y < 9; y++)
            {
                TenYardAresList.Add(new Vector3(x, y, 0f));
            }
        return TenYardAresList;
    }

    void UpdatePassRange()
    {
        int playerPosY = (int)PlayerTile.transform.position.y;

        ShortPassRange = new Range(playerPosY + 10, playerPosY + 20);
        LongPassRange = new Range(playerPosY + 30, playerPosY + 40);
        GoalPassRange = new Range (101, 104);

    }

    IEnumerator SmoothMovement(Vector3 end)
    {
        float sqRemaingDistance = (transform.position - end).sqrMagnitude;

        while (sqRemaingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rd2D.transform.position, end, inverseMovetime * Time.deltaTime);
            rd2D.MovePosition(newPosition);

            sqRemaingDistance = (rd2D.transform.position - end).sqrMagnitude;

            yield return null;
        }
    }

}
