import { Component, EventEmitter, OnInit, Output } from '@angular/core';

import { NgForm } from '@angular/forms';
import {
  NgbModal,
  ModalDismissReasons,
  NgbActiveModal,
} from '@ng-bootstrap/ng-bootstrap';
import { ImagePickerConf } from 'ngp-image-picker';
import { exhaustMap, take } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';

import { ProductsService } from 'src/app/services/products.service';
import { ToastService } from 'src/app/services/toast.service';
import { UploadService } from 'src/app/services/upload.service';
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

  imagePickerConf: ImagePickerConf = {
    borderRadius: '4px',
    language: 'tr',
    width: '100%',
    height: '240px',
    hideDownloadBtn: true,
    hideEditBtn: true,
    compressInitial: 90,
  };

  pageNumber: number = 1;
  pageSize: number = 30;

  closeResult: string = '';
  categories: ICategory[] = [];

  maxImageCount = 5;
  selectedImageNames: string[] = Array.from(
    { length: this.maxImageCount },
    (v, i) => `img${i}`
  );

  product: IProductCreate = {
    sellerIdentityId: '',
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
    private authService: AuthService,
    private uploadService: UploadService
  ) {
    this.selectedImageNames.forEach((name) => {
      sessionStorage.removeItem(name);
    });
  }

  onImageChange(event: string, index: number) {
    sessionStorage.setItem(this.selectedImageNames[index], event);
  }

  onSubmit(addProductForm: NgForm, modal: NgbActiveModal) {
    if (addProductForm.invalid) return;

    if (addProductForm.value['product-images'])
      this.product.images.push({ url: addProductForm.value['product-images'] });

    this.product.categoryId = +addProductForm.value['product-category'];

    this.uploadService.uploadImages(this.selectedImageNames).subscribe({
      next: (response) => {
        this.product.images = response.data;
        this.authService.userSubject
          .pipe(
            take(1),
            exhaustMap((user) => {
              this.product.sellerIdentityId = user.identityId;
              return this.productsService.createProduct(this.product);
            })
          )
          .subscribe({
            next: (response) => {
              this.toastService.showToast({
                icon: 'success',
                title: 'Ürün başarılı bir şekilde eklendi.',
              });

              modal.dismiss();
              addProductForm.reset();
              this.productAddedEvent.emit(true);
            },
            error: (error) => {
              this.toastService.showToast({
                icon: 'error',
                title: 'Ürün eklenirken hata oluştu.',
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

  open(content: any) {
    this.selectedImageNames.forEach((name) => {
      sessionStorage.removeItem(name);
    });

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
