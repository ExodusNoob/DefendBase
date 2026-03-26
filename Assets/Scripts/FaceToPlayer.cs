using UnityEngine;

public class FaceToPlayer : MonoBehaviour
{
    [SerializeField] private Transform rplayer;

    [SerializeField] private Transform graphics;
    [SerializeField] private Transform hitboxes;

    [SerializeField] private bool isFacingRight = true;

    private void Awake()
    {
        GameObject player = GameObject.Find("Player");

        if (player != null)
        {
            rplayer = player.transform;
        }
        else
        {
            Debug.LogError("No se encontró el GameObject llamado Player");
        }
    }

    void Update()
    {
        bool isPlayerLeft = rplayer.position.x < transform.position.x;
        Flip(isPlayerLeft);
    }

    private void Flip(bool isPlayerLeft)
    {
        if ((isFacingRight && isPlayerLeft) || (!isFacingRight && !isPlayerLeft))
        {
            isFacingRight = !isFacingRight;

            FlipTransform(graphics);
            FlipTransform(hitboxes);
        }
    }

    private void FlipTransform(Transform t)
    {
        Vector3 scale = t.localScale;
        scale.x *= -1;
        t.localScale = scale;
    }
}