

$(document).ready(function () {
    //set datatable and change language of datatable to franche page==>operations
    if (!$('#operationTable').dataTable()) {
        $('#operationTable').dataTable({

            "language": {
                "sProcessing": "Traitement en cours...",
                "sSearch": "Rechercher&nbsp;:",
                "sLengthMenu": "Afficher _MENU_ &eacute;l&eacute;ments",
                "sInfo": "Affichage de l'&eacute;lement _START_ &agrave; _END_ sur _TOTAL_ &eacute;l&eacute;ments",
                "sInfoEmpty": "Affichage de l'&eacute;lement 0 &agrave; 0 sur 0 &eacute;l&eacute;ments",
                "sInfoFiltered": "(filtr&eacute; de _MAX_ &eacute;l&eacute;ments au total)",
                "sInfoPostFix": "",
                "sLoadingRecords": "Chargement en cours...",
                "sZeroRecords": "Aucun &eacute;l&eacute;ment &agrave; afficher",
                "sEmptyTable": "Aucune donn&eacute;e disponible dans le tableau",
                "oPaginate": {
                    "sFirst": "Premier",
                    "sPrevious": "Pr&eacute;c&eacute;dent",
                    "sNext": "Suivant",
                    "sLast": "Dernier"
                },
                "oAria": { 
                    "sSortAscending": ": activer pour trier la colonne par ordre croissant",
                    "sSortDescending": ": activer pour trier la colonne par ordre d&eacute;croissant"
                }
            }
        })
    }

 

    $("#checkBoxAll").click(function () {
        $(".checkBoxRow").prop("checked", this.checked);
    });

    $('.checkBoxRow').click(function () {
        if ($('.checkBoxRow:checked').length == $('.checkBoxRow').length) {
            $('#checkBoxAll').prop('checked', true);
        } else {
            $('#checkBoxAll').prop('checked', false);
        }
    });


  

    
    $(".overierId").change(function () {

        var filtre = $("#ConsultationID").val();

        if (filtre == "mois") {
            var overierID = $(this).val();
            var date = $("#datepickerMois").datepicker("getDate");

            showApartirMois(date, overierID);  
        }
        else if (filtre == "jour")
        {
            var overierID = $(this).val();
            var date = ("#datepickerJour").val();
           

            showApartirJour(date, overierID);
        }


    })



    $("#datepickerJour").change(function () {
        
        var overierID = $('.overierId').val();
        var date = new Date($("#datepickerJour").val());


        $('.btnSupp').removeAttr('hidden');
        showApartirJour(date, overierID);
       
    })


    $("#datepickerMois").change(function () {
       
        var overierID = $('.overierId').val();
        var date = $(this).datepicker("getDate");

        $('.btnSupp').removeAttr('hidden');
        showApartirMois(date, overierID);
    })




    //add datepicker input after select something :::: page==>detailpointeuse
    $("#ConsultationID").change(function () {
        var valeur = $('#ConsultationID').val();
        /*var ContainerDatepicker = $('#ContainerDatepicker');*/



        if (valeur == "mois")
        {
            $("#datepickerAff").attr("hidden", true);
            $(".btnSupp").attr("hidden", true);
            $('#datepicker5').removeAttr('hidden');

            
            $("#tableResult").html('');
             
         }
        else if (valeur == "jour")
        {
            $(".btnSupp").attr("hidden", true);
            $("#datepicker5").attr("hidden", true);
            $('#datepickerAff').removeAttr('hidden');
            $("#tableResult").html('');
         }
        else
        {
            $("#datepicker5").attr("hidden", true);
            $('#datepickerAff').attr("hidden", true);
            $(".btnSupp").attr("hidden", true);
            $("#tableResult").html('');
        }
           
            

        
    });

   

   
    $("#AllcheckBox").click(function () {

        $(".RowCheck").each(function () {
            $(this).prop('checked', true);
        });
    });

    $('.RowCheck').click(function () {
        if ($('.RowCheck:checked').length == $('.RowCheck').length) {
            $('#AllcheckBox').prop('checked', true);
        } else {
            $('#AllcheckBox').prop('checked', false);
        }
    });

 
});

