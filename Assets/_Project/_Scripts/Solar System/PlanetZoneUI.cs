using UnityEngine;
using TMPro;
using DG.Tweening;

public class PlanetZoneUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _planetName;
    [SerializeField] private TMP_Text _categoryName;
    [SerializeField] private CanvasGroup _planetNameCG;
    [SerializeField] private CanvasGroup _categoryCG;
    [SerializeField] private bool _showingState;
    [SerializeField] private float _showDuration;
    Sequence seq;

    void Start()
    {
        _planetNameCG.alpha = 0;
        _categoryCG.alpha = 0;
    }


    public void EnterNewZone(string name, bool isPlanet = true)
    {
        // If a sequence is already playing, kill it before starting over
        if (seq != null && seq.IsActive())
        {
            seq.Kill();
        }

        _showingState = true;


        // update text
        _planetName.text = name;

        if (isPlanet)
        {
            _categoryName.text = "Planetary Zone";
        }
        else
        {
            _categoryName.text = "Veil System";
        }


        // animation
            seq = DOTween.Sequence();

        seq.Append(_planetNameCG.DOFade(1f, .8f));
        seq.Join(_categoryCG.DOFade(1f, 1.2f));
        seq.AppendInterval(_showDuration);
        seq.Append(_planetNameCG.DOFade(0f, 1f));
        seq.Join(_categoryCG.DOFade(0f, .8f));

        seq.OnComplete(() =>
        {
            _showingState = false;
        });
    }
}
