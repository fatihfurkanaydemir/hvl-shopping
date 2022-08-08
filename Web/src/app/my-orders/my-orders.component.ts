import { Component, OnInit } from '@angular/core';

export interface Order{
  date: string;
  summary: string;
  receiver: string;
  price: number;
  address: string;
  completed: boolean; //bool to check if order is delivered or not.
  returned: boolean;
}

@Component({
  selector: 'app-my-orders',
  templateUrl: './my-orders.component.html',
  styleUrls: ['./my-orders.component.css']
})

export class MyOrdersComponent implements OnInit {

  constructor() { }

  tabId:string = "tumu";
  ngOnInit(): void {
  }

  tabChange(id:string){
    this.tabId = id;
  }
  orders: Order[] = [
    {
      date:'05/08/2022',
      summary:'1 ürün',
      receiver:'Hasan',
      price: 500,
      address: 'Kağıthane/İstanbul',
      completed:true,
      returned:false
    },
    {
      date:'05/08/2022',
      summary:'2 ürün',
      receiver:'Duygu',
      price: 200,
      address: 'Beykent',
      completed:false,
      returned:true
    },
    {
      date:'05/08/2022',
      summary:'1 ürün',
      receiver:'Murat',
      price: 150,
      address: 'Mersin',
      completed:false,
      returned:false
    },
    {
      date:'05/08/2022',
      summary:'1 ürün',
      receiver:'Furkan',
      price: 150,
      address: 'Antalya',
      completed:true,
      returned:false
    }


  ]

}


