function SelectionMateriel(options) {
    var _this = this;
    _this.AjaxService = new AjaxService();

    _this.LoaderInventaire = new BoutonInventaire({ button: '', loader: '#loaderPopUp' });
    _this.listInventaire = '#'+ options.ListInventaireID;
    _this.listBlocs = '#' + options.listBlocsID;
    _this.listMateriel = '#' + options.listMaterielID;

    if (options.listStockID)
    {
        _this.listStock = '#' + options.listStockID;
    } else {
        _this.listStock = '';
    }
    

    //Variables global
    _this.inventaireId = null;
    _this.blocInventaireId = null;
    _this.materielId = null;


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //FONCTIONS
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    ///////////Blocx Inventaire ///////////////////
    function ChargementBlocsInventaire(options) {

        _this.AjaxService.BlocInventaire.GetListBlocsInventaire({ inventaireId: options.inventaireId }, function (data) {
            var markup = "<option value='0'>Sélectionner un Emplacement</option>";
            for (var x = 0; x < data.length; x++) {
                markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
            }
            $(_this.listBlocs).html(markup);

            InitListMateriel();

        });
    };

    function InitListBloc() {
        var initList = "<option value=''>Aucun</option>";
        $(_this.listBlocs).html(initList);
    };

    ////////////// Matériel /////////////////////////////
    function InitListMateriel() {
        var initList = "<option value=''>Aucun</option>";
        $(_this.listMateriel).html(initList);
    };

    function ChargementMateriel(options) {

        _this.AjaxService.Materiel.GetListMateriel({ blocInventaireId: options.blocInventaireId }, function (data) {
            var markup = "<option value='0'>Sélectionner un Matériel</option>";
            for (var x = 0; x < data.length; x++) {
                markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
            }
            $(_this.listMateriel).html(markup);
        });
    };



    ////////////// Stock /////////////////////////////
    function InitListStock() {
        var initList = "<option value=''>Aucun</option>";
        $(_this.listStock).html(initList);
    };

    function ChargementStock(options) {

        _this.AjaxService.GestionStocks.GetStocksListByMateriel({ materielId: options.materielId }, function (data) {
            var markup = "";
            for (var x = 0; x < data.length; x++) {
                markup += "<option value=" + data[x].Value + ">" + data[x].Text + "</option>";
            }
            $(_this.listStock).html(markup);
        });
    };

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Evenements
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /////////////// Inventaire //////////////////////////
    $('body').on('change', _this.listInventaire, function (e) {

        _this.LoaderInventaire.ActivateLoader();

        _this.inventaireId = $(this).val();

        if (_this.inventaireId > 0) {
            ChargementBlocsInventaire({ inventaireId: _this.inventaireId, blocInventaireId: 0 });
        }
        InitListBloc();
        InitListMateriel();
        InitListStock();

        _this.LoaderInventaire.DisableLoader();
    });


    /////////////// Bloc Inventaire //////////////////////////
    $('body').on('change', _this.listBlocs, function (e) {
        _this.LoaderInventaire.ActivateLoader();

        _this.blocInventaireId = $(this).val();

        if (_this.blocInventaireId > 0) {
            //Chargement de la liste du Matériel
            ChargementMateriel({ blocInventaireId: _this.blocInventaireId, materielId: 0 })
        }
        else {
            InitListMateriel();
        }
        InitListStock();
        _this.LoaderInventaire.DisableLoader();
    });


    /////////////// Matériel //////////////////////////////
    $('body').on('change', _this.listMateriel, function (e) {

        _this.materielId = $(this).val();

        if(_this.listStock != '')
        {
            if (_this.materielId > 0) {
                //Chargement de la liste du Matériel
                ChargementStock({ materielId: _this.materielId })
            }
            else {
                InitListStock();
            }
        }

    });



};//EndObject