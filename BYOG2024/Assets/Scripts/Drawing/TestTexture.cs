using UnityEngine;
using UnityEngine.Serialization;

public class TestTexture : MonoBehaviour
{
    [SerializeField]
    private PictureIDs _picID;

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            _spriteRenderer.sprite = PacmanConfig.Drawings[_picID];
        }
    }
}
