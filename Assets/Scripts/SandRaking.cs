// SandRaking.cs — attach to zen garden sand plane
using UnityEngine;

public class SandRaking : MonoBehaviour
{
    public int textureSize = 512;
    public int brushSize = 6;
    public AudioSource rakeSound;
    public Camera mainCamera;

    private Texture2D sandTexture;
    private Color sandColor = new Color(0.86f, 0.83f, 0.76f);
    private Color grooveColor = new Color(0.75f, 0.72f, 0.65f);

    void Start()
    {
        sandTexture = new Texture2D(textureSize, textureSize);
        Color[] fill = new Color[textureSize * textureSize];
        for (int i = 0; i < fill.Length; i++) fill[i] = sandColor;
        sandTexture.SetPixels(fill);
        sandTexture.Apply();
        GetComponent<Renderer>().material.mainTexture = sandTexture;
    }

    void Update()
    {
        var toolMgr = FindObjectOfType<PlayerToolManager>();
        if (toolMgr == null || toolMgr.currentTool != "Rake") return;
        if (!Input.GetMouseButton(0)) 
        {
            if (rakeSound != null && rakeSound.isPlaying) rakeSound.Stop();
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 50f))
        {
            if (hit.collider.gameObject == gameObject)
            {
                PaintRakeLines(hit.textureCoord);
                if (rakeSound != null && !rakeSound.isPlaying) rakeSound.Play();
            }
        }
    }

    void PaintRakeLines(Vector2 uv)
    {
        // Draw 4 parallel lines like a real rake
        int centerX = (int)(uv.x * textureSize);
        int centerY = (int)(uv.y * textureSize);
        int spacing = brushSize * 2;

        for (int tine = -2; tine <= 1; tine++)
        {
            int offsetX = centerX + (tine * spacing);
            for (int i = -brushSize; i <= brushSize; i++)
            {
                for (int j = -brushSize; j <= brushSize; j++)
                {
                    if (i * i + j * j <= brushSize * brushSize)
                    {
                        int px = Mathf.Clamp(offsetX + i, 0, textureSize - 1);
                        int py = Mathf.Clamp(centerY + j, 0, textureSize - 1);
                        sandTexture.SetPixel(px, py, grooveColor);
                    }
                }
            }
        }
        sandTexture.Apply();
    }
}