using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    private static GameObject Instance;
    [SerializeField]
    private float initialMaxScrollFraction;
    [SerializeField]
    private float cameraEdgeThreshold;

    public delegate void CameraFunction();
    private Rigidbody2D rb;

    private static float DEFAULT_Z = -10;
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 canonicalCameraPosition;
    private float lastScreenHeight;
    private CameraEvent cameraEvent;
    private float maxScrollFraction;

    void Start()
    {
        transform.position = rb.position;
        canonicalCameraPosition = transform.position;
        maxScrollFraction = initialMaxScrollFraction;
    }

    void Awake()
    {
        UpdateCameraSize();
    }

    void OnLevelWasLoaded()
    {
        UpdateCameraSize();
    }

    void Update()
    {
        UpdateCameraSize();
    }

    void UpdateCameraSize()
    {
        lastScreenHeight = Screen.height;
        int scalingFactor;
        if (Screen.height < 540)
        {
            scalingFactor = 1;
        }
        else if (Screen.height < 720)
        {
            scalingFactor = 2;
        }
        else if (Screen.height < 1080)
        {
            scalingFactor = 3;
        }
        else
        {
            scalingFactor = 4;
        }
        Camera.main.orthographicSize = Screen.height / (24f * 2f * scalingFactor);
    }

    void FixedUpdate () {
        float activeDampTime = cameraEvent == null ? dampTime : cameraEvent.DampTime;

        if (cameraEvent != null)
        {
            if (cameraEvent.Completed && ((Vector2)transform.position - rb.position).sqrMagnitude < .5f)
            {
                // Must grab this reference or else the closure will refer to null cameraEvent, causing an exception.
                CameraFunction func = cameraEvent.EndFunction;
                StartCoroutine(Do(cameraEvent.EndWaitTime, func));
                cameraEvent = null;
            }
            else if (cameraEvent.Started == false && (cameraEvent.Target - (Vector2)transform.position).sqrMagnitude < 1f)
            {
                StartCoroutine(Do(cameraEvent.FocusHeadWindow, () => cameraEvent.CameraFunction()));
                cameraEvent.Started = true;
            }
        }

        transform.position = canonicalCameraPosition;
    }

    void HandleMouseLookaround(ref Vector3 targetPosition)
    {
        float screenHeightUnits = Camera.main.orthographicSize * 2;

        if (Input.mousePosition.y >= Screen.height * cameraEdgeThreshold)
        {
            float multiplier = (Input.mousePosition.y - Screen.height * cameraEdgeThreshold) / (Screen.height - Screen.height * cameraEdgeThreshold);
            targetPosition.y += multiplier * maxScrollFraction * screenHeightUnits;
        }

        if (Input.mousePosition.y <= Screen.height * (1 - cameraEdgeThreshold))
        {
            float multiplier = (Screen.height * (1 - cameraEdgeThreshold) - Input.mousePosition.y) / (Screen.height * (1 - cameraEdgeThreshold));
            targetPosition.y -= multiplier * maxScrollFraction * screenHeightUnits;
        }

        if (Input.mousePosition.x >= Screen.width * cameraEdgeThreshold)
        {
            float multiplier = (Input.mousePosition.x - Screen.width * cameraEdgeThreshold) / (Screen.width - Screen.width * cameraEdgeThreshold);
            targetPosition.x += multiplier * maxScrollFraction * screenHeightUnits;
        }

        if (Input.mousePosition.x <= Screen.width * (1 - cameraEdgeThreshold))
        {
            float multiplier = (Screen.width * (1 - cameraEdgeThreshold) - Input.mousePosition.x) / (Screen.width * (1 - cameraEdgeThreshold));
            targetPosition.x -= multiplier * maxScrollFraction * screenHeightUnits;
        }
    }

    public void LoadCameraEvent(CameraFunction cameraFunction, CameraFunction endFunction, float startWaitTime,
            float endWaitTime, Vector2 target, float dampTime, float focusHeadWindow, float focusTailWindow) {
        StartCoroutine(Do(startWaitTime, () => {
                cameraEvent = new CameraEvent(cameraFunction, endFunction,
                    endWaitTime, target, dampTime, focusHeadWindow, focusTailWindow);
        }
        ));
    }

    public void UnloadCameraEvent()
    {
        StartCoroutine(Do(cameraEvent.FocusTailWindow, () => cameraEvent.Completed = true));
    }

    public void SetMaxScrollFraction(float newMaxScrollFraction)
    {
        maxScrollFraction = newMaxScrollFraction;
    }

    public void ResetMaxScrollFraction()
    {
        maxScrollFraction = initialMaxScrollFraction;
    }

    IEnumerator Do(float waitTime, CameraFunction func)
    {
        yield return new WaitForSeconds(waitTime);
        func();
    }

    // Remember always to unload the camera event when done.
    public class CameraEvent {
        public CameraFunction CameraFunction;
        public CameraFunction EndFunction;
        public Vector2 Target;
        public float EndWaitTime;
        public float FocusHeadWindow;
        public float FocusTailWindow;
        public float DampTime;
        public bool Started;
        public bool Completed;

        public CameraEvent(CameraFunction cameraFunction, CameraFunction endFunction, float endWaitTime, Vector2 target, float dampTime,
            float focusHeadWindow, float focusTailWindow)
        {
            CameraFunction = cameraFunction;
            EndFunction = endFunction;
            Target = target;
            EndWaitTime = endWaitTime;
            Started = false;
            Completed = false;
            DampTime = dampTime;
            FocusHeadWindow = focusHeadWindow;
            FocusTailWindow = focusTailWindow;
        }
    }
}