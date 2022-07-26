import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { NgForm } from '@angular/forms';
import {
  NgbModal,
  ModalDismissReasons,
  NgbActiveModal,
} from '@ng-bootstrap/ng-bootstrap';
import { ImagePickerConf } from 'ngp-image-picker';
import { IImage } from 'src/app/models/IImage';

import { ProductsService } from 'src/app/services/products.service';
import { ToastService } from 'src/app/services/toast.service';
import { UploadService } from 'src/app/services/upload.service';
import { environment } from 'src/environments/environment';
import { ICategory } from '../../models/ICategory';
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

  imagePickerConf: ImagePickerConf = {
    borderRadius: '4px',
    language: 'tr',
    width: '100%',
    height: '240px',
    hideDownloadBtn: true,
    hideEditBtn: true,
    compressInitial: 90,
  };

  maxImageCount = 5;
  selectedImageNames: string[] = Array.from(
    { length: this.maxImageCount },
    (v, i) => `img${i}`
  );

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
    price: 1,
    inStock: 0,
    images: [],
    categoryId: 0,
  };

  constructor(
    private productsService: ProductsService,
    private categoriesService: CategoriesService,
    private modalService: NgbModal,
    private toastService: ToastService,
    private uploadService: UploadService
  ) {
    this.selectedImageNames.forEach((name) => {
      sessionStorage.removeItem(name);
    });
  }

  onSubmit(updateProductForm: NgForm, modal: NgbActiveModal) {
    if (updateProductForm.invalid) return;

    this.product.categoryId = +updateProductForm.value['product-category'];

    this.uploadService.uploadImages(this.selectedImageNames).subscribe({
      next: (response) => {
        this.product.images.push(...response.data);
        this.product.images = this.product.images.filter((image) => image);

        this.productsService.updateProduct(this.product).subscribe({
          next: (response) => {
            this.toastService.showToast({
              icon: 'success',
              title: 'Ürün başarılı bir şekilde güncellendi.',
            });

            modal.dismiss();
            updateProductForm.reset();
            this.productUpdatedEvent.emit(true);
          },
          error: (error) => {
            console.log(error);

            this.toastService.showToast({
              icon: 'error',
              title: 'Ürün güncellenirken hata oluştu.',
            });
          },
        });
      },
      error: (error) => {
        this.toastService.showToast({
          icon: 'error',
          title: 'Resimler yüklenirken hata oluştu.',
        });
      },
    });
  }

  ngOnInit() {
    this.categoriesService
      .getAllCategories(this.pageNumber, this.pageSize)
      .subscribe((response) => {
        this.categories = response.data;
      });
  }

  onImageChange(event: string, index: number) {
    if (sessionStorage.getItem(`img${index}`)?.startsWith('unchanged+')) {
      this.product.images[index] = null!;
    }

    sessionStorage.setItem(this.selectedImageNames[index], event);
  }

  getFullImageUrl(image: IImage) {
    if (!image.url) return '';

    return environment.baseUrl + '/' + image.url;
  }

  openModal(content: any) {
    this.selectedImageNames.forEach((name) => {
      sessionStorage.removeItem(name);
    });

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
      this.product.price = response.data.price;

      for (let i = 0; i < this.product.images.length; ++i) {
        sessionStorage.setItem(
          `img${i}`,
          'unchanged+' + this.product.images[i].url
        );
      }
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
