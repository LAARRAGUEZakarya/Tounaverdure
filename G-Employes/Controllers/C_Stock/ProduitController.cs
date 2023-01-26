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
using G_Employes.Data;
using GestionEmployes.Models.G_Stock;
using Microsoft.EntityFrameworkCore;

namespace GestionEmployes.Controllers.C_Stock
{
    public class ProduitController : Controller
    {
        private readonly G_EmployesDbContext gestionEmployeContext;
        private readonly IGestionEmployes<Produit> proditRepository;
        private readonly IGestionEmployes<CategorieProduit> categorieRepository;
        private readonly IGestionEmployes<Operations> operationRepository;
        private readonly IGestionEmployes<Commande> commandeRepository;
        [Obsolete]
        private readonly IHostingEnvironment hosting;
        private readonly G_EmployesDbContext db;

        [Obsolete]
        public ProduitController(G_EmployesDbContext gestionEmployeContext, IGestionEmployes<Produit> proditRepository,IGestionEmployes<CategorieProduit> categorieRepository, IGestionEmployes<Operations> operationRepository, IGestionEmployes<Commande> commandeRepository, IHostingEnvironment hosting,G_EmployesDbContext db)
        {
            this.gestionEmployeContext = gestionEmployeContext;
            this.proditRepository = proditRepository;
            this.categorieRepository = categorieRepository;
            this.operationRepository = operationRepository;
            this.commandeRepository = commandeRepository;
            this.hosting = hosting;
            this.db = db;
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
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [Obsolete]
        public JsonResult CreatePr(CategorieProduitViewModel model)
        {
            

               
                    //pour ajouter l'image a la table detail a partir detailproduitviewmodel ::
                    string  newFileName = "product-default.png", newName= GenereFilename();
                
                    
                    if (model.File != null)
                    {
                        string Upload = Path.Combine(hosting.WebRootPath, "ImagesProducts");
                        string extension = Path.GetExtension(model.File.FileName);
                        newFileName = newName.Trim() + extension.ToLower();
                       
                        string filepath = Path.Combine(Upload, newFileName);
                        model.File.CopyTo(new FileStream(filepath, FileMode.Create));
                    }


                    //pour verifier est ce que l'utilisateur selectione une categorie
                    var categorieExist = categorieRepository.Find(model.CategorieId);
                 
                     if (model.CategorieId == 0 || categorieExist == null)
                    {
                        

                        return Json("selectionner un Categorie dans la list!");
                    }
                    else if (model.Quantite < 1)
                    {
                        
                        return Json("La quantité doit être supérieure à 0");
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
                            ImageUrl = newFileName,
                            TypeUnite= model.typeUnite,
                            Categorie = categorieProduit
                        };


                        proditRepository.Add(produit);
                    string status = "L'ajout";
                    CreateOperationApresAddProduct(produit, status);
                    return Json("done");
                     

                }
              

        }


        public string GenereFilename()
        {
           
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var newFileName = new String(stringChars);
            return newFileName;
        }



        public void CreateOperationApresAddProduct(Produit produit,string status)
        {
            var overier = db.overiers.Find(HttpContext.Session.GetInt32("IDEmployeCon")); 
             var operation = new Operations
            {
                Qtt_Diduir = 0,
                Qtt_Augmenter = produit.Quantite ,
                Date_operation = DateTime.Now,
                typeUnite=produit.TypeUnite,
                Disgination = produit.Desgination,
                Qtt = produit.Quantite,
                Prix = produit.Prix,
                FullNameChef = overier.Nom.ToUpper() +' '+overier.Prenom,
                Email = User.Identity.Name,
                imageUrl = overier.Image,
                CIN = overier.CIN,
                Type = overier.Type,
                Status = status,
                
               
            };

            operationRepository.Add(operation);
        }
        public void CreateOperationApresEditProduct(Produit produit, int qttupdate)
        {

            var overier = db.overiers.Find(HttpContext.Session.GetInt32("IDEmployeCon"));
            var operation = new Operations
            {
                Qtt_Diduir = qttupdate > 0 ? 0 : qttupdate,
                Qtt_Augmenter = qttupdate > 0 ? qttupdate : 0,
                Date_operation = DateTime.Now,

                Disgination = produit.Desgination,
                typeUnite = produit.TypeUnite,
                Qtt = produit.Quantite,
                Prix = produit.Prix,
                FullNameChef = overier.Nom.ToUpper() + ' ' + overier.Prenom,
                Email = User.Identity.Name,
                imageUrl = overier.Image,
                CIN = overier.CIN,
                Type = overier.Type,
                Status = "Modification",


            };

            operationRepository.Add(operation);



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
                typeUnite=produit.TypeUnite,
                Categories = categorieRepository.List().ToList()
            };

