import { CustomFormSetting } from 'src/app/models/CustomFormModels';

export class DisplayCertificateWorkFlowLogModel {
    constructor(
        public Id: string,
        public CertificateStatusName: string,
        public CreateDate: string,
        public Content:number,
        public Certificate_Id:string,
    ) {

    }
}
