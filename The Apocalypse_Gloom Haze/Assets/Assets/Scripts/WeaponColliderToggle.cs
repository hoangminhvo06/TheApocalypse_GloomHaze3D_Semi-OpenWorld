using UnityEngine;

public class WeaponColliderToggle : MonoBehaviour
{
    public Collider weaponCol;

    void Start()
    {
        weaponCol.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            weaponCol.enabled = true;

        if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
            weaponCol.enabled = false;
    }
}
