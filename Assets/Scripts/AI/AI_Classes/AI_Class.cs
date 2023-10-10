using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Class : MonoBehaviour
{
     [SerializeField] private Transform _transform;
     [SerializeField] private Rigidbody _rigidbody;
     [SerializeField] private CapsuleCollider _capsuleCollider;

    public Transform Transform { get => _transform; set => _transform = value; }
    public Rigidbody Rigidbody { get => _rigidbody; set => _rigidbody = value; }
    public CapsuleCollider CapsuleCollider { get => _capsuleCollider; set => _capsuleCollider = value; }

}
