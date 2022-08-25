import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ICategory } from '../models/ICategory';
import { IProduct } from '../models/IProduct';
import { CategoriesService } from '../services/categories.service';
import { NgbRatingConfig } from '@ng-bootstrap/ng-bootstrap';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-categories-page',
  templateUrl: './categories-page.component.html',
  styleUrls: ['./categories-page.component.css'],
})
export class CategoriesPageComponent implements OnInit {
  categories: ICategory[] = [];
  categoriesPageNumber: number = 1;
  categoriesPageSize: number = 30;

  products: IProduct[] = [];
  productsDataCount: number = 0;
  productsPageNumber: number = 1;
  productsPageSize: number = 20;

  constructor(
    private activatedRoute: ActivatedRoute,
    private categoriesService: CategoriesService,
    private config: NgbRatingConfig,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getCategoryProducts();
    this.config.max = 5;
    this.config.readonly = true;
  }

  getCategoryProducts() {
    this.activatedRoute.paramMap.subscribe((paramMap) => {
      const categoryName = paramMap.get('name');

      this.categoriesService
        .getAllCategories(this.categoriesPageNumber, this.categoriesPageSize)
        .subscribe((response) => {
          this.categories = response.data;

          const categoryId = this.getCategoryIdByName(categoryName!);
          if (categoryId === undefined) {
            this.router.navigate(['']);
          } else {
            this.categoriesService
              .getCategoryProducts(
                categoryId!,
                this.productsPageNumber,
                this.productsPageSize
              )
              .subscribe({
                next: (response) => {
                  this.products = response.data.products.filter(
                    (p: any) => p.status !== 'Passive'
                  );
                  this.productsDataCount = this.products.length;
                },
              });
          }
        });
    });
  }

  onPageChange(newPageNumber: number) {
    this.productsPageNumber = newPageNumber;
    this.getCategoryProducts();
  }

  getImageUrl(product: IProduct) {
    if (!product.images[0]?.url.startsWith('http'))
      return environment.baseUrl + '/' + product.images[0]?.url;
    else return product.images[0]?.url;
  }

  getCategoryIdByName(name: string) {
    return this.categories.find((c) => c.name === name)?.id;
  }
}
