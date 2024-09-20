
using System;
using TMPro;
using UnityEngine;

public class ProgrammingQTELetter : MonoBehaviour
{
    private TextMeshProUGUI _letterText;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _letterText = GetComponentInChildren<TextMeshProUGUI>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _letterText.text = "";
    }

    public void Init(string letter)
    {
        _letterText.text = letter;
    }

    public void SetAsCurrentlyActive()
    {
        _spriteRenderer.color = Color.yellow;
    }

    public void DestroyAsSuccess()
    {
        _spriteRenderer.color = Color.green;
        Invoke(nameof(Destroy), 0.1f);
    }

    public void DestroyAsFailure()
    {
        _spriteRenderer.color = Color.red;

        Invoke(nameof(Destroy), 0.1f);
    }
    
    private void Destroy()
    {
        Destroy(gameObject);
    }
}