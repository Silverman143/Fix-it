using UnityEngine;

namespace FixItGame
{
    public class BackgroundScaler : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            ScaleBackground();
        }

        void Update()
        {
            ScaleBackground();
        }

        void ScaleBackground()
        {
            if (spriteRenderer == null) return;

            // Получаем размеры камеры и спрайта
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float cameraHeight = Camera.main.orthographicSize * 2;
            Vector2 cameraSize = new Vector2(cameraHeight * screenAspect, cameraHeight);
            Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

            // Рассчитываем коэффициент масштабирования, сохраняя пропорции
            float scale = Mathf.Max(cameraSize.x / spriteSize.x, cameraSize.y / spriteSize.y);

            // Применяем масштабирование
            transform.localScale = new Vector3(scale, scale, 1);
        }
    }
}
