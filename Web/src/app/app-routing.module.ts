import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CategoriesPageComponent } from './categories-page/categories-page.component';
import { HomePageComponent } from './home-page/home-page.component';
import { SellerDashboardTabComponent } from './seller-panel/seller-dashboard-tab/seller-dashboard-tab.component';
import { SellerManageordersTabComponent } from './seller-panel/seller-manageorders-tab/seller-manageorders-tab.component';
import { SellerManageproductsTabComponent } from './seller-panel/seller-manageproducts-tab/seller-manageproducts-tab.component';
import { SellerPanelComponent } from './seller-panel/seller-panel.component';
import { ProductDetailsComponent } from './product-details/product-details.component';

const appRoutes: Routes = [
  { path: 'shop/:id', component:ProductDetailsComponent},
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
  { path: ':id', component: CategoriesPageComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
