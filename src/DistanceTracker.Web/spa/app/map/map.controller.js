(function () {
    'use strict';

    angular
        .module('app')
        .controller('mapController', ['$scope', '$log', '$window', '$location', 'routeService', 'progressService', 'constantsService', 'shareService', 'authService', mapController]);

    function mapController($scope, $log, $window, $location, routeService, progressService, constantsService, shareService, authService) {
        // Scope default values and initialization

        $scope.distance = 0;
        $scope.progress = 0;
        $scope.setMenuView = function (val) { $scope.menuView = val; };
        $scope.isSharedLink = function () { return shareService.isSharedLink(); };
        if (!$scope.isSharedLink()) { $scope.menuView = "progress"; } else { $scope.menuView = null; }

        // Route functions

        function bindRoute() {
            routeService.getRoute()
                .then(function (route) {
                    addRouteToTheMap(route.startLocation, route.endLocation, route.waypoints);
                });
        }

        function addRouteToTheMap(start, end, waypoints) {
            var googleWaypoints = [];
            angular.forEach(waypoints, function (wp) {
                googleWaypoints.push({
                    location: wp,
                    stopover: false
                });
            });

            directionsService
                .route({
                    origin: start,
                    destination: end,
                    waypoints: googleWaypoints,
                    travelMode: google.maps.TravelMode.WALKING
                },
                function (response, status) {
                    if (status === google.maps.DirectionsStatus.OK) {
                        directionsDisplay.setDirections(response);
                    } else {
                        $log.error('Directions request failed due to ' + status);
                    }
                });
        }

        $scope.$on(constantsService.Events.Route.DELETED, function () {
            isRouteDirty = false;
            bindRoute();
        });

        function onRouteChanged(a) {
            /* jshint validthis:true */
            var route = this.directions.routes[0];
            bindProgress();

            if (!isRouteDirty) {
                $scope.distance = route.legs[0].distance.value;
                isRouteDirty = true;
            } else {
                routeService.saveRoute(route)
                    .then(function () {
                        $scope.distance = route.legs[0].distance.value;
                    });
            }
        }

        // Progress functions

        function getMilestonePoints(googleRoute, dist) {
            if (typeof dist === 'undefined')
                dist = 1000;

            var result = [],
                geo = google.maps.geometry.spherical,
                path = googleRoute.overview_path,
                point = path[0],
                distance = 0,
                leg,
                overflow,
                pos;

            for (var p = 1; p < path.length; ++p) {
                leg = Math.round(geo.computeDistanceBetween(point, path[p]));
                var d1 = distance + 0;
                distance += leg;
                overflow = dist - (d1 % dist);

                if (distance >= dist && leg >= overflow) {
                    if (overflow && leg >= overflow) {
                        pos = geo.computeOffset(point, overflow, geo.computeHeading(point, path[p]));
                        result.push(pos);
                        distance -= dist;
                    }

                    while (distance >= dist) {
                        pos = geo.computeOffset(point, dist + overflow, geo.computeHeading(point, path[p]));
                        result.push(pos);
                        distance -= dist;
                    }
                }
                point = path[p];
            }

            return result;
        }

        function addProgressMarkerToTheMap(progress) {
            if (progress <= 0) {
                hideProgressMarker();
                return;
            }

            var points = getMilestonePoints(directionsDisplay.directions.routes[0], 1000);
            var progressInKilometers = progress / 1000;

            if (progressInKilometers > points.length) {
                hideProgressMarker();
                return;
            }

            progressMarker.setPosition(points[Math.round(progressInKilometers)]);
            progressMarker.setMap(map);
        }

        function hideProgressMarker() {
            progressMarker.setMap(null);
        }

        function bindProgress() {
            progressService.getProgress()
                .then(function (data) {
                    $scope.progress = data;
                    addProgressMarkerToTheMap(data);
                });
        }

        $scope.$on(constantsService.Events.Progress.CHANGED, function () {
            bindProgress();
        });

        $scope.$on(constantsService.Events.Progress.DELETED, function () {
            bindProgress();
        });

        // Auth

        $scope.$on(constantsService.Events.Auth.LOGGED_IN, function () {
            $location.path('/');
            $window.location.reload();
        });

        $scope.onLogOutClick = function () {
            authService.logOut().then(
                function () {
                    $location.path('/');
                    $window.location.reload();
                }
            );
        };

        // Main

        var directionsService,
            directionsDisplay,
            map,
            progressMarker,
            // This variable is here to distinguish when a route is added to the map for the first time during initialization
            // and when the route gets changed with drag and drop.
            // false - first load
            // true - changed with drag and drop
            isRouteDirty = false;

        function start() {
            directionsService = new google.maps.DirectionsService();
            directionsDisplay = new google.maps.DirectionsRenderer({
                draggable: !$scope.isSharedLink()
            });

            map = new google.maps.Map(document.getElementById('map'), {
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                mapTypeControl: false,
                streetViewControl: false
            });

            progressMarker = new google.maps.Marker({
                map: map,
                title: constantsService.Marker.TITLE,
                clickable: true,
                icon: {
                    url: constantsService.Marker.ICON
                }
            });

            google.maps.event.addListener(directionsDisplay, 'directions_changed', onRouteChanged);
            directionsDisplay.setMap(map);
            bindRoute();
        }

        if ($scope.isSharedLink()) {
            start();
        } else {
            // If authenticated - start the application.
            // If not authenticated - user will be redirected to /login via interceptor
            authService.currentUser().then(
                function (user) {
                    $scope.currentUser = user;
                    start();
                }
            );
        }
    }
})();
