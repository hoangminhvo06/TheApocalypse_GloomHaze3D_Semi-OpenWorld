using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DamagePopUpAnimation : MonoBehaviour
{
    public AnimationCurve opacityCurve;//kiểu opacity mờ dần, rõ dần

    private TextMeshProUGUI tmp; //Text bên trong popup
    float time = 0;// Thời gian để chuyển opacity

    private void Awake()
    {
        //Truy text là con thứ 0 của popup
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // Đổi số cuối cùng chính là opacity, do mờ dần rõ dần
        // Nó sẽ được ước lượng và biến thiên theo thời gian
        tmp.color = new Color(1, 1, 1, opacityCurve.Evaluate(time));
        time += Time.deltaTime;
    }
}
