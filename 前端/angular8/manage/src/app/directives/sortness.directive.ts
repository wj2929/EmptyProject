import {Directive, ElementRef, EventEmitter, HostListener, Input, OnInit, Output} from '@angular/core';

@Directive({
  selector: '[sortable]'
})
export class SortnessDirective implements OnInit{

  @Output() sorted = new EventEmitter();// return the sorted array

  @Input() data: [];// to get array of sorting
  @Input() indexName: string;// to get the name of the column which has to be sorted
  @Input() defaultSort: boolean;// to get the name of the column which has to be sorted
  @Input() allowArrows: boolean;// this property should go in the table tag to allow
  @Input() sortMethod: (a, b) => number;
  order:string = 'desc'; // asc or desc // the order in which sorting should be
  // arrow = {up:'&#9650;', down:'&#9660;'};
  private arrow = {up:'▲', down:'▼'};
  private headersTxt =  [];
  private cellsPointer = [];
  constructor(private el: ElementRef) { }

  @HostListener('click') clickHandler(){
    if(this.allowArrows === undefined){
      setTimeout(() => this.sortingMethod(), 200);
    }else{
      this.updateCells();
    }
  }

  sortingMethod(){
    if(this.indexName) {
      this.order = this.order == 'desc' ? 'asc' : 'desc';
      if (this.order == 'desc') {
        this.el.nativeElement.innerText = this.arrow.up + " "  + this.el.nativeElement.innerText;
      } else {
        this.el.nativeElement.innerText = this.arrow.down + " " + this.el.nativeElement.innerText;
      }
      if (this.order === 'asc') {
        if (this.sortMethod) {
          this.data.sort((a, b) => this.sortMethod(a, b));
        } else {
          this.data.sort((a, b) => this._sort(a, b));
        }
      } else {
        if (this.sortMethod) {
          this.data.sort((a, b) => this.sortMethod(b, a));
        } else {
          this.data.sort((a, b) => this._sort(b, a));
        }
      }
      this.sorted.emit(this.data);
    }

  }

  private _sort(a, b) {
    if (a[this.indexName] < b[this.indexName]) return -1;
    else if (a[this.indexName] > b[this.indexName]) return 1;
    return 0;
  }

  ngOnInit(): void {
    this.initHeadersText();
    if (this.defaultSort !== undefined){
      this.sortingMethod()
    }
  }

  initHeadersText(){
    if(this.allowArrows === undefined) return;
    if(this.el.nativeElement.children[0].children[0].cells){
      const cells = this.el.nativeElement.children[0].children[0].cells;
      if(cells){
        this.cellsPointer = cells;
        for (let i = 0; i < cells.length; i++){
          this.headersTxt.push(cells[i].innerText.trim());
        }
      }
    }
  }

  updateCells(){
    for (let i = 0; i < this.cellsPointer.length; i++){
      this.cellsPointer[i].innerText = this.headersTxt[i];
    }
  }


}