function checkall()
{

 
    if ($("#AllcheckBox").is(':checked'))
    {
        $(".RowCheck").each(function () {
            $(this).prop('checked',true );
        });
       
    }
    else
    {
        $(".RowCheck").each(function () {
            $(this).prop('checked', false);
        });
    }
   
    
}


function showApartirMois(date, overierID)
{
    var mois = date.getMonth() + 1;
    var annee = date.getFullYear();
    var user = $('.checkUser').val();


    var dates = [];
    if (overierID == "all")
    {
        dates.push(mois);
        dates.push(annee);
    }
    else
    {
        dates.push(mois);
        dates.push(annee);
        dates.push(overierID);
    }



    $.ajax({
        type: "POST",
        url: "/DetailsPointeuse/AficherNbrHoursParMois",
        data: { dates: dates },

        success: function (result) {

            $("#tableResult").html('<thead class="table-light"></th> <th>Nom</th><th>Salaire</th> <th class="text-center" >Nombre d\'heures par mois</th><th>Total des heures d\'entrée restantes</th><th>Total des heures de sortée restantes</th><th class="text-center">Action</th>  </tr ></thead>');
        
            result.forEach(function (resultRow) {
                if (user == "admin") {
                   
                  
                        var tableRow =
                            "<tbody> <tr>"+
                            
                    "<td>" + resultRow.name + "</td>" +
                            "<td>" + resultRow.salaireParMois + "</td>" +
                            '<td class="text-center">' + resultRow.timenbrhour + "</td>" +
                            '<td class="text-center bg-warning">' + resultRow.sumTimeOfInAugDid + "</td>" +
                            '<td class="text-center bg-secondary text-white">' + resultRow.sumTimeOfOutAugDid + "</td>" +
                            '<td class="text-center"><button onclick="ModifierDetailMois(' + resultRow.idEmploye + ')" class="btn btn-primary">Modifier</button></td>' +

                            "</tr></tbody>";
                  
                  
                }
                else
                {
                  
                        var tableRow =
                            "<tbody> <tr>"+
                            
                            "<td>" + resultRow.name + "</td>" +
                            "<td>" + resultRow.salaireParMois + "</td>" +
                            '<td class="text-center">' + resultRow.timenbrhour + "</td>" +

                            '<td class="text-center bg-warning">' + resultRow.sumTimeOfInAugDid + "</td>" +
                            '<td class="text-center bg-secondary text-white ">' + resultRow.sumTimeOfOutAugDid + "</td>" +
                            '<td class="text-center"><button onclick="ModifierDetailMois(-1)" class="btn btn-primary disabled">Modifier</button></td>' +
                            "</tr></tbody>";
                
                   
                }


                $("#tableResult").append(tableRow);
               
                   
                $('#tableResult').DataTable();
              

            });
           
            
        }

     
    });
   
}



