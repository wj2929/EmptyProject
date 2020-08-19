import { CustomformService } from './../services/customform.service';
import { Injectable, Directive } from '@angular/core';
import { AsyncValidator, AbstractControl, ValidationErrors, AsyncValidatorFn, NG_ASYNC_VALIDATORS } from '@angular/forms';
import { Observable, of, timer } from 'rxjs';

// @Injectable({ providedIn: 'root' })
export function UniqueCustomFormKeyValidator(CustomformService: CustomformService): AsyncValidatorFn {
    return (control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> => {
        return CustomformService.keyExist(control.value);
    };
} 

// @Directive({
//     selector: '[CustomFormKeyExists][formControlName],[CustomFormKeyExists][formControl],[CustomFormKeyExists][ngModel]',
//     providers: [{provide: NG_ASYNC_VALIDATORS, useExisting: UniqueCustomFormKeyDirective, multi: true}]
//   })
//   export class UniqueCustomFormKeyDirective implements AsyncValidator {
//     constructor(private CustomformService: CustomformService) {  }
  
//     validate(control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> {
//        return UniqueCustomFormKeyValidator(this.CustomformService)(control);
//     }
//   } 
