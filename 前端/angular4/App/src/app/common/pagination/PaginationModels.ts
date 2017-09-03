
export class PaginationModel {
    constructor(public AllCount: number = 0,
        public CurrentPage: number = 0,
        public PageCount: number = 0) {

    }
}

/***
 * 分页组件分页事件参数
 */
export class PageChangedEventArguments {
    constructor(public page:number) {

    }

}
