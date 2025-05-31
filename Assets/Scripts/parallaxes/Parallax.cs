using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    private Vector3 lastCameraPos;
    public Vector2 parallaxMultiplier;
    private float textureUnitSizeX;

    void Start()
    {
        lastCameraPos = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    void LateUpdate()
    {
        // Corrected parallax movement by applying an offset rather than overriding
        Vector3 deltaMovement = cameraTransform.position - lastCameraPos;
        transform.position += new Vector3(deltaMovement.x * parallaxMultiplier.x, deltaMovement.y * parallaxMultiplier.y, 0);
        lastCameraPos = cameraTransform.position;

        // Properly resetting the background position when it moves out of bounds
        float cameraOffset = cameraTransform.position.x - transform.position.x;
        if (Mathf.Abs(cameraOffset) >= textureUnitSizeX)
        {
            float offsetX = cameraOffset % textureUnitSizeX;
            transform.position += new Vector3(cameraTransform.position.x + offsetX, 0, 0);
        }
    }
}
