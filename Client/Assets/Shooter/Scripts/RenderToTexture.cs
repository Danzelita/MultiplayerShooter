using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RenderToTexture : MonoBehaviour
{
    [SerializeField] private Camera _camera;                 // Камера, которая рендерит в RenderTexture
    [SerializeField] private RenderTexture _renderTexture;   // Текстура для рендера
    [SerializeField] private string _fileName = "SkinIcon.png";

    private string _savePath = "C:/UnityProjects/MultiplayerShooter/Client/Assets/Shooter/Art/GunsIcons/";

    [ContextMenu(nameof(CreateCapture))]
    public void CreateCapture()
    {
        StartCoroutine(CaptureAfterFrame());
    }
    
    private void Start()
    {
        StartCoroutine(CaptureAfterFrame());
    }

    private IEnumerator CaptureAfterFrame()
    {
        // Дожидаемся конца кадра — иначе RenderTexture может быть ещё не отрендерен
        yield return new WaitForEndOfFrame();
        CaptureAndSaveIcon();
    }

    public void CaptureAndSaveIcon()
    {
        // Если камера задана — рендерим вручную в RenderTexture
        if (_camera != null)
        {
            _camera.targetTexture = _renderTexture;
            _camera.Render();
            _camera.targetTexture = null;
        }

        // Активируем RenderTexture, чтобы можно было считать пиксели
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = _renderTexture;

        // Копируем содержимое
        Texture2D texture = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.ARGB32, false);
        texture.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
        texture.Apply();

        RenderTexture.active = currentRT;

        // Делаем прозрачный фон (если фон черный)
        Color32[] pixels = texture.GetPixels32();
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].r == 0 && pixels[i].g == 0 && pixels[i].b == 0)
                pixels[i].a = 0;
        }
        texture.SetPixels32(pixels);
        texture.Apply();

        // Кодируем в PNG
        byte[] pngData = texture.EncodeToPNG();

        // Создаём папку, если нужно
        if (!Directory.Exists(_savePath))
            Directory.CreateDirectory(_savePath);

        string filePath = Path.Combine(_savePath, _fileName);
        File.WriteAllBytes(filePath, pngData);

        Debug.Log($"✅ Скрин сохранён: {filePath}");

        Destroy(texture);
    }
}
