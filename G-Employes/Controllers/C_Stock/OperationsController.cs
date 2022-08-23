using GestionEmployes.Models;
using GestionEmployes.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Controllers.C_Stock
{
    public class OperationsController : Controller
    {
        private readonly IGestionEmployes<Operations> operationRepository;
        private readonly IGestionEmployes<Overier> overierRepository;
        private readonly IGestionEmployes<Produit> produitRepository;
        //private readonly IToastNotification toastNotification;

        public OperationsController(IGestionEmployes<Operations> operationRepository,IGestionEmployes<Overier> overierRepository,IGestionEmployes<Produit> produitRepository/*,IToastNotification toastNotification*/)
        {
            this.operationRepository = operationRepository;
            this.overierRepository = overierRepository;
            this.produitRepository = produitRepository;
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

            return View(operationRepository.List());
        }

        // GET: OperationsController/Details/5
        public ActionResult Details(int id)
        {
            var operation = operationRepository.Find(id);
            return View(operation);
        }


        

        // GET: OperationsController/Create
        public ActionResult Create()
        {
    
           
               
                //foreach(var item in overierRepository.List())
                // {
                //    if(item.Type == "chef")
                //    {
                //          Overiers.Add(item);
                //    }
       
                // }   
           
            var operation = new OperationProduitOverierViewModel
                {
                produits = produitRepository.List().ToList(),
                ovriers= overierRepository.List().ToList()
                
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
                var overier = overierRepository.Find(model.idOverier);
                var produit = produitRepository.Find(model.idProduit);
                var operation = new Operations
                {
                    Qtt_Diduir = model.Qtt_Diduir,
                    Date_operation = DateTime.Today,
                    ovrier = overier,
                    produit = produit
                };
                if (produit.Quantite < model.Qtt_Diduir)
                {
                    ViewBag.MessageQtt = "La Quantité réel de ce produit est inférieur à " + model.Qtt_Diduir;
                    var operationM = new OperationProduitOverierViewModel
                    {
                        produits = produitRepository.List().ToList(),
                        ovriers = overierRepository.List().ToList()
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
            catch
            {
                return View();
            }
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
                ovriers = overierRepository.List().ToList()
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
                var overier = overierRepository.Find(model.idOverier);
                var produit = produitRepository.Find(model.idProduit);
                var operation = operationRepository.Find(id);
                var nvOperation = new Operations
                {
                    Id = model.IdOperation,

                    Qtt_Diduir = model.Qtt_Diduir,
                    
                    ovrier = overier,
                    produit = produit
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
        public ActionResult Delete(int id, IFormCollection collection)
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
