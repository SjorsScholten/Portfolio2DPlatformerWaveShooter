using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Entity : MonoBehaviour {
    private Rigidbody2D _rigidbody2D;
    public Vector2 Position {
        get => _rigidbody2D.position;
        set => _rigidbody2D.position = value;
    }

    protected virtual void Awake() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
}
