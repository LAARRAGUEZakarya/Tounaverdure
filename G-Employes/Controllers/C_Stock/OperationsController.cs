using G_Employes.Data;
using GestionEmployes.Models;
using GestionEmployes.Models.G_Stock;
using GestionEmployes.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Controllers.C_Stock
{
    [Authorize(Roles = "admin")]
    public class OperationsController : Controller
    {
        private readonly G_EmployesDbContext gestionEmployeContext;
        private readonly IGestionEmployes<Operations> operationRepository;
       
        private readonly IGestionEmployes<Produit> produitRepository;
        private readonly IGestionEmployes<Commande> commandeRepository;

        //private readonly IToastNotification toastNotification;

        public OperationsController(G_EmployesDbContext gestionEmployeContext, IGestionEmployes<Operations> operationRepository,IGestionEmployes<Produit> produitRepository,IGestionEmployes<Commande> commandeRepository/*,IToastNotification toastNotification*/)
        {
            this.gestionEmployeContext = gestionEmployeContext;
            this.operationRepository = operationRepository;
            
            this.produitRepository = produitRepository;
            this.commandeRepository = commandeRepository;
            //this.toastNotification = toastNotification;
        }
        // GET: OperationsController
        public ActionResult Index()
        {
            foreach(var item in produitRepository.List() )
            {
                    if(item.Quantite<5)
                    {
                    //toastNotification.AddWarningToastMessage("Le Quantite de produit \"" + item.Desgination + "\" est inferieur a 5 untite");
                       ViewBag.QttDanger = "Le Quantite de produit \""+item.Desgination+"\" est inferieur a 5 untite";
                    }    
            }

            ViewData["FullName"] = HttpContext.Session.GetString("NomUser") +' '+ HttpContext.Session.GetString("PrenomUser");
            ViewData["Prenom"] = HttpContext.Session.GetString("PrenomUser");
            ViewData["Img"] = HttpContext.Session.GetString("ImageUser");

             

            return View(operationRepository.List());
        }

   
        // GET: OperationsController/Details/5
        public ActionResult Details(int id)
        {
            ViewData["Nom"] = HttpContext.Session.GetString("NomUser");
            ViewData["Prenom"] = HttpContext.Session.GetString("PrenomUser");
            ViewData["Img"] = HttpContext.Session.GetString("ImageUser");
            var operation = operationRepository.Find(id);
            return View(operation);
        }


        

        // GET: OperationsController/Create
        public ActionResult Create()
        {
    
            var operation = new OperationProduitOverierViewModel
                {
                produits = produitRepository.List().ToList(),
                ovriers= gestionEmployeContext.overiers.ToList()
                
                };

                 return View(operation);
           
         
        }


        // POST: OperationsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OperationProduitOverierViewModel model)
        {
            try
            {
               if(ModelState.IsValid)
                {
                    var overier = new Overier{ };
                    if (model.idOverier>=0) overier = gestionEmployeContext.overiers.Find(model.idOverier);
                    var produit = produitRepository.Find(model.idProduit);
                    var operation = new Operations
                    {
                        Qtt_Diduir = model.Qtt_Diduir < 0 ? model.Qtt_Diduir : 0,
                        Qtt_Augmenter = model.Qtt_Diduir > 0 ? model.Qtt_Diduir : 0,
                        Date_operation = DateTime.Now,
                        
                        FullNameChef = overier.Nom +' '+overier.Prenom,
                  
                        CIN = overier.CIN,
                        Email = overier.Email,
                        Type = overier.Type,
                      
                        Disgination = produit.Desgination,
                        Qtt = produit.Quantite,
                        Prix = produit.Prix,
                        Status = model.Qtt_Diduir<0? "Diminuer":"L'ajout"
                    };
                    if (model.Qtt_Diduir < 0 && -produit.Quantite > -model.Qtt_Diduir)
                    {
                        ViewBag.MessageQtt = "La Quantité réel de ce produit est inférieur à " + model.Qtt_Diduir;
                        var operationM = new OperationProduitOverierViewModel
                        {
                            produits = produitRepository.List().ToList(),
                            ovriers = gestionEmployeContext.overiers.ToList()
                        };

                        return View(operationM);
                    }
                    else
                    {
                        operationRepository.Add(operation);
                        if (produit.Quantite > 0)
                            produit.Quantite += model.Qtt_Diduir;//apre l'ajout d'un operation il faut de diduir la quantite de produit a  ete diduir dans la table operation
                        else
                            produit.Quantite -= model.Qtt_Diduir;
                        produitRepository.Update(model.idProduit, produit);
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    var operation = new OperationProduitOverierViewModel
                    {
                        produits = produitRepository.List().ToList(),
                        ovriers = gestionEmployeContext.overiers.ToList()

                    };

                    return View(operation);
                }
              
               }
            catch
            {
                return View();
            }
        }
        public ActionResult acceptCommande(int id)
        {

            var model = commandeRepository.Find(id);
            var operation = new Operations
            {
                Qtt_Diduir = model.Qtt_Diduir,
                Date_operation = DateTime.Now,
                FullNameChef = model.ovrier.Nom + ' ' + model.ovrier.Prenom,
                
                typeUnite=model.produit.TypeUnite,
                CIN = model.ovrier.CIN,
                Email = model.ovrier.Email,
                Type = model.ovrier.Type,
                Disgination = model.produit.Desgination,
                Qtt = model.produit.Quantite,
                Prix = model.produit.Prix,
                imageUrl = model.ovrier.Image,
                Status = "Diminuer"
            };
     
            if (operation.Qtt < -operation.Qtt_Diduir )//cause qtt_D is negative number::
            {
               
                //update coomande state :
                //model.Etat = "R";
                //model.Qtt_Diduir = operation.Qtt_Diduir;
                //commandeRepository.Update(model.Id, model);

                ViewBag.error = "La commande de M." + model.ovrier.Nom + " a été annulée car la quantité commandée (" + model.Qtt_Diduir + ") n'est pas disponible en magasin";
                
                return RedirectToAction(actionName:"Index", controllerName:"Commande",routeValues: ViewBag.error);
            }
            else
            {
                operationRepository.Add(operation);

                //update qtt ::
                var produit = model.produit;
                produit.Quantite += model.Qtt_Diduir;
                produitRepository.Update(model.produit.Id, model.produit);

                //update commande state :
                model.Etat = "A";
                commandeRepository.Update(model.Id, model);
                return RedirectToAction(actionName: "Index",controllerName:"Operations");
            }
        }
        public JsonResult acceptCommandeAll(List<int> listid)
        {


            foreach (var id in listid)
            {
                var item = commandeRepository.Find(id);
                    var operation = new Operations
                    {
                        Qtt_Diduir = item.Qtt_Diduir,
                        Date_operation = DateTime.Now,
                        FullNameChef = item.ovrier.Nom + ' ' + item.ovrier.Prenom,

                        CIN = item.ovrier.CIN,
                        Email = item.ovrier.Email,
                        Type = item.ovrier.Type,
                        Disgination = item.produit.Desgination,
                        Qtt = item.produit.Quantite,
                        typeUnite = item.produit.TypeUnite,
                        Prix = item.produit.Prix,
                        Status = "Diminuer",
                        imageUrl = item.ovrier.Image
                    };

                    if (operation.Qtt < -operation.Qtt_Diduir)
                    {
                        //ViewBag.MessageQtt = "La Quantité réel de ce produit est inférieur à " + model.Qtt_Diduir;
                        //var operationM = new OperationProduitOverierViewModel
                        //{
                        //    produits = produitRepository.List().ToList(),
                        //    ovriers = overierRepository.List().ToList()
                        //};
                        //operation.Qtt_Diduir = -operation.Qtt;
                        //operationRepository.Add(operation);

                        ////update qtt ::
                        //var produit = item.produit;
                        //produit.Quantite += operation.Qtt_Diduir;

                        //produitRepository.Update(item.produit.Id, item.produit);

                        //update coomande state :
                        //item.Etat = "R";
                        //item.Qtt_Diduir = operation.Qtt_Diduir;
                        //commandeRepository.Update(item.Id, item);
                        return Json( "La commande de M." + item.ovrier.Nom + " a été annulée car la quantité commandée (" + item.Qtt_Diduir + ") n'est pas disponible en magasin");

                      
                    }
                    else
                    {
                        operationRepository.Add(operation);

                        //update qtt ::
                        var produit = item.produit;
                        produit.Quantite += item.Qtt_Diduir;
                        produitRepository.Update(item.produit.Id, item.produit);

                        //update commande state :
                        item.Etat = "A";
                        commandeRepository.Update(item.Id, item);
                    }
                
            }
            return Json("done");

        }
        // GET: OperationsController/Edit/5
        public ActionResult Edit(int id)
        {
            var operation = operationRepository.Find(id);
            var model = new OperationProduitOverierViewModel
            {
                IdOperation = operation.Id,
                Qtt_Diduir = operation.Qtt_Diduir,
                produits = produitRepository.List().ToList(),
                ovriers = gestionEmployeContext.overiers.ToList()
            };
            return View(model);
        }

        // POST: OperationsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, OperationProduitOverierViewModel model)
        {
            try
            {
                var overier = gestionEmployeContext.overiers.Find(model.idOverier);
                var produit = produitRepository.Find(model.idProduit);
                var operation = operationRepository.Find(id);
                var nvOperation = new Operations
                {
                    Id = model.IdOperation,

                    Qtt_Diduir = model.Qtt_Diduir,
                  
                };
                operationRepository.Update(operation.Id,operation);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OperationsController/Delete/5
        public ActionResult Delete(int id)
        {
          
            return View();
        }

        // POST: OperationsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id,IFormCollection collection)
        {
            try
            {
           
               foreach(var item in operationRepository.List())
                {
                    operationRepository.Delete(item.Id);
                }
               
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeleteOperations(List<int> listid)
        {
            

            foreach (var item in listid)
            {
                operationRepository.Delete(item);
            }

            return RedirectToAction(nameof(Index));

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deleteall(int id)
        {
            try
            {

                operationRepository.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
