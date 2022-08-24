import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BasketService } from '../basket/basket.service';

@Component({
  selector: 'app-payment-success-page',
  templateUrl: './payment-success-page.component.html',
  styleUrls: ['./payment-success-page.component.css'],
})
export class PaymentSuccessPageComponent implements OnInit {
  constructor(private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    if (!this.route.snapshot.queryParams['session_id'])
      this.router.navigate(['/']);
    else {
      setTimeout(() => {
        this.router.navigate(['/']);
      }, 3000);
    }
  }
}
