using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public bool isSwipe, isThrow;
    public Vector3 direction;

    private Vector3 startingPosition, endingPosition;
    private float startingTime, endingTime;
    private float timePass;

    private float maxTime = 0.3f;
    private float minDistance = 0.3f;

    private void Update()
    {
        if (isSwipe)
        {
            direction = -(startingPosition - Input.mousePosition).normalized;
        }

        if (Input.GetMouseButtonDown(0))
        {
            startingTime = Time.time;
            startingPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            timePass += Time.deltaTime;

            if (timePass > maxTime && !isSwipe && !isThrow)
            {
                isSwipe = true;
                isThrow = false;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            timePass = 0;
            isSwipe = false;

            endingTime = Time.time;
            endingPosition = Input.mousePosition;

            float totalTime = endingTime - startingTime;
            float totalSwipeDistance = Vector3.Distance(endingPosition.normalized,startingPosition.normalized);
            if(totalTime < maxTime && totalSwipeDistance > minDistance)
            {
                /*Vector3 startPos = startingPosition;
                startPos.z = 20;
                startPos = Camera.main.ScreenToWorldPoint(startPos);

                Vector3 endPos = endingPosition;
                endPos.z = 20;
                endPos = Camera.main.ScreenToWorldPoint(endPos);*/

                direction = endingPosition - startingPosition;
                Debug.Log(totalTime + " - " + totalSwipeDistance);
                isThrow = true;
            }

        }
    }

    private Vector3 ScreenToWorldPoint(Vector3 position)
    {
        position.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(position);
    }
}
