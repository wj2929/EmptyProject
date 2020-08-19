import { CustomFormSetting } from 'src/app/models/CustomFormModels';

export class PassportLoginModel {
    constructor(public UserName: string, public Password: string) {

    }
}

export class DisplayUserExtendModel{
    constructor(public Id: string, 
        public OrganName: string,
        public UserName: string,
        public RoleName:string,
        public Organ_Id:string,
        public OrganType:string,
        public CustomFormKeycode:string,
        public CustomFormSetting :CustomFormSetting,
        public CategoryIds:string[]
        ) {

    }
}