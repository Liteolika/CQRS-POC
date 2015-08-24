(function () {
    'use strict';

    angular
        .module('app')
        .controller('mainController', mainController);

    mainController.$inject = ['$location', '$http', 'messageProxy', '$scope'];

    function mainController($location, $http, messageProxy, $scope) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'mainController';
        vm.devices = [];

        vm.selectedDevice = [];

        vm.selectDevice = function(device) {
            vm.selectedDevice = device;
        }

        vm.updateDevice = function () {

            $http({
                method: 'POST',
                url: '/api/device',
                data: vm.selectedDevice
            }).then(function (response) {
                vm.devices = response.data;
                vm.selectedDevice = [];
                getDevices();
            }, function (error) {
                
            });

            
        }

        

        var getDevices = function () {

            $http({
                method: 'GET',
                url: '/api/device'
            }).then(function (response) {
                vm.devices = response.data;
                var a = 1;
            }, function (error) {
                var a = 1;
            });
        }

        var onReceiveMessage = function () {
            $scope.$on('MESSAGE_RECEIVED', function (event, data) {
                toastr.info(data.message.Message, 'New Message: ' + data.message.Message);
                $scope.$apply(function () {
                    //getDevices();
                    $http({
                        method: 'GET',
                        url: '/api/device'
                    }).then(function (response) {
                        vm.devices = response.data;
                        var a = 1;
                    }, function (error) {
                        var a = 1;
                    });
                });
                
            });

            $scope.$on('MESSAGE_CONNECTION_STARTED', function (event, data) {
                toastr.info("Message connection started", "Message connection started");
            });
        }

        activate();

        function activate() {
            getDevices();
            onReceiveMessage();
            toastr.info("Welcome");
        }
        

        //var registerSignalR = function () {
        //    //    //orderHub.server.registerForOrder($scope.orderId);
        //};

        //var messageHub = $.connection.messageHub;
        //$.connection.hub.start().done(registerSignalR);

        //messageHub.client.messageRecieved = function (message) {
        //    $scope.$apply(function () {
        //    //    if ($scope.orderId != id) {
        //    //        alert('Unknown id ' + id);
        //    //        return;
        //    //    }
        //    //    $scope.refreshOrder();
        //        //    console.log('ViewModel updated');
        //        vm.message = message;
        //        getDevices();
        //    });
            
        //};
        

    }
})();
