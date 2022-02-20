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
                yield return moveToX(transform, _targetPosition.position, _speed);
                _transformToB = false;
            }
            else
            {
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
        float journeyLength;
        startTime = Time.time; //Aktualny czas

        Vector3 startPos = targetObject.position; //Aktualna pozycja obiektu, który ma zostaæ przeniesiony 

        journeyLength = Vector3.Distance(startPos, toPosition); // Obliczenie d³ugoœci podró¿y 


        if (startPos == toPosition)
            yield break;

        while (true)
        {
            float distCovered = (Time.time - startTime) * speed; //Odleg³oœæ przebyta = time * speed

            float fracJourney = distCovered / journeyLength; //Czêœæ ukoñczonej podró¿y = aktualna odleg³oœæ podzielona przez ca³kowit¹ odleg³oœæ

            targetObject.position = Vector3.Lerp(startPos, toPosition, fracJourney); //Ustawienie naszej pozycjê jako u³amek odleg³oœci miêdzy znacznikami

            if (fracJourney >= 1) //Wyjœcie jeœli lerp wyniesie 1
                yield break;

            yield return null;
        }
    }
}
