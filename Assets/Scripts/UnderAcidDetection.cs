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
    private Color _underAcidColor;
    [SerializeField]
    private bool _underAcid;

    private Vignette _vg;
    private DepthOfField _dof;
    private ColorAdjustments _ca;

    private float _defaultVg;
    private float _defaultDof;
    private Color _defaultCa;

    private void Start()
    {
        _post.profile.TryGet(out _vg);
        _post.profile.TryGet(out _dof);
        _post.profile.TryGet(out _ca);

        _defaultVg = _vg.intensity.value;
        _defaultDof = _dof.focusDistance.value;
        _defaultCa = _ca.colorFilter.value;
    }

    private void FixedUpdate()
    {
        if (_boundingBox.GetComponent<BoxCollider>().bounds.Contains(_camera.transform.position)) _underAcid = false;
        else _underAcid = true;

        if (!_underAcid)
        {
            _vg.intensity.value = 0.35f;
            _dof.focusDistance.value = 0.1f;
            _ca.colorFilter.value = _underAcidColor;
        }
        else
        {
            _vg.intensity.value = _defaultVg;
            _dof.focusDistance.value = _defaultDof;
            _ca.colorFilter.value = _defaultCa;
        }
    }
}
