import { Component, Input, OnInit } from '@angular/core';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { IOrder, OrderStatus } from 'src/app/models/IOrder';

@Component({
  selector: 'app-orders-actions',
  templateUrl: './orders-actions.component.html',
  styleUrls: ['./orders-actions.component.css'],
})
export class OrdersActionsComponent implements OnInit {
  @Input('order') order!: IOrder;

  constructor(private modalService: NgbModal) {}

  closeResult: string = '';
  OrderStatus = OrderStatus;

  ngOnInit(): void {}

  openModal(content: any) {
    this.modalService
      .open(content, { ariaLabelledBy: 'modal-basic-title' })
      .result.then(
        (result) => {
          this.closeResult = `Closed with: ${result}`;
        },
        (reason) => {
          this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
        }
      );
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }
}
