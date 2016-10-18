require.config({
    baseUrl: '',

    // alias libraries paths.  Must set 'angular'
    paths: {
        'angular': 'Common/Libs/angular.min',
        'angular-route': 'Common/Libs/angular-route.min',
        'angular-ui-router': 'Common/Libs/angular-ui-router.min',
        'angularAMD': 'Common/Libs/angularAMD.min',
        'ngload': 'Common/Libs/ngload.min',
        'angular-resource': 'Common/Libs/angular-resource.min',
        "w5c.validator": "Common/Libs/w5cValidator.min",
        "elif": "Common/Libs/elif",
        'jquery': "Common/Libs/jquery.min",
        "jquery.fixedheadertable" : "../js/jquery.fixedheadertable",
        "uiBootstrap": "Common/Libs/ui-bootstrap-custom-tpls-0.14.3.min",
        "Bootstrap": "../js/bootstrap.min",
        "underscore":"../js/underscore",
        "bootstrapdialog":"../js/jquery.bootstrap.min",
        "simple-angular-dialog":"Common/simple-angular-dialog",
        "manage-image-dialog":"manage/manageimage/imageDialogService",
        "common":"Common/common",
        "bootstrap-datepicker":"../js/bootstrap-datepicker/js/bootstrap-datepicker",
        "search-questionnaire-dialog": "manage/searchQuestionnaire/searchDialogService",
        "search-questionnaire": "manage/searchQuestionnaire/searchService",
        "chart": "Common/Libs/Chart.min",
        "angular-chart": "Common/Libs/angular-chart"
    },

    // Add angular modules that does not support AMD out of the box, put it in a shim
    shim: {
        'angular': {
            exports: 'angular'
        },
        'angular-route': ['angular'],
        'angularAMD': ['angular'],
        'elif': ['angular'],
        'ngload': ['angularAMD'],
        'angular-resource': ['angular'],
        'angular-ui-router': ['angular'],
        'w5c.validator': ['angular'],
        "jquery.fixedheadertable": ["jquery"],
        'ngDialog': {
            deps: ['angular']
        },
        'uiBootstrap': {
            deps: ['angular']
        },
        'Bootstrap': {
            deps: ['jquery']
        },
        'underscore': {
            exports: '_',
            deps: ['jquery']
        },
        'bootstrapdialog': {
            deps: ['jquery']
        },
        "bootstrap-datepicker":{
            deps: ['jquery']
        },
        "angular-chart": {
            deps: ['chart']
        }

    },
    urlArgs: "t=" + (new Date()).valueOf(),
    deps: ['app']
});
