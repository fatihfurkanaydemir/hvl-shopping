import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Emitters } from './emitters/emitters';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  authenticated!: false;
  
  constructor(private router: Router){
  }
  ngOnInit(): void {
    Emitters.authEmitter.subscribe(
      (auth: any) => {
        this.authenticated = auth;
      }
    )
  }
  
  goToPage(pageName:string){
      this.router.navigate([`${pageName}`]);
  }
  
  title = 'Havelsan Shopping';
}
