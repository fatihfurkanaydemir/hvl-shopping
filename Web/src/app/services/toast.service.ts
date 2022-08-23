import { Injectable } from '@angular/core';
import { TimeInterval } from 'rxjs';
import Swal, { SweetAlertOptions, SweetAlertIcon } from 'sweetalert2';

@Injectable({ providedIn: 'root' })
export class ToastService {
  toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 2000,
    timerProgressBar: true,
    // didOpen: (toast) => {
    //   toast.addEventListener('mouseenter', Swal.stopTimer);
    //   toast.addEventListener('mouseleave', Swal.resumeTimer);
    // },
  });

  showToast(options: SweetAlertOptions) {
    this.toast.fire(options);
  }

  showNotification(title: string, html: string, icon: SweetAlertIcon) {
    Swal.fire({
      icon: icon,
      title: title,
      html: html,
      confirmButtonText: 'Tamam',
      background: '#fff url(https://sweetalert2.github.io/images/trees.png)',
      backdrop: `
        rgba(0,0,123,0.4)
        url("https://sweetalert2.github.io/images/nyan-cat.gif")
        left top
        no-repeat
      `,
    });
  }
}