function showApartirJour(date, overierID)
{
    var jour = date.getDate();
    var mois = date.getMonth() + 1;
    var annee = date.getFullYear();

    var user = $('.checkUser').val();


    var dates = [];
    if (overierID == "all") {
        dates.push(jour);
        dates.push(mois);
        dates.push(annee);
    }
    else {
        dates.push(jour);
        dates.push(mois);
        dates.push(annee);
        dates.push(overierID);
    }


    $.ajax({
        type: "POST",
        url: "/DetailsPointeuse/AficherNbrHoursParjour",
        data: { dates: dates },

        success: function (result) {

            $("#tableResult").html('<thead class="table-light"> <th>Nom</th> <th class="text-center" >Nombre d\'heures</th><th>Temp d\'entree</th><th>Rest</th><th>Temp de sortee</th><th>Rest</th>  <th class="text-center">Action</th>  </tr ></thead>');

            result.forEach(function (resultRow) {
                if (user == "admin") {
                    if (resultRow.etatTimeofin == "NVInOut") {
                        var tableRow =
                            "<tbody> <tr>"
                           +
                            "<td>" + resultRow.name + "</td>" +
                            '<td class="text-center">' + resultRow.timenbrhour + "</td>" +
                            '<td class="text-center bg-danger">' + resultRow.timeofin + "</td>" +
                            '<td class="text-center bg-warning ">' + resultRow.timeOfInAugDid + "</td>" +
                            '<td class="text-center bg-danger">' + resultRow.timeofout + "</td>" +
                            '<td class="text-center bg-warning">' + resultRow.timeOfOutAugDid + "</td>" +
                            '<td class="text-center"><button onclick="ModifierDetailJour(' + resultRow.idEmploye + ')" class="btn btn-primary">Modifier</button></td>' +

                            "</tr></tbody>";
                    }
                    else if (resultRow.etatTimeofin == "NVIn") {
                        var tableRow =
                            "<tbody> <tr>"
                           +
                            "<td>" + resultRow.name + "</td>" +
                            '<td class="text-center">' + resultRow.timenbrhour + "</td>" +
                            '<td class="text-center bg-danger">' + resultRow.timeofin + "</td>" +
                            '<td class="text-center bg-warning">' + resultRow.timeOfInAugDid + "</td>" +
                            '<td class="text-center bg-success">' + resultRow.timeofout + "</td>" +
                            '<td class="text-center bg-info">' + resultRow.timeOfOutAugDid + "</td>" +
                            '<td class="text-center"><button onclick="ModifierDetailJour(' + resultRow.idEmploye + ')" class="btn btn-primary">Modifier</button></td>' +

                            "</tr></tbody>";
                    }
                    else if (resultRow.etatTimeofin == "NVOut") {
                        var tableRow =
                            "<tbody> <tr>"
                            + 
                            "<td>" + resultRow.name + "</td>" +
                            '<td class="text-center">' + resultRow.timenbrhour + "</td>" +
                            '<td class="text-center bg-success">' + resultRow.timeofin + "</td>" +
                            '<td class="text-center bg-info">' + resultRow.timeOfInAugDid + "</td>" +
                            '<td class="text-center bg-danger">' + resultRow.timeofout + "</td>" +
                            '<td class="text-center bg-warning">' + resultRow.timeOfOutAugDid + "</td>" +
                            '<td class="text-center"><button onclick="ModifierDetailJour(' + resultRow.idEmploye + ')" class="btn btn-primary">Modifier</button></td>' +

                            "</tr></tbody>";
                    }
                    else {
                        var tableRow =
                            "<tbody> <tr>"
                            + 
                            "<td>" + resultRow.name + "</td>" +
                            '<td class="text-center">' + resultRow.timenbrhour + "</td>" +
                            '<td class="text-center bg-success">' + resultRow.timeofin + "</td>" +
                            '<td class="text-center bg-info">' + resultRow.timeOfInAugDid + "</td>" +
                            '<td class="text-center bg-success">' + resultRow.timeofout + "</td>" +
                            '<td class="text-center bg-info">' + resultRow.timeOfOutAugDid + "</td>" +
                            '<td class="text-center"><button onclick="ModifierDetailJour(' + resultRow.idEmploye + ')" class="btn btn-primary">Modifier</button></td>' +

                            "</tr></tbody>";
                    }

                   
                }
                else
                {
                    if (resultRow.etatTimeofin == "NVInOut") {
                        var tableRow =
                            "<tbody> <tr>"
                            + 
                            "<td>" + resultRow.name + "</td>" +
                            '<td class="text-center">' + resultRow.timenbrhour + "</td>" +
                            '<td class="text-center bg-danger">' + resultRow.timeofin + "</td>" +
                            '<td class="text-center bg-warning ">' + resultRow.timeOfInAugDid + "</td>" +
                            '<td class="text-center bg-danger">' + resultRow.timeofout + "</td>" +
                            '<td class="text-center bg-warning">' + resultRow.timeOfOutAugDid + "</td>" +
                            '<td class="text-center"><button onclick="ModifierDetailJour(-1)" class="btn btn-primary disabled">Modifier</button></td>' +

                            "</tr></tbody>";
                    }
                    else if (resultRow.etatTimeofin == "NVIn") {
                        var tableRow =
                            "<tbody> <tr>"
                            + 
                            "<td>" + resultRow.name + "</td>" +
                            '<td class="text-center">' + resultRow.timenbrhour + "</td>" +
                            '<td class="text-center bg-danger">' + resultRow.timeofin + "</td>" +
                            '<td class="text-center bg-warning">' + resultRow.timeOfInAugDid + "</td>" +
                            '<td class="text-center bg-success">' + resultRow.timeofout + "</td>" +
                            '<td class="text-center bg-info">' + resultRow.timeOfOutAugDid + "</td>" +
                            '<td class="text-center"><button onclick="ModifierDetailJour(-1)" class="btn btn-primary disabled">Modifier</button></td>' +

                            "</tr></tbody>";
                    }
                    else if (resultRow.etatTimeofin == "NVOut") {
                        var tableRow =
                            "<tbody> <tr>"
                            +
                            "<td>" + resultRow.name + "</td>" +
                            '<td class="text-center">' + resultRow.timenbrhour + "</td>" +
                            '<td class="text-center bg-success">' + resultRow.timeofin + "</td>" +
                            '<td class="text-center bg-info">' + resultRow.timeOfInAugDid + "</td>" +
                            '<td class="text-center bg-danger">' + resultRow.timeofout + "</td>" +
                            '<td class="text-center bg-warning">' + resultRow.timeOfOutAugDid + "</td>" +
                            '<td class="text-center"><button onclick="ModifierDetailJour(-1)" class="btn btn-primary disabled">Modifier</button></td>' +

                            "</tr></tbody>";
                    }
                    else {
                        var tableRow =
                            "<tbody> <tr>"
                            + 
                            "<td>" + resultRow.name + "</td>" +
                            '<td class="text-center">' + resultRow.timenbrhour + "</td>" +
                            '<td class="text-center bg-success">' + resultRow.timeofin + "</td>" +
                            '<td class="text-center bg-info">' + resultRow.timeOfInAugDid + "</td>" +
                            '<td class="text-center bg-success">' + resultRow.timeofout + "</td>" +
                            '<td class="text-center bg-info">' + resultRow.timeOfOutAugDid + "</td>" +
                            '<td class="text-center"><button onclick="ModifierDetailJour(-1)" class="btn btn-primary disabled">Modifier</button></td>' +

                            "</tr></tbody>";
                    }
                   
                }
               



                $("#tableResult").append(tableRow);
                if (!$('#tableResult').DataTable())
                    $('#tableResult').DataTable();

            });

        }


    });

}


