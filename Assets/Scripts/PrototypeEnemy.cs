using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PrototypeEnemy : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnDamage()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(_spriteRenderer.DOColor(Color.white, 0.1f));
        sequence.Append(_spriteRenderer.DOColor(_spriteRenderer.color, 0.5f));
    }
}
