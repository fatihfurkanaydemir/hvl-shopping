import { Component, OnInit } from '@angular/core';
import { ICategory } from '../models/ICategory';
import { IProduct } from '../models/IProduct';
import { CategoriesService } from '../services/categories.service';
import { ProductsService } from '../services/products.service';
import { NgbRatingConfig } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css'],
})
export class HomePageComponent implements OnInit {
  categories: ICategory[] = [];
  categoriesPageNumber: number = 1;
  categoriesPageSize: number = 30;

  products: IProduct[] = [];
  productsPageNumber: number = 1;
  productsPageSize: number = 15;

  filter!: string;

  constructor(
    private categoriesService: CategoriesService,
    private productsService: ProductsService,
    private config: NgbRatingConfig
  ) {}

  ngOnInit(): void {
    this.getCategory();
    this.getProducts();
    this.config.max = 5;
    this.config.readonly = true;
  }

  getCategory() {
    this.categoriesService
      .getAllCategories(this.categoriesPageNumber, this.categoriesPageSize)
      .subscribe((response) => {
        this.categories = response.data;
      });
  }
  getProducts() {
    this.productsService
      .getAllProducts(this.productsPageNumber, this.productsPageSize)
      .subscribe((response) => {
        this.products = response.data.filter(
          (p: any) => p.status !== 'Passive'
        );
      });
  }
}
