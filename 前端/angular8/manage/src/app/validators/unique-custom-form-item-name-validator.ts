import { CustomformService } from './../services/customform.service';
import { Injectable, Directive } from '@angular/core';
import { AsyncValidator, AbstractControl, ValidationErrors, AsyncValidatorFn, NG_ASYNC_VALIDATORS } from '@angular/forms';
import { Observable, of, timer } from 'rxjs';

export function UniqueCustomFormItemNameValidator(CustomformService: CustomformService,customFormId): AsyncValidatorFn {
    return (control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> => {
        return CustomformService.itemNameExist(customFormId,control.value);
    };
} 

// @Directive({
//     selector: '[CustomFormItemNameExists][formControlName],[CustomFormItemNameExists][formControl],[CustomFormItemNameExists][ngModel]',
//     providers: [{provide: NG_ASYNC_VALIDATORS, useExisting: UniqueCustomFormItemNameDirective, multi: true}]
//   })
//   export class UniqueCustomFormItemNameDirective implements AsyncValidator {
//     constructor(private CustomformService: CustomformService,private customFormId:string) {  }
  
//     validate(control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> {
//        return UniqueCustomFormItemNameValidator(this.CustomformService,this.customFormId)(control);
//     }
//   } 
