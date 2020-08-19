import { CustomFormSetting } from 'src/app/models/CustomFormModels';

export class DisplayLogModel {
    constructor(
        public Id: string,
        public UserName: string,
        public Content: string,
        public CustomFormSetting:CustomFormSetting,
        public JsonObj:any
    ) {

    }
}
