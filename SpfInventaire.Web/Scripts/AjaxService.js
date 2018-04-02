function AjaxService() {
    var _this = this;

    // Constructeur  de la classe de base
    function Base(serviceName) {

        //Construction de l'url du service
        var serviceUrl = applicationUrl + serviceName + '/';
        //Initialisations des messages d'erreurs
        this.messages = new AjaxMessages();

        /*** Méthodes privées ***/

        function _buildServiceMethodUrl(methodName) {
            return serviceUrl + methodName;
        };

        //Appelle une méthode WCF de type GET
        function _Get(methodName, data, callback, message) {
            $.get(_buildServiceMethodUrl(methodName), data).success(function (msg, status, headers, config) {
                callback(msg);
            }).error(function (xhr, status, headers, config) {
                handleWcfCallError(status, message);
            });
        };

        //Appelle une méthode WCF de type POST
        function _Post(methodName, data, callback, message) {
            $.post(_buildServiceMethodUrl(methodName), data).success(function (msg, status, headers, config) {
                callback(msg);
            }).error(function (xhr, status, headers, config) {
                handleWcfCallError(status, message);
            });
        };

        //Gère les erreurs
        function handleWcfCallError(status, message) {
            if (message && message.error)
                alert(message.error);
            if (message && message.spinnerKey)
                stopSpinnerOnError(message.spinnerKey); //Stop le spinner associé à la requête
        }

        //Déclenche levent d'arret du spinner global, afin d el'arêter si il est actif
        function stopSpinnerOnError(spinnerKey) {
            vtSpinnerService.stop({ key: spinnerKey });
        }


        /*** Méthodes publiques ***/
        this.Get = _Get;
        this.Post = _Post;
        this.buildServiceMethodUrl = _buildServiceMethodUrl;
    };


    ////////////////Gestion Inventaire ////////////////////
    function Inventaire() {
        var _this = this;
        var base = new Base('Inventaire');

        function _getFormSaisieInventaire(request, callback) {
            base.Post('GetFormSaisieInventaire', request, callback, base.messages.GetAjaxList);
        };

        _this.GetFormSaisieInventaire = _getFormSaisieInventaire;
    };

    function ValidationInventaire() {
        var _this = this;
        var base = new Base('ValidationInventaire');

        function _validation(request, callback) {
            base.Post('AjaxCreate', request, callback, base.messages.GetAjaxList);
        };

        _this.Validation = _validation;
    };

    function Ticket() {
        var _this = this;
        var base = new Base('TicketIncident');

        function _getFormCreateTicket(request, callback) {
            base.Post('GetFormCreate', request, callback, base.messages.GetAjaxList);
        };

        function _getFormDetailTicket(request, callback) {
            base.Post('GetFormDetail', request, callback, base.messages.GetAjaxList);
        };

        _this.GetFormCreateTicket = _getFormCreateTicket;
        _this.GetFormDetailTicket = _getFormDetailTicket;
    };

    function BlocInventaire() {
        var _this = this;
        var base = new Base('BlocInventaire');

        //Get List Bloc Inventaire
        function _getListBlocsInventaire(request, callback) {
            base.Post('GetBlocsInventaireByInventaire', request, callback, base.messages.GetAjaxList);
        };

        //Get form Add Bloc Inventaire
        function _getFormBlocInventaire(request, callback) {
            base.Post('GetFormCreate', request, callback, base.messages.GetAjaxForm);
        };

        //Get form Edit Bloc Inventaire
        function _getFormEditBlocInventaire(request, callback) {
            base.Get('Edit', request, callback, base.messages.GetAjaxForm);
        };

        _this.GetListBlocsInventaire = _getListBlocsInventaire;
        _this.GetFormBlocInventaire = _getFormBlocInventaire;
        _this.GetFormEditBlocInventaire = _getFormEditBlocInventaire;
    };

    function Materiel() {
        var _this = this;
        var base = new Base('Materiel');

        //Get List Materiel
        function _getListMateriel(request, callback) {
            base.Post('GetMaterielsByBlocsInventaire', request, callback, base.messages.GetAjaxList);
        };

        //Get form Add Materiel
        function _getFormMateriel(request, callback) {
            base.Post('GetFormCreate', request, callback, base.messages.GetAjaxForm);
        };

        //Get form Edit Materiel
        function _getFormEditMateriel(request, callback) {
            base.Get('Edit', request, callback, base.messages.GetAjaxForm);
        };

        _this.GetListMateriel = _getListMateriel;
        _this.GetFormMateriel = _getFormMateriel;
        _this.GetFormEditMateriel = _getFormEditMateriel;
    };


    function CreationInventaire() {
        var _this = this;
        var base = new Base('CreationInventaire');

        //Get form Sort
        function _getFormSort(request, callback) {
            base.Post('GetSortForm', request, callback, base.messages.GetAjaxForm);
        };

        _this.GetFormSort = _getFormSort;
    };

    function GestionStocks() {
        var _this = this;
        var base = new Base('StockMateriel');

        function _getListStockByMateriel(request, callback) {
            base.Post('GetStocksByMateriel', request, callback, base.messages.GetAjaxList);
        };

        function _getFormAddStock(request, callback) {
            base.Post('GetFormCreate', request, callback, base.messages.GetAjaxForm);
        };

        function _getFormEditStock(request, callback) {
            base.Get('Edit', request, callback, base.messages.GetAjaxForm);
        };

        function _deleteStock(request, callback) {
            base.Post('AjaxDelete', request, callback, base.messages.GetAjaxForm);
        };

        function _getFormTransfertStock(request, callback) {
            base.Post('GetFormTransfert', request, callback, base.messages.GetAjaxForm);
        };

        function _getListingStocks(request, callback) {
            base.Post('AjaxListingStocks', request, callback, base.messages.GetAjaxForm);
        };

        function _getStocksListByMateriel(request, callback) {
            base.Post('GetStocksListByMateriel', request, callback, base.messages.GetAjaxForm);
        };

        

        _this.GetListStockByMateriel = _getListStockByMateriel;
        _this.GetFormAddStock = _getFormAddStock;
        _this.GetFormEditStock = _getFormEditStock;
        _this.DeleteStock = _deleteStock;
        _this.GetFormTransfertStock = _getFormTransfertStock;
        _this.GetListingStocks = _getListingStocks;
        _this.GetStocksListByMateriel = _getStocksListByMateriel;
    };


    function SortieStock() {
        var _this = this;
        var base = new Base('SortieStock');

        function _getFormCreateSortieStock(request, callback) {
            base.Post('GetFormCreate', request, callback, base.messages.GetAjaxList);
        };

        function _getStockBySearch(request, callback) {
            base.Post('GetStockBySearch', request, callback, base.messages.GetAjaxList);
        };

        _this.GetFormCreateSortieStock = _getFormCreateSortieStock;
        _this.GetStockBySearch = _getStockBySearch;
    };


    ///////////////// Gestion Roles /////////////////////
    function Roles() {
        var _this = this;
        var base = new Base('User');

        //Add Role
        function _addRole(request, callback) {
            base.Post('RoleAddToUser', request, callback, base.messages.GetAjaxList);
        };

        //Get Roles User
        function _getRolesUser(request, callback) {
            base.Post('GetUserRoles', request, callback, base.messages.GetAjaxList);
        }; 

        //Delete Role User
        function _deleteRoleUser(request, callback) {
            base.Post('DeleteRoleForUser', request, callback, base.messages.GetAjaxList);
        };

        _this.AddRole = _addRole;
        _this.GetRolesUser = _getRolesUser;
        _this.DeleteRoleUser = _deleteRoleUser;
    };

    _this.Inventaire = new Inventaire();
    _this.ValidationInventaire = new ValidationInventaire();
    _this.Ticket = new Ticket();
    _this.BlocInventaire = new BlocInventaire();
    _this.Materiel = new Materiel();
    _this.CreationInventaire = new CreationInventaire();
    _this.GestionStocks = new GestionStocks();
    _this.SortieStock = new SortieStock();

    _this.Roles = new Roles();
    
};

