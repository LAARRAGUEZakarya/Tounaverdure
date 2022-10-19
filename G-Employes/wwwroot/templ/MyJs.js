

/*$(document).ready(function () {*/
/*console.log("ready!");*/

$(document).ready(function () {
    var id = $("#BtnAddEquipe").val();
    var check = $("#BtnAddEquipe").is(':checked');


    $.ajax({
        url: "/Employe/GetEquipe",
        type: "POST",
        dataType: "JSON",
        contentType: false, // NEEDED, DON'T OMIT THIS (requires jQuery 1.6+)
        processData: false, // NEEDED, DON'T OMIT THIS
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var opt = $('<option></option>');
                opt.attr('value', data[i].id).text(data[i].nom_equipe);

                if (data[i].id == id)
                {
                    opt.attr("selected", "selected");
                    

                }


                $("#showdata").append(opt);
            }
        }

    });
    /*input new equipe*/


    $("#showdata").change(function () {

        var content = ' <div id="new"><fieldset><label> Nouveau equipe</label><input class="form-control" id="newEquipe"/><div class="d-flex justify-content-center mt-3"><button type="button" id="btnAddEq" class="btn btn-success"><i class="mdi mdi-plus-circle me-2"></i>ajouter</button></div></fieldset></div>';

        var sel = $(this).val();
        if (sel == "new") {
            $("#show_new").html('');
            $("#show_new").html(content);
        }
        else {
            $("#show_new").html('');

        }

        $("#btnAddEq").click(function () {
            var nomeq = $("#newEquipe").val();

            $.ajax({
                url: "/Employe/addEquipe",
                type: "POST",
                dataType: "JSON",
                data: { 'nom': nomeq },
                success: function (data) {
                    if (data.id != "") {

                        var opt = '<option value="' + data.id + '" selected >' + data.nom_equipe + '</option>';
                        $("#showdata").append(opt);
                        $("#new").hide();
                        $("#check-pr").html('<input type="checkbox" id="BtnAddProjet"><label for="BtnAddProjet" class="m-2">Ajouter equipe dans une projet</label></div> <div class="form-group" id="inp-projet"></div>');
                        $("#BtnAddProjet").click(function () {
                            var ch = $("#BtnAddProjet").is(':checked');
                            if (ch == true) {
                                $("#inp-projet").html('<label>Les projets</label><select id="showprojet" name="Projet.Id" "asp-for="Projet.Id" class="form-select mb-3"><option value="-2">--select projet--</option><option value="newpr">Ajouter nouveau projet</option></select><div id="show_pr"></div>');
                            } else {
                                $("#inp-projet").html('<div></div>')
                            }
                            //Ajouter equipe dans une projet
                            $.ajax({
                                url: "/Employe/GetProjet",
                                type: "POST",
                                dataType: "JSON",
                                contentType: false, // NEEDED, DON'T OMIT THIS (requires jQuery 1.6+)
                                processData: false, // NEEDED, DON'T OMIT THIS
                                success: function (data) {
                                    for (var i = 0; i < data.length; i++) {
                                        var opt = $('<option></option>');
                                        opt.attr('value', data[i].id).text(data[i].nom_projet);
                                        $("#showprojet").append(opt);
                                    }
                                }

                            });


                            //ajouter nouveau projet
                            $("#showprojet").change(function () {

                                var contente = '<div id="newpr"><fieldset><label> Nouveau projet</label><input class="form-control" id="newProjet"/><label> Date de debut</label><input type="date" class="form-control" id="dateDb"/><label>Date de fin</label><input type="date" class="form-control" id="dateFin"/><label>Etat</label><input class="form-control" id="etat"/><div class="d-flex justify-content-center mt-3"><button type="button" id="btnAddpr" class="btn btn-success  "><i class="mdi mdi-plus-circle me-2"></i>ajouter</button></div></fieldset></div>';

                                var selct = $(this).val();
                                if (selct == "newpr") {
                                    $("#show_pr").html('');
                                    $("#show_pr").html(contente);
                                }
                                else {
                                    $("#show_pr").html('');

                                }
                                $("#btnAddpr").click(function () {
                                    var nomp = $("#newProjet").val();
                                    var dateD = $("#dateDb").val();
                                    var dateF = $("#dateFin").val();
                                    var etat = $("#etat").val();

                                    $.ajax({
                                        url: "/Employe/addProjet",
                                        type: "POST",
                                        dataType: "JSON",
                                        data: { 'nomP': nomp, 'dateD': dateD, 'dateF': dateF, 'etat': etat },
                                        success: function (data) {

                                            var opt = '<option value="' + data.id + '" selected>' + data.nom_projet+'</option>';
                                            $("#showprojet").append(opt);


                                            $("#newpr").hide();
                                        }





                                    });


                                });

                            });



                        });
                    }
                    else {
                        swal("error!", data, "error");
                    }

                }


            });


        });

      

    });

    /* ajouter projet*/
    $("#BtnAddProjet").click(function () {
        var ch = $("#BtnAddProjet").is(':checked');
        if (ch == true) {
            $("#inp-projet").html('<label>Les projets</label><select id="showprojet" name="Projet.Id" "asp-for="Projet.Id" class="form-select mb-3"><option value="-2">--select projet--</option><option value="newpr">Ajouter nouveau projet</option></select><div id="show_pr"></div>');
        } else {
            $("#inp-projet").html('<div></div>')
        }
        //Ajouter equipe dans une projet
        $.ajax({
            url: "/Employe/GetProjet",
            type: "POST",
            dataType: "JSON",
            contentType: false, // NEEDED, DON'T OMIT THIS (requires jQuery 1.6+)
            processData: false, // NEEDED, DON'T OMIT THIS
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var opt = $('<option></option>');
                    opt.attr('value', data[i].id).text(data[i].nom_projet);
                    $("#showprojet").append(opt);
                }
            }

        });


        //ajouter nouveau projet
        $("#showprojet").change(function () {

            var contente = '<div id="newpr"><fieldset><label> Nouveau projet</label><input class="form-control" id="newProjet"/><label> Date de debut</label><input type="date" class="form-control" id="dateDb"/><label>Date de fin</label><input type="date" class="form-control" id="dateFin"/><label>Etat</label><input class="form-control" id="etat"/><div class="d-flex justify-content-center mt-3"><button type="button" id="btnAddpr" class="btn btn-success  "><i class="mdi mdi-plus-circle me-2"></i>ajouter</button></div></fieldset></div>';

            var selct = $(this).val();
            if (selct == "newpr") {
                $("#show_pr").html('');
                $("#show_pr").html(contente);
            }
            else {
                $("#show_pr").html('');

            }
            $("#btnAddpr").click(function () {
                var nomp = $("#newProjet").val();
                var dateD = $("#dateDb").val();
                var dateF = $("#dateFin").val();
                var etat = $("#etat").val();

                $.ajax({
                    url: "/Employe/addProjet",
                    type: "POST",
                    dataType: "JSON",
                    data: { 'nomP': nomp, 'dateD': dateD, 'dateF': dateF, 'etat': etat },
                    success: function (data) {

                        var opt = '<option value="' + data.id + '" selected>' + data.nom_projet + '</option>';
                        $("#showprojet").append(opt);


                        $("#newpr").hide();
                    }





                });


            });

        });



    });

    //on load check box projet checked and load info to select///////////////////////////////
    //Ajouter equipe dans une projet
    $.ajax({
        url: "/Employe/GetProjet",
        type: "POST",
        dataType: "JSON",
        contentType: false, // NEEDED, DON'T OMIT THIS (requires jQuery 1.6+)
        processData: false, // NEEDED, DON'T OMIT THIS
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if ($("#BtnAddProjet").val() == data[i].id) {
                    var opt = $('<option selected></option>');
                    opt.attr('value', data[i].id).text(data[i].nom_projet);
                    $("#showprojet").append(opt);
                }
                else {
               var opt = $('<option></option>');
                opt.attr('value', data[i].id).text(data[i].nom_projet);
                $("#showprojet").append(opt);
                }
               
            }
        }

    });
    //ajouter nouveau projet
    $("#showprojet").change(function () {

        var contente = '<div id="newpr"><fieldset><label> Nouveau projet</label><input class="form-control" id="newProjet"/><label> Date de debut</label><input type="date" class="form-control" id="dateDb"/><label>Date de fin</label><input type="date" class="form-control" id="dateFin"/><label>Etat</label><input class="form-control" id="etat"/><div class="d-flex justify-content-center mt-3"><button type="button" id="btnAddpr" class="btn btn-success  "><i class="mdi mdi-plus-circle me-2"></i>ajouter</button></div></fieldset></div>';

        var selct = $(this).val();
        if (selct == "newpr") {
            $("#show_pr").html('');
            $("#show_pr").html(contente);
        }
        else {
            $("#show_pr").html('');

        }
        $("#btnAddpr").click(function () {
            var nomp = $("#newProjet").val();
            var dateD = $("#dateDb").val();
            var dateF = $("#dateFin").val();
            var etat = $("#etat").val();

            $.ajax({
                url: "/Employe/addProjet",
                type: "POST",
                dataType: "JSON",
                data: { 'nomP': nomp, 'dateD': dateD, 'dateF': dateF, 'etat': etat },
                success: function (data) {

                    var opt = '<option value="' + data.id + '" selected>' + data.nom_projet + '</option>';
                    $("#showprojet").append(opt);


                    $("#newpr").hide();
                }





            });


        });

    });


});


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//ajouter equipe
$("#BtnAddEquipe").click(function () {

    /*var id = $("#BtnAddEquipe").val();*/
    var check = $("#BtnAddEquipe").is(':checked');
    if (check == true) {
        $("#inp-equipe").html('<div><label>Les equipe</label><select id="showdata" name="Equipe.Id" asp-for="Equipe.Id" class="form-select mb-3" ><option value="-2">--select equipe--</option><option value="new">Ajouter nouveau equipe</option></select ><div class="form-group mt-3"><div class="form-group inp-equipe" id="show_new"></div><div id="check-pr"></div> </div></div>');
    } else {
        $("#inp-equipe").html('<div></div>')
    }

    $.ajax({
        url: "/Employe/GetEquipe",
        type: "POST",
        dataType: "JSON",
        contentType: false, // NEEDED, DON'T OMIT THIS (requires jQuery 1.6+)
        processData: false, // NEEDED, DON'T OMIT THIS
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var opt = $('<option></option>');
                opt.attr('value', data[i].id).text(data[i].nom_equipe);

                //if (data[i].id == id)
                //{
                //    opt.attr("selected", "selected");
                //}


                $("#showdata").append(opt);
            }
        }

    });

   
    /*input new equipe*/


    $("#showdata").change(function () {

        var content = ' <div id="new"><fieldset><label> Nouveau equipe</label><input class="form-control" id="newEquipe"/><div class="d-flex justify-content-center mt-3"><button type="button" id="btnAddEq" class="btn btn-success"><i class="mdi mdi-plus-circle me-2"></i>ajouter</button></div></fieldset></div>';

        var sel = $(this).val();
        if (sel == "new") {
            $("#show_new").html('');
            $("#show_new").html(content);
        }
        else {
            $("#show_new").html('');

        }

        $("#btnAddEq").click(function () {
            var nomeq = $("#newEquipe").val();

            $.ajax({
                url: "/Employe/addEquipe",
                type: "POST",
                dataType: "JSON",
                data: { 'nom': nomeq },
                success: function (data) {
                    if (data.id != "") {

                        var opt = '<option value="' + data.id + '" selected >' + data.nom_equipe + '</option>';
                        $("#showdata").append(opt);
                        $("#new").hide();
                        $("#check-pr").html('<input type="checkbox" id="BtnAddProjet"><label for="BtnAddProjet" class="m-2">Ajouter equipe dans une projet</label></div> <div class="form-group" id="inp-projet"></div>');
                        $("#BtnAddProjet").click(function () {
                            var ch = $("#BtnAddProjet").is(':checked');
                            if (ch == true) {
                                $("#inp-projet").html('<label>Les projets</label><select id="showprojet" name="Projet.Id" "asp-for="Projet.Id" class="form-select mb-3"><option value="-2">--select projet--</option><option value="newpr">Ajouter nouveau projet</option></select><div id="show_pr"></div>');
                            } else {
                                $("#inp-projet").html('<div></div>')
                            }
                            //Ajouter equipe dans une projet
                            $.ajax({
                                url: "/Employe/GetProjet",
                                type: "POST",
                                dataType: "JSON",
                                contentType: false, // NEEDED, DON'T OMIT THIS (requires jQuery 1.6+)
                                processData: false, // NEEDED, DON'T OMIT THIS
                                success: function (data) {
                                    for (var i = 0; i < data.length; i++) {
                                        var opt = $('<option></option>');
                                        opt.attr('value', data[i].id).text(data[i].nom_projet);
                                        $("#showprojet").append(opt);
                                    }
                                }

                            });


                            //ajouter nouveau projet
                            $("#showprojet").change(function () {

                                var contente = '<div id="newpr"><fieldset><label> Nouveau projet</label><input class="form-control" id="newProjet"/><label> Date de debut</label><input type="date" class="form-control" id="dateDb"/><label>Date de fin</label><input type="date" class="form-control" id="dateFin"/><label>Etat</label><input class="form-control" id="etat"/><div class="d-flex justify-content-center mt-3"><button type="button" id="btnAddpr" class="btn btn-success  "><i class="mdi mdi-plus-circle me-2"></i>ajouter</button></div></fieldset></div>';

                                var selct = $(this).val();
                                if (selct == "newpr") {
                                    $("#show_pr").html('');
                                    $("#show_pr").html(contente);
                                }
                                else {
                                    $("#show_pr").html('');

                                }
                                $("#btnAddpr").click(function () {
                                    var nomp = $("#newProjet").val();
                                    var dateD = $("#dateDb").val();
                                    var dateF = $("#dateFin").val();
                                    var etat = $("#etat").val();

                                    $.ajax({
                                        url: "/Employe/addProjet",
                                        type: "POST",
                                        dataType: "JSON",
                                        data: { 'nomP': nomp, 'dateD': dateD, 'dateF': dateF, 'etat': etat },
                                        success: function (data) {

                                            var opt = '<option value="' + data.id + '" selected>' + data.nom_projet + '</option>';
                                            $("#showprojet").append(opt);


                                            $("#newpr").hide();
                                        }





                                    });


                                });

                            });



                        });
                    }
                    else {
                        swal("error!", data, "error");
                    }

                }


            });


        });



        if (sel > '0' && sel != "new") {
            $("#check-pr").html('<input type="checkbox" id="BtnAddProjet"><label for="BtnAddProjet" class="m-2">Ajouter equipe dans une projet</label></div> <div class="form-group" id="inp-projet"></div>');
        }
        else {
            $("#check-pr").html('<div></div>');
        }
        /* ajouter projet*/
        $("#BtnAddProjet").click(function () {
            var ch = $("#BtnAddProjet").is(':checked');
            if (ch == true) {
                $("#inp-projet").html('<label>Les projets</label><select id="showprojet" name="Projet.Id" "asp-for="Projet.Id" class="form-select mb-3"><option value="-2">--select projet--</option><option value="newpr">Ajouter nouveau projet</option></select><div id="show_pr"></div>');
            } else {
                $("#inp-projet").html('<div></div>')
            }
            //Ajouter equipe dans une projet
            $.ajax({
                url: "/Employe/GetProjet",
                type: "POST",
                dataType: "JSON",
                contentType: false, // NEEDED, DON'T OMIT THIS (requires jQuery 1.6+)
                processData: false, // NEEDED, DON'T OMIT THIS
                success: function (data) {
                    for (var i = 0; i < data.length; i++) {
                        var opt = $('<option></option>');
                        opt.attr('value', data[i].id).text(data[i].nom_projet);
                        $("#showprojet").append(opt);
                    }
                }

            });


            //ajouter nouveau projet
            $("#showprojet").change(function () {

                var contente = '<div id="newpr"><fieldset><label> Nouveau projet</label><input class="form-control" id="newProjet"/><label> Date de debut</label><input type="date" class="form-control" id="dateDb"/><label>Date de fin</label><input type="date" class="form-control" id="dateFin"/><label>Etat</label><input class="form-control" id="etat"/><div class="d-flex justify-content-center mt-3"><button type="button" id="btnAddpr" class="btn btn-success  "><i class="mdi mdi-plus-circle me-2"></i>ajouter</button></div></fieldset></div>';

                var selct = $(this).val();
                if (selct == "newpr") {
                    $("#show_pr").html('');
                    $("#show_pr").html(contente);
                }
                else {
                    $("#show_pr").html('');

                }
                $("#btnAddpr").click(function () {
                    var nomp = $("#newProjet").val();
                    var dateD = $("#dateDb").val();
                    var dateF = $("#dateFin").val();
                    var etat = $("#etat").val();

                    $.ajax({
                        url: "/Employe/addProjet",
                        type: "POST",
                        dataType: "JSON",
                        data: { 'nomP': nomp, 'dateD': dateD, 'dateF': dateF, 'etat': etat },
                        success: function (data) {

                            var opt = '<option value="' + data.id + '" selected>' + data.nom_projet + '</option>';
                            $("#showprojet").append(opt);


                            $("#newpr").hide();
                        }





                    });


                });

            });



        });

    });









});





