import { Component, OnInit } from '@angular/core';
import { ICategory } from '../models/ICategory';
import { CategoriesService } from '../services/categories.service';
import { NgbRatingConfig } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  categories: ICategory[] = [];
  categoriesPageNumber: number = 1;
  categoriesPageSize: number = 30;


  filter!: string;

  constructor(
    private categoriesService: CategoriesService,
    private config: NgbRatingConfig,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getCategory();
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
  navigateSearch() {
    this.router.navigate(['/search'], { queryParams: { search: this.filter } });
  }
}
