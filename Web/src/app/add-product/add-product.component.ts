import { Component, OnInit } from '@angular/core';
import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';
import { IProduct } from '../models/IProduct';
import { ProductsService } from 'src/app/services/products.service';
import { ICategory } from '../models/ICategory';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent implements OnInit {

  title = 'appBootstrap';
   
  closeResult: string = '';

  category: ICategory = {
    name: '0',
  }
  

  product : IProduct = {
    id: 0,
    name: '',
    code: '',
    description: '',
    inStock: 0,
    sold: 0,
    status: 'active',
    images: [],
    category: this.category,
  }
  

  constructor(private productsService : ProductsService , private modalService: NgbModal) {}

  submit() {
    console.log(this.product)
    this.productsService.sendProduct(this.product)
    .subscribe({
      next: (response) => {
        // this.router.navigate([])
        alert("successfully added")
      }
    });
  }
  ngOnInit() {

  }
    
  /**
   * Write code on Method
   *
   * @return response()
   */
  open(content:any) {
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  } 
    
  /**
   * Write code on Method
   *
   * @return response()
   */
  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return  `with: ${reason}`;
    }
  }

}

