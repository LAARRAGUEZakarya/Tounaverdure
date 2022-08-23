using GestionEmployes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Controllers.C_Stock
{
    public class CategorieProduitController : Controller
    {
        private readonly IGestionEmployes<CategorieProduit> gestionEmployesRepository;

        public CategorieProduitController(IGestionEmployes<CategorieProduit> gestionEmployesRepository)
        {
            this.gestionEmployesRepository = gestionEmployesRepository;
        }
        // GET: CategorieProduitController
        public ActionResult Index()
        {
            var categories = gestionEmployesRepository.List();
            return View(categories);
        }

        // GET: CategorieProduitController/Details/5
        public ActionResult Details(int id)
        {
            var categorie = gestionEmployesRepository.Find(id);
            return View(categorie);
        }

        // GET: CategorieProduitController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategorieProduitController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategorieProduit categorie)
        {
            try
            {
                gestionEmployesRepository.Add(categorie);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategorieProduitController/Edit/5
        public ActionResult Edit(int id)
        {
            var categorie = gestionEmployesRepository.Find(id);

            return View(categorie);
        }

        // POST: CategorieProduitController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CategorieProduit categorie)
        {
            try
            {
                gestionEmployesRepository.Update(id, categorie);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategorieProduitController/Delete/5
        public ActionResult Delete(int id)
        {
            var categorie = gestionEmployesRepository.Find(id);
            return View(categorie);
        }

        // POST: CategorieProduitController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                gestionEmployesRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