//submit Create

$("#formemp").submit(function (event) {
    event.preventDefault();

    $("#ajtEml").prop("disabled", true);

    var fdu = new FormData();
    var files = $('#file')[0].files;
    fdu.append('file', files[0]);
    fdu.append('Nom', $('#Nom').val());
    fdu.append('Prenom', $('#Prenom').val());
    fdu.append('Adress', $('#Adress').val());
    fdu.append('Password', $('#Password').val());
    fdu.append('Tel', $('#Tel').val());
    fdu.append('Date_embauche', $('#date').val());
    fdu.append('CIN', $('#CIN').val());
    fdu.append('Email', $('#Email').val());
    fdu.append('Sexe', $('#Sexe').val());
    fdu.append('type', $('#type').val());
    fdu.append('Salaire', $('#Salaire').val());
    if ($('#showdata').val()) {
        fdu.append('Equipe.Id', $('#showdata').val());
        fdu.append('Equipe.Nom_equipe', "0");
    }
    else {
        fdu.append('Equipe.Id', 0);
        fdu.append('Equipe.Nom_equipe', "0");

    }
    if ($('#Categorie').val()) {
        fdu.append('Categorie.Id', $('#Categorie').val());
    }
    else {
        fdu.append('Categorie.Id', 0);
    }
    
    fdu.append('Projet.Id', $('#showprojet').val());
    fdu.append('EmailOld', $('#emailOld').val());
    fdu.append('typeOld', $('#typeOld').val());





    var checkequipe = $("#BtnAddEquipe").is(':checked');
    var checkprojet = $("#BtnAddProjet").is(':checked');
    if (checkequipe == true && checkprojet == true) {
        //ajouter equipe dans projet
        $.ajax({
            url: "/Employe/addEqPr",
            type: "POST",
            dataType: "JSON",
            data: fdu,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                if (data == "done") {
                    $.ajax({
                        url: "/Employe/CreateZ",
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
                                $("#ajtEml").prop("disabled", false);

                                location.reload();


                            }
                            else {
                                $("#ajtEml").prop("disabled", false);
                                swal("error!", data, "error")
                            }
                        },
                        error: function (data) {
                            swal("error!", 'error', "error")
                        }

                    });
                }
                else {
                    swal("error!", 'error', "error")
                }

             



            },
        });
        /* add employe*/
       
    }
    else {

   

        /* add employe*/
        $.ajax({
            url: "/Employe/CreateZ",
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
                    $("#ajtEml").prop("disabled", false);

                    location.reload();


                }
                else {
                    $("#ajtEml").prop("disabled", false);

                    swal("error!", data, "error")
                }
            },
            error: function (data) {
                swal("error!", 'error send mail', "error")
            }

        });

    }

});

