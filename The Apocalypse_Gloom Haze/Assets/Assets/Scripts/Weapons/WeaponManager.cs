using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Transform gunSocket; // Kéo Gun_Socket vào đây
    public GameObject weaponPrefab; // Model khẩu súng của cậu

    public Transform adsPoint; // Kéo Object AdsPos vào đây
    public Transform hipPoint; // Kéo Object HipPos vào đây
    public float adsSpeed = 10f;
    public GameObject currentWeapon; // Cần biến này để điều khiển khẩu súng đã sinh ra

    void Update()
    {
        // Tìm khẩu súng nếu chưa có (vì súng được sinh ra lúc Start)
        if (currentWeapon == null)
        {
            currentWeapon = gunSocket.GetChild(0).gameObject;
            return;
        }

        if (Input.GetMouseButton(1)) // Chuột phải
        {
            // Di chuyển súng đến vị trí của AdsPos
            currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, adsPoint.localPosition, Time.deltaTime * adsSpeed);
        }
        else // Thả chuột
        {
            // Di chuyển súng về vị trí của HipPos
            currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, hipPoint.localPosition, Time.deltaTime * adsSpeed);
        }
    }

    void Start()
    {
        EquipWeapon();
    }

    void EquipWeapon()
    {
        if (weaponPrefab != null && gunSocket != null)
        {
            // Tạo súng mới
            GameObject currentWeapon = Instantiate(weaponPrefab);

            // Gắn vào Socket
            currentWeapon.transform.SetParent(gunSocket);

            // QUAN TRỌNG: Reset toàn bộ vị trí và góc xoay về 0 của Socket
            currentWeapon.transform.localPosition = Vector3.zero;
            currentWeapon.transform.localRotation = Quaternion.identity;
            currentWeapon.transform.localScale = Vector3.one; // Đảm bảo to nhỏ đúng chuẩn
        }
    }
}