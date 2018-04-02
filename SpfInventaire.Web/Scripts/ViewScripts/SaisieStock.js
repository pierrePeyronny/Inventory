
function SaisieStock(options) {
    var _this = this;
    _this.AjaxService = new AjaxService();

    _this.LoaderInventaire = new BoutonInventaire({ button: '', loader: '#loaderGeneral' });
    _this.champRecherche = $('#champRecherche'); 
    _this.btRecherche = new BoutonInventaire({ button: '#btRecherche', loader: '#loaderRecherche' });
    _this.divAffichageStock = $('#divAffichageStock');
    _this.divValidationStock = $('#messageValidationInventaire');
    _this.divErreurValidationStock = $('#messageErreurValidationInventaire');

    _this.MaterielID = null;

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //FONCTIONS
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Evenements
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    $('body').on('click', '.transfert', function (e) {

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
                    _this.divAffichageStock.html("");
                    _this.champRecherche.val("");
                },
                title: 'Transfert de stock'
            }
            modalPopUpInstance.initPopUp(options);
        });
    });


    _this.btRecherche.button.click(function () {

        if (_this.champRecherche.val() != '') {
            _this.btRecherche.ActivateLoader();

            _this.AjaxService.SortieStock.GetStockBySearch({ search: _this.champRecherche.val() }, function (data) {
                _this.divAffichageStock.html(data);
                _this.btRecherche.DisableLoader();
            });
        } else {
            _this.divAffichageStock.html("");            
            _this.btRecherche.DisableLoader();
        }


    });


}//End Object