import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';
import { ICategory } from '../models/ICategory';
import { IProduct } from '../models/IProduct';
import { ProductsService } from '../services/products.service';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.css']
})
export class EditProductComponent implements OnInit {

  constructor(private productsService : ProductsService , private modalService: NgbModal, private route: ActivatedRoute) {}

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
    category: this.category
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(
      {next: (params: any) => {
        const id = params.get('id');

        if(id){
          this.productsService.getProduct(id)
          .subscribe({
            next: (response: any) => {
              this.product = response;
            }
          })
        }
      }
      }
    )
  }
  closeResult = '';


  open(content: any) {
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
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
  submitUpdate(){
    this.productsService.updateProduct(this.product.id, this.product)
    .subscribe({
      next: (response : any) => {
        // this.router.navigate([])
        alert("successfully updated")
      }
    });
  }
}
