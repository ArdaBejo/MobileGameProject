using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class StoreShot : MonoBehaviour
{
    bool capturing;

    public async Awaitable CaptureAsync(string tag)
    {
        if (!Debug.isDebugBuild || capturing) return;
        capturing = true;
        var hidden = new List<Behaviour>();
        var bars = new List<Renderer>();
        Texture2D frame = null;
        Texture2D image = null;
        try
        {
            foreach (var canvas in FindObjectsByType<Canvas>(FindObjectsSortMode.None))
            {
                if (!canvas.enabled) continue;
                hidden.Add(canvas);
                canvas.enabled = false;
            }
            var player = GreyboxLane.Instance?.Player;
            var healthBar = player != null ? player.Find("HealthBar") : null;
            if (healthBar != null)
            {
                foreach (var renderer in healthBar.GetComponentsInChildren<Renderer>())
                {
                    if (!renderer.enabled) continue;
                    bars.Add(renderer);
                    renderer.enabled = false;
                }
            }

            await Awaitable.EndOfFrameAsync(destroyCancellationToken);
            frame = ScreenCapture.CaptureScreenshotAsTexture();
            // Export the central 9:16 region. Tall-screen edges and the debug watermark fall outside it.
            int width = frame.width;
            int height = width * 16 / 9;
            if (height > frame.height)
            {
                height = frame.height;
                width = height * 9 / 16;
            }
            image = new Texture2D(width, height, TextureFormat.RGB24, false);
            image.SetPixels(frame.GetPixels((frame.width - width) / 2, (frame.height - height) / 2, width, height));
            image.Apply();
            string filename = $"{Path.GetFileName(tag)}_{width}x{height}.png";
            string path = Path.Combine(Application.persistentDataPath, filename);
            File.WriteAllBytes(path, image.EncodeToPNG());
            Debug.Log($"[StoreShot] Saved {path}");
        }
        catch (OperationCanceledException) { }
        finally
        {
            foreach (var canvas in hidden)
                if (canvas != null) canvas.enabled = true;
            foreach (var renderer in bars)
                if (renderer != null) renderer.enabled = true;
            if (frame != null) Destroy(frame);
            if (image != null) Destroy(image);
            capturing = false;
        }
    }
}
