using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IconTabController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    [SerializeField]
    private bool _isActiveIcon = false;

    [SerializeField]
    private Image _highlight;
    private IClothingUIManager _manager;
    public ScriptableObject uiAssets;

    private float _currHighlightAlpha;
    private Coroutine _lerpCoroutine;
    [SerializeField]
    private float _lerpDuration;

    public void OnPointerClick(PointerEventData eventData)
    {
        _manager.OnIconTabSwitch(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //highlight effect here, will be changed later
        StartLerp(1.0f);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!_isActiveIcon) StartLerp(0);

    }
    public void EnableTab()
    {
        _isActiveIcon = true;
        StartLerp(1.0f);
    }

    public void DisableTab()
    {
        _isActiveIcon = false;
        Color c = _highlight.color;
        StartLerp(0);
    }

    public void SetCategoryManager(IClothingUIManager manager)
    {
        _manager = manager;
    }

    public void SetAssets(ScriptableObject asset)
    {
        uiAssets = asset;
    }

    private void StartLerp(float targetValue)
    {
        StopLerp();
        _lerpCoroutine = StartCoroutine(LerpValue(targetValue));
    }

    private void StopLerp()
    {
        if (_lerpCoroutine != null)
        {
            StopCoroutine(_lerpCoroutine);
            _lerpCoroutine = null;
        }
    }

    private IEnumerator LerpValue(float targetValue)
    {
        float startValue = _currHighlightAlpha;
        float elapsed = 0f;

        while (elapsed < _lerpDuration)
        {
            elapsed += Time.deltaTime;
            _currHighlightAlpha = Mathf.Lerp(startValue, targetValue, elapsed / _lerpDuration);
            SetHighlightAlpha(_currHighlightAlpha);
            yield return null;
        }

        _currHighlightAlpha = targetValue;
        SetHighlightAlpha(_currHighlightAlpha);
        _lerpCoroutine = null;
    }

    private void SetHighlightAlpha(float alpha)
    {
        Color c = _highlight.color;
        _highlight.color = new Color(c.r, c.g, c.b, alpha);
    }
}
