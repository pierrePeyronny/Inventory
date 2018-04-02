
function ModalPopUpObject() {
    var _this = this;
    _this.onCloseCallback = null;
    _this.modaldiv = $("#PopUpModal");
    _this.titrePopUp = $("#titrePopUpModal");
    _this.contenuPopUp = $("#contenuPopUpModal");

    function _close(options) {
        _this.onCloseCallback(options);
        _this.modaldiv.modal('hide');
    }
    _this.close = _close;

    function _clear() {
        _this.onCloseCallback = null;
        _this.titrePopUp.empty();
        _this.contenuPopUp.empty();
    }
    _this.clear = _clear;

    function _show() {
        _this.modaldiv.modal('show');
    }
    _this.show = _show;

    function _initPopUp(options) {
        _clear();
        _this.onCloseCallback = options.onCloseCallback;
        _this.titrePopUp.html(options.title);
        _this.contenuPopUp.html(options.data);
        if (options.show == true) {
            _show();
        }
    }
    _this.initPopUp = _initPopUp;

    return _this;
}

var modalPopUpInstance = ModalPopUpObject();