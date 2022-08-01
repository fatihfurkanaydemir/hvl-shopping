import { Component, OnInit } from '@angular/core';
import { ProductsService } from 'src/app/services/products.service';
import { IProduct } from '../models/IProduct';
import { IImage } from '../models/IImage';
import { ICategory } from '../models/ICategory';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {

  //_id: string = '';
  productCategory = {
    id: 0,
    name: ''
  }
  product: IProduct = {
    id: 0,
    name: '',
    code: '',
    description: '',
    images: [],
    inStock: 0,
    sold: 0,
    status: '',
    category: this.productCategory
  };

  constructor(private productsService: ProductsService, private activateRoute: ActivatedRoute) { }

  ngOnInit(): void {

    this.getProductByID();
  }

  getProductByID() {
    this.productsService.getProduct(2).subscribe((product) => {
        this.product = product.data;
        console.log(this.product);
    });
  }

}
