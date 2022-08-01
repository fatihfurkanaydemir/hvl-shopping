import { Component, OnInit } from '@angular/core';
import { ICategory } from 'src/app/models/ICategory';
import { CategoriesService } from 'src/app/services/categories.service';

@Component({
  selector: 'app-admin-manage-categories-tab',
  templateUrl: './admin-manage-categories-tab.component.html',
  styleUrls: ['./admin-manage-categories-tab.component.css'],
})
export class AdminManageCategoriesTabComponent implements OnInit {
  categories: ICategory[] = [];
  pageNumber: number = 1;
  pageSize: number = 12;
  dataCount: number = 0;

  constructor(private categoriesService: CategoriesService) {}

  ngOnInit(): void {
    this.getCategories();
  }

  getCategories() {
    this.categoriesService
      .getAllCategories(this.pageNumber, this.pageSize)
      .subscribe((response) => {
        this.categories = response.data;
        this.dataCount = +response.dataCount;
      });
  }

  onPageChange(newPageNumber: number) {
    this.pageNumber = newPageNumber;
    this.getCategories();
  }
}