$("#formEdit").submit(function (event) {
    event.preventDefault();
    var edt = new FormData();
    var files = $('#file')[0].files;
    edt.append('file', files[0]);
    edt.append('Image', $('#Image').val());
    edt.append('Id', $('#Id').val())
    edt.append('Nom', $('#Nom').val());
    edt.append('Prenom', $('#Prenom').val());
    edt.append('Adress', $('#Adress').val());
    edt.append('Password', $('#Password').val());
    edt.append('Tel', $('#Tel').val());
    edt.append('Date_embauche', $('#date').val());
    edt.append('CIN', $('#CIN').val());
    edt.append('Email', $('#Email').val());
    edt.append('Sexe', $('#Sexe').val());
    edt.append('type', $('#type').val());
    edt.append('Salaire', $('#Salaire').val());
    
    edt.append('Categorie.Id', $('#Categorie').val());
    edt.append('Equipe.Id', $('#showdata').val());
    edt.append('Projet.Id', $('#showprojet').val());
    edt.append('EmailOld', $('#emailOld').val());
    edt.append('typeOld', $('#typeOld').val());

    var checkequipe = $("#BtnAddEquipe").is(':checked');
    var checkprojet = $("#BtnAddProjet").is(':checked');
    if (checkequipe == true && checkprojet == true) {
        //ajouter equipe dans projet
        $.ajax({
            url: "/Employe/addEqPr",
            type: "POST",
            dataType: "JSON",
            data: edt,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                if (data == "done") {
                    $.ajax({
                        url: "/Employe/Editz",
                        type: "POST",
                        dataType: "JSON",
                        data: edt,
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
                                swal("error!", "", "error")
                            }
                        }
                    });
                }
                else {
                    swal("error!", "", "error")
                }

            },
        });
    
    }

    else {

    
    /* edit employe*/
    $.ajax({
        url: "/Employe/Editz",
        type: "POST",
        dataType: "JSON",
        data: edt,
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
                swal("error!", "", "error")
            }
        }
    });
    }
})



