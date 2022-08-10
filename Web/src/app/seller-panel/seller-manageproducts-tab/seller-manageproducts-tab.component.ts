import { Component, OnInit } from '@angular/core';
import { exhaustMap, take, tap } from 'rxjs';
import { IProduct } from 'src/app/models/IProduct';
import { AuthService } from 'src/app/services/auth.service';
import { ProductsService } from 'src/app/services/products.service';

@Component({
  selector: 'app-seller-manageproducts-tab',
  templateUrl: './seller-manageproducts-tab.component.html',
  styleUrls: ['./seller-manageproducts-tab.component.css'],
})
export class SellerManageproductsTabComponent implements OnInit {
  filter! : string;
  filterTerm! : string;
  filterMetadata = { count: 0 };
  products: IProduct[] = [];
  pageNumber: number = 1;
  pageSize: number = 12;
  dataCount: number = 0;

  constructor(
    private productsService: ProductsService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.getProducts();
  }

  getProducts() {
    this.authService.userSubject
      .pipe(
        take(1),
        exhaustMap((user) => {
          return this.productsService.getAllProductsByIdentityId(
            user.identityId,
            this.pageNumber,
            this.pageSize
          );
        })
      )
      .subscribe((response) => {
        this.products = response.data.products;
        this.dataCount = +response.dataCount;
      });
  }
  search(){
    this.filterTerm = this.filter;
  }

  onPageChange(newPageNumber: number) {
    this.pageNumber = newPageNumber;
    this.getProducts();
  }
}
