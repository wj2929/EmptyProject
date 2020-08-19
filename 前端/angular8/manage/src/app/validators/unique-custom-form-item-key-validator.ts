import { CustomformService } from './../services/customform.service';
import { Injectable, Directive } from '@angular/core';
import { AsyncValidator, AbstractControl, ValidationErrors, AsyncValidatorFn, NG_ASYNC_VALIDATORS } from '@angular/forms';
import { Observable, of, timer } from 'rxjs';

export function UniqueCustomFormItemKeyValidator(CustomformService: CustomformService,customFormId): AsyncValidatorFn {
    return (control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> => {
        return CustomformService.itemKeyExist(customFormId,control.value);
    };
} 

// @Directive({
//     selector: '[CustomFormItemKeyExists][formControlName],[CustomFormItemKeyExists][formControl],[CustomFormItemKeyExists][ngModel]',
//     providers: [{provide: NG_ASYNC_VALIDATORS, useExisting: UniqueCustomFormItemKeyDirective, multi: true}]
//   })
//   export class UniqueCustomFormItemKeyDirective implements AsyncValidator {
//     constructor(private CustomformService: CustomformService,private customFormId:string) {  }
  
//     validate(control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> {
//        return UniqueCustomFormItemKeyValidator(this.CustomformService,this.customFormId)(control);
//     }
//   } 
