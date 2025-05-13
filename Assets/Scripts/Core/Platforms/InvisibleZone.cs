using System.Collections;
using UnityEngine;

public class InvisibleZone : MonoBehaviour
{
    [SerializeField] float disableDuration;
    [SerializeField] float enableDuration;

    private void OnEnable()
    {
        StartCoroutine(StartBehavior());
    }
    private IEnumerator StartBehavior()
    {
        yield return new WaitForSeconds(enableDuration);
        gameObject.SetActive(false);
        Invoke(nameof(ActiveSelf), disableDuration);        
    }
    void ActiveSelf()
    {
        gameObject.SetActive(true);
    }


}
