import { ICategory } from './ICategory';
import { IImage } from './IImage';
import { ISeller } from './ISeller';

export interface IProduct {
  id: number;
  name: string;
  code: string;
  description: string;
  images: IImage[];
  price: number;
  inStock: number;
  sold: number;
  status: string;
  seller: ISeller;
  category: ICategory;
}
