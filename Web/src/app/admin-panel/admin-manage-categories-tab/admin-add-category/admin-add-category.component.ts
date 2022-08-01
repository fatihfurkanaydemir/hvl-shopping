import { Component, EventEmitter, OnInit, Output } from '@angular/core';

import { NgForm } from '@angular/forms';
import {
  NgbModal,
  ModalDismissReasons,
  NgbActiveModal,
} from '@ng-bootstrap/ng-bootstrap';
import { ICategoryCreate } from 'src/app/models/ICategoryCreate';

import { ToastService } from 'src/app/services/toast.service';
import { CategoriesService } from '../../../services/categories.service';

@Component({
  selector: 'app-admin-add-category',
  templateUrl: './admin-add-category.component.html',
  styleUrls: ['./admin-add-category.component.css'],
})
export class AdminAddCategoryComponent implements OnInit {
  @Output('OnCategoryAdded') categoryAddedEvent: EventEmitter<boolean> =
    new EventEmitter();

  closeResult: string = '';

  category: ICategoryCreate = {
    name: '',
  };

  constructor(
    private categoriesService: CategoriesService,
    private modalService: NgbModal,
    private toastService: ToastService
  ) {}

  onSubmit(addCategoryForm: NgForm, modal: NgbActiveModal) {
    if (addCategoryForm.invalid) return;

    this.categoriesService
      .createCategory(this.category)
      .subscribe((response) => {
        if (response.succeeded) {
          this.toastService.showToast({
            icon: 'success',
            title: 'Kategori başarılı bir şekilde eklendi.',
          });

          modal.dismiss();
          addCategoryForm.reset();
          this.categoryAddedEvent.emit(true);
        } else {
          this.toastService.showToast({
            icon: 'error',
            title: 'Ürün eklenirken hata oluştu.',
          });

          modal.dismiss();
          addCategoryForm.reset();
          this.categoryAddedEvent.emit(true);
        }
      });
  }

  ngOnInit() {}

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