/* display Categorie*/
$("#type").change(function () {

    var chec = $("#type").val();
    if (chec == 'overier') {
        $("#categorieO").html('');
        $("#categorieO").html('<div><label>Fonctionnalite</label><select asp-for="Categorie.Id" id="Categorie" class="form-select mb-3" ><option value="-2">--select fonctionnalite--</option></select></div>');
        $.ajax({
            url: "/Employe/GetCategorie",
            type: "POST",
            dataType: "JSON",
            contentType: false, // NEEDED, DON'T OMIT THIS (requires jQuery 1.6+)
            processData: false, // NEEDED, DON'T OMIT THIS
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var opt = $('<option value="' + data[i].id + '" >' + data[i].fonctionnalite + '</option>');

                    $("#Categorie").append(opt);
                }

            }
        });
    }
    else {
        $("#categorieO").html('<div></div>');
    }


});

$(document).ready(function () {
    var chec = $("#type").val();
    if (chec == 'overier') {
        $("#categorieO").html('');
        $("#categorieO").html('<div><label>Fonctionnalite</label><select asp-for="Categorie.Id" id="Categorie" class="form-select mb-3" ><option value="-2">--select fonctionnalite--</option></select></div>');
        $.ajax({
            url: "/Employe/GetCategorie",
            type: "POST",
            dataType: "JSON",
            contentType: false, // NEEDED, DON'T OMIT THIS (requires jQuery 1.6+)
            processData: false, // NEEDED, DON'T OMIT THIS
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var opt = $('<option value="' + data[i].id + '" >' + data[i].fonctionnalite + '</option>');

                    $("#Categorie").append(opt);
                }

            }
        });
    }
    else {
        $("#categorieO").html('<div></div>');
    }
  
});


