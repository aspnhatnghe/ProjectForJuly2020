using System.ComponentModel.DataAnnotations;

namespace MyProjectForJuly2020.ViewModels
{
    public class RegisterVM
    {
        [MaxLength(100)]
        [Required]
        public string HoTen { get; set; }
        [MaxLength(20)]
        [Required]
        public string SoDienThoai { get; set; }
        [MaxLength(100)]
        [Required]
        public string Email { get; set; }
        [Required]
        public string MatKhau { get; set; }
        [Compare("MatKhau", ErrorMessage ="Mật khẩu không khớp")]
        public string NhapLaiMatKhau { get; set; }
        public string DiaChi { get; set; }
    }
}
