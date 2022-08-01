import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

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
import { IProduct } from '../../models/IProduct';
import { IProductCreate } from '../../models/IProductCreate';
import { IProductUpdate } from '../../models/IProductUpdate';
import { CategoriesService } from '../../services/categories.service';

@Component({
  selector: 'app-product-actions',
  templateUrl: './product-actions.component.html',
  styleUrls: ['./product-actions.component.css'],
})
export class ProductActionsComponent implements OnInit {
  @Output('OnProductUpdated') productUpdatedEvent: EventEmitter<boolean> =
    new EventEmitter();

  @Input() productId!: number;
  @Input() productStatus!: string;

  // to get categories
  pageNumber: number = 1;
  pageSize: number = 30;

  closeResult: string = '';
  categories: ICategory[] = [];

  product: IProductUpdate = {
    id: 0,
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

  onSubmit(updateProductForm: NgForm, modal: NgbActiveModal) {
    if (updateProductForm.invalid) return;

    if (updateProductForm.value['product-images'])
      this.product.images.push({
        url: updateProductForm.value['product-images'],
      });

    this.product.categoryId = +updateProductForm.value['product-category'];

    this.productsService.updateProduct(this.product).subscribe((response) => {
      if (response.succeeded) {
        this.toastService.showToast({
          icon: 'success',
          title: 'Ürün başarılı bir şekilde güncellendi.',
        });

        modal.dismiss();
        updateProductForm.reset();
        this.productUpdatedEvent.emit(true);
      } else {
        this.toastService.showToast({
          icon: 'error',
          title: 'Ürün güncellenirken hata oluştu.',
        });
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

  openModal(content: any) {
    this.getProduct();

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

  onProductStatusChangeClicked(currentStatus: string) {
    if (currentStatus === 'Active') {
      this.productsService
        .deactivateProduct(this.productId)
        .subscribe((response) => {
          this.productUpdatedEvent.emit(true);
        });
    } else if (currentStatus === 'Passive') {
      this.productsService
        .activateProduct(this.productId)
        .subscribe((response) => {
          this.productUpdatedEvent.emit(true);
        });
    }
  }

  getProduct() {
    this.productsService.getProduct(this.productId).subscribe((response) => {
      this.product.id = response.data.id;
      this.product.categoryId = response.data.category.id;
      this.product.code = response.data.code;
      this.product.description = response.data.description;
      this.product.images = response.data.images;
      this.product.name = response.data.name;
      this.product.inStock = response.data.inStock;
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
}
