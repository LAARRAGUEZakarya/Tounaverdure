using ClosedXML.Excel;
using ExcelDataReader;
using G_Employes.Data;
using GestionEmployes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Controllers.C_Employes
{
    public class DetailsPointeuseController : Controller
    {
        private readonly IGestionEmployes<DetailsPointeuse> detailsRepository;
        private readonly G_EmployesDbContext db;

        public DetailsPointeuseController(IGestionEmployes<DetailsPointeuse> detailsRepository,G_EmployesDbContext db)
        {
            this.detailsRepository = detailsRepository;
            this.db = db;
        }


        // GET: DetailsPointeuseController
        public ActionResult Index()
        {
            var path = $@"C:\\ZKTeco\\ZKT.xls";
            

            FileInfo file = new FileInfo(path);
           
            if (file.Exists)
            {
                var Datemonth = this.GetDetailsPointeuse(path);
                this.NomberHourParDay(Datemonth);
                file.Delete();
            }

            ViewData["IDEMP"] = HttpContext.Session.GetInt32("IDEmployeCon");

            ViewBag.listOveriers = db.overiers.Where(o=>o.Type!="admin");
            var DetailsPointeuse = detailsRepository.List();

            return View(DetailsPointeuse);
        }

        //fro read excel file and add all informations to DB
        private string GetDetailsPointeuse(string filename)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            int i = 0;
            var Datemonth="";
            using(var stream = System.IO.File.Open(filename,FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                 
                        while(reader.Read())
                      {
                        
                        if (i == 1)
                        {
                            Datemonth = reader.GetValue(3).ToString();

                            //
                            var checkdetails = detailsRepository.List().Where(d=>d.dateWorkCheck==DateTime.Parse(Datemonth));
                         

                            if(checkdetails.Count()==0)
                            {
                                        var detailspointeuse = new DetailsPointeuse
                                        {

                                            IdEmploye = int.Parse((string)reader.GetValue(2)),
                                            Name = (string)reader.GetValue(1),
                                            dateWorkCheck = DateTime.Parse(Datemonth),
                                            Status = (string)reader.GetValue(4),
                                            Etat = false,
                             
                                        };

                                //add details pointeuse to db:::::::::::
                                detailsRepository.Add(detailspointeuse);
                            }
                           





                           
                         
                        }
                        i = 1;
                      
                      }



                }
            }
            return Datemonth;
        }



        //for delete the excel file from the folder selected:::::
        //private void deleteFileExcel(string filename)
        //{
        //    System.IO.File.Delete(filename);
        //}

        
        
        //for calculate the nomber of hours get work every one::::::
        private void NomberHourParDay(string Datemonth)
        {
            var date = Datemonth.Split('/');
            var dateyear = date[2].Split(' ');
            int id_Employe=0 ,i,j ,daycheck=0       , Day , lastDay=int.Parse(date[0]);

            TimeSpan NbrHours;



            for (Day = 1; Day <= lastDay; Day++)
            {
                var listDetaiPerDay = detailsRepository.List().Where(c => c.dateWorkCheck.Month == int.Parse(date[1]) && c.dateWorkCheck.Year== int.Parse(dateyear[0]) && c.dateWorkCheck.Day == Day);


                foreach (var item in listDetaiPerDay)
                {
                    
                    if(item.IdEmploye != id_Employe || Day!=daycheck )
                    {
                        daycheck = item.dateWorkCheck.Day;
                         id_Employe = item.IdEmploye;

                         List<DetailsPointeuse> listDetailParIdEmploye = listDetaiPerDay.Where(c => c.IdEmploye == item.IdEmploye).OrderBy(c => c.dateWorkCheck).ToList();

                        for( i=0;i<=listDetailParIdEmploye.Count()-1;i++)
                        {
                           
                            if (listDetailParIdEmploye[i].Status == "C/Out" && !listDetailParIdEmploye[i].Etat)
                            {
                                for (j = i-1; j >= 0; j--)
                                {

                                    if (listDetailParIdEmploye[j].Status == "C/In" && !listDetailParIdEmploye[j].Etat)
                                    {

                                        NbrHours = listDetailParIdEmploye[i].NbrHoursParJour.Add(listDetailParIdEmploye[i].dateWorkCheck.TimeOfDay.Subtract(listDetailParIdEmploye[j].dateWorkCheck.TimeOfDay));

                                         foreach (var updateEmp in listDetailParIdEmploye)
                                        {
                                            TimeSpan timeofintowork = new TimeSpan(8, 00, 00);
                                            if (updateEmp.TimeOfIN ==TimeSpan.Parse("00:00:00"))
                                            {
                                                updateEmp.TimeOfIN = listDetailParIdEmploye[j].dateWorkCheck.TimeOfDay;
                                              
                                                updateEmp.TimeOfInAugDid = updateEmp.TimeOfIN.Subtract( timeofintowork).ToString();
                                                if (updateEmp.TimeOfIN < timeofintowork) updateEmp.etatTimeofin = "V";
                                                else updateEmp.etatTimeofin = "NVIn";


                                               
                                            } 

                                            updateEmp.TimeOfOut = listDetailParIdEmploye[i].dateWorkCheck.TimeOfDay;
                                            TimeSpan timeofouttowork = new TimeSpan(18, 00, 00);
                                            updateEmp.TimeOfOutAugDid = timeofouttowork.Subtract(updateEmp.TimeOfOut).ToString();

                                         


                                            if (updateEmp.TimeOfOut > timeofouttowork)
                                            {
                                                if (updateEmp.TimeOfIN > timeofintowork)
                                                {
                                                    updateEmp.etatTimeofin = "NVIn";
                                                }
                                                else
                                                    updateEmp.etatTimeofin = "V";
                                            }
                                            else
                                            {
                                                if (updateEmp.TimeOfIN > timeofintowork)
                                                {
                                                    updateEmp.etatTimeofin = "NVInOut";
                                                }
                                                else
                                                    updateEmp.etatTimeofin = "NVOut";
                                            }
                                                

                                            updateEmp.NbrHoursParJour = NbrHours;
                                            detailsRepository.Update(item.Id, updateEmp);


                                        }

                                        listDetailParIdEmploye[i].Etat = true;
                                        detailsRepository.Update(0, listDetailParIdEmploye[i]);

                                        listDetailParIdEmploye[j].Etat = true;
                                        detailsRepository.Update(0, listDetailParIdEmploye[j]);
                                        break;

                                    }
                                    


                                }



                        }
                    }

                    
                    }
            }





        }

            for (Day = 1; Day <= lastDay; Day++)
            {
                var listDetaiPermonth = detailsRepository.List().Where(c => c.dateWorkCheck.Month == int.Parse(date[1]) && c.dateWorkCheck.Year == int.Parse(dateyear[0]) && c.dateWorkCheck.Day == Day);

                foreach (var item in listDetaiPermonth)
                {
                    
                    if (item.IdEmploye != id_Employe || Day != daycheck)
                    {
                        if (item.IdEmploye != id_Employe) 
                            
                        daycheck = item.dateWorkCheck.Day;
                        id_Employe = item.IdEmploye;

                        var listDetailParIdEmploye = listDetaiPermonth.Where(c => c.IdEmploye == item.IdEmploye  ).OrderBy(c => c.dateWorkCheck).ToList();

                     
                        foreach (var emp in detailsRepository.List().Where(c => c.IdEmploye == item.IdEmploye && c.dateWorkCheck.Month == int.Parse(date[1]) && c.dateWorkCheck.Year == int.Parse(dateyear[0])).OrderBy(c => c.dateWorkCheck))
                        {
                            if(item.TimeOfOutAugDid!=null)
                            {
                                if(emp.SumTimeOfInAugDid==null)
                                {
                                    emp.SumTimeOfInAugDid = item.TimeOfInAugDid;
                                    emp.SumTimeOfOutAugDid = item.TimeOfOutAugDid;
                                }
                                else
                                {
                                    emp.SumTimeOfInAugDid = (TimeSpan.Parse(emp.SumTimeOfInAugDid).Add(TimeSpan.Parse(item.TimeOfInAugDid))).ToString();
                                    emp.SumTimeOfOutAugDid = (TimeSpan.Parse(emp.SumTimeOfOutAugDid).Add(TimeSpan.Parse(item.TimeOfOutAugDid))).ToString();

                                }


                               
                            }
                          

                            emp.NbrHoursParMois += item.NbrHoursParJour;

                            detailsRepository.Update(emp.Id, emp);
                        }
                    }
                }

            }


        }

        public ActionResult Export()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Details");
                var cell = 1;
                worksheet.Cell(cell, 1).Value = "ID";
                worksheet.Cell(cell, 2).Value = "Full Name";
                worksheet.Cell(cell, 3).Value = "Nombre d'heure par jour";
                worksheet.Cell(cell, 4).Value = "Nombre d'heure par mois";
                worksheet.Cell(cell, 4).Value = "Temp d'entree";
                worksheet.Cell(cell, 4).Value = "Rest";
                worksheet.Cell(cell, 4).Value = "Temp de sortee";
                worksheet.Cell(cell, 4).Value = "Rest";
                foreach (var item in detailsRepository.List().Where(d=>d.dateWorkCheck.Month==DateTime.Now.Month && d.dateWorkCheck.Year==DateTime.Now.Year))
                {
                    cell++;
                    worksheet.Cell(cell, 1).Value = item.IdEmploye;
                    worksheet.Cell(cell, 2).Value = item.Name;
                    worksheet.Cell(cell, 3).Value = item.NbrHoursParJour;
                    worksheet.Cell(cell, 4).Value = item.NbrHoursParMois;
                    worksheet.Cell(cell, 5).Value = item.TimeOfIN;
                    worksheet.Cell(cell, 6).Value = item.TimeOfInAugDid;
                    worksheet.Cell(cell, 7).Value = item.TimeOfOut;
                    worksheet.Cell(cell, 8).Value = item.TimeOfOutAugDid;

                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "Application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Details de Pointeuse.xls");
                }
            }
        }

        public JsonResult AficherNbrHoursParMois(List<int> dates)
        {
            try
            {

                int mois = dates[0], annee = dates[1];
                if(dates.Count==3)
                {
                   var listDetaiPermonth = detailsRepository.List().
                        Where(c => c.dateWorkCheck.Month == mois && c.dateWorkCheck.Year == annee && c.IdEmploye==dates[2])
                        .Select(c => new { c.IdEmploye, c.Name,timenbrhour = (c.NbrHoursParMois.Hours + " : " + c.NbrHoursParMois.Minutes),c.SumTimeOfInAugDid,c.SumTimeOfOutAugDid })
                        .Distinct().ToList();

                    return Json(listDetaiPermonth);
                }
                else
                {
                    var listDetaiPermonth = detailsRepository.List()
                        .Where(c => c.dateWorkCheck.Month == mois && c.dateWorkCheck.Year == annee)
                        .Select(c => new { c.IdEmploye ,c.Name, timenbrhour = (c.NbrHoursParMois.Hours + " : " + c.NbrHoursParMois.Minutes), c.SumTimeOfInAugDid, c.SumTimeOfOutAugDid})
                        .Distinct().ToList();

                    return Json(listDetaiPermonth);
                }

                
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
           
        }

        public JsonResult AficherNbrHoursParjour(List<int> dates)
        {
            try
            {

                int mois = dates[1], annee = dates[2],jour=dates[0];
                if (dates.Count == 4)
                {
                    var listDetaiPerJour = detailsRepository.List()
                        .Where(c => c.dateWorkCheck.Day == jour && c.dateWorkCheck.Month == mois 
                        && c.dateWorkCheck.Year == annee && c.IdEmploye == dates[3])

                        .Select(c => new { c.IdEmploye, c.Name, timenbrhour = (c.NbrHoursParJour.Hours + " : " + c.NbrHoursParJour.Minutes + " : " + c.NbrHoursParJour.Seconds),timeofin =(c.TimeOfIN.Hours+":"+c.TimeOfIN.Minutes+":"+c.TimeOfIN.Seconds), c.TimeOfInAugDid ,timeofout=(c.TimeOfOut.Hours+":"+c.TimeOfOut.Minutes+":"+c.TimeOfOut.Seconds),c.TimeOfOutAugDid,c.etatTimeofin})
                        .Distinct().ToList();

                    return Json(listDetaiPerJour);
                }
                else
                {
                    var listDetaiPerJour = detailsRepository.List()
                        .Where(c => c.dateWorkCheck.Day == jour && c.dateWorkCheck.Month == mois && c.dateWorkCheck.Year == annee)
                        .Select(c => new { c.IdEmploye, c.Name, timenbrhour = (c.NbrHoursParJour.Hours + " : " + c.NbrHoursParJour.Minutes + " : " + c.NbrHoursParJour.Seconds), timeofin = (c.TimeOfIN.Hours + ":" + c.TimeOfIN.Minutes + ":" + c.TimeOfIN.Seconds), c.TimeOfInAugDid , timeofout = (c.TimeOfOut.Hours + ":" + c.TimeOfOut.Minutes + ":" + c.TimeOfOut.Seconds), c.TimeOfOutAugDid,c.etatTimeofin})
                        .Distinct().ToList();

                    return Json(listDetaiPerJour);
                }


            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }



        // GET: DetailsPointeuseController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DetailsPointeuseController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DetailsPointeuseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DetailsPointeuseController/Edit/5
        public ActionResult EditMois(int id,int month,int year)
        {
            var details = new DetailsPointeuse();
            foreach(var det in detailsRepository.List())
            {
                if (det.IdEmploye == id  && det.dateWorkCheck.Month == month && det.dateWorkCheck.Year == year)
                    details = det;
            }
          
            return View(details);
        }

        //metho for get view
        public ActionResult EditJour(int id, int day, int month, int year)
        {
            var details = new DetailsPointeuse();
            foreach (var det in detailsRepository.List())
            {
                if (det.IdEmploye == id && det.dateWorkCheck.Day == day && det.dateWorkCheck.Month == month && det.dateWorkCheck.Year == year)
                    details = det;
            }

            return View(details);
        }

        // POST: DetailsPointeuseController/Edit/5

        public JsonResult EditDetailsMois(int id,DateTime date,TimeSpan nbrHMois,TimeSpan sumin,TimeSpan sumout)
        {
            try
            {
                var details = new DetailsPointeuse();

               
                foreach (var det in detailsRepository.List().Where(d => d.IdEmploye == id && d.dateWorkCheck.Month == date.Month && d.dateWorkCheck.Year == date.Year))
                {
                    det.NbrHoursParMois = nbrHMois;
                    det.SumTimeOfInAugDid = sumin.ToString();
                    det.SumTimeOfOutAugDid = sumout.ToString();
                    detailsRepository.Update(id, det);
                }
                return Json("Success");
            }
            catch
            {
                return Json("Error");
            }
        }
        public JsonResult EditDetailsJour(int id, DateTime date, TimeSpan nbrHJour)
        {
            try
            {
                var details = new DetailsPointeuse();


                foreach (var det in detailsRepository.List().Where(d => d.IdEmploye == id && d.dateWorkCheck.Month == date.Month && d.dateWorkCheck.Day==date.Day && d.dateWorkCheck.Year == date.Year))
                {
                    det.NbrHoursParJour = nbrHJour;
                    detailsRepository.Update(id, det);
                }
                return Json("Success");
            }
            catch
            {
                return Json("Error");
            }
        }
        public ActionResult DeleteDetails(List<int> listid, int day, int month, int year)
        {


            foreach (var item in listid)
            {

                foreach (var det in detailsRepository.List().Where(d => d.IdEmploye == item && d.dateWorkCheck.Month == month && d.dateWorkCheck.Day == day && d.dateWorkCheck.Year == year))
                {
                    det.NbrHoursParJour = TimeSpan.Parse("00:00:00");
                    detailsRepository.Update(item, det);
                }
                foreach (var det in detailsRepository.List().Where(d => d.IdEmploye == item && d.dateWorkCheck.Month == month && d.dateWorkCheck.Year == year))
                {
                    det.NbrHoursParMois = TimeSpan.Parse("00:00:00"); ;
                    detailsRepository.Update(item, det);
                }

               
            }

            return RedirectToAction(nameof(Index));

        }
    }
}
