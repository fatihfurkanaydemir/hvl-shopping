import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderTotalsComponent } from './order-totals.component';

@NgModule({
  declarations: [OrderTotalsComponent],
  imports: [
    CommonModule
  ],
  exports:[
    OrderTotalsComponent
  ]
})
export class OrderTotalsModule { }