function ModifierDetailMois(id)
{
    date = $("#datepickerMois").datepicker("getDate");
   
    var mois = date.getMonth() + 1;
    var annee = date.getFullYear();

    $.ajax({
        type: "GET",
        url: "/DetailsPointeuse/EditMois",
        data: { Id: id, month:mois, year:annee },
        success: function (res) {
         
            
                         $("#form-modal .modal-body").html(res);
                        $("#form-modal .modal-title").html("Modification");
                        $("#form-modal").modal('show');
           
            

        }
        , Error: function (request, status, error) {
            alert(request.responseText);
        }
    })
   
}

function ModifierDetailJour(id) {
    date = new Date(new Date($("#datepickerJour").val()));

    var jour = date.getDate();
    var mois = date.getMonth() + 1;
    var annee = date.getFullYear();

    $.ajax({
        type: "GET",
        url: "/DetailsPointeuse/EditJour",
        data: { Id: id,day:jour, month: mois, year: annee },
        success: function (res) {


            $("#form-modal .modal-body").html(res);
            $("#form-modal .modal-title").html("Modification");
            $("#form-modal").modal('show');



        }
        , Error: function (request, status, error) {
            alert(request.responseText);
        }
    })

}


$("#deleteDetails").click(function () {

    Swal.fire({
        title: 'Suppression?',
        text: "Vous voulez vraiment supprimer le nombre d'heures de travail de mois et de jours pour l'employé sélectionné ?!",
      
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Oui'
    }).then((result) => {

        if (result.isConfirmed)
        {
            var date = Date.now();
            var jour = 7;
            var filtrer = $('#ConsultationID').val();
            if (filtrer == "mois") {
                date = $("#datepickerMois").val();
            }
            else if (filtrer == "jour") {

                date = new Date(new Date($("#datepickerJour").val()));
                jour = date.getDate();
            }




            var mois = date.getMonth() + 1;
            var annee = date.getFullYear();
            var overierID = $('.overierId').val();





            var listid = [];
            $(".RowCheck:checked").each(function () {
                listid.push($(this).val());
            });

            if (listid.length > 0) {
                $.ajax({
                    type: "POST",
                    url: "/DetailsPointeuse/DeleteDetails",
                    data: { listid: listid, day: jour, month: mois, year: annee },
                    //contentType: "application/json; charset=utf-8",
                    //dataType: "json",
                    success: function (r) {


                        if (filtrer == "mois")
                            showApartirMois(date, overierID);
                        else if (filtrer == "jour")
                            showApartirJour(date, overierID);


                    }
                });

          
            }
            Swal.fire(
                'Supperssion terminer!',
                'l\'operation terminer.',
                'success'
            )
        }
    })

});

