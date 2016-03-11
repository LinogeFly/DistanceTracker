(function () {
    'use strict';

    angular
        .module('app')
        .constant('constantsService', {
            Marker: {
                ICON: 'img/marker.png',
                TITLE: 'I\'m here, woohoo!'
            },
            Events: {
                Route: {
                    DELETED: 'route:deleted',
                    CHANGED: 'route:changed'
                },
                Progress: {
                    DELETED: 'progress:deleted',
                    CHANGED: 'progress:changed'
                },
                Auth: {
                    LOGGED_IN: 'auth:logged-in'
                }
            },
            Errors: {
                Common: {
                    UNEXPECTED_ERROR: 'An unexpected error has occurred.'
                }, 
                Auth: {
                    INVALID_USERNAME_OR_PASSWORD: 'Invalid username or password.',
                    USERNAME_ALREADY_EXISTS: 'Account with this username already exists.'
                }
            },
            Dialog: {
                DELETE_PROGRESS_CONFIRM_TITLE: 'Are you sure you want to reset your progress?',
                DELETE_ROUTE_CONFIRM_TITLE: 'Are you sure you want to reset your route?'
            }
        });
})();