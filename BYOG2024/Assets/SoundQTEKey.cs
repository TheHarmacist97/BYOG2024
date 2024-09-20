using System;
using System.Collections;
using System.Collections.Generic;
using QTEs.SoundDesignQTE;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public enum QTEKey
{
    W,
    S,
    A,
    D
}

public class SoundQTEKey : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _speed;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private QTEKey _targetKey;
    public QTEKey TargetKey => _targetKey;

    public void SetUp(Color color, float speed, QTEKey keyQueueElement)
    {
        _spriteRenderer.color = color;
        _speed = speed;
        _targetKey = keyQueueElement;
        _text.color = color;
        _text.text = keyQueueElement.ToString();
    }

    public void KillSelf(bool userSucceeded)
    {
        //do some polish here
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void Update()
    {
        transform.Translate(Time.deltaTime * _speed * Vector2.left);
    }
    
}
