using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject _monitor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_monitor.GetComponent<Monitor>().isRunning)
            {
                _monitor.GetComponent<Monitor>().ScreenOn();
            }
        }
    }
}
