import { ICategory } from './ICategory';
import { IImage } from './IImage';

export interface IProductCreate {
  sellerIdentityId: string;
  name: string;
  code: string;
  description: string;
  images: IImage[];
  price: number;
  inStock: number;
  categoryId: number;
}
