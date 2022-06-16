using UnityEngine;
 
public class CameraController : MonoBehaviour
{
    public float TimeToReachNextBackground;
    public bool IsMovingCamera;
    private Camera mainCamera;
    private float camTime;
    private Vector2 startPosition;
    private Vector2 endPosition;

    private WaveController waveController;
    private StageBackgroundController stageBackgroundController;

    void Awake()
    {
        mainCamera = Camera.main;

        waveController = FindObjectOfType<WaveController>();
        stageBackgroundController = FindObjectOfType<StageBackgroundController>();

        UpdateCameraPosition();
    }
    void Update()
    {
        if (IsMovingCamera && !PauseController.Instance.IsPaused() && !GameOverController.Instance.IsGameOver)
        {
            camTime += Time.deltaTime;
            mainCamera.transform.position = Vector2.Lerp(startPosition, endPosition, camTime / TimeToReachNextBackground);
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, -10);
            if (camTime >= TimeToReachNextBackground)
            {
                IsMovingCamera = false;
                camTime = 0;
                waveController.StartWave();
            }
        }
    }
    public void UpdateCameraPosition()
    {
        startPosition = mainCamera.transform.position;
        endPosition = stageBackgroundController.StageBackgroundRenderers[waveController.CurWave].transform.position;
    }
}
