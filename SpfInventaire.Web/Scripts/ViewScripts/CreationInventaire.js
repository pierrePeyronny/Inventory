function CreationInventaire() {
    var _this = this;
    _this.AjaxService = new AjaxService();

    _this.listInventaires = $('#listInventaires input');
    _this.listBlocs = $('#listBlocs');
    _this.listMateriel = $('#listMateriel');

    _this.blocGestionStocks = $('#blocGestionStocks');


    //Déclaration des boutons de la page
    _this.buttonAddBloc = new BoutonInventaire({ button: '#buttonAddBloc', loader: '#loaderBloc' });
    _this.buttonEditBloc = new BoutonInventaire({ button: '#buttonEditBloc', loader: '#loaderBloc' });
    _this.buttonSortBloc = new BoutonInventaire({ button: '#buttonSortBloc', loader: '#loaderBloc' });


    _this.buttonAddMateriel = new BoutonInventaire({ button: '#buttonAddMateriel', loader: '#loaderMateriel' });
    _this.buttonEditMateriel = new BoutonInventaire({ button: '#buttonEditMateriel', loader: '#loaderMateriel' });
    _this.buttonSortMateriel = new BoutonInventaire({ button: '#buttonSortMateriel', loader: '#loaderMateriel' });

    _this.buttonAddStock = new BoutonInventaire({ button: '#buttonAddStock', loader: '#loaderMateriel' });

    //Initialisation des boutons
    _this.buttonAddBloc.Disable();
    _this.buttonEditBloc.Disable();
    _this.buttonSortBloc.Disable();
    _this.buttonAddMateriel.Disable();
    _this.buttonEditMateriel.Disable();
    _this.buttonSortMateriel.Disable();
    _this.buttonAddStock.Disable();

    //Variables global
    _this.inventaireId = null;
    _this.blocInventaireId = null;
    _this.materielId = null;

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //FONCTIONS
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    ///////////Blocx Inventaire ///////////////////
    function ChargementBlocsInventaire(options) {

        var findBloc = false;

        _this.AjaxService.BlocInventaire.GetListBlocsInventaire({ inventaireId: options.inventaireId }, function (data) {
            var markup = "<option value='0'>Sélectionner un Bloc</option>";
            for (var x = 0; x < data.length; x++) {

                if (options.blocInventaireId > 0 && options.blocInventaireId == data[x].Value) {
                    markup += "<option value=" + data[x].Value + " selected>" + data[x].Text + "</option>";
                    findBloc = true;
                } else {
                    markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
                }
            }
            $(_this.listBlocs).html(markup);

            if (!findBloc) {
                _this.buttonEditBloc.Disable();

                //mise à zéro des autres listes
                InitListMateriel();
            }
        });
    };


    ////////////// Matériel /////////////////////////////
    function InitListMateriel() {
        var initList = "<option value=''>Aucun Matériel</option>";
        $(_this.listMateriel).html(initList);

        _this.buttonAddMateriel.Disable();
        _this.buttonEditMateriel.Disable();
        _this.buttonSortMateriel.Disable();
        _this.buttonAddStock.Disable();
        $(_this.blocGestionStocks).html('');

    };

    function ChargementMateriel(options) {

        var findMateriel = false;

        _this.AjaxService.Materiel.GetListMateriel({ blocInventaireId: options.blocInventaireId }, function (data) {
            var markup = "<option value='0'>Sélectionner un Matériel</option>";
            for (var x = 0; x < data.length; x++) {

                if (options.materielId > 0 && options.materielId == data[x].Value) {
                    markup += "<option value=" + data[x].Value + " selected>" + data[x].Text + "</option>";
                    findMateriel = true;
                } else {
                    markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
                }
            }
            $(_this.listMateriel).html(markup);

            if (!findMateriel) {
                _this.buttonEditMateriel.Disable();
                _this.buttonAddStock.Disable();
            }
        });
    };

    /////////////// Stocks //////////////////////////////
    function ChargementStock(options) {
        _this.buttonAddStock.ActivateLoader();
        var idMateriel = options.materielId;
        if (idMateriel > 0)
        {
            _this.AjaxService.GestionStocks.GetListStockByMateriel({ materielId: idMateriel }, function (data) {
                _this.blocGestionStocks.html(data);
            });
        } else {
            _this.blocGestionStocks.html('');
        }
        _this.buttonAddStock.DisableLoader();
    };

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Evenements
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /////////////// Inventaire //////////////////////////
    _this.listInventaires.change(function (e) {

        _this.inventaireId = $(this).val();
        ChargementBlocsInventaire({ inventaireId: _this.inventaireId, blocInventaireId: 0 });

        _this.buttonAddBloc.Activate();
        _this.buttonSortBloc.Activate();

        _this.buttonAddMateriel.Disable();
        _this.buttonSortMateriel.Disable();

        InitListMateriel();
    });


    /////////////// Bloc Inventaire //////////////////////////
    _this.listBlocs.change(function (e) {
        _this.blocInventaireId = $(this).val();


        if (_this.blocInventaireId > 0) {
            //Activation des boutons
            _this.buttonEditBloc.Activate();
            _this.buttonAddMateriel.Activate();
            _this.buttonSortMateriel.Activate();

            //Chargement de la liste du Matériel
            ChargementMateriel({ blocInventaireId: _this.blocInventaireId, materielId: 0 })
        }
        else {
            _this.buttonEditBloc.Disable();
            _this.buttonAddMateriel.Disable();
            _this.buttonSortMateriel.Disable();
            InitListMateriel();
        }

    });

    //Clic sur le bouton d'ajout d'un Bloc Inventaire
    _this.buttonAddBloc.button.click(function (e) {

        //lancement du loader
        _this.buttonAddBloc.ActivateLoader();

        _this.AjaxService.BlocInventaire.GetFormBlocInventaire({ inventaireId: _this.inventaireId }, function (data) {
            //arret du loader
            _this.buttonAddBloc.DisableLoader();

            var options = {
                show: true,
                data: data,
                onCloseCallback: function (datas) {
                    ChargementBlocsInventaire({ inventaireId: _this.inventaireId, blocInventaireId: datas.blocInventaireId })
                },
                title: 'Création Bloc Inventaire'
            }
            modalPopUpInstance.initPopUp(options);
        });
    });

    //Clic sur le bouton d'édition d'un Bloc
    _this.buttonEditBloc.button.click(function (e) {

        //lancement du loader
        _this.buttonEditBloc.ActivateLoader();

        //récupération du formulaire
        _this.AjaxService.BlocInventaire.GetFormEditBlocInventaire({ id: _this.listBlocs.val() }, function (data) {
            //arret du loader
            _this.buttonEditBloc.DisableLoader();

            var options = {
                show: true,
                data: data,
                onCloseCallback: function (datas) {
                    ChargementBlocsInventaire({ inventaireId: _this.inventaireId, blocInventaireId: datas.blocInventaireId })
                },
                title: 'Modification Bloc Inventaire'
            }
            modalPopUpInstance.initPopUp(options);
        });

    });

    //Clic sur le bouton de modification de l'ordre des Blocs
    _this.buttonSortBloc.button.click(function (e) {
        //lancement du loader
        _this.buttonSortBloc.ActivateLoader();

        _this.AjaxService.CreationInventaire.GetFormSort({ rechercheId: _this.inventaireId, objectList: 'BlocInventaire' }, function (data) {
            //arret du loader
            _this.buttonSortBloc.DisableLoader();

            var options = {
                show: true,
                data: data,
                onCloseCallback: function (datas) {
                    ChargementBlocsInventaire({ inventaireId: _this.inventaireId, blocInventaireId: _this.blocInventaireId })
                },
                title: 'Ordre des Blocs'
            }
            modalPopUpInstance.initPopUp(options);
        });

    });


    /////////////// Matériel //////////////////////////////
    _this.listMateriel.change(function (e) {

        _this.materielId = $(this).val();

        if (_this.materielId > 0) {
            _this.buttonEditMateriel.Activate();
            _this.buttonAddStock.Activate();
        } else {
            _this.buttonEditMateriel.Disable();
            _this.buttonAddStock.Disable();
        }
        //Gestion affichage des stocks
        ChargementStock({ materielId: _this.materielId });
    });

    //Clic sur le bouton d'ajout d'un Matériel
    _this.buttonAddMateriel.button.click(function (e) {
        //lancement du loader
        _this.buttonAddMateriel.ActivateLoader();

        _this.AjaxService.Materiel.GetFormMateriel({ blocInventaireId: _this.blocInventaireId }, function (data) {

            //arret du loader
            _this.buttonAddMateriel.DisableLoader();

            var options = {
                show: true,
                data: data,
                onCloseCallback: function (datas) {
                    ChargementMateriel({ blocInventaireId: _this.blocInventaireId, materielId: datas.materielId })
                },
                title: 'Création Matériel'
            }
            modalPopUpInstance.initPopUp(options);
        });
    });

    //Clic sur le bouton de modification d'un Matériel
    _this.buttonEditMateriel.button.click(function (e) {

        _this.buttonEditMateriel.ActivateLoader();

        _this.AjaxService.Materiel.GetFormEditMateriel({ id: _this.listMateriel.val() }, function (data) {
            //arret du loader
            _this.buttonEditMateriel.DisableLoader();

            var options = {
                show: true,
                data: data,
                onCloseCallback: function (datas) {
                    ChargementMateriel({ blocInventaireId: _this.blocInventaireId, materielId: datas.materielId })
                },
                title: 'Modification Détail'
            }
            modalPopUpInstance.initPopUp(options);
        });
    });

    //Clic sur le bouton de modification de l'ordre du Matériel
    _this.buttonSortMateriel.button.click(function (e) {
        //lancement du loader
        _this.buttonSortMateriel.ActivateLoader();

        _this.AjaxService.CreationInventaire.GetFormSort({ rechercheId: _this.blocInventaireId, objectList: 'Materiel' }, function (data) {

            //arret du loader
            _this.buttonSortMateriel.DisableLoader();

            var options = {
                show: true,
                data: data,
                onCloseCallback: function (datas) {
                    ChargementMateriel({ blocInventaireId: _this.blocInventaireId, materielId: _this.materielId })
                },
                title: 'Ordre du Matériel'
            }
            modalPopUpInstance.initPopUp(options);
        });
    });


    /////////////// Stocks //////////////////////////////
    _this.buttonAddStock.button.click(function (e) {
        //lancement du loader
        _this.buttonAddStock.ActivateLoader();

        _this.AjaxService.GestionStocks.GetFormAddStock({ materielId: _this.materielId }, function (data) {

            //arret du loader
            _this.buttonAddStock.DisableLoader();

            var options = {
                show: true,
                data: data,
                onCloseCallback: function (datas) {
                    ChargementStock({ materielId: _this.materielId })
                },
                title: 'Création d\'un Stock'
            }
            modalPopUpInstance.initPopUp(options);
        });
    });

    $('body').on('click', '.editButtonTDB', function (e) {
        e.preventDefault();
        var stockId = $(this).data('id');

        _this.buttonAddStock.ActivateLoader();

        _this.AjaxService.GestionStocks.GetFormEditStock({ id: stockId }, function (data) {
            _this.buttonAddStock.DisableLoader();

            var options = {
                show: true,
                data: data,
                onCloseCallback: function (datas) {
                    ChargementStock({ materielId: _this.materielId })
                },
                title: 'Modification d\'un Stock'
            }
            modalPopUpInstance.initPopUp(options);
        });
    });


    $('body').on('click', '.deleteButton', function (e) {
        e.preventDefault();
        var stockId = $(this).data('id');

        if (confirm('Supprimer le Stock ?')) {
            _this.buttonAddStock.ActivateLoader();

            _this.AjaxService.GestionStocks.DeleteStock({ id: stockId }, function (data) {
                ChargementStock({ materielId: _this.materielId })
            });
            _this.buttonAddStock.DisableLoader();
        }
    });

    $('body').on('click', '.TransfertButtonTDB', function (e) {
        e.preventDefault();
        var stockId = $(this).data('id');
        _this.buttonAddStock.ActivateLoader();

        _this.AjaxService.GestionStocks.GetFormTransfertStock({ id: stockId }, function (data) {
            _this.buttonAddStock.DisableLoader();

            var options = {
                show: true,
                data: data,
                onCloseCallback: function (datas) {
                    ChargementStock({ materielId: _this.materielId })
                },
                title: 'Transfert d\'un Stock'
            }
            modalPopUpInstance.initPopUp(options);
        });
        _this.buttonAddStock.DisableLoader();
    });


};//End Object