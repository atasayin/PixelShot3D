using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DragAndShootBehaviour : MonoBehaviour
{

    [Header("Editor Controls")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private TrajectoryProjectionBehaviour trajectoryProjection;
    [SerializeField] private BallBehavior ballBehavior;

    [Header("Movement Controls")]
    [SerializeField] public float _normalizer = 20f;
    [SerializeField] public float _minAmplitude = 5;
    [SerializeField] public float _maxAmplitude = 20f;
    [SerializeField] [Range(0, 90f)] private float _angleLimit = 75f;
    



    private bool _isThrew, _isDragEnough;
    private Vector3 _startPosition, _endPosition, _normalized;
    private Vector3 _ballStartPosition;
    private Vector3 _ballPressedDragPosition;
    
    private GameObject _physicalBall;
    private Rigidbody _physicalBallRigidbody;

    public delegate void BallThrowAction();
    public static event BallThrowAction OnBallThrow;

    private const float STRETCHING_COEF = 0.035f;

    private void Awake()
    {
        _ballPressedDragPosition = new Vector3(0, 0.2f, -0.1f);   
    }

    private void OnEnable()
    {
        DisableCollisonWithInGameObject();
        _ballStartPosition = transform.position;
        
    }

    private void OnDisable()
    {   
        _isThrew = false;
    }
 
    private void Update()
    { 
        if (!_isThrew)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnDragStart();
            }
            else if (Input.GetMouseButton(0))
            {
                OnDraging();

            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnDragEnd();
            }
        }
        
    }

    private void EnableCollisonWithInGameObject() => ballBehavior.EnableCollisonWithInGameObject();

    private void DisableCollisonWithInGameObject() => ballBehavior.DisableCollisonWithInGameObject();


    private IEnumerator DieAfterSeconds(int waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        ballBehavior.RunDeathEffectBehaviours();


        gameObject.SetActive(false);
    }

    private void OnDragStart()
    { 
        _startPosition = Utility.Vector2Dto3D(Input.mousePosition);
        //SetupTrajectoryLineBall();
        trailRenderer.enabled = false;
        transform.position = _ballPressedDragPosition;
    }

    private void OnDraging() {

        _endPosition = Utility.Vector2Dto3D(Input.mousePosition);
        _normalized = (_startPosition - _endPosition) / _normalizer;

        float angle;

        if (_normalized.magnitude <= _minAmplitude)
        {
            _isDragEnough = false;
        }
        else
        {
            if (_normalized.magnitude >= _maxAmplitude)
            {
                _normalized = _normalized.normalized * _maxAmplitude;
            }

            angle = Vector3.Angle(Utility.VectorNormalZ , _normalized);
            if (angle > _angleLimit)
            {
                return;
            }
           
            
          // PhysicalSceneTrajectoryLine(_normalized);

            _isDragEnough = true;

            StretchBallAccordingToDrawPower();
        }


        trajectoryProjection.DrawProjection(_normalized, _maxAmplitude);

        
    }
    
    private void StretchBallAccordingToDrawPower()
    {
        Vector3 inverseDirection = -_normalized.normalized;
        float power = _normalized.magnitude;

        transform.position = _ballStartPosition + inverseDirection * power * STRETCHING_COEF;
    }

    private void OnDragEnd()
    {
        trajectoryProjection.ResetLineRenderer();
        //ResetPhysicalBallTransform();
        transform.position = _ballStartPosition;
        if (_isDragEnough)
        {
            trailRenderer.enabled = true;
            EnableCollisonWithInGameObject();
            _isThrew = true;
            _rigidbody.useGravity = true;
            _rigidbody.AddForce(_normalized, ForceMode.Impulse);
            OnBallThrow?.Invoke();

            StartCoroutine(DieAfterSeconds(3));

        }
        
    }

    public bool IsThrew
    {
        get { return _isThrew; }
        set { _isThrew = value; }
    }


    public Vector3 DragVector
    {
        get { return _normalized; }
        set { _normalized = value; }
    }

    public bool IsDragEnough
    {
        get { return _isDragEnough; }
        set { _isDragEnough = value; }
    }


    public float MaxDragMag
    {
        get { return _maxAmplitude; }
        set { _maxAmplitude = value; }
    }

}
