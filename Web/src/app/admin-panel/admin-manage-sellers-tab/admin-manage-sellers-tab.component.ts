import { Component, OnInit } from '@angular/core';
import { ISeller } from 'src/app/models/ISeller';
import { SellersService } from 'src/app/services/sellers.service';

@Component({
  selector: 'app-admin-manage-sellers-tab',
  templateUrl: './admin-manage-sellers-tab.component.html',
  styleUrls: ['./admin-manage-sellers-tab.component.css'],
})
export class AdminManageSellersTabComponent implements OnInit {
  sellers: ISeller[] = [];
  pageNumber: number = 1;
  pageSize: number = 12;
  dataCount: number = 0;

  constructor(private sellersService: SellersService) {}

  ngOnInit(): void {
    this.getCustomers();
  }

  getCustomers() {
    this.sellersService
      .getAllSellers(this.pageNumber, this.pageSize)
      .subscribe({
        next: (response) => {
          this.sellers = response.data;
          this.dataCount = response.dataCount;
        },
      });
  }

  onPageChange(newPageNumber: number) {
    this.pageNumber = newPageNumber;
    this.getCustomers();
  }
}
