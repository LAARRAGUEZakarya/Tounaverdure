using GestionEmployes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Controllers.C_Emploiyes
{
    public class CategorieOverierController : Controller
    {
        private readonly IGestionEmployes<CategorieOverier> categorieRepository;

        public CategorieOverierController(IGestionEmployes<CategorieOverier> categorieRepository)
        {
            this.categorieRepository = categorieRepository;
        }
        // GET: CategorieOverierController
        public ActionResult Index()
        {
            var categories = categorieRepository.List();
            return View(categories);
        }

        // GET: CategorieOverierController/Details/5
        public ActionResult Details(int id)
        {
            var categorieD = categorieRepository.Find(id);
            return View(categorieD);
        }

        // GET: CategorieOverierController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategorieOverierController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategorieOverier categorie)
        {
            try
            {
                var NewCategorie = new CategorieOverier
                {
                    Fonctionnalite = categorie.Fonctionnalite
                };
                categorieRepository.Add(NewCategorie);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategorieOverierController/Edit/5
        public ActionResult Edit(int id)
        {
            var Categorie = categorieRepository.Find(id);

            return View(Categorie);
        }

        // POST: CategorieOverierController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CategorieOverier NewCategorie)
        {
            try
            {
                categorieRepository.Update(id, NewCategorie);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // GET: CategorieOverierController/Delete/5
        public ActionResult Delete(int id)
        {
            var categorie = categorieRepository.Find(id);
            return View(categorie);
        }

       
        // POST: CategorieOverierController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                categorieRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

    
    }
}
