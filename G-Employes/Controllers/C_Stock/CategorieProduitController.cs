using G_Employes;
using G_Employes.Data;
using GestionEmployes.Models;
using GestionEmployes.Models.G_Stock;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Controllers.C_Stock
{
    public class CategorieProduitController : Controller
    {
        
        private readonly IGestionEmployes<Commande> commandeRepository;
        private readonly IGestionEmployes<CategorieProduit> gestionEmployesRepository;
        private readonly IGestionEmployes<Produit> produitRepository;
        private readonly IGestionEmployes<Operations> operationRepository;
        private readonly G_EmployesDbContext db;
        [Obsolete]
        private readonly IHostingEnvironment hosting;

      

        [Obsolete]
        public CategorieProduitController( IGestionEmployes<CategorieProduit> gestionEmployesRepository, IHostingEnvironment hosting, IGestionEmployes<Commande> commandeRepository, G_EmployesDbContext db, IGestionEmployes<Operations> operationRepository, IGestionEmployes<Produit> produitRepository)
        {
            
            this.gestionEmployesRepository = gestionEmployesRepository;
            this.produitRepository = produitRepository;
            this.operationRepository = operationRepository;
            this.commandeRepository = commandeRepository;
            this.commandeRepository = commandeRepository;
            this.hosting = hosting;
            this.db = db;
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
     
        public JsonResult CreateC(CategorieProduit categorie)
        {
           
                gestionEmployesRepository.Add(categorie);
                return Json("done");
             
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
       
        public JsonResult EditC(int id, CategorieProduit categorie)
        {
            
                if (ModelState.IsValid)
                {
                    gestionEmployesRepository.Update(id, categorie);
                    
                   
                }
            return Json("done");
        }

        // GET: CategorieProduitController/Delete/5
        public ActionResult Delete(int id)
        {
            var categorie = gestionEmployesRepository.Find(id);
            return View(categorie);
        }

        // POST: CategorieProduitController/Delete/5
        public void CreateOperationApresAddProduct(Produit produit, string status)
        {
            var overier = db.overiers.Find(HttpContext.Session.GetInt32("IDEmployeCon"));
            var operation = new Operations
            {
                Qtt_Diduir = 0,
                Qtt_Augmenter = produit.Quantite,
                Date_operation = DateTime.Now,
                typeUnite = produit.TypeUnite,
                Disgination = produit.Desgination,
                Qtt = produit.Quantite,
                Prix = produit.Prix,
                FullNameChef = overier.Nom.ToUpper() + ' ' + overier.Prenom,
                Email = User.Identity.Name,
                imageUrl = overier.Image,
                CIN = overier.CIN,
                Type = overier.Type,
                Status = status,


            };

            operationRepository.Add(operation);
        }
        public void CheckcommandeoperationRelation(int id)
        {
            var produit = produitRepository.Find(id);

            foreach (var item in commandeRepository.List())
            {
                if (item.produit == produit)
                {

                    item.produit = null;
                    //item.produit = nproduit;

                }

            }


        }
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
                    {
                        CheckcommandeoperationRelation(item.Id);
                        string status = "Supprimer";
                        CreateOperationApresAddProduct(item, status);
                        produitRepository.Delete(item.Id);


                    }

                }
                gestionEmployesRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Obsolete]
        public void DeletImg(string ovr)
        {
            string img = ovr;
            string Upload = Path.Combine(hosting.WebRootPath, "ImagesProducts");
            string fullpath = Path.Combine(Upload, img);
            if (System.IO.File.Exists(fullpath))
            {
                if (img != "product-default.png")
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    System.IO.File.Delete(fullpath);


                }
            }
        }

        [Obsolete]
        public void DeleteCAt(int id)
        {
            try
            {
                var cat = gestionEmployesRepository.Find(id);
                foreach (var item in produitRepository.List())
                {
                    if (item.Categorie == cat)
                    {
                        CheckcommandeoperationRelation(item.Id);
                        string status = "Supprimer";
                        CreateOperationApresAddProduct(item, status);
                        produitRepository.Delete(item.Id);
                        DeletImg(item.ImageUrl);
                    }
                        
                }

                gestionEmployesRepository.Delete(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Obsolete]
        public void DeleteAllCAt(List<int> listid)
        {
            try
            {
                
                foreach (var item in listid)
                {

                    foreach (var P in produitRepository.List())
                    {
                        if (P.Categorie.Id == item)
                        {
                            CheckcommandeoperationRelation(P.Id);
                            string status = "Supprimer";
                            CreateOperationApresAddProduct(P, status);
                            produitRepository.Delete(P.Id);
                            DeletImg(P.ImageUrl);


                        }

                    }

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
