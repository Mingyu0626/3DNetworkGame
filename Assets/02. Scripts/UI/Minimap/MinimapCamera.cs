using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MinimapCamera : MonoBehaviour
{
    private Transform _target;
    private float _yDistance = 20f;
    private Vector3 _initialEulerAngles;

    private void Start()
    {
        _initialEulerAngles = transform.eulerAngles;
    }

    private void LateUpdate()
    {
        if (_target == null)
        {
            return;
        }

        Vector3 targetPosition = _target.position;
        targetPosition.y += _yDistance;

        transform.position = targetPosition;
        Vector3 targetEulerAngles = _target.eulerAngles;
        targetEulerAngles.x = _initialEulerAngles.x;
        targetEulerAngles.z = _initialEulerAngles.z;
        transform.eulerAngles = targetEulerAngles;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}