import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomePageComponent } from './app/home-page/home-page.component';
import { SellerDashboardTabComponent } from './app/seller-panel/seller-dashboard-tab/seller-dashboard-tab.component';
import { SellerManageordersTabComponent } from './app/seller-panel/seller-manageorders-tab/seller-manageorders-tab.component';
import { SellerManageproductsTabComponent } from './app/seller-panel/seller-manageproducts-tab/seller-manageproducts-tab.component';
import { SellerPanelComponent } from './app/seller-panel/seller-panel.component';

const appRoutes: Routes = [
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
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
