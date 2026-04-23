using UnityEngine;

public class PoliceLights : MonoBehaviour
{
    private Light denXanh;
    private Light denDo;

    [SerializeField] private Vector3 offsetXanh = new Vector3(0.5f, 1.5f, 0f);
    [SerializeField] private Vector3 offsetDo = new Vector3(-0.5f, 1.5f, 0f);

    [SerializeField] private float maxIntensity = 8f;
    [SerializeField] private float nhapNhaySpeed = 10f;

    private float timer = 0f;
    private void Start()
    {
        // Tạo đèn xanh
        GameObject goXanh = new GameObject("DenXanh");
        goXanh.transform.parent = transform;
        goXanh.transform.localPosition = offsetXanh;
        denXanh = goXanh.AddComponent<Light>();
        denXanh.color = Color.blue;
        denXanh.intensity = 0f;
        denXanh.range = 5f;
        denXanh.type = LightType.Point;

        // Tạo đèn đỏ
        GameObject goDo = new GameObject("DenDo");
        goDo.transform.parent = transform;
        goDo.transform.localPosition = offsetDo;
        denDo = goDo.AddComponent<Light>();
        denDo.color = Color.red;
        denDo.intensity = 0f;
        denDo.range = 5f;
        denDo.type = LightType.Point;

    }

    private void Update()
    {
        timer += Time.deltaTime * nhapNhaySpeed;

        // Nhấp nháy xanh đỏ
        denXanh.intensity = Mathf.Clamp01(Mathf.Sin(timer)) * maxIntensity;
        denDo.intensity = Mathf.Clamp01(Mathf.Sin(timer + Mathf.PI)) * maxIntensity;

    }

}
