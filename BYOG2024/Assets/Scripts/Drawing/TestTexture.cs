using UnityEngine;

public class TestTexture : MonoBehaviour
{
    [SerializeField]
    private string _textureName;

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            _spriteRenderer.sprite = PacmanConfig.Drawings[_textureName];
        }
    }
}
