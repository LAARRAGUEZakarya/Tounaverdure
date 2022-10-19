
using G_Employes.Data;
using GestionEmployes.Models;
using GestionEmployes.Models.repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GestionEmployes.Controllers
{
    public class CategorieController : Controller
    {
        private readonly G_EmployesDbContext gestionEmployeContext;
        public CategorieController(G_EmployesDbContext gestionEmployeContext)
        {
            this.gestionEmployeContext = gestionEmployeContext;
        }
        // GET: CategorieController
        public ActionResult Index()
        {
            var cat = gestionEmployeContext.categorie.Include(x => x.Employes).ToList();
            return View(cat);
        }

        // GET: CategorieController/Details/5
        public ActionResult Details(int id)
        {
            return View(gestionEmployeContext.categorie.Find(id));
        }
        List<Overier> ListEmplye()
        {
            var employe = gestionEmployeContext.overiers.ToList();
            employe.Insert(0, new Overier { Id = -1, Nom = "--select Nom--" });

            return employe;
        }

        public JsonResult GetOuvrier(string search)
        {
            var emply = gestionEmployeContext.overiers.Where(x => x.Type == "overier").Where(x => x.Categorie == null).ToList();



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



        // GET: CategorieController/Create
        public ActionResult Create()
        {
            ViewBag.Employe = ListEmplye();
            return View();
        }


        public JsonResult ouvr(Categorie categorie, string Employes)
        {

            var newcat = new Categorie
            {
                Fonctionnalite = categorie.Fonctionnalite.ToUpper(),

                
            };

            foreach (var item in gestionEmployeContext.categorie.ToList())
            {
                if (item.Fonctionnalite == newcat.Fonctionnalite) return Json("Fonctionnalite deja existe");
            }
            gestionEmployeContext.categorie.Add(newcat);
            gestionEmployeContext.SaveChanges();

            if (Employes != null)
            {
                if (Employes != "-1")
                {
                    string[] tokens = Employes.Split(',');
                    foreach (var token in tokens)
                    {
                        var empl = gestionEmployeContext.overiers.Find(int.Parse(token));
                        empl.Categorie = newcat;
                        gestionEmployeContext.overiers.Update(empl);
                        gestionEmployeContext.SaveChanges();
                    }
                }
            }




            return Json("cat");

        }



        public JsonResult Editouvr(Categorie categorie, string Employes)
        {

            var newcate = new Categorie
            {
                Id= categorie.Id,
                Fonctionnalite = categorie.Fonctionnalite.ToUpper(),


            };

          
            gestionEmployeContext.categorie.Update(newcate);
            gestionEmployeContext.SaveChanges();
            foreach (var item in gestionEmployeContext.overiers)
            {
                if (item.Categorie == newcate)
                {
                    item.Categorie = null;
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
                        empl.Categorie = newcate;
                        gestionEmployeContext.overiers.Update(empl);
                        gestionEmployeContext.SaveChanges();
                    }
                }
            }
            else
            {
                gestionEmployeContext.categorie.Update(newcate);
                gestionEmployeContext.SaveChanges();
            }




            return Json("cate");

        }

        // POST: CategorieController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categorie categorie)
        {
            try
            {
                gestionEmployeContext.categorie.Add(categorie);
                gestionEmployeContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategorieController/Edit/5
        public ActionResult Edit(int id)
        {
            var catego = gestionEmployeContext.categorie.Find(id);
            var ch = gestionEmployeContext.overiers.Where(o => o.Categorie == catego).ToList();

            if (ch.Count() >= 1)
            {
                ViewBag.liscat = ch;
            }
            return View(catego);
        }

        // POST: CategorieController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categorie categorie)
        {
            try
            {
                gestionEmployeContext.Entry(categorie).State = EntityState.Modified;
                gestionEmployeContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategorieController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(gestionEmployeContext.categorie.Find(id));
        }

        // POST: CategorieController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var cat = gestionEmployeContext.categorie.Find(id);
                foreach (var item in gestionEmployeContext.overiers)
                {
                    if (item.Categorie == cat)
                    {
                        item.Categorie = null;
                    }

                }
                gestionEmployeContext.Remove(cat);
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
