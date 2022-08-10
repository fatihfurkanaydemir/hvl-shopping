import { Pipe, PipeTransform } from '@angular/core';
import { IProduct } from './models/IProduct';

@Pipe({
  name: 'filter',
})
export class FilterPipe implements PipeTransform {
  transform(products: IProduct[], filterTerm: string, filterMetadata: any) {
    if (products.length == 0 || !filterTerm) {
      return products;
    } else {
      let filteredItems = products.filter((product) => {
        return product.name.toLowerCase().startsWith(filterTerm)
        //  || product.seller.shopName.toLowerCase().includes(filterTerm);
        // || product.category.name.toLowerCase().includes(filterTerm)
      });
      filterMetadata.count = filteredItems.length;
      return filteredItems;
    }
  }
}
