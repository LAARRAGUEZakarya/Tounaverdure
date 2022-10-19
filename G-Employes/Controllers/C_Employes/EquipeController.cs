using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using GestionEmployes.Models.repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

using G_Employes.Data;
using System;
using GestionEmployes.Models;
using Microsoft.AspNetCore.Hosting;

namespace GestionEquipe.Controllers
{
    public class EquipeController : Controller
    {
        private readonly G_EmployesDbContext gestionEmployeContext;
       

        [Obsolete]
        public EquipeController(G_EmployesDbContext gestionequipeContext)
        {
            this.gestionEmployeContext = gestionequipeContext;
            
        }
        // GET: equipeController
        public ActionResult Index()
        {
            var equipe = gestionEmployeContext.equipe.Include(x=>x.Employes).Include(x => x.Projet).ToList();
            return View(equipe);
        }

        // GET: equipeController/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.ListEquipe = 
            ViewBag.ListEmplye = ListEmplye();
            return View(gestionEmployeContext.equipe.Find(id));
        }
        List<Categorie> ListCategories()
        {
            var categories = gestionEmployeContext.categorie.ToList();
            categories.Insert(0, new Categorie { Id = -1, Fonctionnalite = "--select fonctionnalite--" });

            return categories;
        }
        public ActionResult DetailsEq(int id)
        {
            ViewBag.ListCategories = ListCategories();
            ViewBag.ListEmplye = ListEmplye();
            return View(gestionEmployeContext.equipe.Find(id));
        }
        List<Overier> ListEmplye()
        {
            var employe = gestionEmployeContext.overiers.ToList();
            employe.Insert(0, new Overier { Id = -1, Nom = "--List Des Employes--" });

            return employe;
        }
        List<Projet> ListProjet()
        {
            var projet = gestionEmployeContext.projet.ToList();
            projet.Insert(0, new Projet { Id = -1, Nom_projet = "--List Des Projets--" });

            return projet;
        }

        // GET: equipeController/Create
        public ActionResult Create()
        {
            ViewBag.ListEmplye = ListEmplye();
            ViewBag.ListProjet = ListProjet();
            return View();
        }

        //Ajouter equipe dans une projet
        public JsonResult AJprojet(string id)
        {
            List<Projet> projet = new List<Projet>();
            projet = gestionEmployeContext.projet.ToList();
            return Json(projet);
        }


        public JsonResult newProjet(string nomProjet, DateTime dateDe, DateTime dateFin, string Etat)
        {

            if (ModelState.IsValid)
            {
                var Newprojet = new Projet()
                {
                    Nom_projet = nomProjet,
                    Date_debut = dateDe,
                    Date_fin = dateFin,
                    Etat = Etat
                };
                gestionEmployeContext.projet.Add(Newprojet);
                gestionEmployeContext.SaveChanges();
                return Json(Newprojet);

            }
            else
            {
                return Json("errer");

            }



        }

        public JsonResult GetEmploye(string search )
        {
            var emply = gestionEmployeContext.overiers.Where(x => x.Equipe == null).ToList();
            
         
            
                   if (search != null)
                      { 
                
                            emply = gestionEmployeContext.overiers.Where(x => x.Nom.Contains(search)).ToList();
                      }
            
            var modifidata = emply.Select(x => new
            {
                id = x.Id,
                text = x.Prenom + " " + x.Nom 
                

            });
            return Json(modifidata);
        }

        public JsonResult saveProjet(Projet projet)
        {
            var prj = gestionEmployeContext.projet.Find(projet.Id);
            return Json(prj);
        }


