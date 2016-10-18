define(['angularAMD', "common", 'angular', 'angular-ui-router', 'angular-resource', "w5c.validator", "elif", "Bootstrap", "jquery.fixedheadertable", "underscore", "bootstrap-datepicker", "bootstrapdialog", "angular-chart"], function (angularAMD, common) {
    'use strict';
    var app = angular.module('app', ['ui.router', 'ngResource', "w5c.validator", "elif", "chart.js"]);

    app.config(['$stateProvider', '$urlRouterProvider', "w5cValidatorProvider", '$httpProvider', "$sceDelegateProvider",
        function ($stateProvider, $urlRouterProvider, w5cValidatorProvider, $httpProvider, $sceDelegateProvider) {

        if (!$httpProvider.defaults.headers.get) {
            $httpProvider.defaults.headers.get = {};
        }
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
        $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
        $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
 
//        $stateProvider
//          .state('login', angularAMD.route({
//              url: '/login',
//              templateUrl: common.makeTemplateUrl('Account/_login.html'),
//              controllerUrl: 'Account/loginController'
//          }))
//          .state('manage', angularAMD.route({
//              url: '/manage',
//              templateUrl: common.makeTemplateUrl('Manage/_index.html'),
//              controllerUrl: 'Manage/indexController'
//          }))
//          .state('manage.welcome', angularAMD.route({
//              url: '/welcome',
//              templateUrl: common.makeTemplateUrl('Manage/_welcome.html')
//          }))
//          .state('manage.AreaQuestionnaire', angularAMD.route({           //社区教育实验区（示范区）工作情况调查统计表
//              url: '/AreaQuestionnaire',
//              templateUrl: common.makeTemplateUrl('manage/AreaQuestionnaire/_index.html'),
//              controllerUrl: 'manage/AreaQuestionnaire/indexController'
//          }))
//          .state('manage.AreaQuestionnaireStat', angularAMD.route({           //社区教育实验区（示范区）工作情况调查表统计
//              url: '/AreaQuestionnaireStat',
//              templateUrl: common.makeTemplateUrl('manage/AreaQuestionnaire/_stat.html'),
//              controllerUrl: 'manage/AreaQuestionnaire/statController'
//          }))
//          .state('manage.ProvinceQuestionnaire', angularAMD.route({       //省、地（市）社区教育工作情况调查统计表
//              url: '/ProvinceQuestionnaire',
//              templateUrl: common.makeTemplateUrl('manage/ProvinceQuestionnaire/_index.html'),
//              controllerUrl: 'manage/ProvinceQuestionnaire/indexController'
//          }))
//          .state('manage.ProvinceQuestionnaireStat', angularAMD.route({       //省、地（市）社区教育工作情况调查表统计
//              url: '/ProvinceQuestionnaireStat',
//              templateUrl: common.makeTemplateUrl('manage/ProvinceQuestionnaire/_stat.html'),
//              controllerUrl: 'manage/ProvinceQuestionnaire/statController'
//          }))
//          .state('manage.ComplexStat', angularAMD.route({       //综合统计
//              url: '/ComplexStat',
//              templateUrl: common.makeTemplateUrl('manage/ComplexStat/_stat.html'),
//              controllerUrl: 'manage/ComplexStat/statController'
//          }))
//          .state('manage.ComplexStat_Nation', angularAMD.route({       //综合统计_国家级
//              url: '/ComplexStat_Nation',
//              templateUrl: common.makeTemplateUrl('manage/ComplexStat/_stat_nation.html'),
//              controllerUrl: 'manage/ComplexStat/statnationController'
//          }))
//          ;


        $urlRouterProvider.otherwise('/manage');

        $sceDelegateProvider.resourceUrlWhitelist([
            // Allow same origin resource loads.
            'self',
            // Allow loading from our assets domain.  Notice the difference between * and **.
            'http://map.baidu.com/**'
        ]);


        w5cValidatorProvider.config({
            blurTrig: false,
            showError: true,
            removeError: true
        });
        w5cValidatorProvider.setRules({
            UserName: {
                required: "用户名不能为空"
            },
            Password: {
                required: "密码不能为空"
            }
        });

    } ]);

    app.run(function ($rootScope, $templateCache) {
        $rootScope.$on('$routeChangeStart', function (event, next, current) {
            if (typeof (current) !== 'undefined') {
                $templateCache.remove(current.templateUrl);
            }
        });
    });

    return angularAMD.bootstrap(app);
});
