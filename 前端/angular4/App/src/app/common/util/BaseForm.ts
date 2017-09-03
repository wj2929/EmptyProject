import {FormGroup} from "@angular/forms";
export class BaseForm {
    public formModel: FormGroup;
    Error(FieldName: string) {
        return this.formModel.get(FieldName).invalid && this.formModel.get(FieldName).touched;
    }
    hasError(errorCode: string, path?: string[]) {
        return this.formModel.hasError(errorCode, path);
    }
}
