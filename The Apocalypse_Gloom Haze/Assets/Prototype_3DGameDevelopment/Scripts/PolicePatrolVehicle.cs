using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PolicePatrolVehicle : MonoBehaviour
{
    [SerializeField] //Là thuộc tính cho Unity “lưu trữ & hiển thị” biến trong Inspector
    private float tocDoXe = 50f; //Phạm vi truy cập là private, kiểu dl là float
    [SerializeField]
    private float lucReXe = 100f;
    [SerializeField]
    private float lucPhanhXe = 100f;
    [SerializeField]
    private GameObject hieuUngPhanh;
    private float dauVaoDiChuyen;
    private float dauVaoRe;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() //Cập nhật trạng thái game ở mỗi khung hình
    {
        dauVaoDiChuyen = Input.GetAxis("Vertical"); //Di chuyển tới lui theo trục dọc cho nên là Vertical
        dauVaoRe = Input.GetAxis("Horizontal");
        DiChuyenXe(); //Gọi phương thức
        ReXe();
        if (dauVaoDiChuyen>0&&Input.GetKey(KeyCode.LeftShift))
        {
            PhanhXe();
        }
    }
    public void DiChuyenXe()
    {
        rb.AddRelativeForce(Vector3.forward * dauVaoDiChuyen * tocDoXe); //Phương thức AddRelativeForce để tác động 1 lực lên xe để tiến về phía trước
        hieuUngPhanh.SetActive(false);
    }
    public void ReXe()
    {
        Quaternion re = Quaternion.Euler(Vector3.up * dauVaoRe * lucReXe * Time.deltaTime);
        rb.MoveRotation(rb.rotation * re);
    }
    public void PhanhXe()
    {
        if (rb.linearVelocity.z != 0)
        {
            rb.AddRelativeForce(-Vector3.forward * lucPhanhXe);
            hieuUngPhanh.SetActive(true);
       }
    }
}
