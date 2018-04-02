
function DetailUser(options) {
    var _this = this;
    _this.AjaxService = new AjaxService();

    _this.UserId = options.userId;
    _this.ListRoles = $('#listRoles');

    _this.buttonAddRole = new BoutonInventaire({ button: '#linkAddRole', loader: '#loaderRoles' });
    _this.divRoles = $('#divRolesUser');


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //FONCTIONS
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Evenements
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    _this.buttonAddRole.button.click(function (e) {
        //lancement du loader
        _this.buttonAddRole.ActivateLoader();

        var roleId = _this.ListRoles.val();

        _this.AjaxService.Roles.AddRole({ roleId: roleId, userId: _this.UserId }, function (data) {
            //arret du loader
            _this.buttonAddRole.DisableLoader();
            _this.divRoles.html(data);
        });
    });


    $('body').on('click', '.deleteButtonTDB', function (e) {
        e.preventDefault();
        var roleId = $(this).data('id');

        if (confirm('Supprimer le role ?')) {
            _this.buttonAddRole.ActivateLoader();

            

            _this.AjaxService.Roles.DeleteRoleUser({ roleId: roleId, userId: _this.UserId }, function (data) {
                _this.buttonAddRole.DisableLoader();
                _this.divRoles.html(data);
            });
        }
    });
}//End Object