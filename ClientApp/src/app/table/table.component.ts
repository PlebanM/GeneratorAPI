import { Component, OnInit, ViewChild, ViewContainerRef, ComponentFactoryResolver, ComponentRef } from '@angular/core';
import { ColumnTypesGetterService } from '../services/column-types-getter.service';
import { ColumnType } from '../models/column-type';
import { ColumnComponent } from '../column/column.component';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css']
})
export class TableComponent implements OnInit {

  _ref: any;

  columnTypes: Array<ColumnType>;
  columns: Array<ComponentRef<ColumnComponent>> = [];

  @ViewChild('columns', { static: true, read: ViewContainerRef })
  container: ViewContainerRef;

  constructor(private columnTypeGetter: ColumnTypesGetterService, private crf: ComponentFactoryResolver) { }

  ngOnInit() {
    this.columnTypeGetter.getColumnTypes().subscribe(res => {
      this.columnTypes = res;
    });
  }

  addColumn(): void {
    let column = this.crf.resolveComponentFactory(ColumnComponent);
    let columnComponent = this.container.createComponent(column);
    let instance = columnComponent.instance;
    instance.deleteEvent.subscribe(this.onColumnDelete.bind(this));
    instance.columnTypes = this.columnTypes;
    instance._ref = columnComponent;
    this.columns.push(columnComponent);
  }

  onColumnDelete(ref: ComponentRef<ColumnComponent>) {
    this.columns = this.columns.filter(elem => elem != ref);
  }

  removeObject(): void {
    this._ref.destroy();
  }

}
