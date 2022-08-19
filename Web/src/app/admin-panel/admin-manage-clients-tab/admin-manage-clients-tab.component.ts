import { Component, OnInit } from '@angular/core';
import { ICustomer } from 'src/app/models/ICustomer';
import { CustomersService } from 'src/app/services/customers.service';

@Component({
  selector: 'app-admin-manage-clients-tab',
  templateUrl: './admin-manage-clients-tab.component.html',
  styleUrls: ['./admin-manage-clients-tab.component.css'],
})
export class AdminManageClientsTabComponent implements OnInit {
  customers: ICustomer[] = [];
  pageNumber: number = 1;
  pageSize: number = 12;
  dataCount: number = 0;

  constructor(private customerService: CustomersService) {}

  ngOnInit(): void {
    this.getCustomers();
  }

  getCustomers() {
    this.customerService
      .getAllCustomers(this.pageNumber, this.pageSize)
      .subscribe({
        next: (response) => {
          this.customers = response.data;
          this.dataCount = response.dataCount;
        },
      });
  }

  onPageChange(newPageNumber: number) {
    this.pageNumber = newPageNumber;
    this.getCustomers();
  }
}