/*});*/

/* Show Popup Create*/
showInPopup = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#modalcreate").html("");
            $("#modalcreate").html(res);
            $("#exampleModalLongTitle").html(title);
            $("#exampleModalLong").modal('show');



        }
    })
};
function closemodal(close) {
    $(close).modal('hide')
};
//genrate image
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#Image1').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

$("#file").change(function () {
    readURL(this);
});




function randString(id) {
    var set = "a-z,A-Z,0-9,#";
    var dataSet = set.split(',');
    var possible = '';
    if ($.inArray('a-z', dataSet) >= 0) {
        possible += 'abcdefghijklmnopqrstuvwxyz';
    }
    if ($.inArray('A-Z', dataSet) >= 0) {
        possible += 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
    }
    if ($.inArray('0-9', dataSet) >= 0) {
        possible += '0123456789';
    }
    if ($.inArray('#', dataSet) >= 0) {
        possible += '+-*/!.';
    }
    var text = '';
    for (var i = 0; i < 8; i++) {
        text += possible.charAt(Math.floor(Math.random() * possible.length));
    }
    return text;
}

// Create a new password on page load


// Create a new password
$(".getNewPass").click(function () {
    var field = $("#Password");
    field.val(randString(field));
});



function myFunction() {
    var x = document.getElementById("Password");
    if (x.type === "password") {
        x.type = "text";
    } else {
        x.type = "password";
    }
}






/* PARTIE EQUIPE*/


/*ADD PROJET*/

