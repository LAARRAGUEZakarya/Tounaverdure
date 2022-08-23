using GestionEmployes.Models;
using GestionEmployes.Models.G_Stock;
using GestionEmployes.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Controllers.C_Stock
{
    public class CommandeController : Controller
    {
        private readonly IGestionEmployes<Commande> commandeRepository;
        private readonly IGestionEmployes<Produit> produitRepository;
        private readonly RoleManager<IdentityRole> roleManager;

        public object CommandeProduitOverierViewModel { get; private set; }

        public CommandeController(IGestionEmployes<Commande> commandeRepository,IGestionEmployes<Produit> produitRepository)
        {
            this.commandeRepository = commandeRepository;
            this.produitRepository = produitRepository;
     
        }
        // GET: CommandeController
        public  ActionResult Index()
        {
           

        
            foreach (var item in produitRepository.List())
            {
                if (item.Quantite < 5)
                {
                    //toastNotification.AddWarningToastMessage("Le Quantite de produit \"" + item.Desgination + "\" est inferieur a 5 untite");
                    ViewBag.QttDanger = "Le Quantite de produit \"" + item.Desgination + "\" est inferieur a 5 untite";
                }
            }

            var commandes = commandeRepository.List().ToList();
            return View(commandes);
        }

        // GET: CommandeController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var commande = commandeRepository.Find(id);
                return View(commande);
            }
            catch (Exception)
            {

                throw;
            }
        
        }

        // GET: CommandeController/Create
        public ActionResult Create()
        {
            var model =new CommandeProduitOveierViewModel
            {
                produits = produitRepository.List().ToList(),
               
            };
            return View(model);
        }

        // POST: CommandeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CommandeProduitOveierViewModel model)
        {
            try
            {

                /*





                 I NEED SESSION FOR ADD ID AND THE NAME OF CHEF






                */
                var produit = produitRepository.Find(model.idProduit);
                var commande = new Commande
                {
                    Qtt_Diduir = -model.Qtt_Diduir,
                    Date_operation = DateTime.Today,
                    Etat = false,
                    produit = produit
                };
                if (produit.Quantite < model.Qtt_Diduir)
                {
                    ViewBag.MessageQtt = "La Quantité réel de ce produit est inférieur à " + model.Qtt_Diduir;
                    var Commande = new CommandeProduitOveierViewModel
                    {
                        produits = produitRepository.List().ToList(),
                    };

                    return View(Commande);
                }
                else
                {
                    commandeRepository.Add(commande);
                    ViewBag.Commande = commande;
                    //produit.Quantite -= model.Qtt_Diduir;//apre l'ajout d'un operation il faut de diduir la quantite de produit a  ete diduir dans la table operation
                    //produitRepository.Update(model.idProduit, produit);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: CommandeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CommandeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CommandeController/Delete/5
        public ActionResult Delete(int id)
        {

            return View();
        }

        // POST: CommandeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                commandeRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
