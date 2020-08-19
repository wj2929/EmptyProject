import { DisplayCustomFormItemModel } from './../models/CustomFormModels';
import { AsyncValidator, AbstractControl, ValidationErrors, AsyncValidatorFn, NG_ASYNC_VALIDATORS, ValidatorFn } from '@angular/forms';
import { Observable, of, timer } from 'rxjs';

export function CustomformitemValidator(DisplayCustomFormItemModel:DisplayCustomFormItemModel): ValidatorFn {
    return (control: AbstractControl): any => {
        // debugger;
        if(DisplayCustomFormItemModel.ValidationConfig_Required && control.value === "")
            return {required:true};
        
        if(DisplayCustomFormItemModel.ValidationConfig_AllowExtensionValidation && DisplayCustomFormItemModel.ValidationConfig_RegularExpressionValidator != ""){
            let reg:RegExp = new RegExp(DisplayCustomFormItemModel.ValidationConfig_RegularExpressionValidator,'gi');
            if(!reg.test(control.value)) return {RegularExpressionValidator:true};
        }
        return null;
    };
} 