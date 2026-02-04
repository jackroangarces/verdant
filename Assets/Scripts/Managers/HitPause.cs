using UnityEngine;
using System.Collections;

public class HitPause : MonoBehaviour
{
    public static void Freeze(float duration)
    {
        Time.timeScale = 0f;
        Instance.StartCoroutine(ResumeAfter(duration));
    }

    static HitPause Instance;

    void Awake()
    {
        Instance = this;
    }

    static IEnumerator ResumeAfter(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }
}
