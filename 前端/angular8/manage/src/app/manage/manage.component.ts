import { CommonService } from 'src/app/services/common.service';
import { AccountEditPasswordComponent } from './../account/account-edit-password/account-edit-password.component';
import { PassportService } from 'src/app/services/passport.service';
import { Component, OnInit } from '@angular/core';
import { BaseManageForm } from './BaseManageForm';
import * as $ from "jquery";
import { ReturnInfoModel } from '../models/ReturnInfoModel';
import { Router } from '@angular/router';
import { NgbModalRef } from '../common/modal/modal-ref';
import { SweetError } from '../common/common';
import { NgbModal } from '../common/modal/modal';
@Component({
  selector: 'app-manage',
  templateUrl: './manage.component.html',
  styleUrls: ['./manage.component.css']
})
export class ManageComponent extends BaseManageForm implements OnInit {

  userMenus=[];
  constructor(private PassportService:PassportService,
    private router: Router,
    private modalService: NgbModal,
    private CommonService:CommonService) {
    super();


    // let body = document.getElementsByTagName('body')[0];
    // for(var i=body.classList.length-1;i>=0;i--){
    //   body.classList.remove(body.classList[i]);
    // }

    // this.isLogin();
  }

  // isLogin(){
  //   this.PassportService.isLogin((data:ReturnInfoModel)=>{
  //     if(!data.State)
  //       this.router.navigate(["/login"]);
  //   });
  // }

  logout(){
    this.CommonService.setToken('');
    this.router.navigate(["/login"]);
  }

  editPassword(){
    let modal: NgbModalRef = this.modalService.open(AccountEditPasswordComponent,{ backdrop: 'static', keyboard: true});
    modal.componentInstance.SetForm();
    modal.result.then((result: ReturnInfoModel) => {
      if (!result) {
        return;
      }

      // if (!result.State) {
      //   SweetError("", result.Message);
      // } else {
      //   // this.loadOrganPaging();
      // }
    });

  }

  getMenus(){
    this.PassportService.getMenus((data:ReturnInfoModel)=>{
      if(data.State){
        this.userMenus = data.DataObject;
      }
      else
        SweetError('',data.Message);
    });
  }

