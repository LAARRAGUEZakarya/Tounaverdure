using GestionEmployes.Models;
using GestionEmployes.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace GestionEmployes.Controllers.C_Stock
{
    public class ProduitController : Controller
    {
        private readonly IGestionEmployes<Produit> proditRepository;
        private readonly IGestionEmployes<CategorieProduit> categorieRepository;
        [Obsolete]
        private readonly IHostingEnvironment hosting;

        [Obsolete]
        public ProduitController(IGestionEmployes<Produit> proditRepository,IGestionEmployes<CategorieProduit> categorieRepository, IHostingEnvironment hosting)
        {
            this.proditRepository = proditRepository;
            this.categorieRepository = categorieRepository;
            this.hosting = hosting;
        }
        // GET: ProduitController
        public ActionResult Index()
        {
            var produits = proditRepository.List();
            return View(produits);
        }

        // GET: ProduitController/Details/5
        public ActionResult Details(int id)
        {
            var produit = proditRepository.Find(id);
            return View(produit);
        }

        // GET: ProduitController/Create
        public ActionResult Create()
        {

            return View(GetallCategories());
        }

        // POST: ProduitController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public ActionResult Create(CategorieProduitViewModel model)
        {
            try
            {


                //pour ajouter l'image a la table detail a partir detailproduitviewmodel ::
                string filename = string.Empty;
                if (model.File != null)
                {
                    string Upload = Path.Combine(hosting.WebRootPath, "ImagesProducts");
                    filename = model.File.FileName;
                    string filepath = Path.Combine(Upload, filename);
                    model.File.CopyTo(new FileStream(filepath, FileMode.Create));
                }


                //pour verifier est ce que l'utilisateur selectione une categorie
                var categorieExist = categorieRepository.Find(model.CategorieId);
                if (model.CategorieId == 0 || categorieExist == null)
                {
                    ViewBag.Message = "selectionner un Categorie dans la list!";

                    return View(GetallCategories());
                }
                else
                {


                var categorieProduit = categorieRepository.Find(model.CategorieId);
                var produit = new Produit
                {
                    Id = model.produitId,
                    Desgination = model.Desgination,
                    Prix = model.Prix,
                    Quantite = model.Quantite,
                    ImageUrl = filename,
                    Categorie = categorieProduit
                };

               
                proditRepository.Add(produit);  
                    return RedirectToAction(nameof(Index));
                }


                


                
                //var detail = new Details
                //{
                //    ImageUrl = filename,
                //    produit = produit
                //};
                //detailRepository.Add(detail);
            
            }
            catch
            {
                return View();
            }
        }


   
        // GET: ProduitController/Edit/5
        public ActionResult Edit(int id)
        {
            var produit = proditRepository.Find(id);
        


            var model = new CategorieProduitViewModel {
                produitId = produit.Id,
                Desgination = produit.Desgination,
                Prix = produit.Prix,
                Quantite = produit.Quantite,
                ImageUrl = produit.ImageUrl,
                CategorieId =produit.Categorie.Id,  
                Categories = categorieRepository.List().ToList()
            };

            return View(model);
        }

        // POST: ProduitController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public ActionResult Edit(int id,CategorieProduitViewModel model)
        {
            try
            {

                string filename = string.Empty; 
                string oldFilename =model.ImageUrl;
                if (model.File != null)
                {
                    string Upload = Path.Combine(hosting.WebRootPath, "ImagesProducts");
                    filename = model.File.FileName;

                    string filepath = Path.Combine(Upload, filename);
                    //pour modifier une image il faut le supp
                    //Supprimer l'acienne image 
                 
                    string fullpathold = oldFilename==null?null : Path.Combine(Upload, oldFilename);

                     if (filepath != fullpathold )
                     {
                        if(fullpathold != null)
                        {
                            System.IO.File.Delete(fullpathold);
                            //pour enregistrer l'image
                            model.File.CopyTo(new FileStream(filepath, FileMode.Create));
                        }
                        else
                        model.File.CopyTo(new FileStream(filepath, FileMode.Create));


                    }
                     

                 
                }

                
                var Categorie = categorieRepository.Find(model.CategorieId);
               
                var nvProduit = new Produit
                {
                    Id = id,
                    Desgination = model.Desgination,
                    Prix = model.Prix,
                    Quantite = model.Quantite,
                    ImageUrl = filename==""? oldFilename:filename,
                    Categorie = Categorie
                };

                proditRepository.Update(id, nvProduit);

              
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProduitController/Delete/5
        public ActionResult Delete(int id)
        {
            var produit = proditRepository.Find(id);
            return View(produit);
        }

        // POST: ProduitController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                proditRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        CategorieProduitViewModel GetallCategories()
        {
            var model = new CategorieProduitViewModel
            {
                Categories = categorieRepository.List().ToList()

            };

            return model;
        }
    }
}
