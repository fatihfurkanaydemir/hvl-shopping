import { Component, OnInit } from '@angular/core';
import { IProduct } from 'src/app/models/IProduct';
import { ProductsService } from 'src/app/services/products.service';

@Component({
  selector: 'app-seller-manageproducts-tab',
  templateUrl: './seller-manageproducts-tab.component.html',
  styleUrls: ['./seller-manageproducts-tab.component.css'],
})
export class SellerManageproductsTabComponent implements OnInit {
  products: IProduct[] = [];
  pageNumber: number = 1;
  pageSize: number = 12;
  dataCount: number = 0;

  constructor(private productsService: ProductsService) {}

  ngOnInit(): void {
    this.getProducts();
  }

  getProducts() {
    this.productsService
      .getAllProducts(this.pageNumber, this.pageSize)
      .subscribe((response) => {
        this.products = response.data;
        this.dataCount = +response.dataCount;
      });
  }

  onPageChange(newPageNumber: number) {
    this.pageNumber = newPageNumber;
    this.getProducts();
  }
}