  ngOnInit() {

    this.getMenus();

    let body = document.getElementsByTagName('body')[0];
    for (var i = body.classList.length - 1; i >= 0; i--) {
      body.classList.remove(body.classList[i]);
    }
    body.classList.add("sidebar-opposite-visible");

    // ========================================
    //
    // Navbar
    //
    // ========================================


    // Navbar navigation
    // -------------------------

    // Prevent dropdown from closing on click
    $(document).delegate(".dropdown-content", "click", function (e) {
      e.stopPropagation();
    });

    // Disabled links
    $(document).delegate(".navbar-nav .disabled a", "click", function (e) {
      e.preventDefault();
      e.stopPropagation();
    });


    // Collapse elements
    // -------------------------

    //
    // Sidebar categories
    //

    var _this = this;

    // Hide if collapsed by default
    $('.category-collapsed').children('.category-content').hide();


    // Rotate icon if collapsed by default
    $('.category-collapsed').find('[data-action=collapse]').addClass('rotate-180');



    // Collapse on click
    $(document).delegate(".category-title [data-action=collapse]", "click", function (e) {
      e.preventDefault();
      var $categoryCollapse = $(this).parent().parent().parent().nextAll();
      $(this).parents('.category-title').toggleClass('category-collapsed');
      $(this).toggleClass('rotate-180');


      _this.containerHeight(); // adjust page height

      $categoryCollapse.slideToggle(150);
    });

    //
    // Panels
    //

    // Hide if collapsed by default
    $('.panel-collapsed').children('.panel-heading').nextAll().hide();


    // Rotate icon if collapsed by default
    $('.panel-collapsed').find('[data-action=collapse]').addClass('rotate-180');


    // Collapse on click
    //$('.panel [data-action=collapse]').on("click",function (e) {
    $(document).delegate(".panel [data-action=collapse]", "click", function (e) {
      e.preventDefault();
      var $panelCollapse = $(this).parent().parent().parent().parent().nextAll();
      $(this).parents('.panel').toggleClass('panel-collapsed');
      $(this).toggleClass('rotate-180');

      _this.containerHeight(); // recalculate page height

      $panelCollapse.slideToggle(150);
    });



    // Remove elements
    // -------------------------

    // Panels
    $(document).delegate(".panel [data-action=close]", "click", function (e) {
      e.preventDefault();
      var $panelClose = $(this).parent().parent().parent().parent().parent();

      _this.containerHeight(); // recalculate page height

      $panelClose.slideUp(150, function () {
        $(this).remove();
      });
    });


    // Sidebar categories
    $(document).delegate(".category-title [data-action=close]", "click", function (e) {
      e.preventDefault();
      var $categoryClose = $(this).parent().parent().parent().parent();

      _this.containerHeight(); // recalculate page height

      $categoryClose.slideUp(150, function () {
        $(this).remove();
      });
    });


    // ========================================
    //
    // Sidebars
    //
    // ========================================


    // Mini sidebar
    // -------------------------

    // Toggle mini sidebar
    $(document).delegate(".sidebar-main-toggle", "click", function (e) {
      e.preventDefault();

      // Toggle min sidebar class
      $('body').toggleClass('sidebar-xs');
    });



    // Sidebar controls
    // -------------------------

    // Disable click in disabled navigation items
    $(document).delegate(".navigation .disabled a", "click", function (e) {
      e.preventDefault();
    });


    // Adjust page height on sidebar control button click
    $(document).delegate(".sidebar-control", "click", function (e) {
      _this.containerHeight();
    });


    // Hide main sidebar in Dual Sidebar
    $(document).delegate(".sidebar-main-hide", "click", function (e) {
      e.preventDefault();
      $('body').toggleClass('sidebar-main-hidden');
    });


    // Toggle second sidebar in Dual Sidebar
    $(document).delegate(".sidebar-secondary-hide", "click", function (e) {
      e.preventDefault();
      $('body').toggleClass('sidebar-secondary-hidden');
    });


    // Hide all sidebars
    $(document).delegate(".sidebar-all-hide", "click", function (e) {
      e.preventDefault();

      $('body').toggleClass('sidebar-all-hidden');
    });



    //
    // Opposite sidebar
    //

    // Collapse main sidebar if opposite sidebar is visible
    $(document).delegate(".sidebar-opposite-toggle", "click", function (e) {
      e.preventDefault();

      // Opposite sidebar visibility
      $('body').toggleClass('sidebar-opposite-visible');

      // If visible
      if ($('body').hasClass('sidebar-opposite-visible')) {

        // Make main sidebar mini
        $('body').addClass('sidebar-xs');

        // Hide children lists
        $('.navigation-main').children('li').children('ul').css('display', '');
      }
      else {

        // Make main sidebar default
        $('body').removeClass('sidebar-xs');
      }
    });


    // Hide main sidebar if opposite sidebar is shown
    $(document).delegate(".sidebar-opposite-main-hide", "click", function (e) {
      e.preventDefault();

      // Opposite sidebar visibility
      $('body').toggleClass('sidebar-opposite-visible');

      // If visible
      if ($('body').hasClass('sidebar-opposite-visible')) {

        // Hide main sidebar
        $('body').addClass('sidebar-main-hidden');
      }
      else {

        // Show main sidebar
        $('body').removeClass('sidebar-main-hidden');
      }
    });


    // Hide secondary sidebar if opposite sidebar is shown
    $(document).delegate(".sidebar-opposite-secondary-hide", "click", function (e) {
      e.preventDefault();

      // Opposite sidebar visibility
      $('body').toggleClass('sidebar-opposite-visible');

      // If visible
      if ($('body').hasClass('sidebar-opposite-visible')) {

        // Hide secondary
        $('body').addClass('sidebar-secondary-hidden');

      }
      else {

        // Show secondary
        $('body').removeClass('sidebar-secondary-hidden');
      }
    });


    // Hide all sidebars if opposite sidebar is shown
    $(document).delegate(".sidebar-opposite-hide", "click", function (e) {
      e.preventDefault();

      // Toggle sidebars visibility
      $('body').toggleClass('sidebar-all-hidden');

      // If hidden
      if ($('body').hasClass('sidebar-all-hidden')) {

        // Show opposite
        $('body').addClass('sidebar-opposite-visible');

        // Hide children lists
        $('.navigation-main').children('li').children('ul').css('display', '');
      }
      else {

        // Hide opposite
        $('body').removeClass('sidebar-opposite-visible');
      }
    });


    // Keep the width of the main sidebar if opposite sidebar is visible
    $(document).delegate(".sidebar-opposite-fix", "click", function (e) {
      e.preventDefault();

      // Toggle opposite sidebar visibility
      $('body').toggleClass('sidebar-opposite-visible');
    });



    // Mobile sidebar controls
    // -------------------------

    // Toggle main sidebar
    $(document).delegate(".sidebar-mobile-main-toggle", "click", function (e) {
      e.preventDefault();
      $('body').toggleClass('sidebar-mobile-main').removeClass('sidebar-mobile-secondary sidebar-mobile-opposite');
    });


    // Toggle secondary sidebar
    $(document).delegate(".sidebar-mobile-secondary-toggle", "click", function (e) {
      e.preventDefault();
      $('body').toggleClass('sidebar-mobile-secondary').removeClass('sidebar-mobile-main sidebar-mobile-opposite');
    });


    // Toggle opposite sidebar
    $(document).delegate(".sidebar-mobile-opposite-toggle", "click", function (e) {
      e.preventDefault();
      $('body').toggleClass('sidebar-mobile-opposite').removeClass('sidebar-mobile-main sidebar-mobile-secondary');
    });

  }

  // Calculate min height
  containerHeight() {
    var availableHeight = $(window).height() - $('.page-container').offset().top - $('.navbar-fixed-bottom').outerHeight();

    $('.page-container').attr('style', 'min-height:' + availableHeight + 'px');
  }

}