            return View(model);
        }

        // POST: ProduitController/Edit/5
       
        [Obsolete]
        public JsonResult EditPr(int id,CategorieProduitViewModel model)
        {
            id = model.produitId;
               
                    string newFileName = string.Empty, newName = GenereFilename();
                    string oldFilename = model.ImageUrl;
                    if (model.File != null)
                    {
                        string Upload = Path.Combine(hosting.WebRootPath, "ImagesProducts");
                       
                        string extension = Path.GetExtension(model.File.FileName);
                        newFileName = newName.Trim() + extension.ToLower();
                        string filepath = Path.Combine(Upload, newFileName);
                        //pour modifier une image il faut le supp
                        //Supprimer l'acienne image 

                        string fullpathold = Path.Combine(Upload, oldFilename);

                       
                            if (oldFilename != "product-default.png")
                            {

                                System.GC.Collect();
                                System.GC.WaitForPendingFinalizers();
                                System.IO.File.Delete(fullpathold);
                                //pour enregistrer l'image
                                model.File.CopyTo(new FileStream(filepath, FileMode.Create));
                            }
                            else
                                model.File.CopyTo(new FileStream(filepath, FileMode.Create));


                        



                    

                   
                        var Categorie = categorieRepository.Find(model.CategorieId);

                        var nvProduit = new Produit
                        {
                            Id = id,
                            Desgination = model.Desgination,
                            Prix = model.Prix,
                            Quantite = model.Quantite + model.QttUpdate,
                            ImageUrl = newFileName == "" ? oldFilename : newFileName,
                            TypeUnite=model.typeUnite,
                            Categorie = Categorie
                        };

                        proditRepository.Update(id, nvProduit);
                        CreateOperationApresEditProduct(nvProduit, model.QttUpdate);
                   
                    

                   
                    }
                    else
                    {
                        var Categorie = categorieRepository.Find(model.CategorieId);

                        var nvProduit = new Produit
                        {
                            Id = id,
                            Desgination = model.Desgination,
                            Prix = model.Prix,
                            Quantite = model.Quantite + model.QttUpdate,
                            ImageUrl = model.ImageUrl,
                            TypeUnite = model.typeUnite,
                            Categorie = Categorie
                        };

                        proditRepository.Update(id, nvProduit);
                        CreateOperationApresEditProduct(nvProduit, model.QttUpdate);


                    }
                
           return Json("done");
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
        public JsonResult DeleteProd(int id)
        {
            try
            {
                CheckcommandeoperationRelation(id);
                var produit = proditRepository.Find(id);
                string status = "Supprimer";
                CreateOperationApresAddProduct(produit, status);
                proditRepository.Delete(id);
                DeletImg(produit.ImageUrl);
                return Json("done");

            }
            catch (Exception)
            {
                throw;

            }
        }

        [Obsolete]
        public JsonResult DeleteAllProd(List<int> listid)
        {
            try
            {
                foreach(var item in listid)
                {
                    CheckcommandeoperationRelation(item);
                    var produit = gestionEmployeContext.produits.AsNoTracking().SingleOrDefault(p => p.Id == item);
                    string status = "Supprimer";
                    CreateOperationApresAddProduct(produit, status);
                    proditRepository.Delete(item);
                    DeletImg(produit.ImageUrl);

                }
                return Json("done");
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public void CheckcommandeoperationRelation(int id)
        {
            var produit = proditRepository.Find(id);
           
            foreach (var item in commandeRepository.List())
            {
                if (item.produit == produit)
                {

                    item.produit = null;
                    //item.produit = nproduit;

                }

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
