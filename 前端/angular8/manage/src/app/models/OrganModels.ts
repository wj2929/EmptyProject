import { CustomFormSetting } from 'src/app/models/CustomFormModels';

export class DisplayOrganModel {
    constructor(
        public Id: string,
        public Name: string,
        public OrganStatusName: string,
        public OrganStatus:number,
        public OrganType:number,
        public OrganTypeName:string,
        public CustomFormKeycode:string,
        public CustomFormSetting :CustomFormSetting,
        public CategoryIds:Array<string>
    ) {

    }
}
