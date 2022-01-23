using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    [SerializeField]
    Transform _startPosition;
    [SerializeField]
    Transform _targetPosition;

    [SerializeField]
    float _speed = 4.0f;

    [SerializeField]
    bool _onStart;
    [SerializeField]
    bool _pingPong;
    bool _transformToB;
    bool _isDone = false;
    bool _open = false;

    private void Start()
    {
        if (_onStart)
        {
            OperateObject();
        }
    }

    public void OperateObject()
    {
        StopAllCoroutines();
        if (_pingPong)
        {
            if (!_isDone)
            {
                _isDone = true;
                StartCoroutine(PingPong());
            }
            else
            {
                _isDone = false;
            }
        }
        else
        {
            StartCoroutine(Single());
        }
    }

    IEnumerator PingPong()
    {
        while (true)
        {
            if (_transformToB)
            {
                //Move to B and wait for the Move to finish
                yield return moveToX(transform, _targetPosition.position, _speed);
                _transformToB = false;
            }
            else
            {
                //Move to A and wait for the Move to finish
                yield return moveToX(transform, _startPosition.position, _speed);
                _transformToB = true;
            }
        }
    }

    IEnumerator Single()
    {
        if (_open)
        {
            _open = false;
            yield return moveToX(transform, _startPosition.position, _speed);
        }
        else
        {
            _open = true;
            yield return moveToX(transform, _targetPosition.position, _speed);
        }
    }

    IEnumerator moveToX(Transform targetObject, Vector3 toPosition, float speed)
    {
        float startTime;
        // Total distance between the markers.
        float journeyLength;
        startTime = Time.time;

        //Get the current position of the object to be moved
        Vector3 startPos = targetObject.position;
        // Calculate the journey length.
        journeyLength = Vector3.Distance(startPos, toPosition);


        if (startPos == toPosition)
            yield break;

        while (true)
        {
            // Distance moved = time * speed.
            float distCovered = (Time.time - startTime) * speed;

            // Fraction of journey completed = current distance divided by total distance.
            float fracJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            targetObject.position = Vector3.Lerp(startPos, toPosition, fracJourney);

            //Exit if lerp time reaches 1
            if (fracJourney >= 1)
                yield break;

            yield return null;
        }
    }
}
