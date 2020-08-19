import { CustomFormSetting } from 'src/app/models/CustomFormModels';
import Dictionary from '../common/util/util';

export class DisplayDataInfoModel {
    constructor(
        public Id: string,
        public Name: string,
        public OrderBy: number,
        public Level:number,
        public ParentDataInfo_Id:string,
        public Index: string,
        public CustomFormKeycode:string,
        public CustomFormSetting :CustomFormSetting,
        public CustomFormSettingDescription :string,
        public CustomFormSettingDictionary : Dictionary,
        public obj1 :any,
        public obj2 :any
    ) {

    }
}