$("#AddProjet").click(function () {
    var ch = $("#AddProjet").is(':checked');
    if (ch == true) {
        $("#show-projet").html('<label>Les projets</label><select id="OpProjet" name="Projet.Id" "asp-for="Projet.Id" class="form-select mb-3"><option value="-2">--select projet--</option><option value="newpr">Ajouter nouveau projet</option></select><div id="show_pro"></div>');
    } else {
        $("#show-projet").html('<div></div>')
    }
    //Ajouter equipe dans une projet
    $.ajax({
        url: "/Equipe/AJprojet",
        type: "GET",
        dataType: "JSON",
        contentType: false, // NEEDED, DON'T OMIT THIS (requires jQuery 1.6+)
        processData: false, // NEEDED, DON'T OMIT THIS
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var opt = $('<option></option>');
                opt.attr('value', data[i].id).text(data[i].nom_projet);
                $("#OpProjet").append(opt);
               
            }
        }

    });


    //ajouter nouveau projet
    $("#OpProjet").change(function () {

        var contente = '<div id="newpr"><fieldset><label> Nouveau projet</label><input class="form-control" id="newProjet"/><label> Date de debut</label><input type="date" class="form-control" id="dateDb"/><label>Date de fin</label><input type="date" class="form-control" id="dateFin"/><label>Etat</label><input class="form-control" id="etat"/><div class="d-flex justify-content-center mt-3"><button type="button" id="btnAddpr" class="btn btn-success  "><i class="mdi mdi-plus-circle me-2"></i>ajouter</button></div></fieldset></div>';

        var selct = $(this).val();
        if (selct == "newpr") {
            $("#show_pro").html('');
            $("#show_pro").html(contente);
        }
        else {
            $("#show_pro").html('');

        }
        $("#btnAddpr").click(function () {
            var nomProjet = $("#newProjet").val();
            var dateDe = $("#dateDb").val();
            var dateFin = $("#dateFin").val();
            var Etat = $("#etat").val();

            $.ajax({
                url: "/Equipe/newProjet",
                type: "POST",
                dataType: "JSON",
                data: { 'nomProjet': nomProjet, 'dateDe': dateDe, 'dateFin': dateFin, 'Etat': Etat },
                success: function (data) {

                    
                    if (data == "errer") {
                        swal("error!", "nom projet déja existe !!!", "error");
                    }
                    else {
                        var opt = $('<option></option>');
                        opt.attr('selected', 'value', data.id).text(data.nom_projet);
                        $("#OpProjet").append(opt);
                        $("#newpr").hide();
                    }

                }

            });
        });
    });
});

$(document).ready(function () {
    var valP = $("#AddProjet").val();

    var ch = $("#AddProjet").is(':checked');
    if (ch == true) {
        $("#show-projet").html('<label>Les projets</label><select id="OpProjet" name="Projet.Id" "asp-for="Projet.Id" class="form-select mb-3"><option value="-2">--select projet--</option><option value="newpr">Ajouter nouveau projet</option></select><div id="show_pro"></div>');
    } else {
        $("#show-projet").html('<div></div>')
    }
    //Ajouter equipe dans une projet
    $.ajax({
        url: "/Equipe/AJprojet",
        type: "GET",
        dataType: "JSON",
        contentType: false, // NEEDED, DON'T OMIT THIS (requires jQuery 1.6+)
        processData: false, // NEEDED, DON'T OMIT THIS
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var opt = $('<option></option>');
                opt.attr('value', data[i].id).text(data[i].nom_projet);
                $("#OpProjet").append(opt);
                if (data[i].id == valP) {
                    opt.attr("selected", "selected");
                }
            }
        }

    });


    //ajouter nouveau projet
    $("#OpProjet").change(function () {

        var contente = '<div id="newpr"><fieldset><label> Nouveau projet</label><input class="form-control" id="newProjet"/><label> Date de debut</label><input type="date" class="form-control" id="dateDb"/><label>Date de fin</label><input type="date" class="form-control" id="dateFin"/><label>Etat</label><input class="form-control" id="etat"/><div class="d-flex justify-content-center mt-3"><button type="button" id="btnAddpr" class="btn btn-success  "><i class="mdi mdi-plus-circle me-2"></i>ajouter</button></div></fieldset></div>';

        var selct = $(this).val();
        if (selct == "newpr") {
            $("#show_pro").html('');
            $("#show_pro").html(contente);
        }
        else {
            $("#show_pro").html('');

        }
        $("#btnAddpr").click(function () {
            var nomProjet = $("#newProjet").val();
            var dateDe = $("#dateDb").val();
            var dateFin = $("#dateFin").val();
            var Etat = $("#etat").val();

            $.ajax({
                url: "/Equipe/newProjet",
                type: "POST",
                dataType: "JSON",
                data: { 'nomProjet': nomProjet, 'dateDe': dateDe, 'dateFin': dateFin, 'Etat': Etat },
                success: function (data) {

                    if (data == "errer") {
                        swal("error!", "nom projet déja existe !!!", "error");
                    }
                    else {
                        var opt = $('<option></option>');
                        opt.attr('selected', 'value', data.id).text(data.nom_projet);
                        $("#OpProjet").append(opt);
                        $("#newpr").hide();
                    }

                }

            });
        });
    });

});

