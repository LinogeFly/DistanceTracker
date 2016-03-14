(function () {
    'use strict';

    angular
        .module('app')
        .factory('notificationService', notificationService);

    function notificationService() {
        toastr.options = {
            "positionClass": "toast-top-center",
            "preventDuplicates": true,
        };

        var service = {
            showError: showError
        };

        return service;

        function showError(msg) {
            toastr.error(msg, 'Error');
        }
    }
})();