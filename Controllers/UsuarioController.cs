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
            ResponsavelViewModel Responsavel = null;

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT Dependent.dependentid, Dependent.dependentname, Vaccine.VaccineID, Vaccine.VaccineName, Vaccine_Dose.VaccineDate, Dose.DoseId, Dose.DoseType FROM User INNER JOIN dependent ON (User.UserID = dependent.User_UserID) INNER JOIN vaccine_dep on (dependent.DependentID = vaccine_dep.DependentID) INNER JOIN vaccine ON (vaccine_dep.VaccineID = vaccine.VaccineID) INNER JOIN vaccine_dose ON (vaccine.vaccineid = vaccine_dose.vaccineid) INNER JOIN dose on (Vaccine_dose.doseID = dose.doseID) WHERE User.Userid =" + id + " GROUP BY vaccineid, dependentid, doseid", conn))
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        if(Responsavel == null)
                        {
                            Responsavel = new ResponsavelViewModel
                            {
                                Dependentes = new List<DependenteViewModel>()
                            };
                        }

                        var last = Responsavel.Dependentes.LastOrDefault();

                        if(last == null)
                        {
                            Responsavel.Dependentes.Add(new DependenteViewModel
                            {
                                DependentID = dataReader.GetInt32(0),
                                DependentName = dataReader.GetString(1),
                                Vacinas = new List<VacinaViewModel>(),
                            });
                            last = Responsavel.Dependentes.LastOrDefault();
                        }

                        if (last.DependentID != dataReader.GetInt32(0))
                        {
                            Responsavel.Dependentes.Add(new DependenteViewModel
                            {
                                DependentID = dataReader.GetInt32(0),
                                DependentName = dataReader.GetString(1),
                                Vacinas = new List<VacinaViewModel>()
                            });
                        }

                        foreach (var dependent in Responsavel.Dependentes)
                        {
                            var lastv = dependent.Vacinas.LastOrDefault();
                            if(lastv == null)
                            {
                                dependent.Vacinas.Add(new VacinaViewModel
                                {
                                    VacinaId = dataReader.GetInt32(2),
                                    VacinaName = dataReader.GetString(3),
                                    VacinaData = dataReader.GetDateTime(4),
                                    Doses = new List<DoseViewModel>()
                                });
                                lastv = dependent.Vacinas.LastOrDefault();
                            }

                            if (lastv.VacinaId != dataReader.GetInt32(2) && dependent.DependentID == dataReader.GetInt32(0))
                            {
                                dependent.Vacinas.Add(new VacinaViewModel
                                {
                                    VacinaId = dataReader.GetInt32(2),
                                    VacinaName = dataReader.GetString(3),
                                    VacinaData = dataReader.GetDateTime(4),
                                    Doses = new List<DoseViewModel>()
                                });
                                lastv = dependent.Vacinas.LastOrDefault();
                            }

                            if (lastv.VacinaId == dataReader.GetInt32(2) && dependent.DependentID == dataReader.GetInt32(0))
                            {
                                foreach (var vacina in dependent.Vacinas)
                                {
                                    var lastd = vacina.Doses.LastOrDefault();
                                    if (lastd == null)
                                    {
                                        vacina.Doses.Add(new DoseViewModel
                                        {
                                            DoseID = dataReader.GetInt32(5),
                                            DoseType = dataReader.GetString(6)
                                        });
                                        lastd = vacina.Doses.LastOrDefault();
                                    }
                                    if (lastd.DoseID != dataReader.GetInt32(5) && vacina.VacinaId == dataReader.GetInt32(2))
                                    {
                                        vacina.Doses.Add(new DoseViewModel
                                        {
                                            DoseID = dataReader.GetInt32(5),
                                            DoseType = dataReader.GetString(6)
                                        });
                                    }
                                }
                            }
                            
                        }
                    }
                }
                
                ViewBag.EstaNaHome = true;
                ViewData["Responsavel"] = Responsavel;
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

