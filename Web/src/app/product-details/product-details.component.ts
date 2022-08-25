import { Component, OnInit } from '@angular/core';
import { ProductsService } from 'src/app/services/products.service';
import { IProduct } from '../models/IProduct';
import { ICategory } from '../models/ICategory';
import { ActivatedRoute, Router } from '@angular/router';
import { ISeller } from '../models/ISeller';
import { BasketService } from '../basket/basket.service';
import { environment } from 'src/environments/environment';
import { Subject } from 'rxjs';

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

  currentImage: Subject<string> = new Subject<string>();

  productCategory: ICategory = {
    id: 0,
    name: '',
    productCount: 0,
  };

  seller: ISeller = {
    identityId: '',
    id: 0,
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    shopName: '',
    address: {
      id: 0,
      title: '',
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
    private basketService: BasketService,
    private productsService: ProductsService,
    private activateRoute: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this._id = Number(this.activateRoute.snapshot.paramMap.get('id'));
    this.getProductByID();
  }

  addItemToBasket() {
    this.basketService.addItemToBasket(this.product, this._desiredCount);
  }

  getImageUrl(index: number) {
    if (!this.product.images[0]?.url.startsWith('http'))
      return environment.baseUrl + '/' + this.product.images[index]?.url;
    else return this.product.images[0]?.url;
  }

  getProductByID() {
    this.productsService.getProduct(this._id).subscribe((product) => {
      this.product = product.data;
      this.seller = product.data.seller;

      this.currentImage.next(this.getImageUrl(this._urlCount));
    });
  }

  incrementDesiredCount() {
    if (this._desiredCount > this.product.inStock) {
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
    if (this._urlCount >= this.product.images.length - 1) {
      this._urlCount = 0;
      this.currentImage.next(this.getImageUrl(this._urlCount));
    } else {
      this._urlCount += 1;
      this.currentImage.next(this.getImageUrl(this._urlCount));
    }
  }

  decrementUrlCount() {
    if (this._urlCount <= 0) {
      this._urlCount = this.product.images.length - 1;
      this.currentImage.next(this.getImageUrl(this._urlCount));
    } else {
      this._urlCount -= 1;
      this.currentImage.next(this.getImageUrl(this._urlCount));
    }
  }

  tabChange(id: string) {
    this.tabId = id;
  }
}
