using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum QTEKey
{
    Up,
    Down,
    Left,
    Right
}

public class SoundQTEKey : MonoBehaviour
{
    public QTEKey _qteKey;
    public Collider2D _keyCollider;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _speed;

    public void SetUp(Color color, float speed)
    {
        _spriteRenderer.color = color;
        _speed = speed;
    }

    public void Update()
    {
        transform.Translate(Time.deltaTime * _speed * Vector2.left);
    }
    
}
