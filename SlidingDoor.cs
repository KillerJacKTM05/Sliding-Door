using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SafeZone;

public class SlidingDoor : MonoBehaviour
{
    [SerializeField]private bool isOpen = false;
    [SerializeField] private Transform targetDoor;
    private bool isActive = false;
    private Coroutine routine;
    private Coroutine daily;
    /*public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Npc"))
        {
            isOpen = true;
            OpenCloseDoor(isOpen);
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Npc"))
        {
            isOpen = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Npc"))
        {
            isOpen = false;
            OpenCloseDoor(isOpen);
        }
    }
    */
    private void Start()
    {
        Invoke(nameof(StartFunc), 0.5f);
    }
    private void StartFunc()
    {
        daily = StartCoroutine(DailyRoutine());
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 2);
    }
    private void OpenCloseDoor(bool state)
    {
        routine = StartCoroutine(OpenCloseRoutine(state));
    }
    private IEnumerator OpenCloseRoutine(bool state)
    {
        if (!isActive)
        {
            isOpen = state;
            isActive = true;
            float fl = 0;
            Vector3 currentTransform = targetDoor.transform.position;
            Vector3 newTransform = new Vector3(0, 0, 0);
            if (state)
            {
                newTransform = targetDoor.transform.position - transform.forward;
            }
            else
            {
                newTransform = targetDoor.transform.position + transform.forward;
            }
            while (fl < 1.0f)
            {
                targetDoor.transform.position = Vector3.Lerp(currentTransform, newTransform, fl);
                yield return new WaitForSeconds(0.02f);
                fl += 0.05f;
            }
            yield return new WaitForSeconds(0.5f);
            isActive = false;
        }      
    }
    private IEnumerator DailyRoutine()
    {
        while (true)
        {
            if (GameManager.Instance.GetGameStarted())
            {
                bool hitted = false;
                Collider[] cols = Physics.OverlapSphere(transform.position, 2f, LayerMask.GetMask("SelfMoving"));
                Collider[] player = Physics.OverlapSphere(transform.position, 2f, LayerMask.GetMask("Default"));
                foreach (Collider col in cols)
                {
                    if(col.gameObject.CompareTag("Npc"))
                     {
                        hitted = true;
                     }
                }
                foreach(Collider co in player)
                {
                    if (co.gameObject.CompareTag("Player"))
                    {
                        hitted = true;
                    }
                }
                if(hitted && !isOpen)
                {
                    OpenCloseDoor(true);
                }
                else if(!hitted && isOpen)
                {
                    OpenCloseDoor(false);
                }
                else
                {

                }
                yield return new WaitForSeconds((60f / GameManager.Instance.GetGameSettings().realWorldTimeForEachGameHour));
            }
            else if (GameManager.Instance.GetIsGameFinished())
            {
                StopCoroutine(daily);
            }
            else
            {
                yield return new WaitForSeconds((60f / GameManager.Instance.GetGameSettings().realWorldTimeForEachGameHour));
            }
        }
    }
}
