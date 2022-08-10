import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IBasket } from '../models/IBasket';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  initBasket!: IBasket;
  baseUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject<IBasket>(this.initBasket);
  basket$ = this.basketSource.asObservable();

  constructor(private http: HttpClient) {
    
   }

  getBasket(id: string){
    return this.http.get(this.baseUrl + 'basket?id=' + id)
    .pipe(
      map((basket: any) =>{
        this.basketSource.next(basket);
      })
    );
  }

  setBasket(basket: IBasket){
    return this.http.post(this.baseUrl + 'basket', basket).subscribe((response: any) => {
        this.basketSource.next(response);
    });
  }

  getCurrentBasketValue(){
    return this.basketSource.value;
  }
}
