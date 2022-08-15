import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, empty, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Basket, IBasket, IBasketItem, IBasketTotals } from '../models/IBasket';
import { IProduct } from '../models/IProduct';
import { ProductsService } from 'src/app/services/products.service';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  initBasket!: IBasket;
  initBasketTotalSource!: IBasketTotals;
  baseUrl = environment.apiUrl;

  private basketSource = new BehaviorSubject<IBasket>(this.initBasket);
  private basketTotalSource = new BehaviorSubject<IBasketTotals>(this.initBasketTotalSource);

  basket$ = this.basketSource.asObservable();
  basketTotal$ = this.basketTotalSource.asObservable();

  constructor(private http: HttpClient, private productsService: ProductsService) {
    
   }

  getBasket(id: string){
    return this.http.get(this.baseUrl + '/basket?id=' + id) 
    .pipe(
      map((basket: any) =>{
        this.basketSource.next(basket);
        this.calculateTotals();
        console.log(this.getCurrentBasketValue());
      })
    );
    
  }

  setBasket(basket: IBasket){
    return this.http.post(this.baseUrl + '/basket', basket).subscribe((response: any) => {
        this.basketSource.next(response);
        this.calculateTotals();
        console.log(response)
    });
  }

  deleteBasket(basket: IBasket) {
    return this.http.delete(this.baseUrl + '/basket?id=' + basket.id).subscribe(() => {
      this.basketSource.next(this.initBasket);
      this.basketTotalSource.next(this.initBasketTotalSource);
      localStorage.removeItem('basket_id');
    });
  }

  getCurrentBasketValue(){
    return this.basketSource.value;
  }

  addItemToBasket(item: IProduct, quantity=1){
    const itemToAdd: IBasketItem = this.mapProductItemToBasketItem(item, quantity);
    const basket = this.getCurrentBasketValue() ?? this.createBasket();
    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);
    this.setBasket(basket);
  }

  incrementItemQuantity(item: IBasketItem, quantity=1){
    const basket = this.getCurrentBasketValue();
    const itemIndex = basket.items.findIndex(x => x.id === item.id);

    this.productsService.getProduct(basket.items[itemIndex].id).subscribe((product) => {
      const stock = product.data.inStock;

      if(basket.items[itemIndex].quantity < stock){
        basket.items[itemIndex].quantity++;
        this.setBasket(basket);
      } else{
        null;
      }
      console.log(basket.items[itemIndex].quantity, stock);
    })
    
    /*
    
    basket.items[itemIndex].quantity++;
    this.setBasket(basket);

    */
    
    
  }

  decrementItemQuantity(item: IBasketItem, quantity=1){
    const basket = this.getCurrentBasketValue();
    const itemIndex = basket.items.findIndex(x => x.id === item.id);
    if(basket.items[itemIndex].quantity > 1){
      basket.items[itemIndex].quantity--;
    } else{
      this.removeItem(item);
    }
    this.setBasket(basket);
  }

  removeItem(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    if(basket.items.some(x => x.id === item.id)){
      basket.items = basket.items.filter(i => i.id != item.id);
      if(basket.items.length > 0 ){
        this.setBasket(basket);
      } else{
        this.deleteBasket(basket);
      }
    }
  }

  getItemsLength(){
    const basket = this.getCurrentBasketValue();
    if (basket == undefined){
      return 0;
    } else {
      return basket.items.length;
    }
  }


  private calculateTotals(){
    const basket = this.getCurrentBasketValue();
    const shipping = 13;
    var subtotal = 0;

    basket.items.forEach(item => {
      subtotal += item.price * item.quantity;
    });

    var total = subtotal + shipping;

    this.basketTotalSource.next({
      shipping,
      subtotal,
      total
    });
  }

  private addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    const index = items.findIndex(i => i.id === itemToAdd.id);
    if (index === -1){
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    } else{
      items[index].quantity += quantity;
    }
    return items;
  }

  private createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  private mapProductItemToBasketItem(item: IProduct, quantity: number): IBasketItem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      quantity,
      pictureUrl: item.images[0].url,
      categoryName: item.category.name
    }
  }
}