        public JsonResult Empl(Equipe equipe, string Employes)
        {
          
          if(equipe.Nom_equipe==null)
            {
                return Json("input empty");

            }
            var prj = gestionEmployeContext.projet.Find(equipe.Projet.Id);
           


            var newEquipe = new Equipe
            {
                Nom_equipe = equipe.Nom_equipe.ToUpper(),

                Projet = prj,
            };
           
            foreach (var item in gestionEmployeContext.equipe.ToList())
            {
                if (item.Nom_equipe == newEquipe.Nom_equipe) return Json("equipe deja existe");
            }
            gestionEmployeContext.equipe.Add(newEquipe);
            gestionEmployeContext.SaveChanges();

            if (Employes != null)
            {
                if (Employes != "-1")
                {
                    string[] tokens = Employes.Split(',');
                    foreach (var token in tokens)
                    {
                        var empl = gestionEmployeContext.overiers.Find(int.Parse(token));
                        empl.Equipe = newEquipe;
                        gestionEmployeContext.overiers.Update(empl);
                        gestionEmployeContext.SaveChanges();
                    }
                }
            }




            return Json("xx");

        }
        public JsonResult Esiteeq(Equipe equipe, string Employes)
        {
            if (equipe.Nom_equipe == null)
            {
                return Json("input empty");

            }

            var prj = gestionEmployeContext.projet.Find(equipe.Projet.Id);

          
            var newEq = new Equipe
            {
                Id = equipe.Id,
                Nom_equipe = equipe.Nom_equipe,
                
                Projet = prj
            };


            gestionEmployeContext.equipe.Update(newEq);

            gestionEmployeContext.SaveChanges();

            foreach (var item in gestionEmployeContext.overiers)
            {
                if (item.Equipe == newEq)
                {
                    item.Equipe = null;
                   

                }
            }
            if (Employes != null)
            {
                if (Employes != "-1")
                {
                   
                    
                    string[] tokens = Employes.Split(',');
                    foreach (var token in tokens)
                    {
                        var empl = gestionEmployeContext.overiers.Find(int.Parse(token));
                        empl.Equipe = newEq;
                        gestionEmployeContext.overiers.Update(empl);
                        gestionEmployeContext.SaveChanges();
                    }
                }

            }
            else
            {
                gestionEmployeContext.equipe.Update(newEq);

                gestionEmployeContext.SaveChanges();

            }


            return Json("zz");

        }

        public string checkNom(string newEq)
        {
            var rt = "done";
            foreach (var item in gestionEmployeContext.equipe.ToList())
            {
                if (item.Nom_equipe == newEq) rt="equipe deja existe ";
            }
            return rt;
        }

        // POST: equipeController/Create

        //public JsonResult CreateE(Equipe equipe)
        //{

        //    var prj = gestionEmployeContext.projet.Find(equipe.Projet.Id);
            

        //    var newEquipe = new Equipe
        //    {
        //        Nom_equipe = equipe.Nom_equipe,
                
        //        Projet = prj,
                

        //    };
            
           
        //        gestionEmployeContext.equipe.Add(newEquipe);
        //        gestionEmployeContext.SaveChanges();
        //        return Json(newEquipe.Id);
        
        //}
   


        // GET: equipeController/Edit/5
        public ActionResult Edit(int id)
        {

            var equi = gestionEmployeContext.equipe.Find(id);

           var ch =  gestionEmployeContext.overiers.Where(o => o.Equipe == equi).ToList();
        

            if (ch.Count() >= 1 )
            {
                ViewBag.listomp = ch ;
            }
            ViewBag.ListProjet = ListProjet();
            return View(equi);
        }

     

        // GET: equipeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(gestionEmployeContext.equipe.Find(id));
        }

        // POST: equipeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {

                var equipe = gestionEmployeContext.equipe.Find(id);
                foreach (var item in gestionEmployeContext.overiers)
                {
                    if(item.Equipe==equipe)
                    {
                        item.Equipe = null;
                    }
                    
                }
                foreach (var item in gestionEmployeContext.projet)
                {
                    if(item.Equipes==equipe)
                    { 
                        item.Equipes = null;
                    }
                        
                }
                gestionEmployeContext.Remove(equipe);
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
