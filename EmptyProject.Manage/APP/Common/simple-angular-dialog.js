angular.module('simple-angular-dialog', []).service('simple-angular-dialog',
	['$rootScope', '$q', '$compile', '$templateCache', '$http', "$document", "$timeout",
	function ($rootScope, $q, $compile, $templateCache, $http, $document, $timeout) {

	    var _this = this;
	    _this.dialogs = {};

	    this.open = function (id, template, model, options) {

	        // Check our required arguments
	        if (!angular.isDefined(id)) {
	            throw "simple-angular-dialog requires id in call to open";
	        }

	        if (!angular.isDefined(template)) {
	            throw "simple-angular-dialog requires template in call to open";
	        }

	        // Set the defaults for model
	        if (!angular.isDefined(model)) {
	            model = null;
	        }

	        // Copy options so the change ot close isn't propogated back.
	        // Extend is used instead of copy because window references are
	        // often used in the options for positioning and they can't be deep
	        // copied.
	        var dialogOptions = {};
	        if (angular.isDefined(options)) {
	            angular.extend(dialogOptions, options);
	        }

	        // Initialize our dialog structure
	        var dialog = { scope: null, ref: null, deferred: $q.defer() };

	        // Get the template from teh cache or url
	        loadTemplate(template).then(
					function (dialogTemplate) {
					    dialogTemplate
                            = ''
                            + '<div class="dialog modal fade">'
                            + '<div class="modal-dialog {{dialogOption.dialogClass}}">'
                            + '<div class="modal-content">'
                            + '<div class="modal-header">'
                            + '<button type="button" class="close" ng-click=\"close()\">&times;</button>'
                            + '<h4 class="modal-title" ng-bind="dialogOption.Title"></h4>'
                            + '</div>'
                            + '<div class="modal-body"><div class="' + id + '">' + dialogTemplate + '</div></div>'
                            + '<div class="modal-footer">'
                            + "<button ng-repeat=\"button in dialogOption.Buttons\" type=\"button\" class=\"btn\" ng-class=\"button.class\" ng-click=\"button.callback(this)\">" +
									"<span class=\"glyphicon\" ng-class=\"button.icon\" ng-show=\"button.icon\"></span> " +
									"<span ng-bind=\"button.value\"></span>" +
								"</button>"
                            + '</div>'
                            + '</div>'
                            + '</div>'
                            + '</div>';
					    // Create a new scope, inherited from the parent.
					    dialog.scope = $rootScope.$new();
					    dialog.scope.model = model;
					    dialog.scope.dialogOption = dialogOptions;
					    dialog.scope.close = function () {
					        // call the bootstrap modal to handle the hide events and remove msgbox after the modal is hidden
					        //					        dialog.ref.modal('hide'); //.one('hidden.bs.modal', function () {
					        //					            if (destroy) {
					        //					                $this.data(parentDataName).append($this);
					        //					                $msgbox.remove();
					        //					            }
					        //					        });

					        dialog.ref.modal('hide').one('hidden.bs.modal', function () {
					            var $msgbox = $("." + id).closest('.dialog');
					            if ($msgbox.length > 0) {
					                $msgbox.remove();
					            }
					        });
					    };
					    var dialogLinker = $compile(dialogTemplate);
					    dialog.ref = $(dialogLinker(dialog.scope));

					    // Handle the case where the user provides a custom close and also
					    // the case where the user clicks the X or ESC and doesn't call
					    // close or cancel.
//					    var customCloseFn = dialogOptions.close;
//					    dialogOptions.close = function (event, ui) {
//					        if (customCloseFn) {
//					            customCloseFn(event, ui);
//					        }
//					        cleanup(id);
//					    };

					    // Initialize the dialog and open it
					    //					    dialog.ref.dialog(dialogOptions);
					    //					    dialog.ref.dialog("open");

					    var body = $document.find('body')

					    //					    var $msgbox = body.find("." + id).closest('.dialog');
					    //					    if ($msgbox.length == 0) {
					    //					        $msgbox.append
					    //					    }
					    var $msgbox = $("." + id).closest('.dialog');
					    if ($msgbox.length > 0) {
					        $msgbox.remove();
					    }
					    body.append(dialog.ref);

					    $timeout(function () {
					        dialog.ref.modal("show");
					        dialog.deferred.resolve();
					    }, 0)

					    // Cache the dialog
					    _this.dialogs[id] = dialog;

					}, function (error) {
					    throw error;
					}
				);

	        // Return our cached promise to complete later
	        return dialog.deferred.promise;
	    };

	    this.close = function (id, result) {

	        // Get the dialog and throw exception if not found
	        var dialog = getExistingDialog(id);

	        // Notify those waiting for the result
	        // This occurs first because the close calls the close handler on the
	        // dialog whose default action is to cancel.
	        dialog.deferred.resolve(result);

	        // Close the dialog (must be last)
	        dialog.ref.dialog("close");
	    };

	    this.cancel = function (id) {

	        // Get the dialog and throw exception if not found
	        var dialog = getExistingDialog(id);

	        // Notify those waiting for the result
	        // This occurs first because the cancel calls the close handler on the
	        // dialog whose default action is to cancel.
	        dialog.deferred.reject();

	        // Cancel and close the dialog (must be last)
	        dialog.ref.dialog("close");
	    };

	    function cleanup(id) {

	        // Get the dialog and throw exception if not found
	        var dialog = getExistingDialog(id);

	        // This is only called from the close handler of the dialog
	        // in case the x or escape are used to cancel the dialog. Don't
	        // call this from close, cancel, or externally.
	        dialog.deferred.reject();
	        dialog.scope.$destroy();

	        // Remove the object from the DOM
	        dialog.ref.remove();

	        // Delete the dialog from the cache
	        delete _this.dialogs[id];
	    };

	    function getExistingDialog(id) {

	        // Get the dialog from the cache
	        var dialog = _this.dialogs[id];
	        // Throw an exception if the dialog is not found
	        if (!angular.isDefined(dialog)) {
	            throw "DialogService does not have a reference to dialog id " + id;
	        }
	        return dialog;
	    };

	    // Since IE8 doesn't support string.trim, provide a manual method.
	    function trim(string) {
	        return string ? string.replace(/^\s+|\s+$/g, '') : string;
	    }

	    // Loads the template from cache or requests and adds it to the cache
	    function loadTemplate(template) {

	        var deferred = $q.defer();
	        var html = $templateCache.get(template);

	        if (angular.isDefined(html)) {
	            // The template was cached or a script so return it
	            html = trim(html);
	            deferred.resolve(html);
	        } else {
	            // Retrieve the template if it is a URL
	            return $http.get(template, { cache: $templateCache }).then(
						function (response) {
						    var html = response.data;
						    if (!html || !html.length) {
						        // Nothing was found so reject the promise
						        return $q.reject("Template " + template + " was not found");
						    }
						    html = trim(html);
						    // Add it to the template cache using the url as the key
						    $templateCache.put(template, html);
						    return html;
						}, function () {
						    return $q.reject("Template " + template + " was not found");
						}
			        );
	        }
	        return deferred.promise;
	    }
	}
]);