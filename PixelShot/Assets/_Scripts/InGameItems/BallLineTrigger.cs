using UnityEngine;

public class BallLineTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<SimplePixel>()?.OnBallLineEffet();
    }
}