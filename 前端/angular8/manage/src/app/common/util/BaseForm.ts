import { FormGroup, FormArray,FormControl } from "@angular/forms";
export class BaseForm {
    public formModel: FormGroup;
    Error(FieldName: string) {
        return this.formModel.get(FieldName).invalid && this.formModel.get(FieldName).touched;
    }
    hasError(errorCode: string, path?: string[]) {
        return this.formModel.hasError(errorCode, path);
    }

    selectedFormModel() {
        return [].concat(...Object.values(this.formModel.value));
    }

    formModelValue(){
        return this.formModel.value;
    }

    validateForm() {
        Object.keys(this.formModel.controls).forEach(field => {
            const control = this.formModel.controls[field];
            control.markAsTouched({ onlySelf: true });

            if(control.constructor == FormGroup){
                this.validateFormGroup(control as FormGroup);
            }
        });
    }

    validateFormGroup(formGroup:FormGroup) {
        if(formGroup.controls.CustomFormItemSettings!=null){
            this.validateFormArray(formGroup.controls.CustomFormItemSettings as FormArray);
        }
        else{
            Object.keys(formGroup.controls).forEach(field =>{
                formGroup.controls[field].markAsTouched({ onlySelf: true });
            });
        }
    }

    validateFormArray(formArray:FormArray){
        formArray.controls.forEach(control =>{
            control.markAsTouched({ onlySelf: true });

            if(control.constructor == FormGroup)
                this.validateFormGroup(control as FormGroup);
        });
    }

}
        