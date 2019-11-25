using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using UBS_mvc.Models;
using MySql.Data.MySqlClient;

namespace UBS_mvc.Controllers
{
    public class UbsController : Controller
    {
        private readonly Appsettings _appSettings;

        public UbsController(Appsettings appSettings)
        {
            _appSettings = appSettings;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login([FromForm] string AttendantEmail, string AttendantPass)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            AtendenteViewModel AtendenteViewModel = new AtendenteViewModel();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT AttendantEmail, AttendantPass FROM Attendant WHERE AttendantEmail LIKE '%" + AttendantEmail + "%' AND AttendantPass LIKE '%" + AttendantPass + "%'", conn))
                {

                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        if (!dataReader.IsDBNull(0) && !dataReader.IsDBNull(1))
                        {
                            return RedirectToAction("BuscarCpf");
                        }
                    }
                }
                            
                return RedirectToAction("Index");

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

        public IActionResult CadastrarAtendente()
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            AtendenteViewModel AtendenteViewModel = new AtendenteViewModel();

            try
            {
                conn.Open();

                return View(new AtendenteViewModel { });
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

        [HttpPost]
        public IActionResult PostAtendente([FromForm] AtendenteViewModel request)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO Attendant (AttendantName, AttendantCpf, AttendantEmail, AttendantPass) VALUES(@AttendantName, @AttendantCpf, @AttendantEmail, @AttendantPass)", conn))
                {

                    cmd.Parameters.AddWithValue("@AttendantName", request.AttendantName);
                    cmd.Parameters.AddWithValue("@AttendantCpf", request.AttendantCpf);
                    cmd.Parameters.AddWithValue("@AttendantEmail", request.AttendantEmail);
                    cmd.Parameters.AddWithValue("@AttendantPass", request.AttendantPass);

                    cmd.ExecuteNonQuery();

                }

                return RedirectToAction("CadastrarAtendente");

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

        public IActionResult CadastrarResponsavel()
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            ResponsavelViewModel ResponsavelViewModel = new ResponsavelViewModel();

            try
            {
                conn.Open();

                return View(new ResponsavelViewModel { });
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

        [HttpPost]
        public IActionResult PostResponsavel([FromForm] ResponsavelViewModel request)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO User (UserName, UserCpf, UserEmail) VALUES(@UserName, @UserCpf, @UserEmail)", conn))
                {

                    cmd.Parameters.AddWithValue("@UserName", request.ResponsavelName);
                    cmd.Parameters.AddWithValue("@UserCpf", request.ResponsavelCpf);
                    cmd.Parameters.AddWithValue("@UserEmail", request.ResponsavelEmail);

                    cmd.ExecuteNonQuery();

                }

                return RedirectToAction("CadastrarResponsavel");

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

        public IActionResult BuscarCpf()
        {
            return View();

        }

        public IActionResult Busca([FromForm] string ResponsavelCpf)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            ResponsavelViewModel Responsavel = new ResponsavelViewModel();


            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT User.UserID, User.UserName, User.UserCpf, User.UserEmail, Dependent.DependentID, Dependent.DependentName, Dependent.DependentBirth, Dependent.DependentBlood, Dependent.DependentAllergy, Dependent.DependentSusNo FROM User left JOIN Dependent ON (User.UserID = Dependent.User_UserID) WHERE User.UserCpf LIKE '%" + ResponsavelCpf + "%'", conn))
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {                        
                        Responsavel.ResponsavelID = dataReader.GetInt32(0);
                        Responsavel.ResponsavelName = dataReader.GetString(1);
                        Responsavel.ResponsavelCpf = dataReader.GetString(2);
                        Responsavel.ResponsavelEmail = dataReader.GetString(3);

                        Responsavel.Dependentes = new List<DependenteViewModel>();
                        
                        if (dataReader.IsDBNull(4))
                        {
                            return RedirectToAction("CadastrarDependente", new {id = Responsavel.ResponsavelID });
                        }
                        else
                        {
                            Responsavel.Dependentes.Add(new DependenteViewModel
                            {
                                ResponsavelID = dataReader.GetInt32(0),
                                DependentID = dataReader.GetInt32(4),
                                DependentName = dataReader.GetString(5),
                                DependentBirth = dataReader.GetDateTime(6),
                                DependentBlood = dataReader.GetString(7),
                                DependentAllergy = dataReader.GetString(8),
                                DependentSus = dataReader.GetString(9)
                            });                            
                        }                        
                    }
                }

                if (Responsavel.ResponsavelCpf == ResponsavelCpf)
                {   
                    if (Responsavel.Dependentes != null)
                    {
                        return RedirectToAction("CadastrarVacina", new { id = Responsavel.ResponsavelID });
                    }                   
                }

                return RedirectToAction("CadastrarResponsavel");
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

        public IActionResult CadastrarDependente(int id)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            DependenteViewModel DependenteViewModel = new DependenteViewModel();
            ResponsavelViewModel Responsavel = new ResponsavelViewModel();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT UserID, UserName, UserCpf, UserEmail FROM User WHERE UserID =" + id, conn))
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        Responsavel.ResponsavelID = dataReader.GetInt32(0);
                        Responsavel.ResponsavelName = dataReader.GetString(1);
                        Responsavel.ResponsavelCpf = dataReader.GetString(2);
                        Responsavel.ResponsavelEmail = dataReader.GetString(3);
                    }
                }

