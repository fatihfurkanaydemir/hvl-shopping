import {
  AfterContentChecked,
  ChangeDetectorRef,
  Component,
  OnInit,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IProduct } from '../models/IProduct';
import { ProductsService } from '../services/products.service';

@Component({
  selector: 'app-search-filter',
  templateUrl: './search-filter.component.html',
  styleUrls: ['./search-filter.component.css'],
})
export class SearchFilterComponent implements OnInit, AfterContentChecked {
  filterTerm!: string;
  filter!: string;
  filterMetadata = { count: 0 };
  products: IProduct[] = [];
  productsDataCount: number = 0;
  productsPageNumber: number = 1;
  productsPageSize: number = 50;

  constructor(
    private activatedRoute: ActivatedRoute,
    private productsService: ProductsService,
    private changeDetector: ChangeDetectorRef
  ) {}
  ngAfterContentChecked(): void {
    // solition for NG0100: ExpressionChangedAfterItHasBeenCheckedError
    this.changeDetector.detectChanges();
  }

  ngOnInit(): void {
    this.getProducts();
  }

  getProducts() {
    this.productsService
      .getAllProducts(this.productsPageNumber, this.productsPageSize)
      .subscribe((response) => {
        this.products = response.data;
        this.productsDataCount = +response.dataCount;

        this.activatedRoute.queryParams.subscribe((params) => {
          this.filterTerm = params['search'];
        });
      });
  }
  onPageChange(newPageNumber: number) {
    this.productsPageNumber = newPageNumber;
    this.getProducts();
  }
}
