import {
  AfterContentChecked,
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { exhaustMap, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IProduct } from '../models/IProduct';
import { ProductsService } from '../services/products.service';

@Component({
  selector: 'app-search-filter',
  templateUrl: './search-filter.component.html',
  styleUrls: ['./search-filter.component.css'],
})
export class SearchFilterComponent implements OnInit, OnDestroy {
  products: IProduct[] = [];
  filterString: string = '';
  productsDataCount: number = 0;
  productsPageNumber: number = 1;
  productsPageSize: number = 25;

  paramsSubscription!: Subscription;

  constructor(
    private activatedRoute: ActivatedRoute,
    private productsService: ProductsService
  ) {}

  ngOnInit(): void {
    this.paramsSubscription = this.activatedRoute.queryParams.subscribe(
      (params) => {
        this.filterString = params['search'];
        this.getProducts(this.filterString);
      }
    );
  }

  ngOnDestroy() {}

  getProducts(filterString: string) {
    this.productsService
      .getProductsBySearchFilter(
        filterString,
        this.productsPageNumber,
        this.productsPageSize
      )

      .subscribe({
        next: (response) => {
          this.products = response.data;
          this.productsDataCount = +response.dataCount;
        },
        error: (error) => {
          console.log(error);
        },
      });
  }

  getImageUrl(product: IProduct) {
    if (!product.images[0]?.url.startsWith('http'))
      return environment.baseUrl + '/' + product.images[0]?.url;
    else return product.images[0]?.url;
  }

  onPageChange(newPageNumber: number) {
    this.productsPageNumber = newPageNumber;
    this.getProducts(this.filterString);
  }
}
