using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProjectForJuly2020.Data;
using MyProjectForJuly2020.ViewModels;

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
            return View();
        }
    }
}