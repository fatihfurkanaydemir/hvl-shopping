import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminDashboardTabComponent } from './admin-panel/admin-dashboard-tab/admin-dashboard.component-tab';
import { AdminManageCategoriesTabComponent } from './admin-panel/admin-manage-categories-tab/admin-manage-categories-tab.component';
import { AdminManageClientsTabComponent } from './admin-panel/admin-manage-clients-tab/admin-manage-clients-tab.component';
import { AdminManageSellersTabComponent } from './admin-panel/admin-manage-sellers-tab/admin-manage-sellers-tab.component';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { HomePageComponent } from './home-page/home-page.component';
import { SellerDashboardTabComponent } from './seller-panel/seller-dashboard-tab/seller-dashboard-tab.component';
import { SellerManageordersTabComponent } from './seller-panel/seller-manageorders-tab/seller-manageorders-tab.component';
import { SellerManageproductsTabComponent } from './seller-panel/seller-manageproducts-tab/seller-manageproducts-tab.component';
import { SellerPanelComponent } from './seller-panel/seller-panel.component';

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
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
