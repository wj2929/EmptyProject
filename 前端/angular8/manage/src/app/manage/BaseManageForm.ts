import {FormGroup} from "@angular/forms";
import { BaseForm } from '../common/util/BaseForm';
export class BaseManageForm extends BaseForm {
    IsLogin(){
        return true;
    }
}
