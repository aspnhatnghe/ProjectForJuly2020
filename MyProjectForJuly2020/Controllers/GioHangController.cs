﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProjectForJuly2020.Data;
using MyProjectForJuly2020.Helpers;
using MyProjectForJuly2020.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MyProjectForJuly2020.Controllers
{
    public class GioHangController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public GioHangController(MyDbContext ctx, IMapper mapper)
        {
            _context = ctx;
            _mapper = mapper;
        }

        public List<CartItem> Carts
        {
            get
            {
                var carts = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (carts == null)
                {
                    carts = new List<CartItem>();
                }
                return carts;
            }
        }

        public IActionResult Index()
        {
            return View(Carts);
        }

        public IActionResult ThemVaoGio(Guid id, string addType, int qty = 1)
        {
            //lấy giỏ hàng hiện tại
            var myCart = Carts;

            //kiểm tra hàng đã có trong giỏ
            var item = myCart.SingleOrDefault(it => it.MaHangHoa == id);
            if (item != null)//đã có
            {
                item.SoLuong += qty;
            }
            else
            {
                var hh = _context.HangHoas.FirstOrDefault(p => p.MaHangHoa == id);
                item = _mapper.Map<CartItem>(hh);
                item.SoLuong = qty;
                myCart.Add(item);
            }
            HttpContext.Session.Set("GioHang", myCart);

            if (addType == "ajax")
                return PartialView("_CartView");

            return RedirectToAction("Index");
        }

        public IActionResult RemoveCartItem(Guid id, bool isAjaxCall = false)
        {
            //lấy giỏ hàng hiện tại
            var myCart = Carts;

            //kiểm tra hàng đã có trong giỏ
            var item = myCart.SingleOrDefault(it => it.MaHangHoa == id);
            if(item != null)
            {
                myCart.Remove(item);
                HttpContext.Session.Set("GioHang", myCart);
            }

            if (isAjaxCall) { }

            return RedirectToAction("Index");
        }

        [Authorize, HttpGet]
        public IActionResult ThanhToan()
        {
            return View();
        }

        [Authorize, HttpPost]
        public IActionResult ThanhToan(ThanhToanVM model)
        {
            if (ModelState.IsValid)
            {
                var emailKh = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                var maKH = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "MaNguoiDung").Value);
                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var donHang = new DonHang
                        {
                            MaDh = Guid.NewGuid(),
                            MaKh = maKH,
                            NgayDat = DateTime.UtcNow,
                            TinhTrangDonHang = TinhTrangDonHang.MoiDatHang,
                            DiaChiGiao = model.DiaChiGiao,
                            NguoiNhan = model.NguoiNhan
                        };
                        _context.Add(donHang);
                        foreach (var item in Carts)
                        {
                            _context.Add(new DonHangChiTiet
                            {
                                MaDh = donHang.MaDh,
                                MaHh = item.MaHangHoa,
                                SoLuong = item.SoLuong,
                                DonGia = item.DonGia
                            });
                        }
                        _context.SaveChanges();
                        trans.Commit();
                        HttpContext.Session.Remove("GioHang");
                        return Redirect("/KhachHang/HangDaMua");
                    }
                    catch(Exception ex)
                    {
                        trans.Rollback();
                        return View();
                    }
                }

            }
            return View();
        }
    }
}