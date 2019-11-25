using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UBS_mvc.Models;

namespace UBS_mvc.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.EstaNaHome = true;
            return View();
        }
        public IActionResult Agenda()
        {
            return View();
        }
        public IActionResult Consulta()
        {
            return View();
        }
        public IActionResult Remedio()
        {
            return View();
        }
        public IActionResult Sus()
        {
            return View();
        }
        public IActionResult Ubs()
        {
            return View();
        }
        public IActionResult Upa()
        {
            return View();
        }
        public IActionResult Vacinacao()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

