(function() {
    'use strict';

    angular
        .module('app')
        .directive('dtCompareTo', dtCompareTo);

    function dtCompareTo() {
        return {
            require: "ngModel",
            scope: {
                otherModelValue: "=dtCompareTo"
            },
            link: function (scope, element, attributes, ngModel) {

                ngModel.$validators.dtCompareTo = function (modelValue) {
                    return modelValue == scope.otherModelValue;
                };

                scope.$watch("otherModelValue", function () {
                    ngModel.$validate();
                });
            }
        };
    }
})();