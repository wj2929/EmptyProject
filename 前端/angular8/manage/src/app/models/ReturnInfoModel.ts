export class ReturnInfoModel {
  constructor(public State: boolean = false, public Message: string, public DataObject?: any) {

  }
}

export class ReturnTokenInfoModel {
  constructor(public State: boolean = false, public Message: string, public AccessToken:string,public Expires:string) {

  }
}

export class ReturnModel<T> {
  constructor(public State: boolean = false, public Message: string, public DataObject: T) {

  }
}

export class BatchProgressInfo {
  constructor(public State: boolean = false,
    public Message: string,
    public AllCount: number = 0,
    public CurrentBatch: number = 0,
    public BatchCount: number = 0,
    public LastBatch : boolean) {

  }
}

export class ReturnListModel<T> {
  constructor(public State: boolean = false, public Message: string, public DataObject: Array<T>) {

  }
}

export class ReturnPagingModel<T> {
  constructor(public State: boolean = false, public Message: string, public DataObject: PagingModel<T>) {

  }
}

export class PagingModel<T>{
  constructor(public Module_Page: ModulePageModel, public PageListInfos: Array<T>) {

  }
}

export class ModulePageModel {
  // constructor() {

  // }
  constructor(
    public AllCount: number = 0,
    public End: number = 0,
    public First: number = 0,
    public Max: number = 0,
    public PageCount: number = 0,
    public PageNum: number = 0,
    public PageSize: number = 0) {

  }
}
