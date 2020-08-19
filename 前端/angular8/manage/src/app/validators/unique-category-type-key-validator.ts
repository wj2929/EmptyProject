import { CategoryService } from './../services/category.service';
import { Injectable, Directive } from '@angular/core';
import { AsyncValidator, AbstractControl, ValidationErrors, AsyncValidatorFn, NG_ASYNC_VALIDATORS } from '@angular/forms';
import { Observable, of, timer } from 'rxjs';

export function UniqueCategoryTypeKeyValidator(CategoryService: CategoryService): AsyncValidatorFn {
    return (control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> => {
        return CategoryService.keyExist(control.value);
    };
} 

// @Directive({
//     selector: '[CategoryTypeKeyExists][formControlName],[CategoryTypeKeyExists][formControl],[CategoryTypeKeyExists][ngModel]',
//     providers: [{provide: NG_ASYNC_VALIDATORS, useExisting: UniqueCategoryTypeKeyDirective, multi: true}]
//   })
//   export class UniqueCategoryTypeKeyDirective implements AsyncValidator {
//     constructor(private CategoryService: CategoryService) {  }
  
//     validate(control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> {
//        return UniqueCategoryTypeKeyValidator(this.CategoryService)(control);
//     }
//   } 
