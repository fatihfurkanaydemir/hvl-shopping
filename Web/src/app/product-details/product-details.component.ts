import { Component, OnInit } from '@angular/core';
import { ProductsService } from 'src/app/services/products.service';
import { IProduct } from '../models/IProduct';
import { IImage } from '../models/IImage';
import { ICategory } from '../models/ICategory';
import { ActivatedRoute, Router } from '@angular/router';
import { identifierName } from '@angular/compiler';
import { ISeller } from '../models/ISeller';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css'],
})
export class ProductDetailsComponent implements OnInit {
  _id: number = 0;
  _price: number = 50; //placeholder
  tabId: string = 'aciklama';
  _desiredCount: number = 1;
  isNavActive: string = 'active';
  _urlCount: number = 0;

  productCategory: ICategory = {
    id: 0,
    name: '',
    productCount: 0,
  };

  seller: ISeller = {
    id: 0,
    firstName: '',
    lastName: '',
    phoneNumber: '',
    shopName: '',
    address: {
      addressDescription: '',
      city: '',
    },
  };

  product: IProduct = {
    id: 0,
    name: '',
    code: '',
    description: '',
    images: [],
    price: 0,
    inStock: 0,
    sold: 0,
    status: '',
    seller: this.seller,
    category: this.productCategory,
  };

  constructor(
    private productsService: ProductsService,
    private activateRoute: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this._id = Number(this.activateRoute.snapshot.paramMap.get('id'));
    this.getProductByID();
  }
  getProductByID() {
    this.productsService.getProduct(this._id).subscribe(
      (product) => {
        //placeholder
        this.product = product.data;
        console.log(this.product);
        console.log('images length: ' + this.product.images.length);
        console.log('url count: ' + this._urlCount);
      },
      (error) => {
        this.router.navigate(['']);
      }
    );
  }

  incrementDesiredCount() {
    if (this._desiredCount > this.product.inStock) {
      console.log('Reached the product stock.');
    } else {
      this._desiredCount += 1;
    }
  }

  decrementDesiredCount() {
    if (this._desiredCount <= 1) {
      return;
    } else {
      this._desiredCount -= 1;
    }
  }

  incrementUrlCount() {
    console.log('url count: ' + this._urlCount);
    if (this._urlCount >= this.product.images.length - 1) {
      this._urlCount = 0;
    } else {
      this._urlCount += 1;
    }
    console.log('url count: ' + this._urlCount);
  }

  decrementUrlCount() {
    if (this._urlCount <= 0) {
      this._urlCount = this.product.images.length - 1;
    } else {
      this._urlCount -= 1;
    }
    console.log(this._urlCount);
  }

  tabChange(id: string) {
    this.tabId = id;
  }

  activeButtonChange() {}
}
