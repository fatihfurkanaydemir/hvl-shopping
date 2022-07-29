import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';

import { SellerPanelComponent } from './seller-panel/seller-panel.component';
import { HomePageComponent } from './home-page/home-page.component';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { SidebarComponent } from './seller-panel/sidebar/sidebar.component';
import { SellerManageordersTabComponent } from './seller-panel/seller-manageorders-tab/seller-manageorders-tab.component';
import { SellerManageproductsTabComponent } from './seller-panel/seller-manageproducts-tab/seller-manageproducts-tab.component';
import { SellerDashboardTabComponent } from './seller-panel/seller-dashboard-tab/seller-dashboard-tab.component';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AddProductComponent } from './add-product/add-product.component';
import { FormsModule } from '@angular/forms';
import { ProductActionsComponent } from './product-actions/product-actions.component';

@NgModule({
  declarations: [
    AppComponent,
    SellerPanelComponent,
    HomePageComponent,
    SidebarComponent,
    SellerManageordersTabComponent,
    SellerManageproductsTabComponent,
    SellerDashboardTabComponent,
    AddProductComponent,
    ProductActionsComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    NgbModule,
    FormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
