using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProjectForJuly2020.Data;
using MyProjectForJuly2020.Helpers;
using MyProjectForJuly2020.ViewModels;
using System.Linq;

namespace MyProjectForJuly2020.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public KhachHangController(MyDbContext ctx, IMapper mapper)
        {
            _context = ctx; _mapper = mapper;
        }

        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DangKy(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var khachHang = _mapper.Map<KhachHang>(model);
                        khachHang.MaNgauNhien = MyTools.GetRandom();
                        khachHang.MatKhau = model.MatKhau.ToSHA512Hash(khachHang.MaNgauNhien);
                        _context.Add(khachHang);
                        _context.SaveChanges();

                        //Add role for user, default Customer
                        var userRole = new UserRole
                        {
                            RoleId = 4,//Khách hàng
                            UserId = khachHang.MaKh
                        };
                        _context.Add(userRole);
                        _context.SaveChanges();

                        transaction.Commit();
                        return RedirectToAction("DangNhap");
                    }
                    catch
                    {
                        transaction.Rollback();
                        return View();
                    }
                }

            }
            return View();
        }

        #region Dang Nhap
        [HttpGet]
        public IActionResult DangNhap(string ReturnUrl = null)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult DangNhap(LoginVM model, string ReturnUrl = null)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            string thongBaoLoi = string.Empty;
            if (ModelState.IsValid)
            {
                var khachHang = _context.KhachHangs.SingleOrDefault(kh => kh.Email == model.Email);
                if (khachHang == null)
                {
                    ViewBag.ThongBaoLoi = "Tài khoản không tồn tại.";
                    return View();
                }
                if (!khachHang.DangHoatDong)
                {
                    ViewBag.ThongBaoLoi = "Tài khoản đang bị khóa.";
                    return View();
                }
                if (khachHang.MatKhau != model.MatKhau.ToSHA512Hash(khachHang.MatKhau))
                {
                    ViewBag.ThongBaoLoi = "Sai thông tin đăng nhập.";
                    return View();
                }
            }

            ViewBag.ThongBaoLoi = thongBaoLoi;
            return View();
        }
        #endregion
    }
}