function PassId(id)
{
    $("#Supprission").modal('show');
    $(".modal-footer #stockid").val(id);
    
}

function supprimerCat()
{
    var Id = $('#stockid').val();
    $.ajax({
        type: "POST",
        url: "CategorieProduit/DeleteCat",
        data: { id: Id },
        success: function () {

            location.reload()
        }
    });
}
function supprimerAllCat() {
    var listid = [];
    $(".checkBoxRow:checked").each(function () {
        listid.push($(this).val());
    });

    $.ajax({
        type: "POST",
        url: "/CategorieProduit/DeleteAllCat",
        data: { listid: listid },
        success: function () {

            location.reload()
        }
    });
}

function supprimerProd() {
    var Id = $('#stockid').val();
    $.ajax({
        type: "POST",
        url: "/Produit/DeleteProd",
        data: { id: Id },
        success: function (data) {
            if (data == "done") {
                location.reload();
            }
            
            
        },
        error: function (data) {
            alert("Error: " + Id);
        }
    });
   
}
function supprimerAllProd() {
    var listid = [];
    $(".checkBoxRow:checked").each(function () {
        listid.push($(this).val());
    });
    $.ajax({
        type: "POST",
        url: "/Produit/DeleteAllProd",
        data: { listid: listid },
        success: function (data) {
            if (data == "done") {
                location.reload();

            }
            

        },
        error: function (data) {
            alert("Error: " + Id);
        }
    });

}


function SuppCommande(){
    var listid = [];
    $(".checkBoxRow:checked").each(function () {
        listid.push($(this).val());
    });

    $.ajax({
        type: "POST",
        url: "/Commande/deleteCommandes",
        data: { listid: listid },
        success: function () {

            location.reload();

        }
    });

}

function CommandeRefuseAll() {
    var listid = [];
    $(".checkBoxRow:checked").each(function () {
        listid.push($(this).val());
    });

    $.ajax({
        type: "POST",
        url: "/Commande/CommandeRefuseAll",
        data: { listid: listid },
        success: function () {

            location.reload();

        }
    });

}

function acceptCommandeAll() {
    var listid = [];
    $(".checkBoxRow:checked").each(function () {
        listid.push($(this).val());
    });

    $.ajax({
        type: "POST",
        url: "/Operations/acceptCommandeAll",
        data: { listid: listid },
        success: function (data) {

            if (data == "done") {
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'success',
                    showConfirmButton: false,
                    timer: 800
                });


                location.reload();


            }
            else {

                swal("error!", data, "error").then(function () {
                    location.reload();;
                });
            }

        }
    });

}



function hideModal() {

    $("#Supprission").modal('hide');
    $("#ACCModal").modal('hide');
    $("#REFModal").modal('hide');
    $("#Supprissionall").modal('hide');
}









function hideModalLayout()
{
    $("#form-modal").modal('hide');
}



ShowInModal = (url, title) =>
{
        $.ajax({
            type: "GET",
            url: url,
            success: function (res) {
                $("#form-modal .modal-body").html(res);
                $("#form-modal .modal-title").html(title);
                $("#form-modal").modal('show');
                
            }
            , Error: function (request, status, error) {
                alert(request.responseText);
            }
        })
}

$("#deleteA").click(function ()
{

    var listid = [];
    $(".checkBoxRow:checked").each(function () {
        listid.push($(this).val());
    });
    

    if (listid.length > 0) {
        $.ajax({
            type: "POST",
            url: "/Operations/DeleteOperations",
            data: { listid: listid },
            //contentType: "application/json; charset=utf-8",
            //dataType: "json",
            success: function (r) {
                location.reload();

            }
        });
        
    }
   /* else swal("Deleted!", "Your imaginary file has been deleted.", "e");*/
   
});




