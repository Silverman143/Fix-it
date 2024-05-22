using UnityEngine;

public class CameraWalls : MonoBehaviour
{
    public GameObject topWall;
    public GameObject bottomWall;
    public GameObject leftWall;
    public GameObject rightWall;

    void Start()
    {
        PositionWalls();
    }

    void Update()
    {
        PositionWalls();
    }

    void PositionWalls()
    {
        if (Camera.main == null) return;

        // Получаем размеры камеры
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        // Размеры стен
        float wallThickness = 1.0f; // Толщина стен может быть настроена по вашему усмотрению

        // Позиционируем и масштабируем стены
        leftWall.transform.position = new Vector3(-cameraWidth / 2 - wallThickness / 2, 0, 0);
        rightWall.transform.position = new Vector3(cameraWidth / 2 + wallThickness / 2, 0, 0);
        topWall.transform.position = new Vector3(0, cameraHeight / 2 + wallThickness / 2, 0);
        bottomWall.transform.position = new Vector3(0, -cameraHeight / 2 - wallThickness / 2, 0);

        //leftWall.transform.localScale = new Vector3(wallThickness, cameraHeight, 1);
        //rightWall.transform.localScale = new Vector3(wallThickness, cameraHeight, 1);
        //topWall.transform.localScale = new Vector3(cameraWidth, wallThickness, 1);
        //bottomWall.transform.localScale = new Vector3(cameraWidth, wallThickness, 1);

        // Обновляем размеры коллайдеров
        UpdateColliderSize(leftWall, wallThickness, cameraHeight);
        UpdateColliderSize(rightWall, wallThickness, cameraHeight);
        UpdateColliderSize(topWall, cameraWidth, wallThickness);
        UpdateColliderSize(bottomWall, cameraWidth, wallThickness);
    }

    void UpdateColliderSize(GameObject wall, float width, float height)
    {
        BoxCollider2D collider = wall.GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            collider.size = new Vector2(width, height);
        }
    }
}
