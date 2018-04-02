function AjaxMessages() {

    //****** Ressource permettant de gérer les messages à afficher en cas d'appel à Ajax **//
    // Merci de respecter l'ordre alphabétique lors de l'insertion de ressoure
    return {
        // key: { info: 'Message en cas de succès', error: 'Message en cas d erreur'}
        GetAjaxList: {
            error: "Une erreur est survenue pendant la récupération du formulaire."
        },

        GetAjaxForm: {
            error: "Une erreur est survenue pendant la récupération du formulaire."
        }

    };
};