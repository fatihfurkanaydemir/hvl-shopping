import { ICategory } from './ICategory';
import { IImage } from './IImage';

export interface IProduct {
  id: number,
  name: string;
  code: string;
  description: string;
  images: IImage[];
  inStock: number;
  sold: number;
  status: string;
  category: ICategory;
}
