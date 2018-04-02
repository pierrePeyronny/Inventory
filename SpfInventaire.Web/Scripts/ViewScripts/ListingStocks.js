function ListingStocks() {
    var _this = this;
    _this.AjaxService = new AjaxService();

    _this.LoaderMateriel = new BoutonInventaire({ button: '', loader: '#loaderMateriel' });
    _this.divResultStocks = '#divResultStocks';

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Evenements
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    $('body').on('change', '#ddlMateriel', function (e) {

        _this.LoaderMateriel.ActivateLoader();

        var typeMaterielId = $(this).val();

        if (typeMaterielId > 0) {

            _this.AjaxService.GestionStocks.GetListingStocks({ typeMaterielID: typeMaterielId }, function (data) {
                $(_this.divResultStocks).html(data);
            });

        } else {
            $(_this.divResultStocks).html('');
        }

        _this.LoaderMateriel.DisableLoader();
    });


};//EndObject