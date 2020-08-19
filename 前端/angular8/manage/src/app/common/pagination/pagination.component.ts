import { PageChangedEventArguments, PaginationModel } from './PaginationModels';
import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent implements OnInit {

  /**
   * 总条目数
   */
  @Input()
  AllCount: number = 0;

  /**
   * 当前页码
   */
  @Input()
  CurrentPage: number = 0;

  /**
   * 总页数
   */
  @Input()
  PageCount: number = 0;

  @Input()
  Key: string = 'DefaultPagination';


  @Output()
  pageChanged: EventEmitter<PageChangedEventArguments> = new EventEmitter<PageChangedEventArguments>();

  constructor() { }

  ngOnInit() {
  }

  OnPageChanged(CurrentPage: number) {
    if (this.CurrentPage != CurrentPage) {
      this.CurrentPage = CurrentPage;
      this.pageChanged.emit(new PageChangedEventArguments(this.CurrentPage));
      // this.pageChanged.emit(new PageChangedEventArguments(this.Key, new PaginationModel(this.AllCount, this.CurrentPage, this.PageCount)));
    }
  }

}
