using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryProjectionBehaviour : MonoBehaviour
{
    [Header("Editor Controls")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Rigidbody rigidbody;

    [Header("Line Controls")]
    [SerializeField] [Range(0, 2f)] private float _totalTimeDuration = 1f;
    [SerializeField] [Range(0.01f, 0.25f)] private float TimeBetweenPoints = 0.01f;
    [SerializeField] private Gradient hitColor;
    [SerializeField] public Gradient missColor;
    [SerializeField] [Range(0.02f, 0.2f)] private float _sphereCastRadius = 0.065f;
    [SerializeField] [Range(0.01f, 0.11f)] private float _maxLineRendererWidth = 0.11f;
    [SerializeField] [Range(0.01f, 0.5f)] private float castMagnitude = 0.11f;
    [SerializeField] private Vector3 ballStartPosition;


    private LayerMask ballCollisionMask;
    private int totalTimeSteps;
    private float _mass;

    private void Awake()
    {
        ballCollisionMask = Utility.CreateLayerMask("InGameObject");
        _mass = rigidbody.mass;
        LevelManager.OnLoadLevel += ChangeMissColorGradient;
    }

    private void OnEnable()
    {
        LevelManager.OnLoadLevel += ChangeMissColorGradient;
    }

    private void OnDisable()
    {
        LevelManager.OnLoadLevel -= ChangeMissColorGradient;
       
    }

    private void ChangeMissColorGradient()
    {
        Color ballHubColor = LevelManager.Instance.LoadedLevel.ballHubColor;
        Utility.CreateMainColorGradient(missColor, ballHubColor, 0f, 1f);
    }



    public void DrawProjection(Vector3 movementVector, float maxAmplitude)
    {
        totalTimeSteps = Mathf.CeilToInt(_totalTimeDuration / TimeBetweenPoints);
        ResetLineRenderer();

        lineRenderer.enabled = true;
        lineRenderer.colorGradient = missColor;
        SetLineRendererWidth(movementVector.magnitude / maxAmplitude);
        //_lineRenderer.positionCount = Mathf.CeilToInt(totalTimeSteps / TimeBetweenPoints) + 1;
        lineRenderer.positionCount = totalTimeSteps + 1;

        Vector3 startPosition = ballStartPosition;
        Vector3 lastPosition, point = startPosition;
        Vector3 velocity = movementVector / _mass;
        float timeAfterReflection = 0f;
        int linePos = 0;
        lineRenderer.SetPosition(linePos, startPosition);
        bool isLineDraw = true;

        int count = 1;


        while (true)
        {
            count++;
            if (point.z < ballStartPosition.z - float.Epsilon)
            {
                lineRenderer.positionCount = linePos - 1;
                break;
            }

            if (count > 1000)
            {

                break;
            }

            if (isLineDraw)
            {
                linePos++;
                if (linePos >= totalTimeSteps + 1)
                {
                    isLineDraw = false;
                }
            }

            lastPosition = point;
            point = startPosition + velocity * timeAfterReflection;
            Vector3 direction = (point - lastPosition).normalized;
            float castMagnitude = (point - lastPosition).magnitude;


            if (isLineDraw)
            {
                lineRenderer.SetPosition(linePos, point);
            }


            if (Physics.SphereCast(lastPosition, _sphereCastRadius, direction, out RaycastHit hit, castMagnitude, ballCollisionMask))
            {
                if (hit.collider.gameObject.CompareTag("TrajectoryPixel"))
                {
                    lineRenderer.colorGradient = hitColor;
                    lineRenderer.positionCount = linePos - 1;
                    isLineDraw = false;
                    break;
                }



                if (!hit.collider.gameObject.CompareTag("PassTrajectoryPixel"))
                {
                    Vector3 hitVectorNormal = hit.normal;
                    hitVectorNormal.y = 0;
                    Vector3 reflect = Vector3.Reflect(direction, hitVectorNormal.normalized);

                    velocity = reflect.normalized * velocity.magnitude;
                    velocity.x *= 0.5f;
                    velocity.y = 0;

                    //  if (velocity.z <= 2.5f) { velocity.z /= 2; }


                    timeAfterReflection = 0;


                    startPosition = hit.point;

                    // Debug.Log(linePos);
                    //   Debug.Break();

                    if (isLineDraw)
                    {
                        lineRenderer.SetPosition(linePos, point);
                    }
                }
            }

            timeAfterReflection += TimeBetweenPoints;
            velocity.z += Physics.gravity.z / 2f * TimeBetweenPoints;
        }

        lineRenderer.positionCount = linePos - 1;
    }


   

    private void SetLineRendererWidth(float currentMagnitudeRatio)
    {
        float lineRendererWidth = currentMagnitudeRatio * _maxLineRendererWidth;
        lineRenderer.startWidth = lineRendererWidth;

    }

    public void ResetLineRenderer()
    {
        lineRenderer.positionCount = 0;
    }

    private void DrawProjection1(Vector3 movementVector, float maxAmplitude)
    {
        totalTimeSteps = Mathf.CeilToInt(_totalTimeDuration / TimeBetweenPoints);
        ResetLineRenderer();

        lineRenderer.enabled = true;
        lineRenderer.colorGradient = missColor;
        SetLineRendererWidth(movementVector.magnitude / maxAmplitude);
        //_lineRenderer.positionCount = Mathf.CeilToInt(totalTimeSteps / TimeBetweenPoints) + 1;
        lineRenderer.positionCount = totalTimeSteps + 1;

        Vector3 startPosition = ballStartPosition;
        Vector3 lastPosition, point = startPosition;
        Vector3 velocity = movementVector / _mass; 
        float timeAfterReflection = 0f;
        int linePos = 0;
        lineRenderer.SetPosition(linePos, startPosition);
        bool isLineDraw = true;

        int count = 1;



        while (true)
        {
            count++;
            if (point.z < ballStartPosition.z - float.Epsilon)
            {
                lineRenderer.positionCount = linePos - 1;
                break;
            }

            if (count > 1000)
            {

                break;
            }

            if (isLineDraw)
            {
                linePos++;
                if (linePos >= totalTimeSteps + 1)
                {
                    isLineDraw = false;
                }
            }

            lastPosition = point;
            point = startPosition + velocity * timeAfterReflection;
            Vector3 direction = (point - lastPosition).normalized;
            float castMagnitude = (point - lastPosition).magnitude;


            if (isLineDraw)
            {
                lineRenderer.SetPosition(linePos, point);
            }


            if (Physics.SphereCast(startPosition, _sphereCastRadius, velocity, out RaycastHit hit, 1000, ballCollisionMask))
            {
                if (hit.collider.gameObject.CompareTag("TrajectoryPixel"))
                {
                    lineRenderer.colorGradient = hitColor;
                    lineRenderer.positionCount = linePos - 1;
                    isLineDraw = false;
                    break;
                }

                if (!hit.collider.gameObject.CompareTag("PassTrajectoryPixel"))
                {
                    Vector3 hitVectorNormal = hit.normal;
                    hitVectorNormal.y = 0;
                    Vector3 reflect = Vector3.Reflect(direction, hitVectorNormal.normalized);

                    velocity = reflect.normalized * velocity.magnitude;
                    velocity.x *= 0.5f;
                    velocity.y = 0;
                    velocity.z *= 0.5f;


                    timeAfterReflection = 0;
                    float distance = Vector3.Distance(startPosition, hit.point);
                    float time = distance / velocity.magnitude;
                    float zVelocity = time * Physics.gravity.z;
                    velocity.z += zVelocity;


                    startPosition = hit.point;

                    if (isLineDraw)
                    {
                        lineRenderer.SetPosition(linePos, point);
                    }
                }
            }

            timeAfterReflection += TimeBetweenPoints;
            //velocity.z += Physics.gravity.z / 2f * TimeBetweenPoints;            
        }

        lineRenderer.positionCount = linePos - 1;
    }

}
