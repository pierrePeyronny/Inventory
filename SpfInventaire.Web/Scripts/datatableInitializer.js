$(document).ready(function () {

    //Permet le tri sur les Colonnes contenant des checkBox
    $.fn.dataTable.ext.order['dom-checkbox'] = function (settings, col) {
        return this.api().column(col, { order: 'index' }).nodes().map(function (td, i) {
            return $('input', td).prop('checked') ? '1' : '0';
        });
    };
    var varColumnDefs;
    var columnCheckboxClass = $('.checkBoxColumn');

    if (columnCheckboxClass.length > 0)
    {
        varColumnDefs = [
            { targets: "checkBoxColumn", "orderDataType": "dom-checkbox" },
            { targets: "noSortingColumn", "searchable": false, "orderable": false }
        ];
    } else
    {
        varColumnDefs = [
            { targets: "noSortingColumn", "searchable": false, "orderable": false }
        ];
    }


    //Initialisation DataTable de base
    $('#tableauDatatable').DataTable(
    {
        "order": [[$('th.firstColumnOrder').index(), "dsc"]],
        "pageLength": 25,
        columnDefs: varColumnDefs,
        language: {
            processing: "Traitement en cours...",
            search: "Rechercher&nbsp;:",
            lengthMenu: "Afficher _MENU_ &eacute;l&eacute;ments",
            info: "Affichage de l'&eacute;lement _START_ &agrave; _END_ sur _TOTAL_ &eacute;l&eacute;ments",
            infoEmpty: "Affichage de l'&eacute;lement 0 &agrave; 0 sur 0 &eacute;l&eacute;ments",
            infoFiltered: "(filtr&eacute; de _MAX_ &eacute;l&eacute;ments au total)",
            infoPostFix: "",
            loadingRecords: "Chargement en cours...",
            zeroRecords: "Aucun &eacute;l&eacute;ment &agrave; afficher",
            emptyTable: "Aucune donnée disponible dans le tableau",
            paginate: {
                first: "Premier",
                previous: "Pr&eacute;c&eacute;dent",
                next: "Suivant",
                last: "Dernier"
            },
            aria: {
                sortAscending: ": activer pour trier la colonne par ordre croissant",
                sortDescending: ": activer pour trier la colonne par ordre décroissant"
            }
        }
    });//Fin DataTable

});//Fin Document Ready**
