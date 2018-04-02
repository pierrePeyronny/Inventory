
function SaisieInventaire(options) {
    var _this = this;
    _this.AjaxService = new AjaxService();

    _this.LoaderInventaire = new BoutonInventaire({ button: '', loader: '#loaderGeneral' });
    _this.listInventaire = $('#listInventaires');
    _this.divAffichageInventaire = $('#divAffichageInventaire');
    _this.btValidation = new BoutonInventaire({ button: '#btValidation', loader: '#loaderValidation' });
    _this.divValidationInventaire = $('#messageValidationInventaire');
    _this.divErreurValidationInventaire = $('#messageErreurValidationInventaire');

    _this.InventaireID = null;
    _this.MaterielID = null;

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //FONCTIONS
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Evenements
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    _this.listInventaire.change(function (e) {
        //On efface les messages precedent
        _this.divValidationInventaire.hide();
        _this.divErreurValidationInventaire.hide();

        _this.LoaderInventaire.ActivateLoader();
        _this.InventaireID = $(this).val();

        if (_this.InventaireID > 0)
        {
            _this.AjaxService.Inventaire.GetFormSaisieInventaire({ id: _this.InventaireID }, function (data) {
                _this.divAffichageInventaire.html(data);
                $(_this.btValidation.button).show();
                _this.LoaderInventaire.DisableLoader();
            });
        } else {
            _this.divAffichageInventaire.html("");
            _this.btValidation.button.hide();
            _this.LoaderInventaire.DisableLoader();
        }

    });


    $('body').on('click', '.presenceMateriel .signaler', function (e) {

        //lancement du loader
        _this.LoaderInventaire.ActivateLoader();

        //Récupération de l'id Matériel
        _this.MaterielID = $(this).data('id');

        _this.AjaxService.Ticket.GetFormCreateTicket({ materielID: _this.MaterielID }, function (data) {
            //arret du loader
            _this.LoaderInventaire.DisableLoader();

            var options = {
                show: true,
                data: data,
                onCloseCallback: function (datas) {
                    //Nothing to do
                },
                title: 'Signaler un problème'
            }
            modalPopUpInstance.initPopUp(options);
        });
    });

    $('body').on('click', '.presenceMateriel .transfert', function (e) {

        //lancement du loader
        _this.LoaderInventaire.ActivateLoader();

        //Récupération de l'id Matériel
        _this.MaterielID = $(this).data('id');

        _this.AjaxService.SortieStock.GetFormCreateSortieStock({ materielID: _this.MaterielID }, function (data) {
            //arret du loader
            _this.LoaderInventaire.DisableLoader();

            var options = {
                show: true,
                data: data,
                onCloseCallback: function (datas) {
                    //Nothing to do
                },
                title: 'Transfert de stock'
            }
            modalPopUpInstance.initPopUp(options);
        });
    });

    $('body').on('click', '.presenceMateriel .detailTicket', function (e) {

        //lancement du loader
        _this.LoaderInventaire.ActivateLoader();

        //Récupération de l'id Matériel
        _this.MaterielID = $(this).data('id');

        _this.AjaxService.Ticket.GetFormDetailTicket({ materielID: _this.MaterielID }, function (data) {
            //arret du loader
            _this.LoaderInventaire.DisableLoader();

            var options = {
                show: true,
                data: data,
                onCloseCallback: function (datas) {
                    //Nothing to do
                },
                title: 'Détail du Problème'
            }
            modalPopUpInstance.initPopUp(options);
        });
    });


    _this.btValidation.button.click(function () {

        if (_this.InventaireID > 0)
        {
            _this.btValidation.ActivateLoader();

            _this.AjaxService.ValidationInventaire.Validation({ inventaireId: _this.InventaireID }, function (data) {

                if (data = 1)
                {
                    _this.divValidationInventaire.show();
                } else {
                    _this.divErreurValidationInventaire.show();
                }          
                _this.btValidation.DisableLoader();
            });        
        }

    });


}//End Object