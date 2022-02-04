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
        // Ca�kowita odleg�o�� mi�dzy znacznikami
        float journeyLength;
        startTime = Time.time;

        // Aktualna pozycja obiektu, kt�ry ma zosta� przeniesiony 
        Vector3 startPos = targetObject.position;
        // Obliczenie d�ugo�ci podr�y 
        journeyLength = Vector3.Distance(startPos, toPosition);


        if (startPos == toPosition)
            yield break;

        while (true)
        {
            // Odleg�o�� przebyta = time * speed
            float distCovered = (Time.time - startTime) * speed;

            // Cz�� uko�czonej podr�y = aktualna odleg�o�� podzielona przez ca�kowit� odleg�o��
            float fracJourney = distCovered / journeyLength;

            // Ustawienie naszej pozycj� jako u�amek odleg�o�ci mi�dzy znacznikami
            targetObject.position = Vector3.Lerp(startPos, toPosition, fracJourney);

            // Wyj�cie je�li lerp wyniesie 1
            if (fracJourney >= 1)
                yield break;

            yield return null;
        }
    }
}
