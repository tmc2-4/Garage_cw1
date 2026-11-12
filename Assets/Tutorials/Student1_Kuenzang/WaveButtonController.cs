using UnityEngine;

public class WaveButtonController : MonoBehaviour
{
    public Animator armAnimator;

    void Start()
    {
        armAnimator.speed = 0f;
    }

    public void PlayWave()
    {
        armAnimator.speed = 1f;
        armAnimator.Play("MinifigureWave", 0, 0f);
    }
}