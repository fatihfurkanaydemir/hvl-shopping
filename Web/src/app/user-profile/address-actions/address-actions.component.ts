import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  ModalDismissReasons,
  NgbActiveModal,
  NgbModal,
} from '@ng-bootstrap/ng-bootstrap';
import { IAddress } from 'src/app/models/IAddress';
import { ICustomer } from 'src/app/models/ICustomer';
import { IDeleteAddress } from 'src/app/models/IDeleteAddress';
import { IEditAddress } from 'src/app/models/IEditAddress';
import { ToastService } from 'src/app/services/toast.service';
import { UserService } from 'src/app/services/user.service';
import { SharedValues } from 'src/app/shared/SharedValues';

@Component({
  selector: 'app-address-actions',
  templateUrl: './address-actions.component.html',
  styleUrls: ['./address-actions.component.css'],
})
export class AddressActionsComponent implements OnInit {
  @Input('address') address!: IAddress;

  @Output('OnAddressChanged') addressChangedEvent: EventEmitter<boolean> =
    new EventEmitter<boolean>();

  @Input('customer') customer!: ICustomer;

  cities = SharedValues.cities;
  closeResult: string = '';

  editAddressForm: FormGroup = new FormGroup({
    title: new FormControl('', [Validators.required]),
    description: new FormControl('', [Validators.required]),
    city: new FormControl('Ankara', [Validators.required]),
  });

  constructor(
    private userService: UserService,
    private toastService: ToastService,
    private modalService: NgbModal
  ) {}

  ngOnInit(): void {
    this.editAddressForm.patchValue({
      title: this.address.title,
      city: this.address.city,
      description: this.address.addressDescription,
    });
  }

  onSubmit(editAddressModal: NgbActiveModal) {
    if (this.editAddressForm.invalid) return;

    const editAddressData: IEditAddress = {
      identityId: this.customer.identityId,
      addressId: this.address.id,
      title: this.editAddressForm.value.title,
      addressDescription: this.editAddressForm.value.description,
      city: this.editAddressForm.value.city,
    };

    this.userService.editAddress(editAddressData).subscribe({
      next: (response) => {
        this.toastService.showToast({
          icon: 'success',
          title: 'Adres Düzenlendi',
        });

        editAddressModal.close();
        this.editAddressForm.reset({
          city: 'Ankara',
        });
        this.addressChangedEvent.emit(true);
      },
      error: (error) => {
        console.log(error);

        this.toastService.showToast({
          icon: 'error',
          title: error.error?.message ?? 'Bir hata meydana geldi',
        });
        if (error.error.errors) {
          error.error.errors.forEach((err: any) => {
            this.toastService.showToast({
              icon: 'error',
              title: err,
            });
          });
        }
      },
    });
  }

  onDeleteAddress(deleteAddressModal: NgbActiveModal) {
    const deleteAddressData: IDeleteAddress = {
      addressId: this.address.id,
      identityId: this.customer.identityId,
    };

    this.userService.deleteAddress(deleteAddressData).subscribe({
      next: (response) => {
        this.toastService.showToast({
          icon: 'success',
          title: 'Adres Silindi',
        });

        deleteAddressModal.close();
        this.addressChangedEvent.emit(true);
      },
      error: (error) => {
        this.toastService.showToast({
          icon: 'error',
          title: error.error.message,
        });
      },
    });
  }

  isFormControlInvalid(name: string) {
    return (
      this.editAddressForm.get(name)?.invalid &&
      (this.editAddressForm.get(name)?.dirty ||
        this.editAddressForm.get(name)?.touched)
    );
  }

  isFormControlValid(name: string) {
    return (
      this.editAddressForm.get(name)?.valid &&
      (this.editAddressForm.get(name)?.dirty ||
        this.editAddressForm.get(name)?.touched)
    );
  }

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
