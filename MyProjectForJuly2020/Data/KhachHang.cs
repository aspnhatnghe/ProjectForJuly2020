using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProjectForJuly2020.Data
{
    [Table("KhachHang")]
    public class KhachHang
    {
        [Key]
        public int MaKh { get; set; }
        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string MatKhau { get; set; }
        public string DiaChi { get; set; }
        public bool DangHoatDong { get; set; }
        public string MaNgauNhien { get; set; }
    }
}