function editDetailsMois() {
   
    var date = $('#dateCheck').val();
    var overierID = parseInt($('#idemp').val());
   
    var NbrHMois = $('#nbrMois').val();
    var sumin = $('#sumtimein').val();
    var sumout = $('#sumtimeout').val();

    var overier = $('.overierId').val();
    

    $.ajax({

            type: "POST",
        url: "/DetailsPointeuse/EditDetailsMois",
        data: { id: overierID, date: date, nbrHMois: NbrHMois, sumin: sumin, sumout: sumout },
          
            success: function (r) {

                if (r == "Success") {

                   
                        date = $("#datepickerMois").datepicker("getDate");
                        showApartirMois(date, overier);
                  
                }
                else
                {
                    $("#form-modal .modal-title").html(r);
                }
                    
              
              
            }
        });

    
    $("#form-modal").modal('hide');

};




function editDetailsJour() {

    var date = $('#dateCheck').val();
    var overierID = parseInt($('#idemp').val());

    var NbrHJour = $('#nbrJour').val();
    
    var overier = $('.overierId').val();


    $.ajax({

        type: "POST",
        url: "/DetailsPointeuse/EditDetailsJour",
        data: { id: overierID, date: date, nbrHJour: NbrHJour },

        success: function (r) {

            if (r == "Success") {


                date = new Date(new Date($("#datepickerJour").val()));
                showApartirJour(date, overier);

            }
            else {
                $("#form-modal .modal-title").html(r);
            }



        }
    });


    $("#form-modal").modal('hide');

};


//////////othman

$("#createPrd").submit(function (event) {
    event.preventDefault();

    

    var fdu = new FormData();
    var files = $('#File')[0].files;
    fdu.append('File', files[0]);
    fdu.append('Desgination', $('#Desgination').val());
    fdu.append('Quantite', $('#Quantite').val());
    fdu.append('typeUnite', $('#typeUnite').val());
    fdu.append('Prix', $('#Prix').val());
    fdu.append('CategorieId', $('#CategorieId').val());


    $.ajax({
        url: "/Produit/CreatePr",
        type: "POST",
        dataType: "JSON",
        data: fdu,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data == "done") {
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'success',
                    showConfirmButton: false,
                    timer: 800
                });


                location.reload();


            }
            else {

                swal("error!", data, "error")
            }
        }
    });
});



$("#cretC").submit(function (event) {
    event.preventDefault();

  var cat = new FormData();
    cat.append('Type', $('#Type').val());

    $.ajax({
        url: "/CategorieProduit/CreateC",
        type: "POST",
        dataType: "JSON",
        data: cat,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data == "done") {
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'success',
                    showConfirmButton: false,
                    timer: 800
                });


                location.reload();


            }
           
        }
    });
});


$("#editC").submit(function (event) {
    event.preventDefault();

    var cat = new FormData();
    cat.append('Type', $('#Type').val());
    cat.append('Id', $('#Id').val());


    $.ajax({
        url: "/CategorieProduit/EditC",
        type: "POST",
        dataType: "JSON",
        data: cat,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data == "done") {
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'success',
                    showConfirmButton: false,
                    timer: 800
                });


                location.reload();


            }

        }
    });
});


$("#edirpr").submit(function (event) {
    event.preventDefault();



    var fdu = new FormData();
    var files = $('#File')[0].files;
    fdu.append('File', files[0]);
    fdu.append('Desgination', $('#Desgination').val());
    fdu.append('QttUpdate', $('#QttUpdate').val());
    fdu.append('typeUnite', $('#typeUnite').val());
    fdu.append('Prix', $('#Prix').val());
    fdu.append('CategorieId', $('#CategorieId').val());
    fdu.append('ImageUrl', $('#ImageUrl').val());
    fdu.append('Quantite', $('#Quantite').val());
    fdu.append('produitId', $('#produitId').val());

    
    $.ajax({
        url: "/Produit/EditPr",
        type: "POST",
        dataType: "JSON",
        data: fdu,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            
            if (data == "done") {
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'success',
                    showConfirmButton: false,
                    timer: 800
                });


                location.reload();


            }
            
        }
    });
});


