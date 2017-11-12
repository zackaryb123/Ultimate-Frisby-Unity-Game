using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour {

    public float Movetime = 0.05f;
    public LayerMask BlockingLayer;
    
    BoxCollider2D boxCollider;
    Rigidbody2D rd2D;
    float inverseMovetime;
    Component hitComponent;

    protected virtual void Start ()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rd2D = GetComponent<Rigidbody2D>();
        inverseMovetime = 1f / Movetime;
	}

    protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, BlockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }

    protected IEnumerator SmoothMovement (Vector3 end)
    {
        float sqRemaingDistance = (transform.position - end).sqrMagnitude;

        while (sqRemaingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rd2D.position, end, inverseMovetime * Time.deltaTime);
            rd2D.MovePosition(newPosition);

            sqRemaingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }
    }

    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;
        Move(xDir, yDir, out hit);

        if (hit.transform == null)
            return;
    }
}
