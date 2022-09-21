using System.Collections;
using UnityEngine;

public class CanonBehaviour : MonoBehaviour
{   
    [SerializeField] private Transform fireTransform;
    [SerializeField] private float angleLimit = 40f;

    private const int BALL_COUNT = 40;
    private const float BALL_INTERVAL = 0.1F;
    private const float MAX_POWER = 20f;

    private float _screenWidthRotation; 
    private float _inputScreenRatio;
    private bool _isFire;
    private bool _IsCoroutineStart;

    private void Awake()
    {
        _screenWidthRotation = Screen.width * 0.8f;
    }
    
    void Update()
    {
        if (LevelManager.Instance.IsUltActive)
        {
            LevelManager.Instance.IsPlaying = true;

            if (Input.GetMouseButton(0))
            {
                SetCanonAngle();
            }
           

            if (!_IsCoroutineStart)
            {
                _IsCoroutineStart = true;
                StartCoroutine(FireBallsCoroutine());
            }
            

        }
    }

    private void SetCanonAngle()
    {
        _inputScreenRatio = Input.mousePosition.x / _screenWidthRotation - 0.5f;
        float angle = 2 * angleLimit * _inputScreenRatio;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    private IEnumerator FireBallsCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(BALL_INTERVAL);
        for (int i = 0; i < BALL_COUNT; i++)
        {
            FireBall();
            yield return wait;

        }

        yield return wait;
        FinishCanon();
    }

    private void FireBall()
    {
        GameObject ball = ObjectManager.Instance.GetBallObjectFromPool();
        BallBehavior ballBehavior = ball.GetComponent<BallBehavior>();

        ballBehavior.MakeUltBall();
        ballBehavior.EnableCollisonWithInGameObject();

        ball.GetComponent<DragAndShootBehaviour>().enabled = false;

        Rigidbody rigidbody = ball.GetComponent<Rigidbody>();
        rigidbody.useGravity = true;

        ball.transform.position = fireTransform.position; 
        Vector3 direction = transform.forward * MAX_POWER;
        rigidbody.AddForce(direction, ForceMode.Impulse);
    }

    private void FinishCanon()
    {
        LevelManager.Instance.IsUltActive = false;
        LevelManager.Instance.GameCycle();

        if (!LevelManager.Instance.IsWin)
        {
            UIManager.Instance.OpenBallHub();
        }
        UIManager.Instance.UltButtonUnAvailable();
        UIManager.Instance.CloseCanon();
        _IsCoroutineStart = false;
    }

    public bool IsFire
    {
        get { return _isFire; }
        set { _isFire = value; }
    }
}
