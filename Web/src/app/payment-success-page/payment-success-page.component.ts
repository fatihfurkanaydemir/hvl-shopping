import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-payment-success-page',
  templateUrl: './payment-success-page.component.html',
  styleUrls: ['./payment-success-page.component.css'],
})
export class PaymentSuccessPageComponent implements OnInit {
  constructor(private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    console.log(this.route.snapshot);
  }
}
