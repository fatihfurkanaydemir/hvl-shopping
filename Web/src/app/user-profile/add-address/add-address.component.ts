import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  ModalDismissReasons,
  NgbActiveModal,
  NgbModal,
} from '@ng-bootstrap/ng-bootstrap';
import { IAddAddress } from 'src/app/models/IAddAddress';
import { ICustomer } from 'src/app/models/ICustomer';
import { ToastService } from 'src/app/services/toast.service';
import { UserService } from 'src/app/services/user.service';
import { SharedValues } from 'src/app/shared/SharedValues';

@Component({
  selector: 'app-add-address',
  templateUrl: './add-address.component.html',
  styleUrls: ['./add-address.component.css'],
})
export class AddAddressComponent implements OnInit {
  @Output('OnAddressAdded') addressAddedEvent: EventEmitter<boolean> =
    new EventEmitter<boolean>();
  constructor(
    private modalService: NgbModal,
    private userService: UserService,
    private toastService: ToastService
  ) {}

  @Input('customer') customer!: ICustomer;

  cities = SharedValues.cities;
  closeResult: string = '';

  addAddressForm: FormGroup = new FormGroup({
    title: new FormControl('', [Validators.required]),
    description: new FormControl('', [Validators.required]),
    city: new FormControl('Ankara', [Validators.required]),
  });

  ngOnInit(): void {}

  onSubmit(modal: NgbActiveModal) {
    if (this.addAddressForm.invalid) return;

    const addAddressData: IAddAddress = {
      identityId: this.customer.identityId,
      title: this.addAddressForm.value.title,
      addressDescription: this.addAddressForm.value.description,
      city: this.addAddressForm.value.city,
    };

    this.userService.addAddress(addAddressData).subscribe({
      next: (response) => {
        this.toastService.showToast({
          icon: 'success',
          title: 'Adres Eklendi',
        });

        modal.close();
        this.addAddressForm.reset({
          city: 'Ankara',
        });
        this.addressAddedEvent.emit(true);
      },
      error: (error) => {
        this.toastService.showToast({
          icon: 'error',
          title: error.error.message,
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

  isFormControlInvalid(name: string) {
    return (
      this.addAddressForm.get(name)?.invalid &&
      (this.addAddressForm.get(name)?.dirty ||
        this.addAddressForm.get(name)?.touched)
    );
  }

  isFormControlValid(name: string) {
    return (
      this.addAddressForm.get(name)?.valid &&
      (this.addAddressForm.get(name)?.dirty ||
        this.addAddressForm.get(name)?.touched)
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