/*ajouter employe*/

$("#AddEmploye").click(function () {
    var ch = $("#AddEmploye").is(':checked');
    if (ch == true) {
        $("#show-employe").html('<select multiple asp-for="Employes" name="Employes" id="Employes" class="form-select mb-3"></select>');

        $("#Employes").select2({
            dropdownParent: $('#exampleModalLong'),
            Placeholder: 'Employes',
            ajax: {
                url: "/Equipe/GetEmploye",
                dataType: "json",
                data: function (params) {
                    return {
                        search: params.term
                    };
                },
                processResults: function (data, params) {
                    return {
                        results: data

                    };
                }
            }
        });

    } else {
        $("#show-employe").html('<div></div>')
    }

   
});

$(document).ready(function () {
    var ch = $("#AddEmploye").is(':checked');
    if (ch == true) {
        $("#show-employe").html('<select multiple asp-for="Employes" name="Employes" id="Employes" class="form-select mb-3"></select>');

        $("#Employes").select2({
            dropdownParent: $('#exampleModalLong'),
            Placeholder: 'Employes',
            ajax: {
                url: "/Equipe/GetEmploye",
                dataType: "json",
                data: function (params) {
                    return {
                        search: params.term
                    };
                   
                },
                processResults: function (data, params) {
                    return {
                        results: data

                    };
                   

                }
            }
            

        });


        var str = $("#w3review").val();
        var strLines = str.split("&");
        for (var i in strLines) {
            var obj = JSON.parse(strLines[i]);
            var newOption = new Option(obj.text, obj.id, true, true);
            $('#Employes').append(newOption).trigger('change');
        }
        
       

    } else {
        $("#show-employe").html('<div></div>')
    }
   
   

});


/*form create equipe*/

$("#FormEquipe").submit(function (event) {
    event.preventDefault();
    var eqp = new FormData();
    eqp.append('Nom_equipe', $('#Nom_equipe').val());
    if ($('#Employes').val()) {
        eqp.append('Employes', $('#Employes').val());
    }
    else {
        eqp.append('Employes', '-1');
    }
    eqp.append('Projet.Id', $('#OpProjet').val());

    

        $.ajax({
            url: "/Equipe/Empl",
            type: "POST",
            dataType: "JSON",
            data : eqp,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                if (data == "xx") {
                    Swal.fire({
                        position: 'center',
                        icon: 'success',
                        title: 'success',
                        showConfirmButton: false,
                        timer: 800
                    })
                   /* swal("success", "", "success")*/
                    location.reload();
                } else {
                    swal("error!", data, "error")
                }

                

            },
        });

});


/*edite equipe*/

$("#EditeEquipe").submit(function (event) {
    event.preventDefault();
    var etp = new FormData();
    etp.append('Nom_equipe', $('#Nom_equipe').val());
    if ($('#Employes').val()) {
        etp.append('Employes', $('#Employes').val());
    }
    else {
        etp.append('Employes', '-1');
    }
    etp.append('Id', $('#Id').val())
    etp.append('Projet.Id', $('#OpProjet').val());

  

    $.ajax({
        url: "/Equipe/Esiteeq",
        type: "POST",
        dataType: "JSON",
        data: etp,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data == "zz") {
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'success',
                    showConfirmButton: false,
                    timer: 800
                })
                location.reload();
            } else {
                swal("error!", data, "error")
            }



        },
    });

});





/*PARTIE PROJET*/



/*add les equipes*/

$("#AddListEq").click(function () {
    var chk = $("#AddListEq").is(':checked');
    if (chk == true) {
        $("#show-eq").html('<select multiple asp-for="Equipes" name="Equipes" id="Equipes" class="form-select mb-3"></select>');

        $("#Equipes").select2({
            dropdownParent: $('#exampleModalLong'),
            Placeholder: 'Equipes',
            ajax: {
                url: "/Projet/GetEQ",
                dataType: "json",
                data: function (params) {
                    return {
                        search: params.term
                    };
                },
                processResults: function (data, params) {
                    return {
                        results: data

                    };
                }
            }
        });

    } else {
        $("#show-eq").html('<div></div>')
    }


});

/*Create Projet*/

$("#FormProjet").submit(function (event) {
    event.preventDefault();
    var prj = new FormData();
    prj.append('Nom_projet', $('#Nom_projet').val());
    if ($('#Equipes').val()) {
        prj.append('Equipes', $('#Equipes').val());
    }
    else {
        prj.append('Equipes', '-1');
    }
    prj.append('Date_debut', $('#Date_debut').val());
    prj.append('Date_fin', $('#Date_fin').val());
    prj.append('Etat', $('#Etat').val());



    $.ajax({
        url: "/Projet/CreatP",
        type: "POST",
        dataType: "JSON",
        data: prj,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data == "xx") {
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'success',
                    showConfirmButton: false,
                    timer: 800
                });
                location.reload();
            } else {
                swal("error!", data, "error")
            }



        },
    });

});

/*Edite Projet*/

