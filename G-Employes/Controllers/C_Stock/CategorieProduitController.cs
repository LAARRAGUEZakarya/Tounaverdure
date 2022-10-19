using G_Employes;
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
        private readonly IGestionEmployes<Produit> produitRepository;

        public CategorieProduitController(IGestionEmployes<CategorieProduit> gestionEmployesRepository, IGestionEmployes<Produit> produitRepository)
        {
            this.gestionEmployesRepository = gestionEmployesRepository;
            this.produitRepository = produitRepository;
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
           
                gestionEmployesRepository.Add(categorie);
                return RedirectToAction(nameof(Index));
             
        }

        //public JsonResult CreateJ(CategorieProduit categorie)
        //{
        //    try
        //    {
               
        //            gestionEmployesRepository.Add(categorie);
        //            return Json("done");
               
                
        //    }
        //    catch
        //    {
        //        return Json("Error");
        //    }
        //}

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
                if (ModelState.IsValid)
                {
                    gestionEmployesRepository.Update(id, categorie);
                    return RedirectToAction(nameof(Index));
                }
                var categorien = gestionEmployesRepository.Find(id);

                return View(categorien);

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
                var cat = gestionEmployesRepository.Find(id);
                foreach(var item in produitRepository.List())
                {
                    if (item.Categorie == cat)
                        produitRepository.Delete(item.Id);
                }
                gestionEmployesRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public void DeleteCAt(int id)
        {
            try
            {
                var cat = gestionEmployesRepository.Find(id);
                foreach (var item in produitRepository.List())
                {
                    if (item.Categorie == cat)
                        produitRepository.Delete(item.Id);
                }

                gestionEmployesRepository.Delete(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DeleteAllCAt(List<int> listid)
        {
            try
            {
      
                foreach (var item in listid)
                {

                    gestionEmployesRepository.Delete(item);
                }

          
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
