using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UnderAcidDetection : MonoBehaviour
{
    [SerializeField]
    private GameObject _boundingBox;
    [SerializeField]
    private GameObject _camera;
    [SerializeField]
    private Volume _post;
    [SerializeField]
    private Color _underWaterColor;
    [SerializeField]
    private bool _underWater;

    private Vignette _vg;
    private DepthOfField _dof;
    private ColorAdjustments _ca;

    private void Start()
    {
        _post.profile.TryGet(out _vg);
        _post.profile.TryGet(out _dof);
        _post.profile.TryGet(out _ca);
    }

    private void FixedUpdate()
    {
        if (_boundingBox.GetComponent<BoxCollider>().bounds.Contains(_camera.transform.position)) _underWater = false;
        else _underWater = true;

        if (!_underWater)
        {
            _vg.intensity.value = 0.35f;
            _dof.focusDistance.value = 0.1f;
            _ca.colorFilter.value = _underWaterColor;
        }
        else
        {
            _vg.intensity.value = 0.292f;
            _dof.focusDistance.value = 5f;
            _ca.colorFilter.value = Color.white;
        }
    }
}
