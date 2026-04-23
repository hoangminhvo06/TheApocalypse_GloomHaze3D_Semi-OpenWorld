using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DamagePoUpGenerator : MonoBehaviour
{
    public static DamagePoUpGenerator current;
    public GameObject prefab;//chính là canvas đã lưu trong prefab
    private void Awake()
    {
        current = this;
    }
    public void CreatePopup(Vector3 position, string text)
    {
        var popup = Instantiate(prefab, position, Quaternion.identity);

        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        Destroy(popup, 1f);

    }
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.I))
    //     {
    //         CreatePopup(Vector3.one, Random.Range(0, 1000).ToString());
    //     }

        
    // }
}
