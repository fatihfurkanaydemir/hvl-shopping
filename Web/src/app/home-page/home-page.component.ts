import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ICategory } from '../models/ICategory';
import { IProduct } from '../models/IProduct';
import { CategoriesService } from '../services/categories.service';
import { ProductsService } from '../services/products.service';
import {NgbRatingConfig} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css'],
})
export class HomePageComponent implements OnInit {
  categories: ICategory[] = [];
  pageNumber: number = 1;
  pageSize: number = 12;
  dataCount: number = 0;
  products: IProduct[] = [];

  constructor(
    private categoriesService: CategoriesService,
    private productsService: ProductsService,
    private config: NgbRatingConfig,
  ) {}

  ngOnInit(): void {
    this.getCategory();
    this.getProducts();
    this.config.max = 5;
    this.config.readonly = true;
  }

  getCategory() {
    this.categoriesService
      .getAllCategories(this.pageNumber, this.pageSize)
      .subscribe((response) => {
        this.categories = response.data;
        this.dataCount = +response.dataCount;
      });
  }
  getProducts() {
    this.productsService
      .getAllProducts(this.pageNumber, this.pageSize)
      .subscribe((response) => {
        this.products = response.data;
        this.dataCount = +response.dataCount;
      });
  }
}
