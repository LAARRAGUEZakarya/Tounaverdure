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

namespace GestionEmployes.Controllers.C_Stock
{
    public class ProduitController : Controller
    {
        private readonly IGestionEmployes<Produit> proditRepository;
        private readonly IGestionEmployes<CategorieProduit> categorieRepository;
        private readonly IGestionEmployes<Operations> operationRepository;
        private readonly IGestionEmployes<Commande> commandeRepository;
        [Obsolete]
        private readonly IHostingEnvironment hosting;
        private readonly G_EmployesDbContext db;

        [Obsolete]
        public ProduitController(IGestionEmployes<Produit> proditRepository,IGestionEmployes<CategorieProduit> categorieRepository, IGestionEmployes<Operations> operationRepository, IGestionEmployes<Commande> commandeRepository, IHostingEnvironment hosting,G_EmployesDbContext db)
        {
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public ActionResult Create(CategorieProduitViewModel model)
        {
            try
            {

                if(ModelState.IsValid)
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
                        
                        CreateOperationApresAddProduct(produit, "L'ajout");
                        return RedirectToAction(nameof(Index));

                    }
                }
                else
                {
                    return Json("");
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
                Status = "L'ajout",
                
               
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public ActionResult Edit(int id,CategorieProduitViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
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
                                System.IO.File.Delete(fullpathold);
                                //pour enregistrer l'image
                                model.File.CopyTo(new FileStream(filepath, FileMode.Create));
                            }
                            else
                                model.File.CopyTo(new FileStream(filepath, FileMode.Create));


                        



                    }

                   
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
                   
                    

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var produit = proditRepository.Find(id);



                    var modelEdit = new CategorieProduitViewModel
                    {
                        produitId = produit.Id,
                        Desgination = produit.Desgination,
                        Prix = produit.Prix,
                        Quantite = produit.Quantite,
                        ImageUrl = produit.ImageUrl,
                        CategorieId = produit.Categorie.Id,
                        typeUnite=produit.TypeUnite,
                        Categories = categorieRepository.List().ToList()
                    };
                  
                    return View(modelEdit);

                }
                
            }
            catch
            {
                return View();
            }
        }


        public void DeleteProd(int id)
        {
            try
            {
                CheckcommandeoperationRelation(id);
                proditRepository.Delete(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteAllProd(List<int> listid)
        {
            try
            {
                foreach(var item in listid)
                {
                    CheckcommandeoperationRelation(item);
                    proditRepository.Delete(item);
                }
            
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