$("#EditPrj").submit(function (event) {
    event.preventDefault();
    var prj = new FormData();
    prj.append('Nom_projet', $('#Nom_projet').val());
    if ($('#Equipes').val()) {
        prj.append('Equipes', $('#Equipes').val());
    }
    else {
        prj.append('Equipes', '-1');
    }
    prj.append('Date_debut', $('#Date_debut').val());
    prj.append('Date_fin', $('#Date_fin').val());
    prj.append('Etat', $('#Etat').val());
    prj.append('Id', $('#Id').val())


    $.ajax({
        url: "/Projet/EditeP",
        type: "POST",
        dataType: "JSON",
        data: prj,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data == "zz") {
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'success',
                    showConfirmButton: false,
                    timer: 800
                });
                location.reload();
            } else {
                swal("error!", data, "error")
            }



        },
    });

});




    $(document).ready(function () {
        var chk = $("#AddListEq").is(':checked');
        if (chk == true) {
            $("#show-eq").html('<select multiple asp-for="Equipes" name="Equipes" id="Equipes" class="form-select mb-3"></select>');

            $("#Equipes").select2({
                 dropdownParent: $('#exampleModalLong'),
                Placeholder: 'Equipes',
                ajax: {
                    url: "/Projet/GetEQ",
                    dataType: "json",
                    data: function (params) {
                        return {
                            search: params.term
                        };
                    },
                    processResults: function (data, params) {
                        return {
                            results: data

                        };
                    }
                }
            });


            var str = $("#w3review").val();
            var strLines = str.split("&");
            for (var i in strLines) {
                var obj = JSON.parse(strLines[i]);
                var newOption = new Option(obj.text, obj.id, true, true);
                $('#Equipes').append(newOption).trigger('change');
            }

        } else {
            $("#show-eq").html('<div></div>')
        }


    });
    
/*PARTIE CATEGORIE*/


$("#AddOuvrier").click(function () {
    var ch = $("#AddOuvrier").is(':checked');
    if (ch == true) {
        $("#show-Ouvrier").html('<select multiple asp-for="Employes" name="Employes" id="Employes" class="form-select mb-3"></select>');

        $("#Employes").select2({
            dropdownParent: $('#exampleModalLong'),
            Placeholder: 'Employes',
            ajax: {
                url: "/Categorie/GetOuvrier",
                dataType: "json",
                data: function (params) {
                    return {
                        search: params.term
                    };
                },
                processResults: function (data, params) {
                    return {
                        results: data

                    };
                }
            }
        });

    } else {
        $("#show-Ouvrier").html('<div></div>')
    }


});



/*add categorier*/

$("#FormCat").submit(function (event) {
    event.preventDefault();
    var cat = new FormData();
    cat.append('Fonctionnalite', $('#Fonctionnalite').val());
    if ($('#Employes').val()) {
        cat.append('Employes', $('#Employes').val());
    }
    else {
        cat.append('Employes', '-1');
    }




    $.ajax({
        url: "/Categorie/ouvr",
        type: "POST",
        dataType: "JSON",
        data: cat,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data == "cat") {
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'success',
                    showConfirmButton: false,
                    timer: 800
                });
                location.reload();
            } else {
                swal("error!", data, "error")
            }



        },
    });

});


/*edite categorie*/
$("#editCat").submit(function (event) {
    event.preventDefault();
    var ecat = new FormData();
    ecat.append('Fonctionnalite', $('#Fonctionnalite').val());
    if ($('#Employes').val()) {
        ecat.append('Employes', $('#Employes').val());
    }
    else {
        ecat.append('Employes', '-1');
    }
    ecat.append('Id', $('#Id').val())




    $.ajax({
        url: "/Categorie/Editouvr",
        type: "POST",
        dataType: "JSON",
        data: ecat,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data == "cate") {
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'success',
                    showConfirmButton: false,
                    timer: 800
                });
                location.reload();
            } else {
                swal("error!", data, "error")
            }



        },
    });

});

$(document).ready(function () {
    var ch = $("#AddOuvrier").is(':checked');
    if (ch == true) {
        $("#show-Ouvrier").html('<select multiple asp-for="Employes" name="Employes" id="Employes" class="form-select mb-3"></select>');

        $("#Employes").select2({
            dropdownParent: $('#exampleModalLong'),
            Placeholder: 'Employes',
            ajax: {
                url: "/Categorie/GetOuvrier",
                dataType: "json",
                data: function (params) {
                    return {
                        search: params.term
                    };
                },
                processResults: function (data, params) {
                    return {
                        results: data

                    };
                }
            }
        });
        var str = $("#w3review").val();
        var strLines = str.split("&");
        for (var i in strLines) {
            var obj = JSON.parse(strLines[i]);
            var newOption = new Option(obj.text, obj.id, true, true);
            $('#Employes').append(newOption).trigger('change');
        }

    } else {
        $("#show-Ouvrier").html('<div></div>')
    }
   
});



$("#Btndetaleq").click(function(){
    var eq = $(this).is(':checked');
    if (eq == true) {
        $('#detaleq').show();

    }
    else {
        $('#detaleq').hide();
    }
});

$("#Btndetalpr").click(function () {
    var eq = $(this).is(':checked');
    if (eq == true) {
        $('#detalpr').show();

    }
    else {
        $('#detalpr').hide();
    }
});