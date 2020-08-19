import { CustomFormSetting } from 'src/app/models/CustomFormModels';
import Dictionary from '../common/util/util';

export class DisplayUserThirdLoginExtendModel {
    constructor(
        public Id:string,
        public OpenId: string,
        public BindUserName: string,
        public Key: number,
        public AccessToken:number,
        public LastBindDateTime:Date,
        public BindUserNameCount: number,
        public CustomFormSetting :CustomFormSetting,
        public CustomFormSettingDictionary : Dictionary
    ) {

    }
}

export class DisplayUserThirdLoginHistoryModel {
    constructor(
        public CreateDate:Date,
        public BindUserName: string
    ) {

    }
}
