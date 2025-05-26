using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;

public class PlanetZoneUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _planetName;
    [SerializeField] private CanvasGroup _planetNameCG;
    [SerializeField] private CanvasGroup _categoryCG;
    [SerializeField] private Transform _planetZoneRoot;
    [SerializeField] private bool _showingState;
    [SerializeField] private float _showDuration;

    void Start()
    {
        _planetZoneRoot.gameObject.SetActive(false);
    }


    public IEnumerator EnterNewZone(string name)
    {
        if (_showingState) yield return null;

        _showingState = true;

        _planetNameCG.alpha = 0;
        _categoryCG.alpha = 0;

        _planetName.text = name;

        _planetZoneRoot.gameObject.SetActive(true);

        _planetNameCG.DOFade(1, 1f);

        yield return new WaitForSeconds(.5f);

        _categoryCG.DOFade(1, 1f);

        yield return new WaitForSeconds(_showDuration);

        _planetNameCG.DOFade(0, .8f);
        _categoryCG.DOFade(0, .4f)
        .OnComplete(() =>
        {
            _planetZoneRoot.gameObject.SetActive(false);
            _showingState = false;
        });
    }
}
