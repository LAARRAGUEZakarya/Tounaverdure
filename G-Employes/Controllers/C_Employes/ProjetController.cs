
using G_Employes.Data;
using GestionEmployes.Models.repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GestionEmployes.Controllers
{
    public class ProjetController : Controller
    {
        private readonly G_EmployesDbContext gestionEmployeContext;
        public ProjetController(G_EmployesDbContext gestionEmployeContext)
        {
            this.gestionEmployeContext = gestionEmployeContext;
        }
        // GET: ProjetController
        public ActionResult Index()
        {
            var pro = gestionEmployeContext.projet.Include(x=>x.Equipes).ToList();
            return View(pro);
        }

        // GET: ProjetController/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.ListEquipe = ListEquipe();
            return View(gestionEmployeContext.projet.Find(id));
        }

        public ActionResult DetailsPro(int id)
        {
            ViewBag.ListEquipe = ListEquipe();
            return View(gestionEmployeContext.projet.Find(id));
        }
        List<Equipe> ListEquipe()
        {
            var employe = gestionEmployeContext.equipe.ToList();
            employe.Insert(0, new Equipe { Id = -1, Nom_equipe = "-- List Des Equipe--" });

            return employe;
        }

        // GET: ProjetController/Create
        public ActionResult Create()
        {
            ViewBag.ListEquipe = ListEquipe();
            return View();
        }

        public JsonResult GetEQ(string search)
        {
            var equi = gestionEmployeContext.equipe.Where(x => x.Projet == null).ToList();



            if (search != null)
            {

                equi = gestionEmployeContext.equipe.Where(x => x.Nom_equipe.Contains(search)).ToList();
            }

            var modifidata = equi.Select(x => new
            {
                id = x.Id,
                text = x.Nom_equipe


            });
            return Json(modifidata);
        }

        //create projet

        public JsonResult CreatP(Projet projet, string Equipes)
        {


            



            var newprj = new Projet
            {
                Nom_projet = projet.Nom_projet.ToUpper(),
                Date_debut = projet.Date_debut,
                Date_fin = projet.Date_fin,
                Etat = projet.Etat,

               
            };

            foreach (var item in gestionEmployeContext.projet.ToList())
            {
                if (item.Nom_projet == newprj.Nom_projet) return Json("projet deja existe");
            }
            gestionEmployeContext.projet.Add(newprj);
            gestionEmployeContext.SaveChanges();

            if (Equipes != null)
            {
                if (Equipes != "-1")
                {
                    string[] tokens = Equipes.Split(',');
                    foreach (var token in tokens)
                    {
                        var equi = gestionEmployeContext.equipe.Find(int.Parse(token));
                        equi.Projet = newprj;
                        gestionEmployeContext.equipe.Update(equi);
                        gestionEmployeContext.SaveChanges();
                    }
                }
            }




            return Json("xx");

        }
        //edite projet
        public JsonResult EditeP(Projet projet, string Equipes)
        {


            var newpr = new Projet
            {
                Id = projet.Id,
                Nom_projet = projet.Nom_projet.ToUpper(),
                Date_debut = projet.Date_debut,
                Date_fin = projet.Date_fin,
                Etat = projet.Etat,

            };


            gestionEmployeContext.projet.Update(newpr);

            gestionEmployeContext.SaveChanges();

            foreach (var item in gestionEmployeContext.equipe)
            {
                if (item.Projet == newpr)
                {
                    item.Projet = null;


                }
            }
            if (Equipes != null)
            {
                if (Equipes != "-1")
                {


                    string[] tokens = Equipes.Split(',');
                    foreach (var token in tokens)
                    {
                        var eqi = gestionEmployeContext.equipe.Find(int.Parse(token));
                        eqi.Projet = newpr;
                        gestionEmployeContext.equipe.Update(eqi);
                        gestionEmployeContext.SaveChanges();
                    }
                }

            }
            else
            {
                gestionEmployeContext.projet.Update(newpr);

                gestionEmployeContext.SaveChanges();

            }


            return Json("zz");

        }









        // POST: ProjetController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Projet projet)
        {
            try
            {
                var equipe = gestionEmployeContext.equipe.Find(projet.Equipes);
                var newProjet = new Projet
                {
                   Nom_projet = projet.Nom_projet,
                    Date_debut = projet.Date_debut,
                    Date_fin = projet.Date_fin,
                    Etat = projet.Etat,
                    Equipes = projet.Equipes ,

                };
                var check = false;
                foreach (var item in gestionEmployeContext.projet.ToList())
                {
                    if (item.Nom_projet == projet.Nom_projet) check = true;

                }
                if (check)
                {
                   
                    ViewBag.msg = "le nom de projet deja existe";
                    return View();
                }
                gestionEmployeContext.projet.Add(newProjet);
                gestionEmployeContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProjetController/Edit/5
        public ActionResult Edit(int id)
        {

            var proj = gestionEmployeContext.projet.Find(id);

            var ch = gestionEmployeContext.equipe.Where(o => o.Projet == proj).ToList();

            if (ch.Count() >= 1)
            {
                ViewBag.listequipe = ch;
            }

            return View(proj);
           
        }

        // POST: ProjetController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Projet projet)
        {
            try
            {
                gestionEmployeContext.Entry(projet).State = EntityState.Modified;
                gestionEmployeContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProjetController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(gestionEmployeContext.projet.Find(id));
        }

        // POST: ProjetController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var pro = gestionEmployeContext.projet.Find(id);

                foreach (var item in gestionEmployeContext.equipe)
                {
                    if (item.Projet == pro)
                    {
                        item.Projet = null;
                    }

                }
                gestionEmployeContext.Remove(pro);
                gestionEmployeContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
