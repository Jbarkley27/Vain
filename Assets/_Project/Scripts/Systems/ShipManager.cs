using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public static ShipManager Instance;
    public Ship CurrentShip;
    public GameObject visualRoot;
    public ShaderData currentShader;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found a Inventory object, destroying new one");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        EquipNewShip(CurrentShip);
    }


    public void EquipNewShip(Ship newship)
    {
        Destroy(visualRoot.transform.GetChild(0).gameObject);

        CurrentShip = Instantiate(newship, visualRoot.transform).GetComponent<Ship>();

        CurrentShip.ApplyNewShader(currentShader);
    }


    public void EquipNewShader(ShaderData shaderData)
    {
        CurrentShip.ApplyNewShader(shaderData);
    }
}