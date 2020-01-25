using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWithMouse : MonoBehaviour
{
    public float length = 5;
    private bool isClick = false;
    private Vector3 nowPos;
    private Vector3 oldPos;

    // 鼠标抬起
    private void OnMouseUp()
    {
        isClick = false;
    }

    // 鼠标按下
    private void OnMouseDown()
    {
        isClick = true;
    }

    private void Update()
    {
        nowPos = Input.mousePosition;
        // 鼠标按下不松手
        if (isClick)
        {
            Vector3 offset = nowPos - oldPos;
            if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y) && Mathf.Abs(offset.x) > length)
            {
                transform.Rotate(Vector3.up, -offset.x);
            }
        }
        oldPos = Input.mousePosition;
    }
}
