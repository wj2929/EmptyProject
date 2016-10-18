define(['angularAMD', "common", "jquery.fixedheadertable", "simple-angular-dialog", "bootstrapdialog"], function (angularAMD, common) {
    'use strict';
    angularAMD.controller('manageHeaderController', ['$scope', '$state', "$http", "simple-angular-dialog", function ($scope, $state, $http, SADialog) {
        $http.get("/Account/GetUserName").success(function (data) {
            if (data.State) {
                $scope.UserName = data.Message;
                if (location.href.indexOf("/manage/") == -1)
                    location.href = "#/manage/welcome";
            } else
                location.href = "#/login";
        });

        $scope.logout = function () {
            $http.post("/Account/LogoutService").success(function (data) {
                if (data.State) {
                    location.href = "#/login";
                } else
                    $scope.UserName = data.Message;
            });
        };

        $scope.ModifyPassword = function () {
            var model = {
                entity: {},
                validateOptions: {}
            };
            SADialog.open("modifypassword",
                            common.makeTemplateUrl("manage/_modifypassword.html"),
                            model,
                            {
                                Title: "修改密码",
                                //dialogClass: "modal-sm",
                                Buttons: [
                                {
                                    value: "确定",
                                    callback: function (result) {
                                        result.validateForm.doValidate();
                                        if (result.validateForm.$errors.length == 0) {
                                            $http.get("/Account/CheckPassword?Password=" + model.entity.OldPassword).success(function (data) {
                                                if (data.State) {
                                                    $http.post("/Account/ModifyPassword", model.entity).success(function (data) {
                                                        if (data.State) {
                                                            result.$parent.close();
                                                            $.messager.alert("密码修改成功！");
                                                        }
                                                        else
                                                            $.messager.alert(data.Message);
                                                    });
                                                }
                                                else {
                                                    $.messager.alert("旧密码错误！");
                                                }
                                            });
                                        }

                                    }
                                }]
                            });
        }
    } ]);

    angularAMD.directive('manageHeader', function () {
        return {
            restrict: 'A',
            templateUrl: common.makeTemplateUrl('manage/_header.html'),
            controller: 'manageHeaderController'
        };
    });


    angularAMD.directive('manageHeaderMenu', function () {
        return {
            restrict: 'A',
            templateUrl: common.makeTemplateUrl('manage/_headermenu.html')
        };
    });

    angularAMD.directive('resize', ['$window', function ($window) {
        return {
            link: function (scope, elem, attrs) {
                scope.onResize = function () {
                    if (scope.$$childTail && scope.$$childTail.autoHeight)
                        scope.$$childTail.autoHeight();
                }
                scope.onResize();

                angular.element($window).bind('resize', function () {
                    scope.onResize();
                })
            }
        }
    } ]);

    angularAMD.filter('sumOfValue', function () {
        return function (data, key) {
            if (angular.isUndefined(data) && angular.isUndefined(key))
                return 0;
            var sum = 0;
            angular.forEach(data, function (value) {
                sum = sum + parseFloat(value[key]);
            });
            return sum;
        }
    });

    angularAMD.directive('computeProportion', function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                //                function parser(value) {
                //                    debugger;
                //                    var total = scope.$eval(attrs.total);
                //                    alert(total);
                //                    return total;
                //                }
                //                debugger;
                //                ngModel.$parsers.push(parser);

                ngModel.$formatters.push(function (value) {
                    var total = scope.$eval(attrs.computeProportion);
                    if (total != null) {
                        return common.format("{0} , 总数 {2} , 占比 {1}%", value, (value * 100 / total).toFixed(2), total);
                    }
                    else
                        return "";
                });
            }
        };
    });

    angularAMD.directive('drawChart', function () {
        return {
            restrict: 'EA',
            scope: { statItems: "=" },
            templateUrl: common.makeTemplateUrl("manage/ComplexStat/_drawChart.html")
//            controller: 'drawChartController'
        };
    });
});
