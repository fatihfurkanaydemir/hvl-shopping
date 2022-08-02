import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';

import { SellerPanelComponent } from './seller-panel/seller-panel.component';
import { HomePageComponent } from './home-page/home-page.component';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { SidebarComponent as SellerSideBarComponent } from './seller-panel/sidebar/sidebar.component';
import { AdminSidebarComponent } from './admin-panel/admin-sidebar/admin-sidebar.component';
import { SellerManageordersTabComponent } from './seller-panel/seller-manageorders-tab/seller-manageorders-tab.component';
import { SellerManageproductsTabComponent } from './seller-panel/seller-manageproducts-tab/seller-manageproducts-tab.component';
import { SellerDashboardTabComponent } from './seller-panel/seller-dashboard-tab/seller-dashboard-tab.component';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AddProductComponent } from './seller-panel/add-product/add-product.component';
import { FormsModule } from '@angular/forms';
import { ProductActionsComponent } from './seller-panel/product-actions/product-actions.component';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { AdminDashboardTabComponent } from './admin-panel/admin-dashboard-tab/admin-dashboard.component-tab';
import { AdminManageClientsTabComponent } from './admin-panel/admin-manage-clients-tab/admin-manage-clients-tab.component';
import { AdminManageSellersTabComponent } from './admin-panel/admin-manage-sellers-tab/admin-manage-sellers-tab.component';
import { AdminManageCategoriesTabComponent } from './admin-panel/admin-manage-categories-tab/admin-manage-categories-tab.component';
import { AdminAddCategoryComponent } from './admin-panel/admin-manage-categories-tab/admin-add-category/admin-add-category.component';
import { AdminCategoryActionsComponent } from './admin-panel/admin-manage-categories-tab/admin-category-actions/admin-category-actions.component';
import { CategoriesPageComponent } from './categories-page/categories-page.component';

@NgModule({
  declarations: [
    AppComponent,
    SellerPanelComponent,
    HomePageComponent,
    SellerSideBarComponent,
    SellerManageordersTabComponent,
    SellerManageproductsTabComponent,
    SellerDashboardTabComponent,
    AddProductComponent,
    ProductActionsComponent,
    AdminPanelComponent,
    AdminSidebarComponent,
    AdminDashboardTabComponent,
    AdminManageClientsTabComponent,
    AdminManageSellersTabComponent,
    AdminManageCategoriesTabComponent,
    AdminAddCategoryComponent,
    AdminCategoryActionsComponent,
    CategoriesPageComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    NgbModule,
    FormsModule,
    [SweetAlert2Module.forRoot()],
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
