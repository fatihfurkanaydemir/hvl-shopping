import { Component, OnInit } from '@angular/core';
import { map } from 'rxjs';
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
  pageSize: number = 15;

  constructor(private productsService: ProductsService) {}

  ngOnInit(): void {}

  getProducts() {
    this.productsService
      .getAllProducts(this.pageNumber, this.pageSize)
      .pipe(map((response) => response.data))
      .subscribe((data) => (this.products = data));
  }
}
