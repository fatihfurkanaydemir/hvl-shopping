import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-seller-login',
  templateUrl: './seller-login.component.html',
  styleUrls: ['./seller-login.component.css']
})
export class SellerLoginComponent implements OnInit {

  constructor() { }
  tabId: string = 'login';

  ngOnInit(): void {
  }
  tabChange(id: string) {
    this.tabId = id;
    console.log(id);
  }
}
