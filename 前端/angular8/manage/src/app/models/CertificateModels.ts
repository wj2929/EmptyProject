import { CustomFormSetting } from 'src/app/models/CustomFormModels';

export class DisplayCertificateModel {
    constructor(
        public Id: string,
        public Xm:string,
        public Xb:string,
        public Sfzh:string,
        public Fzrq:string,
        public Fzdw:string,
        public Name: string,
        public OrganStatusName: string,
        public OrganStatus:number,
        public CustomFormKeycode:string,
        public CustomFormSetting :CustomFormSetting,
        public Organ_Id:string,
        public Category_Id:string,
        public CertificateStatus:number,
        public CustomFormSettingDescription :string
    ) {

    }
}
