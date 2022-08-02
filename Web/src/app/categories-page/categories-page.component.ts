import { Component, DoCheck, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ICategory } from '../models/ICategory';
import { IProduct } from '../models/IProduct';
import { CategoriesService } from '../services/categories.service';
import { ProductsService } from '../services/products.service';
import {NgbRatingConfig} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-categories-page',
  templateUrl: './categories-page.component.html',
  styleUrls: ['./categories-page.component.css'],
})
export class CategoriesPageComponent implements OnInit, DoCheck {
  dataId: any;
  dataProducts: any;
  categories: ICategory[] = [];
  pageNumber: number = 1;
  pageSize: number = 12;
  dataCount: number = 0;
  products: IProduct[] = [];
  currentRate = 5;

  constructor(
    private activatedRoute: ActivatedRoute,
    private productsService: ProductsService,
    private categoriesService: CategoriesService,
    private config: NgbRatingConfig
  ) {}
  ngDoCheck(): void {
    this.getDataProducts();
  }

  ngOnInit(): void {
    this.getProducts();
    this.getCategory();
    this.config.max = 5;
    this.config.readonly = true;
  }
  getProducts() {
    this.productsService
      .getAllProducts(this.pageNumber, this.pageSize)
      .subscribe((response) => {
        this.products = response.data;
        this.dataCount = +response.dataCount;
      });
  }

  getCategory() {
    this.categoriesService
      .getAllCategories(this.pageNumber, this.pageSize)
      .subscribe((response) => {
        this.categories = response.data;
        this.dataCount = +response.dataCount;
      });
  }
  getDataProducts() {
    this.dataId = this.activatedRoute.snapshot.paramMap.get('id');
    this.dataProducts = this.products.filter(
      (c) => c.category.name === this.dataId
    );
  }
}
