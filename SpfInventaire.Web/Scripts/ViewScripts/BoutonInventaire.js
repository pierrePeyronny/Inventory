function BoutonInventaire(params) {
    var _this = this;
    _this.button = $(params.button);
    _this.loaderListe = $(params.loader);

    //fonction de désactivation du bouton
    _this.Disable = function () {
        _this.button.prop('disabled', 'disabled');
    }

    //fonction l'activation du bouton
    _this.Activate = function () {
        _this.button.prop('disabled', '');
    }

    //fonction d'activation du laoder le la liste associer
    _this.ActivateLoader = function () {
        _this.loaderListe.show();
    }

    //fonction de désactivation du laoder le la liste associer
    _this.DisableLoader = function () {
        _this.loaderListe.hide();
    }

}