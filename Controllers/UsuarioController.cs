using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using UBS_mvc.Models;

namespace UBS_mvc.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly Appsettings _appSettings;

        public UsuarioController(Appsettings appSettings)
        {
            _appSettings = appSettings;
        }
        public IActionResult Index()
        {
            ViewBag.EstaNaHome = true;
            return View();
        }
        public IActionResult Agenda()
        {
            return View();
        }
        public IActionResult Busca([FromForm] string ResponsavelCpf)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            ResponsavelViewModel Responsavel = null;

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT UserID FROM User WHERE UserCpf LIKE '%" + ResponsavelCpf + "%'", conn))
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        if (Responsavel == null)
                        {
                            Responsavel = new ResponsavelViewModel
                            {
                                ResponsavelID = dataReader.GetInt32(0)
                            };
                        }
                    }
                }

                if(Responsavel == null)
                {
                    return RedirectToAction("Index", "Usuario");
                }

                return RedirectToAction("Consulta", new { id = Responsavel.ResponsavelID });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }
        }

        public IActionResult Consulta(int id)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            TelaViewModel Tela = new TelaViewModel();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT Dependent.dependentid, Dependent.dependentname, Vaccine.VaccineID, Vaccine.VaccineName, Vaccine_Dep.VaccineDate, Dose.DoseId, Dose.DoseType FROM User INNER JOIN dependent ON (User.UserID = dependent.User_UserID) INNER JOIN vaccine_dep on (dependent.DependentID = vaccine_dep.DependentID) INNER JOIN vaccine ON (vaccine_dep.VaccineID = vaccine.VaccineID) INNER JOIN dose on (Vaccine.VaccineID = dose.VaccineID) WHERE User.Userid =" + id + " GROUP BY vaccineid", conn))
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        if(Tela.Responsavel == null)
                        {
                            Tela.Responsavel = new ResponsavelViewModel();
                            Tela.Responsavel.Dependentes = new List<DependenteViewModel>();
                        }
                        
                        Tela.Responsavel.Dependentes.Add(new DependenteViewModel
                        {
                            DependentID = dataReader.GetInt32(0),
                            DependentName = dataReader.GetString(1)
                        });

                        if (Tela.Vacinas == null)
                        {
                            Tela.Vacinas = new List<VacinaViewModel>();
                        }

                        Tela.Vacinas.Add(new VacinaViewModel
                        {
                            VacinaId = dataReader.GetInt32(2),
                            VacinaName = dataReader.GetString(3)
                        });

                        if (Tela.Doses == null)
                        {
                            Tela.Doses = new List<DoseViewModel>();
                        }

                        Tela.Doses.Add(new DoseViewModel
                        {
                            VacinaData = dataReader.GetDateTime(4),
                            DoseID = dataReader.GetInt32(5),
                            DoseType = dataReader.GetString(6)
                        });
                    }
                }
                
                ViewBag.EstaNaHome = true;
                ViewData["Tela"] = Tela;
                return View();

            }
            catch (Exception ex)
            {
                return View(ex);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }
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

