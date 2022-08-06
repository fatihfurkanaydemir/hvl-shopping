import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminDashboardTabComponent } from './admin-panel/admin-dashboard-tab/admin-dashboard.component-tab';
import { AdminManageCategoriesTabComponent } from './admin-panel/admin-manage-categories-tab/admin-manage-categories-tab.component';
import { AdminManageClientsTabComponent } from './admin-panel/admin-manage-clients-tab/admin-manage-clients-tab.component';
import { AdminManageSellersTabComponent } from './admin-panel/admin-manage-sellers-tab/admin-manage-sellers-tab.component';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { CategoriesPageComponent } from './categories-page/categories-page.component';
import { HomePageComponent } from './home-page/home-page.component';
import { SellerDashboardTabComponent } from './seller-panel/seller-dashboard-tab/seller-dashboard-tab.component';
import { SellerManageordersTabComponent } from './seller-panel/seller-manageorders-tab/seller-manageorders-tab.component';
import { SellerManageproductsTabComponent } from './seller-panel/seller-manageproducts-tab/seller-manageproducts-tab.component';
import { SellerPanelComponent } from './seller-panel/seller-panel.component';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { LoginPageComponent } from './login-page/login-page.component';
import { RegisterPageComponent } from './register-page/register-page.component';
import { SellerLoginComponent } from './seller-panel/seller-login/seller-login.component';
import { MyOrdersComponent } from './my-orders/my-orders.component';
import { UserProfileComponent } from './user-profile/user-profile.component';

const appRoutes: Routes = [
  { path: 'my-orders', component:MyOrdersComponent},
  { path: 'seller-login', component:SellerLoginComponent},
  { path: 'login', component: LoginPageComponent},
  { path: 'register', component: RegisterPageComponent},
  { path: 'user-profile', component: UserProfileComponent},
  { path: 'product/:id', component: ProductDetailsComponent },
  { path: '', component: HomePageComponent, pathMatch: 'full' },
  {
    path: 'seller-panel',
    component: SellerPanelComponent,
    children: [
      { path: 'dashboard', component: SellerDashboardTabComponent },
      { path: 'manage-products', component: SellerManageproductsTabComponent },
      { path: 'manage-orders', component: SellerManageordersTabComponent },
    ],
  },
  {
    path: 'admin-panel',
    component: AdminPanelComponent,
    children: [
      { path: 'dashboard', component: AdminDashboardTabComponent },
      { path: 'manage-clients', component: AdminManageClientsTabComponent },
      { path: 'manage-sellers', component: AdminManageSellersTabComponent },
      {
        path: 'manage-categories',
        component: AdminManageCategoriesTabComponent,
      },
    ],
  },
  { path: ':name', component: CategoriesPageComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
