using UnityEngine;

public class StarfieldColorManager : MonoBehaviour
{
    public float lerpSpeed = 2f;
    private Camera cam;
    public Color currentColor;
    public Color ColorToLerpTo;
    public Color defaultVoidColor;

    void Awake()
    {
        cam = Camera.main;
        cam.backgroundColor = defaultVoidColor;
        ColorToLerpTo = defaultVoidColor;
    }

    void Update()
    {
        if (ColorToLerpTo == null || Mathf.Approximately(GlobalDataStore.Instance.Player.GetComponent<Rigidbody>().linearVelocity.magnitude, 0f)) return;
        currentColor = Color.Lerp(currentColor, ColorToLerpTo, Time.deltaTime * lerpSpeed);
        cam.backgroundColor = currentColor;
    }


    public void ChangeColor(Color planetThemeColor, bool leftAPlanet = false)
    {
        if (leftAPlanet)
        {
            ColorToLerpTo = defaultVoidColor;
            return;
        }

        ColorToLerpTo = planetThemeColor;
    }
}