                ViewData["ResponsavelID"] = Responsavel;

                return View(new DependenteViewModel { });

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

        [HttpPost]
        public IActionResult PostDependente([FromForm] DependenteViewModel request)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("INSERT INTO Dependent (DependentName, DependentBirth, DependentBlood, DependentAllergy, DependentSusNo, User_UserID) VALUES(@DependentName, @DependentBirth, @DependentBlood, @DependentAllergy, @DependentSusNo, @User_UserID)", conn))
                {
                    cmd.Parameters.AddWithValue("@DependentName", request.DependentName);
                    cmd.Parameters.AddWithValue("@DependentBirth", request.DependentBirth);
                    cmd.Parameters.AddWithValue("@DependentBlood", request.DependentBlood);
                    cmd.Parameters.AddWithValue("@DependentAllergy", request.DependentAllergy);
                    cmd.Parameters.AddWithValue("@DependentSusNo", request.DependentSus);
                    cmd.Parameters.AddWithValue("@User_UserID", request.ResponsavelID);

                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("BuscarCpf");

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


        public IActionResult CadastrarVacina(int id)
        {
            MySqlConnection conn = new MySqlConnection(_appSettings.ConnectionString);
            ResponsavelViewModel Responsavel = null;
            List<VacinaViewModel> Vacinas = new List<VacinaViewModel>();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT User.UserID, User.UserName, User.UserCpf, User.UserEmail, Dependent.DependentID, Dependent.DependentName, Dependent.DependentBirth, Dependent.DependentBlood, Dependent.DependentAllergy, Dependent.DependentSusNo FROM User left JOIN Dependent ON (User.UserID = Dependent.User_UserID) WHERE User.UserID =" + id, conn))
                {
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        if(Responsavel == null)
                        {
                            Responsavel = new ResponsavelViewModel
                            {
                                ResponsavelID = dataReader.GetInt32(0),
                                ResponsavelName = dataReader.GetString(1),
                                ResponsavelCpf = dataReader.GetString(2),
                                ResponsavelEmail = dataReader.GetString(3)
                            };                            
                            Responsavel.Dependentes = new List<DependenteViewModel>();
                        }                    
                        Responsavel.Dependentes.Add(new DependenteViewModel
                        {
                            ResponsavelID = dataReader.GetInt32(0),
                            DependentID = dataReader.GetInt32(4),
                            DependentName = dataReader.GetString(5),
                            DependentBirth = dataReader.GetDateTime(6),
                            DependentBlood = dataReader.GetString(7),
                            DependentAllergy = dataReader.GetString(8),
                            DependentSus = dataReader.GetString(9)
                        });
                    }
                }

                using (MySqlCommand cmd2 = new MySqlCommand("SELECT Vaccine.VaccineID, Vaccine.VaccineName FROM Vaccine", conn))
                {
                    MySqlDataReader dataReader = cmd2.ExecuteReader();
                    while (dataReader.Read())
                    {

                        Vacinas.Add(new VacinaViewModel
                        {
                            VacinaId = dataReader.GetInt32(0),
                            VacinaName = dataReader.GetString(1)
                        });

                    }
                }

                ViewData["ResponsavelID"] = Responsavel;
                ViewData["Vacinas"] = Vacinas;

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
    }
}