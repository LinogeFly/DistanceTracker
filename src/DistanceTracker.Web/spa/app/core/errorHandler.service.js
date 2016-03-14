(function () {
    'use strict';

    angular
        .module('app')
        .factory('errorHandlerService', ['notificationService', 'constantsService', errorHandlerService]);

    function errorHandlerService(notificationService, constantsService) {
        var service = {
            handle: handle
        };

        return service;

        function handle(error) {
            // No error specific hanlers yet. Just general error message.
            // Full error message will be in the browser console.

            notificationService.showError(constantsService.Errors.Common.UNEXPECTED_ERROR);
        }
    }
})();