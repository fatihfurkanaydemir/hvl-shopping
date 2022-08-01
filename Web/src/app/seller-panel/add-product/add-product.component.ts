import { Component, EventEmitter, OnInit, Output } from '@angular/core';

import { NgForm } from '@angular/forms';
import {
  NgbModal,
  ModalDismissReasons,
  NgbActiveModal,
} from '@ng-bootstrap/ng-bootstrap';

import { ProductsService } from 'src/app/services/products.service';
import { ToastService } from 'src/app/services/toast.service';
import Swal from 'sweetalert2';
import { ICategory } from '../../models/ICategory';
import { IProductCreate } from '../../models/IProductCreate';
import { CategoriesService } from '../../services/categories.service';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css'],
})
export class AddProductComponent implements OnInit {
  @Output('OnProductAdded') productAddedEvent: EventEmitter<boolean> =
    new EventEmitter();

  pageNumber: number = 1;
  pageSize: number = 30;

  closeResult: string = '';
  categories: ICategory[] = [];

  product: IProductCreate = {
    name: '',
    code: '',
    description: '',
    inStock: 0,
    images: [],
    categoryId: 0,
  };

  constructor(
    private productsService: ProductsService,
    private categoriesService: CategoriesService,
    private modalService: NgbModal,
    private toastService: ToastService
  ) {}

  onSubmit(addProductForm: NgForm, modal: NgbActiveModal) {
    if (addProductForm.invalid) return;

    if (addProductForm.value['product-images'])
      this.product.images.push({ url: addProductForm.value['product-images'] });

    this.product.categoryId = +addProductForm.value['product-category'];

    this.productsService.createProduct(this.product).subscribe((response) => {
      if (response.succeeded) {
        this.toastService.showToast({
          icon: 'success',
          title: 'Ürün başarılı bir şekilde eklendi.',
        });

        modal.dismiss();
        addProductForm.reset();
        this.productAddedEvent.emit(true);
      } else {
        this.toastService.showToast({
          icon: 'error',
          title: 'Ürün eklenirken hata oluştu.',
        });

        modal.dismiss();
        addProductForm.reset();
        this.productAddedEvent.emit(true);
      }
    });
  }

  ngOnInit() {
    this.categoriesService
      .getAllCategories(this.pageNumber, this.pageSize)
      .subscribe((response) => {
        this.categories = response.data;
      });
  }

  open(content: any) {
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
