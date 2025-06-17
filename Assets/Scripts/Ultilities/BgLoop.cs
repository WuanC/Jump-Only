using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgLoop : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float heightSize;
    [SerializeField] float pixelPerUnit;

    Vector2 startPos;
    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
        if (Mathf.Abs(transform.position.y - startPos.y) >= heightSize / pixelPerUnit)
        {
            transform.position = startPos;
        }
    }
}

